using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Materie
{
    public partial class MateriaPostPage
    {
        [Inject] private MaterieService Service { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;

        private MateriaPostDto formModel = new();
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
                Toast.ShowSuccess("Materia registrata con successo");
                Nav.NavigateTo("/Materie");
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
