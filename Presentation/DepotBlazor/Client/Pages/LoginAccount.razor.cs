using Microsoft.AspNetCore.Components;
using System.Net;
using Blazored.LocalStorage;
using MudBlazor;
using System.Text.RegularExpressions;
using Domain.Models;
using System.Net.Http;
using System.Net.Http.Json;
using DepotBlazor.Client.Services;

namespace DepotBlazor.Client.Pages
{
    public partial class LoginAccount
    {
        #region Servicios y variables
        [Inject] NavigationManager navigation { get; set; }
        [Inject] IDialogService dialogService { get; set; }
        [Inject] ITokenService tokenService { get; set; }
        [Inject] HttpClient httpClient { get; set; }

        bool success, connecting;
        Login login = new();
        #endregion

        #region Verifica el contraseña si cumple con la reglas
        /// <summary>
        /// Verifica el contraseña si cumple con la reglas
        /// </summary>
        /// <param name="pw">contraseña</param>
        /// <returns>lista</returns>
        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "La contraseña es requerida!";
                yield break;
            }

            if (pw.Length < 8)
                yield return "La contraseña debe tener minimo de 8 caracteres";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "La contraseña debe tener al menos una mayuscula";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "La contraseña debe tener al menos una minuscula";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "La contraseña debe tener al menos un numero";
            if (!Regex.IsMatch(pw, @"[!\""#\\$%&'()*+,-./:;=?@^_`{|}~]"))
                yield return "La contraseña debe tener al menos un caracter especial";
        }
        #endregion

        #region Conecta a endpoint loginAccount
        /// <summary>
        /// Conecta a endpoint loginAccount
        /// </summary>
        async Task LoginAccountApi(Login login)
        {
            if(login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
				await dialogService.ShowMessageBox("Aviso", $"informacion incorrecta.", yesText: "OK");
			connecting = true;
            var result = await httpClient.PostAsJsonAsync($"/api/v1/Auth/loginAccount", login);
            var response = await result.Content.ReadFromJsonAsync<Response<string>>();
            connecting = false;
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                await tokenService.Set(response.Data);
                navigation.NavigateTo("/");
                return;
            }
            await dialogService.ShowMessageBox("Aviso", $"El usuario o contraseña es incorrecto.", yesText: "OK");
            StateHasChanged();
            connecting = false;
            return;
        }
        #endregion
    }
}