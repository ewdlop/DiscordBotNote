using Discord.WebSocket;

namespace StrewHatCrew.Bot
{
    public class SanjiBot(CancellationTokenSource? cancellationTokenSource) : Bot(cancellationTokenSource ?? new CancellationTokenSource())
    {
        public override string GetCharacterName() => "サンジ";
        public override string GetCommandPrefix() => "!sanji";

        protected readonly string[] _recipes = {
            "特製シーフードパスタ",
            "海の宝石スープ",
            "至高のビーフステーキ"
        };

        public override Task HandleCommandAsync(SocketMessage message, string command, CancellationToken cancellationToken = default)
        {
            switch (command)
            {
                case "cook":
                    string recipe = _recipes[_random.Next(_recipes.Length)];
                    return Task.Run(()=>message.Channel.SendMessageAsync($"今日の料理は{recipe}だ。"), cancellationToken);
                case "ladies":
                    return Task.Run(()=>message.Channel.SendMessageAsync("メロリーン、レディたち！"), cancellationToken);
                case "diablojambe":
                    return Task.Run(()=>message.Channel.SendMessageAsync("悪魔風脚！"), cancellationToken);
                case "help":
                    return Task.Run(()=>message.Channel.SendMessageAsync(
                        "使用可能なコマンド:\n" +
                        "!sanji cook - 料理メニューの提案\n" +
                        "!sanji ladies - レディへの挨拶\n" +
                        "!sanji diablojambe - 戦闘技の使用"
                    ), cancellationToken);
                default:
                    return Task.CompletedTask;
            }
        }
    }
}