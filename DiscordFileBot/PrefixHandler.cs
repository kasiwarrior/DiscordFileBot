using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;
using Discord.Commands;
using Microsoft.Extensions.Configuration;

namespace DiscordFileBot
{
    public class PrefixHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        public PrefixHandler(DiscordSocketClient client, CommandService commands, IConfigurationRoot config)
        {
            _client = client;
            _commands = commands;
            _config = config;
        }

        public async Task InitializeAsync()
        {
            _client.MessageReceived += HandelCommandAsync;
        }

        public void AddModuel<T>()
        {
            _commands.AddModuleAsync<T>(null);
        }

        private async Task HandelCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (!(message.HasCharPrefix(_config["prefix"][0], ref argPos) || !message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);

        }
    }
}
