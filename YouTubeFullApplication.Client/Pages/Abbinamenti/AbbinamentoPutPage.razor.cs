using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using YouTubeFullApplication.Client.Services;
using YouTubeFullApplication.Dto;

namespace YouTubeFullApplication.Client.Pages.Abbinamenti
{
    public partial class AbbinamentoPutPage
    {
        [Inject] private AbbinamentiService Service { get; set; } = default!;
        [Inject] private DocentiService DocentiService { get; set; } = default!;
        [Inject] private ClassiService ClassiService { get; set; } = default!;
        [Inject] private MaterieService MaterieService { get; set; } = default!;
        [Inject] private IToastService Toast { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        [Parameter] public Guid Id { get; set; }

        private IEnumerable<ClasseDto> classi = [];
        private IEnumerable<MateriaDto> materie = [];
        private IEnumerable<DocenteDto> docenti = [];
        private AbbinamentoPutDto? formModel;
        private EditContext? editContext = null;
        private ValidationMessageStore? validationMessageStore = null;

        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetByIdAsync(Id, Token);
            if (result.Success)
            {
                formModel = new()
                {
                    Id = result.Content!.Id,
                    Classe_Id = result.Content.Classe.Id,
                    Docente_Id = result.Content.Docente.Id,
                    Materia_Id = result.Content.Materia.Id
                };
                editContext = new(formModel);
                validationMessageStore = new(editContext);
                Task[] tasks = [LoadClassiAsync(), LoadMaterieAsync(), LoadDocentiAsync()];
                await Task.WhenAll(tasks);
            }
            else
            {
                errorMessage = result.ErrorMessage;
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
            var result = await Service.PutAsync(formModel!, Token);
            if (result.Success)
            {
                Toast.ShowSuccess("Abbinamento modificato con successo");
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
