namespace Custom_Hacker_News_Account_API.Models.DTOS
{
    public class PostDTO
    {

        public int PostId { get; set; }

        public string Title { get; set; } 

        public bool Dead { get; set; } 

        public bool Deleted { get; set; } = false;

        public int AccountId { get; set; }

        public string Url { get; set; } 

        public int? Upvotes { get; set; }
        public List<CommentDTO> Comments { get; set; }
        public AccountInfoDTO Account { get; set; }



    }
}
