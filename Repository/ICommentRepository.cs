using Custom_Hacker_News_Account_API.Models.DTOS;

namespace Custom_Hacker_News_Account_API.Repository
{
    public interface ICommentRepository
    {
        Comment CreateComment(int accountId, int idPost, CreateAndUpdateCommentDTO CreatedCommentDTO);
        Comment DeleteCommentById(int id);
        Comment GetCommentById(int id);
        Comment UpdateComment(int id, CreateAndUpdateCommentDTO commentToUpdate);
        Comment UpvoteRecieved(int id);
        Comment UpvoteRemoved(int id);
    }
}