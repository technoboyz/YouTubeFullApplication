using Blazored.Modal.Services;
using Blazored.Modal;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Materie
{
    public partial class MaterieListPage
    {
        [CascadingParameter] private IModalService Modal { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private MaterieService Service { get; set; } = default!;
        private MateriaRequestDto request = new();
        private PagedResultDto<MateriaListDto>? content;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            isLoading = false;
        }

        private async Task LoadDataAsync()
        {
            isBusy = true;
            errorMessage = null;
            var result = await Service.GetAllAsync(request, Token);
            if (result.Success)
            {
                content = result.Content;
            }
            else
            {
                errorMessage = result.Errors!.First().Value.First();
            }
            isBusy = false;
        }

        private async Task PageRequestAsync(PaginationRequest paginationRequest)
        {
            request.Page = paginationRequest.Page;
            request.PageSize = paginationRequest.PageSize;
            await LoadDataAsync();
        }

        private async Task DeleteAsync(MateriaListDto item)
        {
            var parameters = new ModalParameters().Add("Message", $"Eliminare la materia {item.Nome}?");
            var modal = Modal.Show<ConfirmDialog>("Conferma Eliminazione", parameters);
            var modalResult = await modal.Result;
            if (!modalResult.Cancelled)
            {
                var result = await Service.DeleteByIdAsync(item.Id, Token);
                if (result.Success)
                {
                    Toast.ShowSuccess("Materia eliminata con successo");
                    request.Page = 1;
                    await LoadDataAsync();
                }
                else
                {
                    errorMessage = result.Errors?.First().Value.First();
                }
            }
        }
    }
}
