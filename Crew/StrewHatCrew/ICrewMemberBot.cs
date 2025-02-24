using Discord.WebSocket;

namespace StrewHatCrew
{
    public interface ICrewMemberBot : IDisposable, IAsyncDisposable
    {
        abstract Task HandleCommandAsync(SocketMessage message, string command, CancellationToken cancellationToken = default);
        abstract string GetCharacterName();
        abstract string GetCommandPrefix();
    }
}