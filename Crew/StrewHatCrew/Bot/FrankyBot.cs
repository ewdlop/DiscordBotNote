using Discord.WebSocket;

namespace StrewHatCrew.Bot
{
    public class FrankyBot(CancellationTokenSource? cancellationTokenSource) : Bot(cancellationTokenSource ?? new CancellationTokenSource())
    {
        public override string GetCharacterName() => "フランキー";
        public override string GetCommandPrefix() => "!franky";

        protected readonly string[] _weapons = {
            "フランキーラジカルビーム！",
            "ストロング・ライト！",
            "フレッシュ・ファイアー！"
        };

        public override Task HandleCommandAsync(SocketMessage message, string command, CancellationToken cancellationToken = default)
        {
            switch (command)
            {
                case "super":
                    return Task.Run(()=> message.Channel.SendMessageAsync("スーパーー！"), cancellationToken);
                case "weapon":
                    string weapon = _weapons[_random.Next(_weapons.Length)];
                    return Task.Run(()=> message.Channel.SendMessageAsync(weapon), cancellationToken);
                case "cola":
                    return Task.Run(()=> message.Channel.SendMessageAsync("コーラ補給完了！エネルギー満タン！"), cancellationToken);
                case "help":
                    return Task.Run(()=> message.Channel.SendMessageAsync(
                        "使用可能なコマンド:\n" +
                        "!franky super - スーパーポーズ\n" +
                        "!franky weapon - 武器の使用\n" +
                        "!franky cola - コーラ補給"
                    ), cancellationToken);
                default:
                    return Task.CompletedTask;
            }
        }
    }
}