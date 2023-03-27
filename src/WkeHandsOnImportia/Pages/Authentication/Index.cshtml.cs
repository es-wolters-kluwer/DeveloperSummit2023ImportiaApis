using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Security.Claims;
using WkeHandsOnImportia.Models;

namespace WkeHandsOnImportia.Pages
{
    [Authorize]
    public class AuthenticationModel : PageModel
    {

        public WkeTokenModel Token { get; set; }


        public void OnGet()
        {

            this.Token = new WkeTokenModel()
            {
                ClientId = User.FindFirstValue("aud"),
                AccessToken = User.FindFirstValue("access_token"),
                RefreshToken = User.FindFirstValue("refresh_token"),
                Name = User.FindFirstValue("comp_display_name"),
                AccessTokenExpiration = DateTimeOffset.FromUnixTimeSeconds((long)Convert.ToDouble(User.FindFirstValue("exp"))).DateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'GMT'K", CultureInfo.InvariantCulture),
                AuthenticationDateTime = DateTimeOffset.FromUnixTimeSeconds((long)Convert.ToDouble(User.FindFirstValue("auth_time"))).DateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'GMT'K", CultureInfo.InvariantCulture),
                WKUserId = User.FindFirstValue("wk.es.clientid"),
                WKIdCDA = User.FindFirstValue("wk.es.idcda"),
                OtherInfo = String.Format("SecondUserId:{0}, UserId:{1}", User.FindFirstValue("wk.es.secondclientid"), User.FindFirstValue("wk.es.a3equipouserid"))
            };
        }

        public async Task<IActionResult> OnPostLogout()
        {            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);            
            return Redirect("~/");
        }
    }
}
