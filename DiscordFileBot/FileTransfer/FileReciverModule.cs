using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.Yaml;
using Discord.Commands;
using System.Threading;

namespace DiscordFileBot.FileTransfer
{
    class FileReciverModule : ModuleBase<SocketCommandContext>
    {
        bool downloadCheck = false;
        [Command("download?")]
        public async Task HandelRecive()
        {
            Console.WriteLine("want to dowload files? \ny/n");
            if(Console.ReadLine() == "y")
            {
                downloadCheck = true;
                Context.Channel.SendMessageAsync("!:thumbsup:");
                Thread.Sleep(3000);

            }
            
        }
    }
}
