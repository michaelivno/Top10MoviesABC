namespace Top10MoviesApi.DTO
{
    public class MovieDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public int ranking { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CreateMovieDto
    {
        public string title { get; set; }
        public string category { get; set; }
        public int ranking { get; set; }
        public string ImageUrl { get; set; }
    }

    public class UpdateMovieDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public int ranking { get; set; }
        public string ImageUrl { get; set; }
    }
}
