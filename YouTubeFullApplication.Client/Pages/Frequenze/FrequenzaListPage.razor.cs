using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Frequenze
{
    public partial class FrequenzaListPage
    {
        [CascadingParameter] private IModalService Modal { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private FrequenzeService Service { get; set; } = default!;
        private FrequenzaRequestDto request = new();
        private PagedResultDto<FrequenzaListDto>? content;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            isLoading = false;
        }

        private async Task LoadDataAsync()
        {
            errorMessage = null;
            isBusy = true;
            var result = await Service.GetAllAsync(request, Token);
            if(result.Success)
            {
                content = result.Content;
            }
            else
            {
                errorMessage = result.ErrorMessage;
            }
            isBusy = false;
        }

        private async Task PageRequestAsync(PaginationRequest paginationRequest)
        {
            request.Page = paginationRequest.Page;
            request.PageSize = paginationRequest.PageSize;
            await LoadDataAsync();
        }

        private async Task DataRequestAsync()
        {
            request.Page = 1;
            await LoadDataAsync();
        }

        private async Task DeleteAsync(FrequenzaListDto item)
        {
            var parameters = new ModalParameters().Add("Message", $"Eliminare la frequenza selezionata?");
            var modal = Modal.Show<ConfirmDialog>("Conferma Eliminazione", parameters);
            var modalResult = await modal.Result;
            if (!modalResult.Cancelled)
            {
                var result = await Service.DeleteByIdAsync(item.Id, Token);
                if (result.Success)
                {
                    Toast.ShowSuccess("Frequenza eliminata con successo");
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
