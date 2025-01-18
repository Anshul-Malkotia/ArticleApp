namespace ArticleApp.Model
{
    public class ContentTable
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string? CreatedBy { get; set; }
    }

}
