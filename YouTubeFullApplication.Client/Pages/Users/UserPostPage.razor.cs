using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Data;
using System.Threading;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Users
{
    public partial class UserPostPage
    {
        [Inject] private UsersService Service { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;

        private UserRegisterRequestDto formModel = new();
        private EditContext? editContext;
        private ValidationMessageStore? validationMessageStore;
        private IEnumerable<string> roles = [];

        protected override void OnInitialized()
        {
            editContext = new(formModel);
            validationMessageStore = new(editContext);
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadRolesAsync();
            isLoading = false;
        }

        private async Task LoadRolesAsync()
        {
            var result = await Service.GetRolesAsync(Token);
            if (result.Success)
            {
                roles = result.Content!;
            }
            else
            {
                errorMessage = result.ErrorMessage;
            }
        }

        private async Task OnSubmitAsync()
        {
            isBusy = true;
            errorMessage = null;
            var result = await Service.PostAsync(formModel, Token);
            if (result.Success)
            {
                Toast.ShowSuccess("User registrato con successo");
                Nav.NavigateTo("/Users");
            }
            else
            {
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    foreach (var error in result.Errors!)
                    {
                        foreach (var message in error.Value)
                        {
                            validationMessageStore!.Add(editContext!.Field(error.Key), message);
                        }
                    }
                    editContext!.NotifyValidationStateChanged();
                    validationMessageStore!.Clear();
                }
                else
                {
                    errorMessage = result.ErrorMessage;
                }
            }
            isBusy = false;
        }
    }
}
