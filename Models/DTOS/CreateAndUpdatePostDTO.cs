﻿namespace Custom_Hacker_News_Account_API.Models.DTOS
{
    public class CreateAndUpdatePostDTO
    {

        public int PostId { get; set; }
        public string Title { get; set; } = null!;
        public int AccountId { get; set; }
        public string Url { get; set; } = null!;

        
    }
}
