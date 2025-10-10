namespace MovieWebApp.Presentation.Request
{
    public class MovieUploadRequest
    {
        public string MovieName { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public List<int> GenreIds { get; set; } = new List<int>();
        public IFormFile Poster { get; set; }  
        public IFormFile Video { get; set; }   
    }
}
