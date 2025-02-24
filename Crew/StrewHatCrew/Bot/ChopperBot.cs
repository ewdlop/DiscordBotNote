using Discord.WebSocket;

namespace StrewHatCrew.Bot
{
    public class ChopperBot(CancellationTokenSource? cancellationTokenSource) : Bot(cancellationTokenSource ?? new CancellationTokenSource())
    {
        public override string GetCharacterName() => "チョッパー";
        public override string GetCommandPrefix() => "!chopper";

        protected readonly string[] _treatments = {
            "しっかり休養を取って、この薬を3回に分けて飲んでください。",
            "冷やして安静にしていてください。",
            "包帯を巻き替えましょう。"
        };

        public override Task HandleCommandAsync(SocketMessage message, string command, CancellationToken cancellationToken = default)
        {
            switch (command)
            {
                case "cure":
                    string treatment = _treatments[_random.Next(_treatments.Length)];
                    return Task.Run(()=>message.Channel.SendMessageAsync(treatment), cancellationToken);
                case "transform":
                    return Task.Run(() => message.Channel.SendMessageAsync("ランブルボール！"), cancellationToken);
                case "doctor":
                    return Task.Run(() =>message.Channel.SendMessageAsync("人の命を救うのが医者だ！"), cancellationToken);
                case "help":
                    return Task.Run(() => message.Channel.SendMessageAsync(
                        "使用可能なコマンド:\n" +
                        "!chopper cure - 治療アドバイス\n" +
                        "!chopper transform - 変身\n" +
                        "!chopper doctor - 医者としての信念"
                    ));
                default:
                    return Task.CompletedTask;
            }
        }
    }
}