using Microsoft.Extensions.Configuration;

namespace StrewHatCrew
{
    // メインプログラム
    public partial class Program : IDisposable, IAsyncDisposable
    {
        protected static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        protected bool _disposed = false;

        public static async Task Main(string[] args)
        {
            // 設定ファイルからトークンを読み込む
            IConfigurationRoot? config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            try
            {

               await using StrawHatCrewBot bot = new StrawHatCrewBot(config, _cancellationTokenSource);


               Console.CancelKeyPress += (sender, e) => {
                   Console.WriteLine("Shutdown signal received. Stopping bot gracefully...");
                   _cancellationTokenSource.Cancel();
                   e.Cancel = true; // Prevent immediate termination
               };

                await bot.StartAsync(config["Token"], _cancellationTokenSource.Token);

                Console.WriteLine("Bot started successfully! Press Ctrl+C to stop.");

                // Ctrl+Cで終了するまで実行を継続
                await Task.Delay(Timeout.InfiniteTimeSpan, _cancellationTokenSource.Token);
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine(ex);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("Bot shutdown complete.");
            }
        }

        protected virtual void Dispose(bool dispose)
        {
            if (!_disposed)
            {
                if (dispose)
                {
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource.Dispose();
                }
                Interlocked.Exchange(ref _disposed, true);
            }
        }

        protected async virtual ValueTask DisposeAsync(bool dispose)
        {
            if (!_disposed)
            {
                if (dispose)
                {
                    await Task.Run(_cancellationTokenSource.CancelAsync, _cancellationTokenSource.Token);
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource.Dispose();
                }
                Interlocked.Exchange(ref _disposed, true);
            }
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        public ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            return DisposeAsync(true);
        }
    }
}