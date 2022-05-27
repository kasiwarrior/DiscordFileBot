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
        
        public static async Task HandelSetup()
        {
            string directory = AppContext.BaseDirectory;

            Directory.CreateDirectory(directory + "/Input-Folder");
            Directory.CreateDirectory(directory + "/Temp-Spliting-Folder");
            Directory.CreateDirectory(directory + "/Temp-Dowload-Folder");
            Directory.CreateDirectory(directory + "/Output-Folder");
        }
        
        public static async Task HandelClean()
        {
            string directory = AppContext.BaseDirectory;

            Directory.Delete(directory + "/Temp-Spliting-Folder");
            Directory.Delete(directory + "/Temp-Dowload-Folder");
        }
    }
}
