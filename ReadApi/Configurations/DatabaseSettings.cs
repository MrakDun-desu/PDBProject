namespace ReadApi.Configurations
{
    public class DatabaseSettings
    {
        public string ConnectionStrings { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UserCollection { get; set; } = null!;
        public string ProductCollection { get; set; } = null!;
    }
}
