using Custom_Hacker_News_Account_API.Models;
using Custom_Hacker_News_Account_API.Models.DTOS;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;

namespace Custom_Hacker_News_Account_API.Manual_Mapping
{
    public static class ManualMapper
    {
        public static AccountInfoDTO MapAccountToDTO(this AccountInfo account)
        {


            if (account == null)
            {
                // Handle null AccountInfo object
                return null;
            }
            var accountDto = new AccountInfoDTO
            {
                AccountId = account.AccountId,
                Username = account.Username,
                Firstname = account.Firstname,
                Lastname = account.Lastname,
                BirthDate = account.BirthDate,
                Email = account.Email,
                Password = account.Password,
                IsBanned = account.IsBanned,
                //AccountStatistic = account.AccountStatistic != null ? MapAccountStatToDTO(account.AccountStatistic) : null,
                //Posts = account.Posts != null ? account.Posts.Select(post => MapPostToDTO(post)).ToList() : null,
                //Comments = account.Comments != null ? account.Comments.Select(comment => MapCommentToDTO(comment)).ToList() : null
            };
            return accountDto;
        }

        //public static PostDTO MapCreatePostDTOToDTO(CreateAndUpdatePostDTO createandUpdatePostDTO)
        //{
        //    return new PostDTO
        //    {
        //        Title = createandUpdatePostDTO.Title,
        //        Url = createandUpdatePostDTO.Url,
        //        AccountId = createandUpdatePostDTO.AccountId
        //    };
        //}

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


        public static IEnumerable<PostDTO> MapAccountsToDTOs(this IEnumerable<Post> posts)
        {
            var DTOS = new List<PostDTO>();
            foreach (var post in posts)
            {
                DTOS.Add(post.MapPostToDTO());
            }
            return DTOS;
        }

        public static PostDTO MapPostToDTO(this Post accountPost)
        {
            return new PostDTO
            {
                PostId = accountPost.PostId,
                Title = accountPost.Title,
                Username = accountPost.Account != null ? accountPost.Account.Username : "Unknown_User!!!",
                Dead = accountPost.Dead,
                Deleted = accountPost.Deleted,
                AccountId = accountPost.AccountId,
                Url = accountPost.Url,
                Comments = accountPost.Comments
                       .Where(c => c != null) // Filter out null comments and accounts
                       .Select(c => c.MapCommentToDTO()) // Map valid comments to DTOs
                       .ToList(),
                //Account = accountPost.Account.MapAccountToDTO(),

            };
        }

        public static Post MapDTOToPost(this PostDTO accountPostdto)
        {
          
            //var account = accountPostdto.Account != null ? accountPostdto.Account.MapDTOToAccount() : null;

           
            var comments = accountPostdto.Comments != null ? accountPostdto.Comments.Select(c => c.MapDTOToComment()).ToList() : new List<Comment>();
            return new Post
            {
                PostId = accountPostdto.PostId,
                Title = accountPostdto.Title,
                Dead = accountPostdto.Dead,
                Username = accountPostdto.Username,
                Deleted = accountPostdto.Deleted,
                AccountId = accountPostdto.AccountId,
                Url = accountPostdto.Url,
                //Account = account,
                Comments = comments,
           
                
               

            };
        }

        //public static Post MapDTOToPost(this PostDTO accountPostdto)
        //{
        //    var post = new Post
        //    {
        //        PostId = accountPostdto.PostId,
        //        Title = accountPostdto.Title,
        //        Dead = accountPostdto.Dead,
        //        Deleted = accountPostdto.Deleted,
        //        AccountId = accountPostdto.AccountId,
        //        AccountStatistics = new List<AccountStatistic>() // Initializing the collection
        //    };

        //    // Map AccountStatistics
        //    if (accountPostdto.AccountStatistics != null)
        //    {
        //        foreach (var accountStatDto in accountPostdto.AccountStatistics)
        //        {
        //            // Map AccountStatisticDTO to AccountStatistic
        //            var accountStat = new AccountStatistic
        //            {
        //                Karma = accountStatDto.Karma,
        //                SubmissionCount = accountStatDto.SubmissionCount,
        //                CommentCount = accountStatDto.CommentCount,
        //                LastTimeActive = accountStatDto.LastTimeActive,
        //                UpvotesReceived = accountStatDto.UpvotesReceived
        //            };

        //            // Add the mapped AccountStatistic to Post's AccountStatistics
        //            post.AccountStatistics.Add(accountStat);
        //        }
        //    }

        //    return post;
        //}


        //public static AccountStatisticDTO MapAccountStatToDTO(this AccountStatistic accountStat)
        //{
        //    return new AccountStatisticDTO
        //    {
        //        AccountStatId = accountStat.AccountStatId,
        //        CommentCount = accountStat.CommentCount,
        //        SubmissionCount = accountStat.SubmissionCount,
        //        Karma = accountStat.Karma,
        //        UpvotesReceived = accountStat.UpvotesReceived,
        //        LastTimeActive = accountStat.LastTimeActive
        //    };
        //}

        //public static AccountStatistic MapDTOToAccountStat(this AccountStatisticDTO accountStat)
        //{
        //    return new AccountStatistic
        //    {
        //        AccountStatId = accountStat.AccountStatId,
        //        CommentCount = accountStat.CommentCount,
        //        SubmissionCount = accountStat.SubmissionCount,
        //        Karma = accountStat.Karma,
        //        UpvotesReceived = accountStat.UpvotesReceived,
        //        LastTimeActive = accountStat.LastTimeActive
        //    };
        //}



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
                Author = comment.Author != null ? comment.Author : "Unknown_User!!!",
                Content = comment.Content,
                TimePosted = comment.TimePosted,
                Upvotes = comment.Upvotes
            };
         
            
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
            //if (comment.Post != null)
            //{
            //    mappedComment.Post = comment.Post.MapDTOToPost();
            //}
            //if (comment.Account != null)
            //{
            //    mappedComment.Account = comment.Account.MapDTOToAccount();
            
            return mappedComment;
        }

        public static Comment MapCreateUpDateCommentDTOToComment(this CreateAndUpdateCommentDTO comment)
        {
            return new Comment
            {
                CommentId = comment.CommentId,
                AccountId = comment.AccountId,
                PostId = comment.PostId,
                Author = comment.Author,
                Content = comment.Content,
                TimePosted = comment.TimePosted,
            };
        }

        public static Comment MapCreateCommentDTOToComment(CreateAndUpdateCommentDTO createCommentDTO)
        {
            return new Comment
            {
                Author = createCommentDTO.Author,
                Content = createCommentDTO.Content,
                TimePosted = createCommentDTO.TimePosted
            };
        }

        public static Post MapCreateUpdatePostDTOToPost(this CreateAndUpdatePostDTO posts)
        {
            return new Post
            {
                PostId = posts.PostId,
                Title = posts.Title,
                AccountId = posts.AccountId,
                Url = posts.Url,


            };
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



