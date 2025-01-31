using Microsoft.AspNetCore.Components;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Docenti
{
    public partial class DocenteDetailsPage
    {
        [Inject] private ApiClientService Service { get; set; } = default!;
        [Parameter] public Guid Id { get; set; }
        private DocenteDetailsDto? model;

        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetDetailsByIdAsync<DocenteDetailsDto>("docenti", Id, Token);
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
