using Microsoft.AspNetCore.Components;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Studenti
{
    public partial class StudenteDetailsPage : IDisposable
    {
        [Inject] private StudentiService Service { get; set; } = default!;
        [Parameter] public Guid Id { get; set; }
        private CancellationTokenSource cancellationTokenSource = new();
        private bool isLoading = true;
        private string? errorMessage;
        private StudenteDetailsDto? model;

        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetDetailsByIdAsync(Id, cancellationTokenSource.Token);
            if(result.Success)
            {
                model = result.Content;
            }
            else
            {
                errorMessage = result.ErrorMessage;
            }
            isLoading = false;
        }


        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
