using System;
using System.Collections.Generic;

namespace Custom_Hacker_News_Account_API.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public bool Dead { get; set; }

    public bool Deleted { get; set; }

    public int AccountId { get; set; }

    public string? Url { get; set; }

    public int? Upvotes { get; set; }

    public string? Username { get; set; }

    public virtual AccountInfo Account { get; set; } = null!;

    public virtual ICollection<AccountStatistic> AccountStatistics { get; set; } = new List<AccountStatistic>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
