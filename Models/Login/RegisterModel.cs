namespace Custom_Hacker_News_Account_API.Models.Login
{
    public class RegisterModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string[] Roles { get; set; } 
    }
}
