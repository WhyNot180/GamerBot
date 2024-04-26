using Discord;
using Discord.Interactions;

namespace GamerBot.Modules
{
    public class InteractionModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Receive a ping in ms!")]
        public async Task HandlePingCommand()
        {
            await RespondAsync($"PONG! ({Context.Client.Latency} ms)");
        }
    }
}
