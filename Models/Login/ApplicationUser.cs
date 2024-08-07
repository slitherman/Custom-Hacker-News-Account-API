using Microsoft.AspNetCore.Identity;

namespace Custom_Hacker_News_Account_API.Models.Login
{
    public class ApplicationUser: IdentityUser
    {


        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsBanned { get; set; }
        public string? Role { get; set; }



    }
}
