using Discord.WebSocket;

namespace StrewHatCrew.Bot
{
    public class UsoppBot(CancellationTokenSource? cancellationTokenSource) : Bot(cancellationTokenSource ?? new CancellationTokenSource())
    {
        public override string GetCharacterName() => "ウソップ";
        public override string GetCommandPrefix() => "!usopp";

        protected readonly string[] _lies = {
            "俺様は8000人の部下を持つ大海賊だ！",
            "昨日は巨大なゴールデンキングフィッシュと戦ったんだ！",
            "実は俺、伝説の狙撃手の息子なんだぜ！"
        };

        public override Task HandleCommandAsync(SocketMessage message, string command, CancellationToken cancellationToken = default)
        {
            switch (command)
            {
                case "lie":
                    string lie = _lies[_random.Next(_lies.Length)];
                    return Task.Run(() => message.Channel.SendMessageAsync(lie), cancellationToken);
                case "kabuto":
                    return Task.Run(()=> message.Channel.SendMessageAsync("必殺・緑星！"), cancellationToken);
                case "sogeking":
                    return Task.Run(()=> message.Channel.SendMessageAsync(
                        "そ～げ～キングのぉ～島へ～よ～うこそ～♪"
                    ), cancellationToken);
                case "help":
                    return Task.Run(()=> message.Channel.SendMessageAsync(
                        "使用可能なコマンド:\n" +
                        "!usopp lie - ウソップの冒険話\n" +
                        "!usopp kabuto - 必殺技の使用\n" +
                        "!usopp sogeking - そげキングの歌"
                    ), cancellationToken);
                default:
                    return Task.CompletedTask;
            }
        }
    }
}