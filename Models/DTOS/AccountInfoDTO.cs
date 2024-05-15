using System.Runtime.InteropServices;

namespace Custom_Hacker_News_Account_API.Models.DTOS
{
    public class AccountInfoDTO
    {
        public int AccountId { get; set; }

        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public string Username { get; set; } = null!;

        public bool IsBanned { get; set; }

        //public AccountStatisticDTO AccountStatistic { get; set; }

        //public List<PostDTO> Posts { get; set; }
        //public List<CommentDTO> Comments { get; set; }

    }
}

