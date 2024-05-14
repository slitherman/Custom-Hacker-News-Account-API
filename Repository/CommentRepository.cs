using Custom_Hacker_News_Account_API.Manual_Mapping;
using Custom_Hacker_News_Account_API.Models;
using Custom_Hacker_News_Account_API.Models.DTOS;

namespace Custom_Hacker_News_Account_API.Repository
{
    public class CommentRepository
    {

        public readonly AccountDbContext _dbContext;

        public AccountRepository _accRepo;
        public PostRepository _postRepo;
     

        public CommentRepository(AccountDbContext context ,AccountRepository _repo, PostRepository postRepository)
        {
            _dbContext = context;
            _accRepo = _repo;
            _postRepo = postRepository;

        }

        //public Comment CreateComment(int accountId, int idPost,CreateAndUpdateCommentDTO CreatedCommentDTO)
        //{
        //    using var transaction = _dbContext.Database.BeginTransaction();

        //    try
        //    {
        //        var accid = _accRepo.GetAccountById(accountId);
        //        var post = _postRepo.GetPostById(idPost);
        //        //var commentDTO = ManualMapper.MapCreateUpDateCommentDTOToComment(CreatedCommentDTO

          
        //        var comment = new CreateAndUpdateCommentDTO
        //        {
        //            AccountId = accid.AccountId,
        //            Author = accid.Username,
        //            PostId = post.PostId,
        //            Content = CreatedCommentDTO.Content,
        //            TimePosted = CreatedCommentDTO.TimePosted,
        //        };

        //        if (accid.AccountId != comment.AccountId || post.PostId != comment.PostId)
        //        {
        //            throw new InvalidOperationException($"Could not create the comment due to the account id {accountId} or the post with the post id {idPost} not being present");
        //        }
        //        Console.WriteLine($"Attempting to create comment with AccountId: {comment.AccountId}");

        //        var commentUpdated = comment.MapCreateUpDateCommentDTOToComment();

        //        _dbContext.Comments.Add(commentUpdated);
        //        _dbContext.SaveChanges();
            
              
        //        transaction.Commit();
        //        return commentUpdated;
        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //        throw new Exception("Failed to create comment", ex);
        //    }
        //}
        public Comment CreateComment(int accountId, int idPost, CreateAndUpdateCommentDTO CreatedCommentDTO)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var accid = _accRepo.GetAccountById(accountId);
                var post = _postRepo.GetPostById(idPost);

                // Map CreateAndUpdateCommentDTO to Comment
                var comment = ManualMapper.MapCreateCommentDTOToComment(CreatedCommentDTO);

                // Populate additional properties
                comment.AccountId = accid.AccountId;
                comment.Author = accid.Username;
                comment.PostId = post.PostId;
                comment.Content = CreatedCommentDTO.Content;
                comment.TimePosted = CreatedCommentDTO.TimePosted;
                
                // Additional checks if needed
                if (accid.AccountId != comment.AccountId || post.PostId != comment.PostId)
                {
                    throw new InvalidOperationException($"Could not create the comment due to the account id {accountId} or the post with the post id {idPost} not being present");
                }

                Console.WriteLine($"Attempting to create comment with AccountId: {comment.AccountId}");

                // Add and save the comment
                _dbContext.Comments.Add(comment);
                _dbContext.SaveChanges();

                transaction.Commit();
                return comment;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Failed to create comment", ex);
            }
        }


        public Comment GetCommentById (int id)
        {
            var foundComment = _dbContext.Comments.FirstOrDefault(x  => x.CommentId == id);
            if (foundComment != null)
            {
                return foundComment;
            }
            throw new ArgumentNullException($"Could not find the specified comment with the id {id}");
        }


        public Comment DeleteCommentById (int id)
        {

            var CommentToDelete = GetCommentById(id);
            if (CommentToDelete == null)
            {
                throw new ArgumentNullException($"Could not find the specified comment with the id {id}");
            }
            
            _dbContext.Comments.Remove(CommentToDelete);
            _dbContext.SaveChanges();
            int method = 5;
            _accRepo.modifyAccountStats(method, CommentToDelete.CommentId);
            return CommentToDelete;
        }

        public void UpvoteRecieved(int id)
        {
            var comment = GetCommentById(id);
            if(comment == null)
            {
                throw new ArgumentNullException($"Selected post with the id {id} doesnt exist");
            }
            int method = 3;
            _accRepo.modifyAccountStats(method, comment.AccountId);
            _dbContext.SaveChanges();
        }

        public Comment UpdateComment(int id, CreateAndUpdateCommentDTO commentToUpdate)
        {

            var existingComment = GetCommentById(id);
            if (existingComment == null)
            {
                throw new ArgumentNullException($"Could not find the specified comment with the id {id}");
            }

            try
            {
               existingComment.Author = commentToUpdate.Author;
               existingComment.Content = commentToUpdate.Content;
               existingComment.TimePosted = commentToUpdate.TimePosted;

               

                _dbContext.SaveChanges();
                return existingComment;
           
                
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to update comment", e);
            }


        }


    }
}
