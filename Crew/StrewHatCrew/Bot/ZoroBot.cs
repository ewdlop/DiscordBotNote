using Discord.WebSocket;

namespace StrewHatCrew.Bot
{
    public class ZoroBot(CancellationTokenSource? cancellationTokenSource) : Bot(cancellationTokenSource)
    {
        public override string GetCharacterName() => "ゾロ";
        public override string GetCommandPrefix() => "!zoro";

        protected readonly string[] _directions = { "北", "南", "東", "西" };

        public override Task HandleCommandAsync(SocketMessage message, string command, CancellationToken cancellationToken = default)
        {
            switch (command)
            {
                case "direction":
                    string wrongDirection = _directions[_random.Next(_directions.Length)];
                    return Task.Run(()=> message.Channel.SendMessageAsync($"ん？{wrongDirection}はあっちじゃないのか？"), cancellationToken);
                case "training":
                    return Task.Run(()=> message.Channel.SendMessageAsync("修行あるのみ。2000回の素振りだ。"), cancellationToken);
                case "santoryu":
                    return Task.Run(()=> message.Channel.SendMessageAsync("三刀流奥義 - 鬼気分人"), cancellationToken);
                case "help":
                    return Task.Run(()=> message.Channel.SendMessageAsync(
                        "使用可能なコマンド:\n" +
                        "!zoro direction - 方向音痴な案内\n" +
                        "!zoro training - 修行メニュー\n" +
                        "!zoro santoryu - 三刀流の技を披露"
                    ), cancellationToken);
                default:
                    return Task.CompletedTask;
            }
        }
    }
}