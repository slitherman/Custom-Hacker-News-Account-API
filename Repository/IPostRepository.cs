using Custom_Hacker_News_Account_API.Models.DTOS;

namespace Custom_Hacker_News_Account_API.Repository
{
    public interface IPostRepository
    {
  

        Post CreatePost(CreateAndUpdatePostDTO postToAdd);
        PostDTO DeletePost(int id);
        IEnumerable<PostDTO> GetAllPosts();
        PostDTO GetPostById(int id);
        PostDTO UpdatePost(int id, CreateAndUpdatePostDTO postToUpdate);
        public IEnumerable<PostDTO> GetMostPopularPosts(int MinComments);
    }
}