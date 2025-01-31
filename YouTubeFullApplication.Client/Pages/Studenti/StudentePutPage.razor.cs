using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Studenti
{
    public partial class StudentePutPage : IDisposable
    {
        [Inject] private StudentiService Service { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        [Parameter] public Guid Id { get; set; }
        private CancellationTokenSource cancellationTokenSource = new();
        private bool isLoading = true;
        private bool isBusy = false;
        private string? errorMessage;
        
        private StudentePutDto? formModel;
        private EditContext? editContext;
        private ValidationMessageStore? validationMessageStore;

        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetByIdAsync(Id, cancellationTokenSource.Token);
            if (result.Success)
            {
                formModel = new()
                {
                    Nome = result.Content!.Nome,
                    Cognome = result.Content!.Cognome,
                    CodiceFiscale = result.Content!.CodiceFiscale,
                    DataNascita = result.Content!.DataNascita
                };
                editContext = new(formModel);
                validationMessageStore = new(editContext);
            }
            else
            {
                errorMessage = result.ErrorMessage;
            }
            isLoading = false;
        }

        private async Task OnSubmitAsync()
        {
            errorMessage = null;
            isBusy = true;
            var result = await Service.PutAsync(formModel!, cancellationTokenSource.Token);
            if(result.Success)
            {
                Toast.ShowSuccess("Studente modificato con successo");
                Nav.NavigateTo("Stuidenti");
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

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
