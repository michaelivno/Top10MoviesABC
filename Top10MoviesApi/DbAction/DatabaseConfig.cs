namespace Top10MoviesApi.Data
{
    public class DatabaseConfig : IDatabaseConfig
    {
        public string ConnectionString { get; set; }
    }
    public interface IDatabaseConfig
    {
        string ConnectionString { get; }
    }
}
