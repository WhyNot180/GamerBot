using Discord;
using Discord.Interactions;
using System.Net.Http.Json;
using System.Web;

namespace GamerBot.Modules
{
    public class InteractionModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Implements slash commands and interactions (i.e. buttons, select menus etc.)
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public InteractionModule(IHttpClientFactory httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }

        [SlashCommand("ping", "Receive a ping in ms!")]
        public async Task HandlePingCommand()
        {
            await RespondAsync($"PONG! ({Context.Client.Latency} ms)");
        }

        [SlashCommand("search", "Search for a steam game")]
        public async Task HandleSearchCommand()
        {
            await RespondWithModalAsync<SteamModal>("search_steam_store");
        }

        [ModalInteraction("search_steam_store")]
        public async Task HandleSteamSearchModal(SteamModal steamModal)
        {
            string input = steamModal.SearchQuery;
            await DeferAsync();

            string searchQuery = HttpUtility.UrlEncode(input);
            string path = "actions/SearchApps/" + searchQuery;

            List<AppSearchResults>? searchResults = await _httpClientFactory.CreateClient("steamCommunityClient").GetFromJsonAsync<List<AppSearchResults>>(path);

            if (searchResults != null && searchResults.Any())
            {
                var select = new SelectMenuBuilder()
                {
                    CustomId = "game_select",
                    Placeholder = "Select Search Result"
                };

                foreach (var app in searchResults)
                {
                    select.AddOption(app.name, app.appid);
                }

                var componentBuilder = new ComponentBuilder();
                componentBuilder.WithSelectMenu(select);
                
                await FollowupAsync(components: componentBuilder.Build());
            }
            else
            {
                await FollowupAsync("No games matching your search have been found. Please try again!");
            }

        }
    }

    /// <summary>
    /// The input overlay for searching steamapps
    /// </summary>
    public class SteamModal : IModal
    {
        public string Title => "Search Steam Store";

        [InputLabel("Search for games on the steam store!")]
        // Note: Steam only allows 32 characters in a title
        [ModalTextInput("search_query", TextInputStyle.Short, maxLength:32)]
        public string SearchQuery { get; set; }
    }
}
