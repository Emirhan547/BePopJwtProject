namespace BePopJwt.API.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Key { get; set; } = "BePopJwt_Development_Key_Change_This_Immediately_123456";
        public string Issuer { get; set; } = "BePopJwt";
        public string Audience { get; set; } = "BePopJwtClients";
    }
}
