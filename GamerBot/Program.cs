using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Web;

namespace GamerBot
{
    public class Program
    {
        //private static DiscordSocketClient _client;

        public static Task Main() => new Program().MainAsync();

        public async Task MainAsync()
        {
            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    services
                    .AddSingleton(X => new DiscordSocketClient(new DiscordSocketConfig
                    {
                        GatewayIntents = GatewayIntents.AllUnprivileged,
                        AlwaysDownloadUsers = true,
                    })))
                .Build();

            await RunAsync(host);
        }

        public async Task RunAsync(IHost host)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            var _client = serviceProvider.GetRequiredService<DiscordSocketClient>();
            
            _client.Log += Log;
            _client.Ready += async () =>
            {
                Console.WriteLine("Bot Ready!");
            };

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
