using Microsoft.AspNetCore.Components;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Abbinamenti
{
    public partial class AbbinamentoDetailsPage
    {
        [Inject] private AbbinamentiService Service { get; set; } = default!;
        [Parameter] public Guid Id { get; set; }
        private AbbinamentoDetailsDto? model;

        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetDetailsByIdAsync(Id, Token);
            if (result.Success)
            {
                model = result.Content;
            }
            else
            {
                errorMessage = result.ErrorMessage;
            }
            isLoading = false;
        }
    }
}
