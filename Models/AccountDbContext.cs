using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Custom_Hacker_News_Account_API.Models;

public partial class AccountDbContext : DbContext
{
    public AccountDbContext()
    {
    }

    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountInfo> AccountInfos { get; set; }

    public virtual DbSet<AccountStatistic> AccountStatistics { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=AccountDb;User Id=falasema4;Password=azhkvck4433;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountInfo>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account___46A222CDC7EC69D9");

            entity.ToTable("Account_Info");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lastname");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AccountStatistic>(entity =>
        {
            entity.HasKey(e => e.AccountStatId).HasName("PK__Account___46A222CD0DE1ACDA");

            entity.ToTable("Account_Statistics");

            entity.Property(e => e.AccountStatId)
                .ValueGeneratedOnAdd()
                .HasColumnName("account_stat_id");
            entity.Property(e => e.CommentCount).HasColumnName("comment_count");
            entity.Property(e => e.CommentId).HasColumnName("comment_Id");
            entity.Property(e => e.Karma).HasColumnName("karma");
            entity.Property(e => e.LastTimeActive)
                .HasColumnType("datetime")
                .HasColumnName("last_time_active");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.SubmissionCount).HasColumnName("submission_count");
            entity.Property(e => e.UpvotesReceived).HasColumnName("upvotes_received");

            entity.HasOne(d => d.AccountStat).WithOne(p => p.AccountStatistic)
                .HasForeignKey<AccountStatistic>(d => d.AccountStatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Statistics_Account_Info");

            entity.HasOne(d => d.Comment).WithMany(p => p.AccountStatistics)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Statistics_Comments");

            entity.HasOne(d => d.Post).WithMany(p => p.AccountStatistics)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Statistics_Posts");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(e => e.CommentId).HasColumnName("Comment_Id");
            entity.Property(e => e.AccountId).HasColumnName("Account_Id");
            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PostId).HasColumnName("Post_Id");
            entity.Property(e => e.TimePosted)
                .HasColumnType("datetime")
                .HasColumnName("Time_Posted");

            entity.HasOne(d => d.Account).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Account_Info");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Posts");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Posts__3ED78766520D0850");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Dead).HasColumnName("dead");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.Url)
                .IsUnicode(false)
                .HasColumnName("URL");

            entity.HasOne(d => d.Account).WithMany(p => p.Posts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Posts_Account_Info");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
