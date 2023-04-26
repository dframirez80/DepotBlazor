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
    public partial class ProductABM
    {
        [Inject]
        NavigationManager navigation { get; set; }

        [Inject]
        IDialogService dialogService { get; set; }

        [Inject]
        ITokenService tokenService { get; set; }

        [Inject]
        HttpClient httpClient { get; set; }

        List<ProductDto> products = new();
        List<DepotDto> depots = new();
        string depotSelected = string.Empty;
        bool showPage, connecting;
        bool addEntity, putEntity;
        ProductDto createEntity = new();
        ProductDto updateEntity = new();
        string token = string.Empty;
        public int QRecordPerPage { get; set; } = 10;
        private string searchString1 = string.Empty;
        private ProductDto selectedItem1 = null;
        private HashSet<ProductDto> selectedItems = new HashSet<ProductDto>();
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
            products = await GetAll();
            depots = await GetAllDepot();
            showPage = true;
        }

#region filtro de busqueda de tabla
        private bool FilterFunc1(ProductDto element) => FilterFunc(element, searchString1);
        private bool FilterFunc(ProductDto element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            string compareString = element.Code.ToLower();
            if (compareString.Contains(searchString.ToLower(), StringComparison.OrdinalIgnoreCase))
                return true;
            compareString = element.Description.ToLower();
            if (compareString.Contains(searchString.ToLower(), StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

#endregion
        async Task Logout()
        {
            tokenService.Clear();
            navigation.NavigateTo("/LoginAccount");
        }

        async Task Update(ProductDto entityDto)
        {
            if (entityDto == null || string.IsNullOrEmpty(entityDto.Code) || string.IsNullOrEmpty(entityDto.Description))
                await dialogService.ShowMessageBox("Aviso", $"informacion incorrecta.", yesText: "OK");
            connecting = true;
            entityDto.UserId = await tokenService.GetUserId();
            entityDto.DepotId = depots.FirstOrDefault(x => x.Name == depotSelected).Id;
            entityDto.DepotName = depotSelected;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PutAsJsonAsync($"/api/v1/Product", entityDto);
            var response = await result.Content.ReadFromJsonAsync<Response<ProductDto>>();
            products = await GetAll();
            connecting = false;
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                await dialogService.ShowMessageBox("Aviso", $"Registro actualizado.", yesText: "OK");
                return;
            }

            await dialogService.ShowMessageBox("Aviso", $"No se pudo aactualizar registro. {response.Message}", yesText: "OK");
            return;
        }

        async Task Delete(ProductDto entityDto)
        {
            if (entityDto.Id <= 0)
                await dialogService.ShowMessageBox("Aviso", $"informacion incorrecta.", yesText: "OK");
            connecting = true;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.DeleteAsync($"/api/v1/Product/{entityDto.Id}");
            var response = await result.Content.ReadFromJsonAsync<Response<bool>>();
            connecting = false;
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                products.Remove(entityDto);
                await dialogService.ShowMessageBox("Aviso", $"Registro eliminado.", yesText: "OK");
                return;
            }

            await dialogService.ShowMessageBox("Aviso", $"No se pudo eliminar registro. {response.Message}", yesText: "OK");
            connecting = false;
            return;
        }

        async Task Create(ProductDto entityDto)
        {
            if (entityDto == null || string.IsNullOrEmpty(entityDto.Code) || string.IsNullOrEmpty(entityDto.Description))
                await dialogService.ShowMessageBox("Aviso", $"informacion incorrecta.", yesText: "OK");
            connecting = true;
            entityDto.UserId = await tokenService.GetUserId();
            entityDto.DepotId = depots.FirstOrDefault(x => x.Name == depotSelected).Id;
            entityDto.DepotName = depotSelected;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PostAsJsonAsync($"/api/v1/Product", entityDto);
            connecting = false;
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                var response = await result.Content.ReadFromJsonAsync<Response<ProductDto>>();
                products.Add(response.Data);
                await dialogService.ShowMessageBox("Aviso", $"Registro exitoso.", yesText: "OK");
                return;
            }

            await dialogService.ShowMessageBox("Aviso", $"No se pudo realizar registro.", yesText: "OK");
            connecting = false;
        }

        async Task<List<ProductDto>> GetAll()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.GetAsync($"/api/v1/Product");
            connecting = false;
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                var response = await result.Content.ReadFromJsonAsync<Response<List<ProductDto>>>();
                return response.Data;
            }

            return new List<ProductDto>();
        }

        async Task<List<DepotDto>> GetAllDepot()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.GetAsync($"/api/v1/Depot");
            var response = await result.Content.ReadFromJsonAsync<Response<List<DepotDto>>>();
            connecting = false;
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }

            return new List<DepotDto>();
        }
    }
}