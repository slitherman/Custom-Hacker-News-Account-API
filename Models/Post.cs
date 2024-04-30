using System;
using System.Collections.Generic;

namespace Custom_Hacker_News_Account_API.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string Type { get; set; } = null!;

    public bool Dead { get; set; }

    public bool Deleted { get; set; }

    public int AccountId { get; set; }

    public virtual AccountInfo Account { get; set; } = null!;
}
