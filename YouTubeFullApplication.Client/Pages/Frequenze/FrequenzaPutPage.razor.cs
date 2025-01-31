using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Frequenze
{
    public partial class FrequenzaPutPage
    {
        [Inject] private FrequenzeService Service { get; set; } = default!;
        [Inject] private ClassiService ClassiService { get; set; } = default!;
        [Inject] private StudentiService StudentiService { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        [Parameter] public Guid Id { get; set; }

        private FrequenzaPutDto? formModel;
        private EditContext? editContext;
        private ValidationMessageStore? validationMessageStore;
        private IEnumerable<ClasseDto> classi = [];
        private StudenteDto? selectedStudente;

        protected override async Task OnInitializedAsync()
        {
            await LoadClassiAsync();
            var result = await Service.GetByIdAsync(Id, Token);
            if (result.Success)
            {
                formModel = new()
                {
                    Id = result.Content!.Id,
                    AnnoScolastico = result.Content.AnnoScolastico,
                    Classe_Id = result.Content.Classe.Id,
                    Esito = result.Content.Esito,
                    Studente_Id = result.Content.Studente.Id
                };
                editContext = new(formModel);
                validationMessageStore = new(editContext);
                selectedStudente = result.Content.Studente;
            }
            isLoading = false;
        }

        private async Task LoadClassiAsync()
        {
            var ressult = await ClassiService.GetItemsAsync(Token);
            if (ressult.Success)
            {
                classi = ressult.Content!;
            }
            else
            {
                errorMessage = ressult.ErrorMessage;
            }
        }

        private async Task<IEnumerable<StudenteDto>> StudentiSuggestAsync(string text)
        {
            errorMessage = null;
            var result = await StudentiService.SuggestAsync(text, Token);
            if (result.Success)
            {
                return result.Content!;
            }
            else
            {
                errorMessage = result.ErrorMessage;
                return [];
            }
        }

        private void StudenteChanged(StudenteDto? studente)
        {
            selectedStudente = studente;
            formModel!.Studente_Id = selectedStudente?.Id;
        }

        protected async Task OnSubmitAsync()
        {
            isBusy = true;
            errorMessage = null;
            var result = await Service.PutAsync(formModel!, Token);
            if (result.Success)
            {
                Toast.ShowSuccess("Frequenza modificata con successo");
                Nav.NavigateTo("/Frequenze");
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
