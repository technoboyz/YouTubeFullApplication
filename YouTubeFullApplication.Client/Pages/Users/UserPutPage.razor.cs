using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Users
{
    public partial class UserPutPage
    {
        [Inject] private UsersService Service { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        [Parameter] public Guid Id { get; set; }
        private UserPutDto? formModel;
        private EditContext? editContext;
        private ValidationMessageStore? validationMessageStore;
        private IEnumerable<string> roles = [];

        protected override async Task OnInitializedAsync()
        {
            Task[] tasks = { LoadDataAsync(), LoadRolesAsync() }; 
            await Task.WhenAll(tasks);
            isLoading = false;
        }

        private async Task LoadDataAsync()
        {
            var result = await Service.GetByIdAsync(Id, Token);
            if (result.Success)
            {
                formModel = new()
                {
                    Id = result.Content!.Id,
                    Nome = result.Content.Nome,
                    Cognome = result.Content.Cognome,
                    Role = result.Content.Role
                };
                editContext = new EditContext(formModel);
                validationMessageStore = new ValidationMessageStore(editContext);
            }
            else
            {
                errorMessage = result.ErrorMessage;
            }
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
            errorMessage = null;
            isBusy = true;
            var result = await Service.PutAsync(formModel!, Token);
            if (result.Success)
            {
                Toast.ShowSuccess("User modificato con successo");
                Nav.NavigateTo("Users");
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
        }
    }
}
