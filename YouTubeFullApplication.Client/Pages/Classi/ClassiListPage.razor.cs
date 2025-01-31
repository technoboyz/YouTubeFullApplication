using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Classi
{
    public partial class ClassiListPage
    {
        [CascadingParameter] private IModalService Modal { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private ClassiService Service { get; set; } = default!;
        private ClasseRequestDto request = new();
        private PagedResultDto<ClasseListDto>? content;

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

        private async Task DeleteAsync(ClasseListDto item)
        {
            var parameters = new ModalParameters().Add("Message", $"Eliminare la classe {item.Nome}?");
            var modal = Modal.Show<ConfirmDialog>("Conferma Eliminazione", parameters);
            var modalResult = await modal.Result;
            if (!modalResult.Cancelled)
            {
                var result = await Service.DeleteByIdAsync(item.Id, Token);
                if (result.Success)
                {
                    Toast.ShowSuccess("Classe eliminata con successo");
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
