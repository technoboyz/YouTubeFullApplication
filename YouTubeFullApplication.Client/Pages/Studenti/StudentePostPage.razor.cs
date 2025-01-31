using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Studenti
{
    public class StudentePostPageBase : ComponentBase, IDisposable
    {
        [Inject] private StudentiService Service { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        protected bool isBusy = false;
        protected StudentePostDto formModel = new();
        protected EditContext? editContext = null;
        protected ValidationMessageStore? validationMessageStore = null;
        private CancellationTokenSource cancellationTokenSource = new();
        protected string? errorMessage;

        protected override void OnInitialized()
        {
            editContext = new(formModel);
            validationMessageStore = new ValidationMessageStore(editContext);
        }

        protected async Task OnSubmitAsync()
        {
            isBusy = true;
            errorMessage = null;
            var result = await Service.PostAsync(formModel, cancellationTokenSource.Token);
            if (result.Success)
            {
                //formModel.Nome = string.Empty;
                //formModel.Cognome = string.Empty;
                //formModel.CodiceFiscale = string.Empty;
                //formModel.DataNascita = default!;
                Toast.ShowSuccess("Studente registrato con successo");
                Nav.NavigateTo("/Studenti");
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

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
