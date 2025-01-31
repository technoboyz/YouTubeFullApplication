using Microsoft.AspNetCore.Components;

namespace YouTubeFullApplication.Client.Pages
{
    public abstract class PageBase : ComponentBase, IDisposable
    {
        private CancellationTokenSource cancellationTokenSource = new();
        protected string? errorMessage;
        protected bool isLoading = true;
        protected bool isBusy = false;

        protected CancellationToken Token => cancellationTokenSource.Token;

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
