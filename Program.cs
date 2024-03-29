﻿using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Controllers;
using Balu_Ass_2.Data.Database;
using Balu_Ass_2.Modals;
using Balu_Ass_2.Views;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;

class Programm
{
    private static DiscordClient? Client { get; set; }
    private static CommandsNextExtension? Commands { get; set; }
    private static Timer? Timer { get; set; }

    static async Task Main(string[] args)
    {
        InitBotConfigController botConfigController = new();
        RegistrateBotController registrateBotController = new();
        ClientReadyController clientReadyController = new();

        //bot configuration
        BotConfig botConfig = botConfigController.InitBotConfig();

        //database configuration
        using var context = new ApplicationDbContext(botConfig);
        context.Database.Migrate();
        context.Database.EnsureCreated();

        //registrate bot
        Client = new DiscordClient(registrateBotController.InitClient(botConfig));

        //provide settings
        await botConfigController.InitProvidedSetup(botConfig, context, Client);

        //init log controller
        LogController.SetContext(ProvidedSetups.Context);

        //init _DataStore
        await _DataStore.InitDataStore();

        //init start bot
        Client.Ready += clientReadyController.ClientReady;

        //add action listener
        Client.ComponentInteractionCreated += ButtonController.ButtonClickEvent;
        Client.ModalSubmitted += ModalController.ModalSubmitEvent;
        Client.ComponentInteractionCreated += DropdownController.DropdwonSubmitEvent;

        Timer = new Timer(StartTimer, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }

    private static async void StartTimer(object state)
    {
        await TimerActionController.TimerActions();
    }
}
