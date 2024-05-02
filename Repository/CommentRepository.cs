using Custom_Hacker_News_Account_API.Manual_Mapping;
using Custom_Hacker_News_Account_API.Models;
using Custom_Hacker_News_Account_API.Models.DTOS;

namespace Custom_Hacker_News_Account_API.Repository
{
    public class CommentRepository
    {

        public readonly AccountDbContext _dbContext;

        public AccountRepository _accRepo;
     

        public CommentRepository(AccountDbContext context ,AccountRepository _repo)
        {
            _dbContext = context;
            _accRepo = _repo;

        }

        public Comment CreateComment (CreateAndUpdateCommentDTO CreatedCommentDTO)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                int method = 4;

                CommentDTO commentDTO = ManualMapper.CreateCommentAsCommentDTO(CreatedCommentDTO);
                Comment comment = commentDTO.CommentAsDTO();

                _dbContext.Comments.Add(comment);
                _accRepo.modifyAccountStats(method, comment.AccountId);
                transaction.Commit();
                return comment;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Failed to create comment, and update account", ex);
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

        public Comment UpdateComment(int id, Comment commentToUpdate)
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
               existingComment.Upvotes = commentToUpdate.Upvotes;

                _dbContext.SaveChanges();
                return commentToUpdate;
           
                
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to update comment", e);
            }


        }


    }
}
