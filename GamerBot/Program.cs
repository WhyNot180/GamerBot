using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace GamerBot
{
    public class Program
    {
        private static DiscordSocketClient _client;

        public static Task Main() => new Program().MainAsync();

        public async Task MainAsync()
        {

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            DiscordSocketConfig socketConfig = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.AllUnprivileged,
                AlwaysDownloadUsers = true,

            };

            _client = new DiscordSocketClient();
            _client.Log += Log;

            var token = config["Discord:Token"];

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
