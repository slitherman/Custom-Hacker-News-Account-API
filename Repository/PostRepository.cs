using Custom_Hacker_News_Account_API.Manual_Mapping;
using Custom_Hacker_News_Account_API.Models;
using Custom_Hacker_News_Account_API.Models.DTOS;
using Microsoft.EntityFrameworkCore;

namespace Custom_Hacker_News_Account_API.Repository
{
    public class PostRepository
    {
        public readonly AccountDbContext _dbContext;
        public AccountRepository _accRepo { get; set; }

        public PostRepository(AccountDbContext DbContext )
        {
            _dbContext = DbContext;
            
        }

        public Post CreatePost(Post post)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                int method = 1;
                 _dbContext.Posts.Add(post);
                _accRepo.modifyAccountStats(method, post.AccountId);
                transaction.Commit();
                return post;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Failed to create post and update account statistics.", ex);
            }

        }
        public Post GetPostById(int id)
        {
            var post = _dbContext.Posts.FirstOrDefault(p => p.PostId == id);

            if (post == null)
            {
                throw new ArgumentNullException($"Could not get the specified post {post}");
            }

            return post;
        }
        public Post DeletePost(int id)
        {
            var post = GetPostById(id);
            if (post == null)
            {
                throw new ArgumentNullException($"Selected post with the id {id} doesnt exist");
            }
            int method = 2;
            _accRepo.modifyAccountStats(method, post.AccountId);
            _dbContext.Posts.Remove(post);
            _dbContext.SaveChanges();
            return post;
        }
        //public Post UpdatePost(int id)
        //{

        //}
        public IEnumerable<Post> GetAllPosts()
        {
            return _dbContext.Posts.ToList();
        }
        public void UpvoteRecieved(int id)
        {
            var post = GetPostById(id);
            if (post == null)
            {
                throw new ArgumentNullException($"Selected post with the id {id} doesnt exist");
            }
            int method = 3;
            _accRepo.modifyAccountStats(method, post.AccountId);
            _dbContext.SaveChanges();
        }
    }
}
