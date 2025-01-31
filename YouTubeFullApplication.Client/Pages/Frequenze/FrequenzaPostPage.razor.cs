using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Frequenze
{
    public partial class FrequenzaPostPage
    {
        [Inject] private FrequenzeService Service { get; set; } = default!;
        [Inject] private StudentiService StudentiService { get; set; } = default!;
        [Inject] private ClassiService ClassiService { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;

        private FrequenzaPostDto formModel = new();
        private EditContext? editContext = null;
        private ValidationMessageStore? validationMessageStore = null;
        private IEnumerable<ClasseDto> classi = [];
        private StudenteDto? selectedStudente;

        protected override void OnInitialized()
        {
            editContext = new(formModel);
            validationMessageStore = new ValidationMessageStore(editContext);
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadClassiAsync();
            isLoading = false;
        }

        private async Task LoadClassiAsync()
        {
            var ressult = await ClassiService.GetItemsAsync(Token);
            if(ressult.Success)
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
            formModel.Studente_Id = selectedStudente?.Id;
        }

        protected async Task OnSubmitAsync()
        {
            isBusy = true;
            errorMessage = null;
            var result = await Service.PostAsync(formModel, Token);
            if (result.Success)
            {
                Toast.ShowSuccess("Frequenza registrata con successo");
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
