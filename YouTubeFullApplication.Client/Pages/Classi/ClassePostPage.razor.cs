using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Classi
{
    public partial class ClassePostPage
    {
        [Inject] private ClassiService Service { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;

        private ClassePostDto formModel = new();
        private EditContext? editContext;
        private ValidationMessageStore? validationMessageStore;

        protected override void OnInitialized()
        {
            editContext = new(formModel);
            validationMessageStore = new(editContext);
        }

        protected async Task OnSubmitAsync()
        {
            isBusy = true;
            errorMessage = null;
            var result = await Service.PostAsync(formModel, Token);
            if (result.Success)
            {
                Toast.ShowSuccess("Classe registrata con successo");
                Nav.NavigateTo("/Classi");
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
