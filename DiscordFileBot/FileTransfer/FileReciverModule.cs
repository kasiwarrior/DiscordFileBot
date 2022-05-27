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
using System.Net;
using System.Linq;

namespace DiscordFileBot.FileTransfer
{
    class FileReciverModule : ModuleBase<SocketCommandContext>
    {
        private static string _prefix;
        public static async Task InitializeFileReciver(string prefix)
        {
            _prefix = prefix;
        }
        bool downloadCheck = false;
        [Command("DownloadCheck")]
        public async Task HandelDownloadCheck()
        {
            Console.WriteLine("want to dowload files? \ny/n");
            if(Console.ReadLine() == "y")
            {
                downloadCheck = true;
                Context.Channel.SendMessageAsync(_prefix + ":thumbsup:");
            }
            
        }
        public static int counter = 0;
        [Command("Download")]
        public async Task HandelDownload()
        {
            Console.WriteLine("ye");
            //await Context.Channel.SendMessageAsync("te");
            string url = Context.Message.Attachments.First().Url;

            using (var client = new WebClient())
            {
                client.DownloadFile(url, AppContext.BaseDirectory + "Temp-Folder2//" + counter);
            }
            counter++;
            Console.WriteLine("downloaded:  " + url);
        }
    }
}
