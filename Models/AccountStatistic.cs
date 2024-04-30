using System;
using System.Collections.Generic;

namespace Custom_Hacker_News_Account_API.Models;

public partial class AccountStatistic
{
    public int AccountStatId { get; set; }

    public int? Karma { get; set; }

    public int? CommentCount { get; set; }

    public int? SubmissionCount { get; set; }

    public DateTime? LastTimeActive { get; set; }

    public int? UpvotesReceived { get; set; }

    public virtual AccountInfo AccountStat { get; set; } = null!;
}
