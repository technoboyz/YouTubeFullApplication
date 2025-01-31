using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Docenti
{
    public partial class DocentePutPage
    {
        [Inject] private ApiClientService Service { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        [Parameter] public Guid Id { get; set; }

        private DocentePutDto? formModel;
        private EditContext? editContext;
        private ValidationMessageStore? validationMessageStore;

        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetByIdAsync<DocenteDto>("docenti", Id, Token);
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
            var result = await Service.PutAsync("docenti", formModel!, Token);
            if (result.Success)
            {
                Toast.ShowSuccess("Docente modificato con successo");
                Nav.NavigateTo("Docenti");
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
