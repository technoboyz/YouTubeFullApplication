using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Docenti
{
    public partial class DocentePostPage
    {
        [Inject] private ApiClientService Service { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        protected DocentePostDto formModel = new();
        protected EditContext? editContext = null;
        protected ValidationMessageStore? validationMessageStore = null;

        protected override void OnInitialized()
        {
            editContext = new(formModel);
            validationMessageStore = new ValidationMessageStore(editContext);
        }

        protected async Task OnSubmitAsync()
        {
            isBusy = true;
            errorMessage = null;
            var result = await Service.PostAsync<DocenteDto>("docenti", formModel, Token);
            if (result.Success)
            {
                Toast.ShowSuccess("Docente registrato con successo");
                Nav.NavigateTo("/Docenti");
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
