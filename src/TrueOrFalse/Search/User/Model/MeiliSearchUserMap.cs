
namespace TrueOrFalse.Search
{
    public class MeiliSearchUserMap
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int WishCountQuestions { get; set; }
        public int Rank { get; set; }
        public int Id { get; set; }
        public List<Language> ContentLanguages { get; set; }
    }
}
