# GamerBot

|Table of Contents|
| --------------- |
|[What Is This Project About?](#what-is-this-project-about)|
|[How to Use the Bot](#how-to-use-the-bot)|
|[Goals](#goals)|
|[Credits](#credits)|

---------

## What Is This Project About?

GamerBot is a bot for the social media platform [Discord](https://discord.com/).
It uses the [Steam](https://store.steampowered.com/) [internal API](https://github.com/Revadike/InternalSteamWebAPI) to get information on games and users, such as pricing and statistics.

## How to Use the Bot

GamerBot uses commands to provide relevant information. You can call a command by typing
it into the message box prefixed by a slash.

### Available commands

#### Ping

`/ping` checks the latency of the bot to Discord's servers, returning a message with the latency in milliseconds.

#### Search

`/search` can be used to look for games on Steam.
Once executed, it brings up a search box, which will return a set of relevant games when submitted.
GamerBot will then provide these titles in a dropdown menu.

## Goals

- [x] Bot comes online
- [x] Command handler working
	- [x] Ping command works
- [x] External API calls working
	- [x] REST calls to steam functional
- [ ] Commands completed
	- [ ] Search game command
		- [ ] Game info response
		- [x] Display game titles
	- [ ] Search User command
		- [ ] User info response
		- [ ] Display user names
	- [ ] Compare users command
		- [ ] Finds user info
		- [ ] Displays statistic differences (i.e. playtime, achievements etc.)
		- [ ] Can bring up steam year-in-review info

## Credits

- Credit to [Discord](https://discord.com/) for providing the api to make bots possible
- Credit to [Discord.NET](https://docs.discordnet.dev/) for providing a library to create bots in .NET
- Many Thanks to [Crenston Customs](https://www.youtube.com/@crenstoncustoms-coding8064) for creating the [tutorials](https://www.youtube.com/playlist?list=PLeQp_dRyK6BIExuFulaKYWC8utbVlOBeT) that helped me create this bot