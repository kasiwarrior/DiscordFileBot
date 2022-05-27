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
using System.IO;

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

        private static int fileNum;
        [Command("FileAmount")]
        public async Task HandelFileAmount([Remainder] string sNum)
        {
            bool parseSucces;
            parseSucces = Int32.TryParse(sNum, out fileNum);
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
                client.DownloadFile(url, AppContext.BaseDirectory + "Temp-Dowload-Folder//" + counter);
            }
            counter++;
            Console.WriteLine("downloaded: " + url);
        }
        [Command("Combine")]
        public async Task HandelCombine([Remainder] string filename)
        {
            Console.WriteLine(fileNum);
            Thread.Sleep(2000);

            Stream sInput;
            Stream sOutput;
            byte[] buffer;
            int chunkSize;
            int chunks = 28;
            int index = 0;
            string path = AppContext.BaseDirectory + "Temp-Dowload-Folder//";
            string pathOut = AppContext.BaseDirectory + "Output-Folder//";


            sOutput = File.Create(pathOut + filename); 


            while (index < chunks)
            {
                Console.WriteLine("index: " + index);
                sInput = File.OpenRead(path + index);
                chunkSize = (int)sInput.Length;
                Console.WriteLine(sInput.Length + " sInput");
                int chunkBytesRead = 0;
                buffer = new byte[sInput.Length];
                while (chunkBytesRead < chunkSize)
                {
                    int bytesRead = sInput.Read(buffer, 0, chunkSize - chunkBytesRead);
                    Console.WriteLine(bytesRead);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    chunkBytesRead += bytesRead;
                }
                sOutput.Write(buffer, 0, chunkBytesRead);
                index++;
            }
            sOutput.Close();

            Thread.Sleep(1000);
            Console.WriteLine("combined");
            //DiscordFileBot.FileTransfer.CommonModule.HandelClean();
            Console.WriteLine("Done");
        }
    }
}
