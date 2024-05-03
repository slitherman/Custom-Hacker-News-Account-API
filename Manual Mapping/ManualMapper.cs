using Custom_Hacker_News_Account_API.Models;
using Custom_Hacker_News_Account_API.Models.DTOS;

namespace Custom_Hacker_News_Account_API.Manual_Mapping
{
    public static class ManualMapper
    {
        public static AccountInfoDTO MapAccountToDTO(this AccountInfo account)
        {
            return new AccountInfoDTO
            {
                AccountId = account.AccountId,
                Username = account.Username,
                Firstname = account.Firstname,
                Lastname = account.Lastname,
                BirthDate = account.BirthDate,
                Email = account.Email,
                Password = account.Password,
                IsBanned = account.IsBanned,
                AccountStatistic = account.AccountStatistic != null ? MapAccountStatToDTO(account.AccountStatistic) : null,
                Posts = account.Posts.Select(post => MapPostToDTO(post)).ToList()
            };
        }

        public static PostDTO MapCreatePostDTOToDTO(CreateAndUpdatePostDTO createandUpdatePostDTO)
        {
            return new PostDTO
            {
                Title = createandUpdatePostDTO.Title,
                Url = createandUpdatePostDTO.Url,
                PostId = createandUpdatePostDTO.PostId,
                AccountId = createandUpdatePostDTO.AccountId
            };
        }

        public static CommentDTO MapCreateCommentToDTO(CreateAndUpdateCommentDTO createCommentDTO)
        {
            return new CommentDTO
            {
                Author = createCommentDTO.Author,
                Content = createCommentDTO.Content,
                TimePosted = createCommentDTO.TimePosted
            };
        }

        public static AccountInfoDTO MapCreateAccountToDTO(CreateAndUpdateAccountDTO createAccountDTO)
        {
            return new AccountInfoDTO
            {
                AccountId = createAccountDTO.AccountId,
                Username = createAccountDTO.Username,
                Firstname = createAccountDTO.Firstname,
                Lastname = createAccountDTO.Lastname,
                BirthDate = createAccountDTO.BirthDate,
                Email = createAccountDTO.Email,
                Password = createAccountDTO.Password
            };
        }

        public static AccountInfo MapDTOToAccount(this AccountInfoDTO account)
        {
            return new AccountInfo
            {
                AccountId = account.AccountId,
                Username = account.Username,
                Firstname = account.Firstname,
                Lastname = account.Lastname,
                BirthDate = account.BirthDate,
                Email = account.Email,
                Password = account.Password,
                IsBanned = account.IsBanned
            };
        }

        public static IEnumerable<AccountInfoDTO> MapAccountsToDTOs(this IEnumerable<AccountInfo> accounts)
        {
            var DTOS = new List<AccountInfoDTO>();
            foreach (var account in accounts)
            {
                DTOS.Add(account.MapAccountToDTO());
            }
            return DTOS;
        }

        public static PostDTO MapPostToDTO(this Post accountPost)
        {
            return new PostDTO
            {
                PostId = accountPost.PostId,
                Title = accountPost.Title,
                Dead = accountPost.Dead,
                Deleted = accountPost.Deleted,
                AccountId = accountPost.AccountId,
                Account = accountPost.Account.MapAccountToDTO(),
                Comments = accountPost.Comments.Select(x => MapCommentToDTO(x)).ToList()
            };
        }

        public static Post MapDTOToPost(this PostDTO accountPostdto)
        {
            return new Post
            {
                PostId = accountPostdto.PostId,
                Title = accountPostdto.Title,
                Dead = accountPostdto.Dead,
                Deleted = accountPostdto.Deleted,
                AccountId = accountPostdto.AccountId
            };
        }

        public static AccountStatisticDTO MapAccountStatToDTO(this AccountStatistic accountStat)
        {
            return new AccountStatisticDTO
            {
                AccountStatId = accountStat.AccountStatId,
                CommentCount = accountStat.CommentCount,
                SubmissionCount = accountStat.SubmissionCount,
                Karma = accountStat.Karma,
                UpvotesReceived = accountStat.UpvotesReceived,
                LastTimeActive = accountStat.LastTimeActive
            };
        }

        public static AccountStatistic MapDTOToAccountStat(this AccountStatisticDTO accountStat)
        {
            return new AccountStatistic
            {
                AccountStatId = accountStat.AccountStatId,
                CommentCount = accountStat.CommentCount,
                SubmissionCount = accountStat.SubmissionCount,
                Karma = accountStat.Karma,
                UpvotesReceived = accountStat.UpvotesReceived,
                LastTimeActive = accountStat.LastTimeActive
            };
        }

        public static CommentDTO MapCommentToDTO(this Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }
            var mappedComment = new CommentDTO
            {
                CommentId = comment.CommentId,
                AccountId = comment.AccountId,
                PostId = comment.PostId,
                Author = comment.Author,
                Content = comment.Content,
                TimePosted = comment.TimePosted,
                Upvotes = comment.Upvotes
            };
            if (comment.Post != null)
            {
                mappedComment.Post = comment.Post.MapPostToDTO();
            }
            if (comment.Account != null)
            {
                mappedComment.Account = comment.Account.MapAccountToDTO();
            }
            return mappedComment;
        }

        public static Comment MapDTOToComment(this CommentDTO comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }
            var mappedComment = new Comment
            {
                CommentId = comment.CommentId,
                AccountId = comment.AccountId,
                PostId = comment.PostId,
                Author = comment.Author,
                Content = comment.Content,
                TimePosted = comment.TimePosted,
                Upvotes = comment.Upvotes
            };
            if (comment.Post != null)
            {
                mappedComment.Post = comment.Post.MapDTOToPost();
            }
            if (comment.Account != null)
            {
                mappedComment.Account = comment.Account.MapDTOToAccount();
            }
            return mappedComment;
        }

        public static IEnumerable<Comment> MapCommentsToDTOs(this IEnumerable<CommentDTO> comments)
        {
            var DTOS = new List<Comment>();
            foreach (var comment in comments)
            {
                DTOS.Add(comment.MapDTOToComment());
            }
            return DTOS;
        }
    }
}



