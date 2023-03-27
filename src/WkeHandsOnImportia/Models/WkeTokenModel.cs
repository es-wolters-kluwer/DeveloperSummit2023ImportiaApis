namespace WkeHandsOnImportia.Models
{
    public class WkeTokenModel
    {

        public string ClientId { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string AccessTokenExpiration { get; set; } = string.Empty;

        public string AuthenticationDateTime { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string WKIdCDA { get; set; } = string.Empty;

        public string WKUserId { get; set; } = string.Empty;

        public string OtherInfo { get; set; } = string.Empty;

    }
}
