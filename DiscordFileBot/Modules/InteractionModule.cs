using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordFileBot.Modules
{
    public class InteractionModule :InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Receive a ping message!")]
        public async Task HandlePingCommand()
        {
            await RespondAsync("Ping!");
        }
    }
}
