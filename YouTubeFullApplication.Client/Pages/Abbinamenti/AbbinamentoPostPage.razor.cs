using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Abbinamenti
{
    public partial class AbbinamentoPostPage
    {
        [Inject] private AbbinamentiService Service { get; set; } = default!;
        [Inject] private DocentiService DocentiService { get; set; } = default!;
        [Inject] private ClassiService ClassiService { get; set; } = default!;
        [Inject] private MaterieService MaterieService { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;

        private IEnumerable<ClasseDto> classi = [];
        private IEnumerable<MateriaDto> materie = [];
        private IEnumerable<DocenteDto> docenti = [];
        private AbbinamentoPostDto formModel = new();
        private EditContext? editContext = null;
        private ValidationMessageStore? validationMessageStore = null;

        protected override void OnInitialized()
        {
            editContext = new(formModel);
            validationMessageStore = new(editContext);
        }

        protected override async Task OnInitializedAsync()
        {
            Task[] tasks = [ LoadClassiAsync(), LoadMaterieAsync(), LoadDocentiAsync() ];
            await Task.WhenAll(tasks);
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

        private async Task LoadMaterieAsync()
        {
            var ressult = await MaterieService.GetItemsAsync(Token);
            if (ressult.Success)
            {
                materie = ressult.Content!;
            }
            else
            {
                errorMessage = ressult.ErrorMessage;
            }
        }

        private async Task LoadDocentiAsync()
        {
            var ressult = await DocentiService.GetItemsAsync(Token);
            if (ressult.Success)
            {
                docenti = ressult.Content!;
            }
            else
            {
                errorMessage = ressult.ErrorMessage;
            }
        }

        protected async Task OnSubmitAsync()
        {
            isBusy = true;
            errorMessage = null;
            var result = await Service.PostAsync(formModel, Token);
            if (result.Success)
            {
                Toast.ShowSuccess("Abbinamento registrato con successo");
                Nav.NavigateTo("/Abbinamenti");
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
