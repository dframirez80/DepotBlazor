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
    public partial class Stock
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
        List<ProductDto> parcialProducts = new();
        string depotSelected = string.Empty;
        bool showPage;
        string token = string.Empty;
        public int QRecordPerPage { get; set; } = 10;
        private string searchString1 = string.Empty;
        private string searchString2 = string.Empty;
        private ProductDto selectedItem1 = null;
        private ProductDto selectedItem2 = null;
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
            parcialProducts = await GetAllProduct();
            products = parcialProducts.GroupBy(p => p.Code).Select(g => new ProductDto { Code = g.Key, Description = g.First(x => x.Code == g.Key).Description, Quantity = g.Sum(p => p.Quantity) }).ToList();
            showPage = true;
        }

        #region filtro de busqueda de tabla1
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

        #region filtro de busqueda de tabla2
        private bool FilterFunc2(ProductDto element) => FilterFunc2p(element, searchString2);
        private bool FilterFunc2p(ProductDto element, string searchString)
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

        async Task<List<ProductDto>> GetAllProduct()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.GetAsync($"/api/v1/Product");
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                var response = await result.Content.ReadFromJsonAsync<Response<List<ProductDto>>>();
                return response.Data;
            }

            return new List<ProductDto>();
        }
    }
}