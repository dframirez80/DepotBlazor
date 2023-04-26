using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using MudBlazor;
using DepotBlazor.Client.Services;
using Domain.Entities;
using Domain.Models;
using System.Net;
using System.Net.Http.Headers;

namespace DepotBlazor.Client.Pages
{
    public partial class DepotABM
    {
        [Inject]
        NavigationManager navigation { get; set; }

        [Inject]
        IDialogService dialogService { get; set; }

        [Inject]
        ITokenService tokenService { get; set; }

        [Inject]
        HttpClient httpClient { get; set; }

        List<DepotDto> depots = new();
        bool showPage, connecting;
        bool addEntity, putEntity;
        DepotDto createEntity = new();
        DepotDto updateEntity = new();
        string token = string.Empty;
        public int QRecordPerPage { get; set; } = 10;
        private string searchString1 = string.Empty;
        private DepotDto selectedItem1 = null;
        private HashSet<DepotDto> selectedItems = new HashSet<DepotDto>();
        public string Estado = "";
        private string infoFormat = "{first_item}-{last_item} de {all_items}";
        protected override async Task OnInitializedAsync()
        {
            bool isvalid = await tokenService.IsTokenValid();
            if (!isvalid)
            {
                navigation.NavigateTo("/loginaccount");
                return;
            }

            token = await tokenService.Get();
            depots = await GetAll();
            showPage = true;
        }

        #region filtro de busqueda de tabla
        private bool FilterFunc1(DepotDto element) => FilterFunc(element, searchString1);
        private bool FilterFunc(DepotDto element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            string compareString = element.Code.ToLower();
            if (compareString.Contains(searchString.ToLower(), StringComparison.OrdinalIgnoreCase))
                return true;
            compareString = element.Name.ToLower();
            if (compareString.Contains(searchString.ToLower(), StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        #endregion

        async Task Logout()
        {
            await tokenService.Clear();
            navigation.NavigateTo("/LoginAccount");
        }

        async Task Update(DepotDto entityDto)
        {
            if (entityDto == null || string.IsNullOrEmpty(entityDto.Code) || string.IsNullOrEmpty(entityDto.Name))
                await dialogService.ShowMessageBox("Aviso", $"informacion incorrecta.", yesText: "OK");
            connecting = true;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PutAsJsonAsync($"/api/v1/Depot", entityDto);
            connecting = false;
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                var response = await result.Content.ReadFromJsonAsync<Response<DepotDto>>();
                await dialogService.ShowMessageBox("Aviso", $"Registro actualizado.", yesText: "OK");
                return;
            }

            await dialogService.ShowMessageBox("Aviso", $"No se pudo aactualizar registro.", yesText: "OK");
            connecting = false;
            return;
        }

        async Task Delete(DepotDto entityDto)
        {
            if (entityDto.Id <= 0)
                await dialogService.ShowMessageBox("Aviso", $"informacion incorrecta.", yesText: "OK");
            var resultPopup = await dialogService.ShowMessageBox("Atencion", $"¿Desea eliminar el deposito y los productos asociados ?",
                                                         yesText: "Si", cancelText: "No");
            if (resultPopup == null)
                return;
            connecting = true;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.DeleteAsync($"/api/v1/Depot/{entityDto.Id}");
            connecting = false;
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                var response = await result.Content.ReadFromJsonAsync<Response<bool>>();
                depots.Remove(entityDto);
                await dialogService.ShowMessageBox("Aviso", $"Registro eliminado.", yesText: "OK");
                return;
            }

            await dialogService.ShowMessageBox("Aviso", $"No se pudo eliminar registro.", yesText: "OK");
            connecting = false;
            return;
        }

        async Task Create(DepotDto entityDto)
        {
            if (entityDto == null || string.IsNullOrEmpty(entityDto.Code) || string.IsNullOrEmpty(entityDto.Name))
                await dialogService.ShowMessageBox("Aviso", $"informacion incorrecta.", yesText: "OK");
            connecting = true;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PostAsJsonAsync($"/api/v1/Depot", entityDto);
            connecting = false;
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                var response = await result.Content.ReadFromJsonAsync<Response<DepotDto>>();
                depots.Add(response.Data);
                await dialogService.ShowMessageBox("Aviso", $"Registro exitoso.", yesText: "OK");
                return;
            }

            await dialogService.ShowMessageBox("Aviso", $"No se pudo realizar registro.", yesText: "OK");
            connecting = false;
            return;
        }

        async Task<List<DepotDto>> GetAll()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.GetAsync($"/api/v1/Depot");
            connecting = false;
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                var response = await result.Content.ReadFromJsonAsync<Response<List<DepotDto>>>();
                return response.Data;
            }

            return new List<DepotDto>();
        }
    }
}