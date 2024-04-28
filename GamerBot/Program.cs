using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Web;
using Discord.Interactions;

namespace GamerBot
{
    public class Program
    {

        public static Task Main() => new Program().MainAsync();

        public async Task MainAsync()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            // Adds dependencies
            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    services
                    .AddSingleton(config)
                    .AddSingleton(X => new DiscordSocketClient(new DiscordSocketConfig
                    {
                        GatewayIntents = GatewayIntents.AllUnprivileged,
                        AlwaysDownloadUsers = true,
                        UseInteractionSnowflakeDate = false,
                    }))
                    .AddSingleton(X => new InteractionService(X.GetRequiredService<DiscordSocketClient>()))
                    .AddSingleton<InteractionHandler>()
                    .AddHttpClient("steamCommunityClient", c => c.BaseAddress = new Uri(config["URIs:SteamCommunity"]))
                    .Services
                    .AddHttpClient("steamStoreClient", c => c.BaseAddress = new Uri(config["URIs:SteamStore"]))
                    .Services
                    .AddHttpClient("valveSoftwareClient", c => c.BaseAddress = new Uri(config["URIs:ValveSoftware"]))
                 )
                .Build();

            await RunAsync(host);
        }

        public async Task RunAsync(IHost host)
        {

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            var _client = serviceProvider.GetRequiredService<DiscordSocketClient>();
            var sCommands = serviceProvider.GetRequiredService<InteractionService>();
            await serviceProvider.GetRequiredService<InteractionHandler>().InitializeAsync();
            var config = serviceProvider.GetRequiredService<IConfigurationRoot>();

            _client.Log += Log;
            sCommands.Log += Log;

            _client.Ready += async () =>
            {
                Console.WriteLine("Bot Ready!");
                await sCommands.RegisterCommandsGloballyAsync(true);
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
