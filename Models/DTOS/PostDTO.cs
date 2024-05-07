namespace Custom_Hacker_News_Account_API.Models.DTOS
{
    public class PostDTO
    {

        public int PostId { get; set; }

        public string Title { get; set; } = null!;

        public string Type { get; set; } = null!;

        public bool Dead { get; set; } = false; 

        public bool Deleted { get; set; } = false;

        public int AccountId { get; set; }

        public string Url { get; set; } = null!;

        public int Upvotes { get; set; }
        public List<CommentDTO> Comments { get; set; }
    public AccountInfoDTO Account { get; set; }



    }
}
