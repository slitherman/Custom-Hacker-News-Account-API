namespace Custom_Hacker_News_Account_API.Models.DTOS
{
    public class CreateAndUpdateAccountDTO
    {
        public int AccountId { get; set; }

        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public string Username { get; set; } = null!;


    }
}
