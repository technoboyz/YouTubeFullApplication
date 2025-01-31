using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Docenti
{
    public partial class DocentiListPage
    {
        [CascadingParameter] private IModalService Modal { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private ApiClientService Service { get; set; } = default!;
        private DocenteRequestDto request = new();
        private PagedResultDto<DocenteListDto>? content;
        private CancellationTokenSource cancellationTokenSource = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            isLoading = false;
        }

        private async Task LoadDataAsync()
        {
            isBusy = true;
            errorMessage = null;
            var result = await Service.GetAllAsync<DocenteListDto>("Docenti", request, cancellationTokenSource.Token);
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

        private async Task DataRequestAsync()
        {
            request.Page = 1;
            await LoadDataAsync();
        }

        private async Task PageRequestAsync(PaginationRequest paginationRequest)
        {
            request.Page = paginationRequest.Page;
            request.PageSize = paginationRequest.PageSize;
            await LoadDataAsync();
        }

        private async Task DeleteAsync(DocenteListDto item)
        {
            var parameters = new ModalParameters().Add("Message", $"Eliminare il docente con Codice Fiscale {item.CodiceFiscale}?");
            var modal = Modal.Show<ConfirmDialog>("Conferma Eliminazione", parameters);
            var modalResult = await modal.Result;
            if (!modalResult.Cancelled)
            {
                var result = await Service.DeleteByIdAsync<DocenteDto>("docenti", item.Id, cancellationTokenSource.Token);
                if (result.Success)
                {
                    Toast.ShowSuccess("Docente eliminato con successo");
                    request.Page = 1;
                    await LoadDataAsync();
                }
                else
                {
                    errorMessage = result.Errors?.First().Value.First();
                }
            }
        }

        private async Task UndeleteAsync(Guid id)
        {
            errorMessage = null;
            var result = await Service.UndeleteByIdAsync("docenti", id, cancellationTokenSource.Token);
            if (result.Success)
            {
                request.Page = 1;
                Toast.ShowSuccess("Elemento correttamente ripristinato.");
                await LoadDataAsync();
            }
            else
            {
                errorMessage = result?.Errors?.First().Value.First();
            }
        }

        private async Task ListSwitchAsync(bool value)
        {
            isBusy = true;
            request.RetriveDeleted = value;
            request.Page = 1;
            await LoadDataAsync();
            isBusy = false;
        }
    }
}
