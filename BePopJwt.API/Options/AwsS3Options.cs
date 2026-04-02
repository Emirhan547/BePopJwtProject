namespace BePopJwt.API.Options
{
    public class AwsS3Options
    {
        public const string SectionName = "AWS";

        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Region { get; set; } = "us-east-1";
    }
}
