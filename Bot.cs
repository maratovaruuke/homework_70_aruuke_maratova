using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace homework_70_aruuke_maratova
{
    internal class Bot
    {
        private readonly TelegramBotClient _bot;

        public Bot(string token)
        {
            _bot = new TelegramBotClient(token);
        }

        public void StartBot()
        {
            _bot.StartReceiving(HandleUpdate, HandleError);
            while (true)
            {
                Console.WriteLine("Bot is worked alright");
                Thread.Sleep(int.MaxValue);
            }
        }

        private Task HandleError(ITelegramBotClient client, Exception ex, CancellationToken token)
        {
            throw new Exception();
        }

        private async Task HandleUpdate(ITelegramBotClient client, Update update, CancellationToken token)
        {
            switch (update.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.Message:
                    await HandleMessage(update.Message);
                    break;
                case Telegram.Bot.Types.Enums.UpdateType.CallbackQuery:
                    await HandleQuery(update.CallbackQuery);
                    break;
            }
        }

        private async Task HandleQuery(CallbackQuery? callbackQuery)
        {
            throw new NotImplementedException();
        }

        private async Task HandleMessage(Message? message)
        {
            var user = message.From;
            var text = message.Text ?? string.Empty;

        }

    }
}
