using Domain;

namespace Application.DTOs
{
    public class BookDTO
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Genre { get; set; } = "";
        public DateTime Date { get; set; }
        public List<Author> Author { get; set; } = new List<Author>();
    }
}
