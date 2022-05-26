﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.Yaml;
using Discord.Commands;

namespace DiscordFileBot
{
    class Program
    {
        private bool isFileMan;
        static Task Main() => new Program().MainAsync();

        public async Task MainAsync()
        {
            await BotSelector();
            
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // use AppContext.BaseDirectory  
                .AddYamlFile("config.yml")
                .Build();

            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                services
                .AddSingleton(config)
                .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig
                {
                    GatewayIntents = GatewayIntents.AllUnprivileged,
                    AlwaysDownloadUsers = true
                }))
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionHandler>()
                .AddSingleton(x => new CommandService())
                .AddSingleton<PrefixHandler>()
                )
                .Build();

            await RunAsync(host);

        }
        public async Task BotSelector()
        {
            string input;
            Console.WriteLine("File Man (1) \nFile Boy (2)");
            input = Console.ReadLine();
            if (input == "1") isFileMan = true;
            if (input == "2") isFileMan = false;

        }

        public async Task RunAsync(IHost host)
        {
            using IServiceScope servicescope = host.Services.CreateScope();
            IServiceProvider provider = servicescope.ServiceProvider;

            var _client = provider.GetRequiredService<DiscordSocketClient>();
            var sCommands = provider.GetRequiredService<InteractionService>();
            await provider.GetRequiredService<InteractionHandler>().InitializAsync();
            var config = provider.GetRequiredService<IConfigurationRoot>();
            var pCommands = provider.GetRequiredService<PrefixHandler>();
            // add commands below
            pCommands.AddModuel<DiscordFileBot.Modules.PrefixModule>();
            pCommands.AddModuel<DiscordFileBot.FileTransfer.CommonModule>();
            if(isFileMan) pCommands.AddModuel<DiscordFileBot.FileTransfer.FileSenderModule>();
            if(!isFileMan) pCommands.AddModuel<DiscordFileBot.FileTransfer.FileReciverModule>();

            await pCommands.InitializeAsync();

            _client.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };
            sCommands.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };
            
            _client.Ready += async () => {
                Console.WriteLine("Bot ready");
                await sCommands.RegisterCommandsToGuildAsync(UInt64.Parse(config["testGuild"]));
            };

            //OTc3NTgzMzk2NTMwMzE1MzE0.GOdLrc.t9OsS-LaHBc0YRjzMiKSJ-dF2OEctHun-iMse4
            if(isFileMan == null)
            {
                Console.WriteLine(":(");
                return;
            }
            if (isFileMan) await _client.LoginAsync(TokenType.Bot, config["tokens:discordMan"]);
            if (!isFileMan) await _client.LoginAsync(TokenType.Bot, config["tokens:discordBoy"]);

            await _client.StartAsync();

            await Task.Delay(-1);
        }
        
    }
}