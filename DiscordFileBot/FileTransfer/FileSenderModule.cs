using System;
using System.Threading.Tasks;
using System.Threading;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.Yaml;
using Discord.Commands;
using System.IO;


namespace DiscordFileBot.FileTransfer
{
    class FileSenderModule : ModuleBase<SocketCommandContext>
    {
        bool downloadCheck = false;
        private static string _prefix;

        public static async Task InitializeFileSender(string prefix)
        {
            _prefix = prefix;
        }



        //just a test command
        [Command("test")]
        public async Task HandelTest()
        {
            //Console.WriteLine("ye");
            //Context.Channel.SendMessageAsync(_prefix);
            //await Context.Channel.SendFileAsync("C:/Users/Isak/source/repos/DiscordFileBot/DiscordFileBot/bin/Debug/net5.0/Temp-Folder/0", "test");
            //await Context.Channel.SendFileAsync("C:/Users/Isak/source/repos/DiscordFileBot/DiscordFileBot/bin/Debug/net5.0/Temp-Folder/0", "test");
            /*Console.WriteLine("1");
            string[] inputFiles = Directory.GetFiles(AppContext.BaseDirectory + "Temp-Folder");
            string[] files = new string[Directory.GetFiles(AppContext.BaseDirectory + "Temp-Folder").Length];
            Console.WriteLine("2");

            bool succes;
            int counter = 0;
            
            foreach (string s in inputFiles)
            {
                string temp = inputFiles[counter];
                Console.WriteLine("temp: " + temp);
                string subtract = AppContext.BaseDirectory + @"Temp-Folder\";
                string final = temp.Replace(subtract, "");
                Console.WriteLine(final);
                succes = Int32.TryParse(final, out int indexNum);
                if (!succes)
                {
                    Console.WriteLine("reeeeeeeeeeeeeeeeeeeeeeeeee");
                    return;
                }
                files[indexNum] = s;
                counter++;
            }
            foreach(string s in inputFiles)
            {
                Console.WriteLine(s);
            }
            foreach(string s in files)
            {
                Console.WriteLine(s);
            }
            */
            /*
            string[] temp2 = new string[temp.Split("Temp-Folder").Length];
            Console.WriteLine("4");
            temp2 = temp.Split('/');
            Console.WriteLine("5");
            Console.WriteLine("yee " + temp2[temp2.Length - 1]);
            string numberString = temp2[1];
            Console.WriteLine(numberString);
            */
            //await FileSplitter();
            //await Context.Channel.SendFileAsync("C:/Users/Isak/source/repos/DiscordFileBot/DiscordFileBot/bin/Debug/net5.0/Temp-Folder/0");
            Context.Channel.SendMessageAsync("Test Done");
        }
        [Command("Send")]
        public async Task HandelConnect() // [Remainder] string filename
        {
            string filename;
            filename = Directory.GetFiles(AppContext.BaseDirectory + "Input-Folder")[0];
            Console.WriteLine("Filename: " + filename);
            
            await Context.Channel.SendMessageAsync(_prefix +"DownloadCheck");
        }

  

        [Command("StartUppload")]
        [Alias(":thumbsup:")]
        public async Task HandelUpplod()
        {
            if (!Context.User.IsBot) return;

            await Context.Channel.SendMessageAsync("Starting...");
            Thread.Sleep(1000);
            await Context.Channel.SendMessageAsync("Splitting");
            await FileSplitter();
            long bytesNum = Directory.GetFiles(AppContext.BaseDirectory + "Temp-Folder").Length;

            await Context.Channel.SendMessageAsync("Upploading: " + bytesNum + " files");
            await FileUpploder();
        }
        private async Task FileSplitter()
        {
            string filepath = Directory.GetFiles(AppContext.BaseDirectory + "Input-Folder")[0];
            Console.WriteLine("splitting");
            Console.WriteLine("third filename: " + filepath);
            string basePath = AppContext.BaseDirectory;
            string tempDir = "Temp-Folder";
            
            long bytes = 0;
            long byteChunks = 0;
            int chunkSize = 6000000;
            Stream sOutput;
            Stream sInput = File.OpenRead(filepath);
            Console.WriteLine("loadedFile");
            Console.WriteLine(sInput.Length / 1000000 + "MB");
            bytes = sInput.Length;
            byteChunks = bytes / 6000000;
            
            Console.WriteLine(byteChunks + " chunks");


            byte[] buffer = new byte[chunkSize];
            int index = 0;
            while (sInput.Position < sInput.Length)
            {
                sOutput = File.Create(basePath + tempDir + "//" + index);

                int chunkBytesRead = 0;
                while (chunkBytesRead < chunkSize)
                {
                    int bytesRead = sInput.Read(buffer, chunkBytesRead, chunkSize - chunkBytesRead);

                    if (bytesRead == 0)
                    {
                        break;
                    }
                    chunkBytesRead += bytesRead;
                }
                sOutput.Write(buffer, 0, chunkBytesRead);
                sOutput.Close();
                index++;
            }
            

        }
        private async Task FileUpploder()
        {

            string[] inputFiles = Directory.GetFiles(AppContext.BaseDirectory + "Temp-Folder");
            string[] files = new string[Directory.GetFiles(AppContext.BaseDirectory + "Temp-Folder").Length];
            Console.WriteLine("2");

            bool succes;
            int counter = 0;

            foreach (string s in inputFiles)
            {
                string temp = inputFiles[counter];
                Console.WriteLine("temp: " + temp);
                string subtract = AppContext.BaseDirectory + @"Temp-Folder\";
                string final = temp.Replace(subtract, "");
                Console.WriteLine(final);
                succes = Int32.TryParse(final, out int indexNum);
                if (!succes)
                {
                    Console.WriteLine("reeeeeeeeeeeeeeeeeeeeeeeeee");
                    return;
                }
                files[indexNum] = s;
                counter++;
            }


            counter = 0;

            foreach (string s in files)
            {
               
                await Context.Channel.SendFileAsync(s,_prefix + "download");
                await Context.Channel.SendMessageAsync(counter.ToString());
                counter++;
            }
            
            
        }
    }
}
