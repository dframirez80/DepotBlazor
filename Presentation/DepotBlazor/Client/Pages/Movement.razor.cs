using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using MudBlazor;
using DepotBlazor.Client.Services;
using Domain.Entities;
using System.Net.Http.Headers;
using System.Net;
using Domain.Models;

namespace DepotBlazor.Client.Pages
{
    public partial class Movement
    {
        [Inject]
        NavigationManager navigation { get; set; }

        [Inject]
        IDialogService dialogService { get; set; }

        [Inject]
        ITokenService tokenService { get; set; }

        [Inject]
        HttpClient httpClient { get; set; }

        List<DepotDto> depotList = new();
        List<DepotDto> sourceDepotList = new();
        List<DepotDto> DestDepotList = new();
        List<ProductDto> products = new();
        ProductMovementDto createEntity = new();
        string destSelected = string.Empty;
        string sourceSelected = string.Empty;
        string codeSelected = string.Empty;
        string descriptionSelected = string.Empty;
        int sourceQuantity, destQuantity;
        bool showPage, connecting, enableQuantity, enableDest;
        string token = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            bool isvalid = await tokenService.IsTokenValid();
            if (!isvalid)
            {
                navigation.NavigateTo("/loginaccount");
                return;
            }

            token = await tokenService.Get();
            products = await GetAllProduct();
            depotList = await GetAllDepot();
            showPage = true;
        }

        async Task Logout()
        {
            tokenService.Clear();
            navigation.NavigateTo("/LoginAccount");
        }

        void GetProductCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                descriptionSelected = products.First(x => x.Code == code).Description;
            }
        }

        async Task GetQuantity()
        {
            enableQuantity = false;
            var exists = products.Where(x => x.DepotName == sourceSelected).Any(y => y.Code == codeSelected);
            if (exists)
            {
                sourceQuantity = products.Where(x => x.DepotName == sourceSelected).First(y => y.Code == codeSelected).Quantity;
                if (sourceQuantity < 1)
                    await dialogService.ShowMessageBox("Aviso", $"El producto {codeSelected} no posee stock.", yesText: "OK");
                else
                    enableQuantity = true;
            }
            else
                await dialogService.ShowMessageBox("Aviso", $"El producto {codeSelected} no existe  en deposito {sourceSelected}.", yesText: "OK");
        }

        async Task CheckDepot()
        {
            enableDest = false;
            if (destSelected == sourceSelected)
                await dialogService.ShowMessageBox("Aviso", $"No se puede enviar el producto al mismo deposito de origen.", yesText: "OK");
            else
                enableDest = true;
        }

        async Task Create(ProductMovementDto entityDto)
        {
            if (destQuantity > sourceQuantity)
            {
                await dialogService.ShowMessageBox("Aviso", $"La cantidad a enviar es mayor que la de origen.", yesText: "OK");
                return;
            }

            connecting = true;
            createEntity.UserId = await tokenService.GetUserId();
            createEntity.DepotIdSource = depotList.First(x => x.Name == sourceSelected).Id;
            createEntity.DepotIdDestination = depotList.First(x => x.Name == destSelected).Id;
            createEntity.ProductId = products.First(x => x.Code == codeSelected).Id;
            createEntity.Quantity = destQuantity;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PostAsJsonAsync($"/api/v1/ProductMovement", entityDto);
            connecting = false;
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                await dialogService.ShowMessageBox("Aviso", $"Registro exitoso.", yesText: "OK");
                return;
            }

            await dialogService.ShowMessageBox("Aviso", $"No se pudo realizar registro.", yesText: "OK");
            connecting = false;
        }

        async Task<List<ProductDto>> GetAllProduct()
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