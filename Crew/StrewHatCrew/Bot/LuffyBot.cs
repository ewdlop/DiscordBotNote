using Discord.WebSocket;

namespace StrewHatCrew.Bot
{
    public class LuffyBot(CancellationTokenSource? cancellationTokenSource) : Bot(cancellationTokenSource ?? new CancellationTokenSource())
    {
        public override string GetCharacterName() => "ルフィ";
        public override string GetCommandPrefix() => "!luffy";

        public override Task HandleCommandAsync(SocketMessage message, string command, CancellationToken cancellationToken = default) => command switch
        {
            "gum" => Task.Run(() => message.Channel.SendMessageAsync("ゴムゴムのー！"), cancellationToken),
            "meat" => Task.Run(() => message.Channel.SendMessageAsync("肉ーーー！！"), cancellationToken),
            "adventure" => Task.Run(() => message.Channel.SendMessageAsync("冒険に行くぞー！もっと面白いところに！"), cancellationToken),
            "help" => Task.Run(() => message.Channel.SendMessageAsync(
                                    "使用可能なコマンド:\n" +
                                    "!luffy gum - ゴムゴムの実の能力を使用\n" +
                                    "!luffy meat - 肉への愛を表現\n" +
                                    "!luffy adventure - 冒険への誘い"
                                ), cancellationToken),
            _ => Task.CompletedTask,
        };
    }
}