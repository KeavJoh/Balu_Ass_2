using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Controllers;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Security;

class Programm
{
    private static DiscordClient? Client { get; set; }
    private static CommandsNextExtension? Commands { get; set; }

    static async Task Main(string[] args)
    {
        InitBotConfigController botConfigController = new();
        BotConfig botConfig = botConfigController.InitBotConfig();
    }
}
