using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Controllers;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;

class Programm
{
    private static DiscordClient? Client { get; set; }
    private static CommandsNextExtension? Commands { get; set; }

    static async Task Main(string[] args)
    {
        //bot configuration
        InitBotConfigController botConfigController = new();
        BotConfig botConfig = botConfigController.InitBotConfig();
        ClientReadyController clientReadyController = new();

        //registrate bot
        RegistrateBotController registrateBot = new();
        ClientReadyController clientReady = new();

        Client = new DiscordClient(registrateBot.InitClient(botConfig));

        Client.Ready += clientReady.ClientReady;
        

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }
}
