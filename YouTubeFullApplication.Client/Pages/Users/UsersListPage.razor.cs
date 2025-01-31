using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Threading;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Users
{
    public partial class UsersListPage
    {
        [CascadingParameter] private IModalService Modal { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private UsersService Service { get; set; } = default!;

        private UserRequestDto request = new();
        private PagedResultDto<UserListDto>? content;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            isLoading = false;
        }

        private async Task LoadDataAsync()
        {
            var result = await Service.GetAllAsync(request, Token);
            if (result.Success)
            {
                content = result.Content;
            }
            else
            {
                errorMessage = result.ErrorMessage;
            }
        }

        private async Task PageRequestAsync(PaginationRequest paginationRequest)
        {
            request.Page = paginationRequest.Page;
            request.PageSize = paginationRequest.PageSize;
            await LoadDataAsync();
        }

        private async Task DeleteAsync(UserListDto item)
        {
            var parameters = new ModalParameters().Add("Message", $"Eliminare l'utente' con Email {item.Email}?");
            var modal = Modal.Show<ConfirmDialog>("Conferma Eliminazione", parameters);
            var modalResult = await modal.Result;
            if (!modalResult.Cancelled)
            {
                var result = await Service.DeleteByIdAsync(item.Id, Token);
                if (result.Success)
                {
                    Toast.ShowSuccess("User eliminato con successo");
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
