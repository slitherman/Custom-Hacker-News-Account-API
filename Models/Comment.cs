using System;
using System.Collections.Generic;

namespace Custom_Hacker_News_Account_API.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int AccountId { get; set; }

    public int PostId { get; set; }

    public string Author { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime TimePosted { get; set; }

    public int Upvotes { get; set; }

    public virtual AccountInfo Account { get; set; } = null!;

    public virtual ICollection<AccountStatistic> AccountStatistics { get; set; } = new List<AccountStatistic>();

    public virtual Post Post { get; set; } = null!;
}
