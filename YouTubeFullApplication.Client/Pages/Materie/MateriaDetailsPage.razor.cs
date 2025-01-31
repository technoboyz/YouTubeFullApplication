using Microsoft.AspNetCore.Components;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Materie
{
    public partial class MateriaDetailsPage
    {
        [Inject] private MaterieService Service { get; set; } = default!;
        [Parameter] public Guid Id { get; set; }
        private MateriaDetailsDto? model;

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
