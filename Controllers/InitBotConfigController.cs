using Balu_Ass_2.BotSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Controllers
{
    internal class InitBotConfigController
    {
        public BotConfig InitBotConfig()
        {
            IConfiguration configuration = InitConfiguration();
            BotConfig botConfig = InitBotConfig(configuration);

            return botConfig;
        }

        static IConfiguration InitConfiguration()
        {
            IConfiguration? configuration = null;
            var builder = new ConfigurationBuilder();

            try
            {
                builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("botsettings.json");

                configuration = builder.Build();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Die benötigte Datei ist nicht vorhanden! Programm wird beendet: " + ex.Message);
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Es ist ein Fehler beim Abrufen der benötigten Datei aufgetreten. Das Programm wird beendet: " + ex.Message);
                Environment.Exit(1);
            }

            return configuration;
        }

        static BotConfig InitBotConfig(IConfiguration configuration)
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((context, service) =>
            {
                BotConfig botConfig = new();
                configuration.Bind(botConfig);
                if (botConfig == null)
                {
                    Console.WriteLine("BotConfig fehlerhaft. Programm wird beendet: ");
                    Environment.Exit(1);
                }
                service.AddSingleton(botConfig);
            })
            .Build();

            var botConfig = host.Services.GetRequiredService<BotConfig>();

            return botConfig;

        }
    }
}
