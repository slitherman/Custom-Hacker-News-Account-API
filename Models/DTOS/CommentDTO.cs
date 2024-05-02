namespace Custom_Hacker_News_Account_API.Models.DTOS
{
    public class CommentDTO
    {
        public int CommentId { get; set; }

        public int AccountId { get; set; }

        public int PostId { get; set; }

        public string Author { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime TimePosted { get; set; }

        public int Upvotes { get; set; }

        public PostDTO Post { get; set; }

        public AccountInfoDTO Account { get; set; }
    }
}
