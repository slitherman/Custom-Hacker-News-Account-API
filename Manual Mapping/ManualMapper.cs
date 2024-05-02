using Custom_Hacker_News_Account_API.Models;
using Custom_Hacker_News_Account_API.Models.DTOS;

namespace Custom_Hacker_News_Account_API.Manual_Mapping
{
    public static class ManualMapper
    {
        public static AccountInfoDTO AccountInfoAsDTO(this AccountInfo account)
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
                // This expression checks if the AccountInfo entity has an associated AccountStatistic.
                // If it does, it maps the AccountStatistic entity to a DTO using the AccountStatAsDTO method.
                // Otherwise, it assigns null to the AccountStatistic property of the DTO.
                AccountStatistic = account.AccountStatistic != null ? AccountStatAsDTO(account.AccountStatistic) : null,
                Posts = account.Posts.Select(post => PostAsDTO(post)).ToList() 




            };
                
        }

        public static CommentDTO CreateCommentAsCommentDTO(CreateAndUpdateCommentDTO createCommentDTO)
        {
            return new CommentDTO
            {

                Author = createCommentDTO.Author,
                Content = createCommentDTO.Content,
                TimePosted = createCommentDTO.TimePosted,
                Upvotes = createCommentDTO.Upvotes,
            };
        }

        public static AccountInfoDTO CreateAccountAsAccountInfoDTO(CreateAndUpdateAccountDTO createAccountDTO)
        {
            return new AccountInfoDTO
            {
                AccountId = createAccountDTO.AccountId,
                Username = createAccountDTO.Username,
                Firstname = createAccountDTO.Firstname,
                Lastname = createAccountDTO.Lastname,
                BirthDate = createAccountDTO.BirthDate,
                Email = createAccountDTO.Email,
                Password = createAccountDTO.Password,
                IsBanned = createAccountDTO.IsBanned,
               
            };
        }

        public static AccountInfo AccountInfoAsDTOReverse(this AccountInfoDTO account)
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



        public static IEnumerable<AccountInfoDTO> AccountInfosAsDTOS(this IEnumerable<AccountInfo> accounts)
        {

            var DTOS = new List<AccountInfoDTO>();

            foreach (var account in accounts)
            {
                DTOS.Add(account.AccountInfoAsDTO());
            }

            return DTOS;
            
        }


        public static PostDTO PostAsDTO(this Post accountPost)
        {
            return new PostDTO
            {

                PostId = accountPost.PostId,
                Title = accountPost.Title,
                Type = accountPost.Type,
                Dead = accountPost.Dead,
                Deleted = accountPost.Deleted,
                AccountId = accountPost.AccountId,
                Account = accountPost.Account.AccountInfoAsDTO(),
                Comments = accountPost.Comments.Select(x => CommentAsDTOReverse(x)).ToList(),


            };
        }

           
        

        public static Post PostAsDTOReserve(this PostDTO accountPostdto)
        {
            return new Post
            {

                PostId = accountPostdto.PostId,
                Title = accountPostdto.Title,
                Type = accountPostdto.Type,
                Dead = accountPostdto.Dead,
                Deleted = accountPostdto.Deleted,
                AccountId = accountPostdto.AccountId,

            };

        }
        public static AccountStatisticDTO AccountStatAsDTO(this AccountStatistic accountStat)
        {
            return new AccountStatisticDTO
            {

               AccountStatId = accountStat.AccountStatId,
               CommentCount = accountStat.CommentCount,
               SubmissionCount = accountStat.SubmissionCount,
               Karma = accountStat.Karma,
               UpvotesReceived = accountStat.UpvotesReceived,
               LastTimeActive = accountStat.LastTimeActive,

            };

        }

        public static AccountStatistic AccountStatAsDTORevese(this AccountStatisticDTO accountStat)
        {
            return new AccountStatistic
            {

                AccountStatId = accountStat.AccountStatId,
                CommentCount = accountStat.CommentCount,
                SubmissionCount = accountStat.SubmissionCount,
                Karma = accountStat.Karma,
                UpvotesReceived = accountStat.UpvotesReceived,
                LastTimeActive = accountStat.LastTimeActive,

            };

        }


        //public static Comment CommentAsDTO(this CommentDTO comment) {


        //    return new Comment
        //    {
        //        CommentId = comment.CommentId,
        //        AccountId = comment.AccountId,
        //        PostId = comment.PostId,
        //        Author = comment.Author,
        //        Content = comment.Content,
        //        TimePosted = comment.TimePosted,
        //        Upvotes = comment.Upvotes,
        //        Post = comment.Post.PostAsDTOReserve(),
        //        Account = comment.Account.AccountInfoAsDTOReverse(),

        //    };

        //}



        public static CommentDTO CommentAsDTOReverse(this Comment comment)
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

            // Map nested Post and AccountInfo
            if (comment.Post != null)
            {
                mappedComment.Post = comment.Post.PostAsDTO();
            }
            if (comment.Account != null)
            {
                mappedComment.Account = comment.Account.AccountInfoAsDTO();
            }

            return mappedComment;
        }

        public static Comment CommentAsDTO(this CommentDTO comment)
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

            // Map nested PostDTO and AccountInfoDTO
            if (comment.Post != null)
            {
                mappedComment.Post = comment.Post.PostAsDTOReserve();
            }
            if (comment.Account != null)
            {
                mappedComment.Account = comment.Account.AccountInfoAsDTOReverse();
            }

            return mappedComment;
        }

        public static IEnumerable<Comment> CommentsAsDTOS(this IEnumerable<CommentDTO> comments)
        {
            var DTOS = new List<Comment>();

            foreach (var comment in comments)
            {
                DTOS.Add(comment.CommentAsDTO());
            }

            return DTOS;
        }


    }
}



