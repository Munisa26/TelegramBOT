using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("5178091216:AAGkxw4X6jo7Wy2Sm6ZUkPQ3MrHghB1dofw");

using var cts = new CancellationTokenSource();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { } // receive all update types
};
botClient.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
{
    try
    {
        Console.WriteLine($"'{update.Message?.Text}'dedi:{update.Message?.Chat.FirstName}");
        if (update.Type==UpdateType.Message && update.Message?.Type==MessageType.Text)
        {
            if(update.Message?.Text?.ToLower() == "salom")
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id,"Assalomu alekum");
            }
            else if(update.Message?.Text=="Paris")
                {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Fransiyaning poytaxti \n Eng go'zal shaxar Parij");
                KeyboardButton[][] buttons =
                {
                    new KeyboardButton[] {
                        new KeyboardButton("Paris Photo"),
                        new KeyboardButton("Paris Audio") }
                };
                ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup(buttons);
                markup.ResizeKeyboard = true;
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Birini tanlang", replyMarkup: markup);
            }
           else  if (update.Message?.Text == "Paris Photo")
            {
                FileStream fileStream = new(@"C: \Users\hpryz\Downloads\paris - header.jpg", FileMode.Open);
                await client.SendPhotoAsync(update.Message.Chat.Id, fileStream);
              /*  InlineKeyboardButton[][] buttons =
                {
                    new InlineKeyboardButton[] {
                        InlineKeyboardButton.WithCallbackData("Vdyo uchun link","https://youtu.be/UfEiKK-iX70")
                    }
                };
                InlineKeyboardMarkup markup = new InlineKeyboardMarkup(buttons);
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Link", replyMarkup: markup);*/
            }
            else if (update?.Message?.Text == "Paris Audio")
            {
                FileStream fileStream2 = new(@"C:\Users\hpryz\Downloads\Telegram Desktop\Ingratax - Paris (Official Video) - ingratax.m4a", FileMode.Open);
                await client.SendAudioAsync(update.Message.Chat.Id, fileStream2);
            }
            else if (update.Message?.Text=="Moscow")
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Rassiyaning poytaxti Makova");
                FileStream fileStream = new(@"C:C:\Users\hpryz\Downloads", FileMode.Open);
                await client.SendPhotoAsync(update.Message.Chat.Id, fileStream);
            }
            else if (update.Message?.Text=="Venice")
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Italyaning Go'zal shaxarlaridan biri Vinetsiya");
                FileStream fileStream = new(@"C:\Users\hpryz\Downloads\Venice.jpg", FileMode.Open);
                await client.SendPhotoAsync(update.Message.Chat.Id, fileStream);
            }
            else if (update.Message?.Text=="Rome")
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Italyaning Poytaxti \n Tarixiy shaxar Rim");
                FileStream fileStream = new(@"C:\Users\hpryz\Downloads\Rome.jfif", FileMode.Open);
                await client.SendPhotoAsync(update.Message.Chat.Id, fileStream);
            }
            else if (update.Message?.Text == "/start")
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, $"Assalomu aleykum\n Bizning ajoyib sayoxatlar botimizga xush kelibsiz");
                KeyboardButton[][] buttons =
                {
                    new KeyboardButton[] {
                        new KeyboardButton("Paris"),
                        new KeyboardButton("Moskow"),
                    },
                    new KeyboardButton[] {
                        new KeyboardButton("Rome"),
                        new KeyboardButton("Venice")
                    }
                };
                ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup(buttons);
                markup.ResizeKeyboard = true;
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Qayerga Sayoxat qilishni xoxlaysiz???", replyMarkup: markup);
            }
            else
            {
                await client.SendTextMessageAsync(update.Message.Chat.Id, "Sayoxat qilishga marxamat");
            }
        }
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}