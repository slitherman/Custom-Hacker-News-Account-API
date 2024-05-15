using System;
using System.Collections.Generic;

namespace Custom_Hacker_News_Account_API.Models;

public partial class AccountInfo
{
    public int AccountId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string Username { get; set; } = null!;

    public bool IsBanned { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
