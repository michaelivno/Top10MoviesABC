namespace Top10MoviesApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public int Ranking { get; set; }
        public string ImageUrl { get; set; }
    }
}
