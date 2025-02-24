using Discord.WebSocket;

namespace StrewHatCrew.Bot
{

    public class BrookBot(CancellationTokenSource? cancellationTokenSource) : Bot(cancellationTokenSource ?? new CancellationTokenSource())
    {
        public override string GetCharacterName() => "ブルック";
        public override string GetCommandPrefix() => "!brook";

        protected readonly string[] _songs = [
            "ビンクスの酒",
            "ソウルキングの歌",
            "新世界の歌"
        ];

        public override Task HandleCommandAsync(SocketMessage message, string command, CancellationToken cancellationToken = default)
        {
            switch (command)
            {
                case "song":
                    string song = _songs[_random.Next(_songs.Length)];
                    return Task.Run(() => message.Channel.SendMessageAsync($"ヨホホホ！{song}を演奏しましょう！"), cancellationToken);
                case "skull":
                    return Task.Run(() => message.Channel.SendMessageAsync("失礼、パンツを見せてもらえませんか？...あ、私には目がないからスカルジョークですよ！ヨホホホ！"), cancellationToken);
                case "soul":
                    return Task.Run(() => message.Channel.SendMessageAsync("魂を震わせる音楽の力！"), cancellationToken);
                case "help":
                    return Task.Run(() => message.Channel.SendMessageAsync(
                        "使用可能なコマンド:\n" +
                        "!brook song - 音楽の演奏\n" +
                        "!brook skull - スカルジョーク\n" +
                        "!brook soul - 魂の力"
                    ), cancellationToken);
                default:
                    return Task.CompletedTask;
            }
        }
    }
}