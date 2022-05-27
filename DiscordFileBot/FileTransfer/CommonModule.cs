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
using System.IO;
using System.Threading;

namespace DiscordFileBot.FileTransfer
{
    class CommonModule : ModuleBase<SocketCommandContext>
    {
        [Command("Setup")]
        public async Task HandelSetup()
        {
            string directory = AppContext.BaseDirectory;

            Directory.CreateDirectory(directory + "/Input-Folder");
            Directory.CreateDirectory(directory + "/Temp-Folder");
            Directory.CreateDirectory(directory + "/Temp-Folder2");
            Directory.CreateDirectory(directory + "/Output-Folder");
            //Thread.Sleep(5000);
            //Directory.Delete(directory + "/Input-Folder");
            Context.Channel.SendMessageAsync(directory);
        }
    }
}
