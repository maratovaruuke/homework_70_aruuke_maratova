using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace homework_70_aruuke_maratova
{
    internal class Bot
    {
        private readonly TelegramBotClient _bot;
        private string variant;

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
            var user = callbackQuery.From;
            var userChoice = callbackQuery.Data;
            var chatId = callbackQuery.Message.Chat.Id;

            var botChoice = GetRandomChoice();

            var result = RockPaperScissors(userChoice, botChoice);

            var options = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Повторить", "/game"),
                    InlineKeyboardButton.WithCallbackData("Завершить", "/end")
                 }
             });

            await _bot.SendTextMessageAsync(chatId, $"Вы выбрали: {userChoice}\nБот выбрал: {botChoice}\n{result}", replyMarkup: options);
        }

        private async Task HandleMessage(Message? message)
        {
            var user = message.From;
            var text = message.Text ?? string.Empty;

            if (user is null)
                return;

            if (text.Length > 0)
            {
                if (text.StartsWith("/"))
                {
                    switch(text)
                    {
                        case "/start":
                            text = "Добро пожаловать! Этот бот позволяет сыграть в камень, ножницы, бумага. Используйте команду /game для начала игры и /help для получения более подробной информации.";
                            await _bot.SendTextMessageAsync(user.Id, text);
                            break;
                        case "/help":
                            text = "После выбора команды /game пользователь выбирает один из трех вариантов, камень, ножницы или бумага. Бот генерит рандомный из них и отправляет результаты игры. После завершения игры пользователю предлогается два варианта Завершить или Продолжить.";
                            await _bot.SendTextMessageAsync(user.Id, text);
                            break;
                        case "/game":
                            await StartGameAsync(user.Id);
                            break;
                        case "/end":
                            break;
                    }
                   
                }
                else
                {
                    await _bot.SendTextMessageAsync(user.Id, text);
                }
            }

        }

        private async Task StartGameAsync(long chatId)
        {
            var options = new InlineKeyboardMarkup(new[]
            {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Камень", "rock"),
                InlineKeyboardButton.WithCallbackData("Ножницы", "scissors"),
                InlineKeyboardButton.WithCallbackData("Бумага", "paper")
            }
        });

            await _bot.SendTextMessageAsync(chatId, "Выберите ваш ход:", replyMarkup: options);
        }

        private static string RockPaperScissors(string botChoice, string userChoice)
        {
            return (botChoice, userChoice) switch
            {
                ("rock", "paper") => "Камень укутан бумагой. Бумага побеждает.",
                ("rock", "scissors") => "Камень ломает ножницы. Камень побеждает.",
                ("paper", "rock") => "Бумага покрывает камень. Бумага побеждает.",
                ("paper", "scissors") => "Бумага разрезана ножницами. Ножницы побеждают.",
                ("scissors", "rock") => "Ножницы сломаны камнем. Камень побеждает.",
                ("scissors", "paper") => "Ножницы режут бумагу. Ножницы побеждают.",
                (_, _) => "Ничья"
            };
        }

        private static string GetRandomChoice()
        {
            var choices = new[] { "rock", "scissors", "paper" };
            var randomIndex = new Random().Next(choices.Length);
            return choices[randomIndex];
        }

    }
}
