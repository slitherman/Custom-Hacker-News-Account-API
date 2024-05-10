using Custom_Hacker_News_Account_API.Manual_Mapping;
using Custom_Hacker_News_Account_API.Models;
using Custom_Hacker_News_Account_API.Models.DTOS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Hosting;

namespace Custom_Hacker_News_Account_API.Repository
{
    public class PostRepository
    {
        public readonly AccountDbContext _dbContext;
        public AccountRepository _accRepo { get; set; }

        public PostRepository(AccountDbContext DbContext, AccountRepository repo )
        {
            _dbContext = DbContext;
            _accRepo = repo;
        }

        public Post CreatePost(CreateAndUpdatePostDTO postToAdd)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                int method = 1;

                if (string.IsNullOrEmpty(postToAdd.Url))
                {
                    postToAdd.Url = "default-url";
                }
                var account = _accRepo.GetAccountById(postToAdd.AccountId);
                // Create a new Post object
                var post = new Post
                {
                    Title = postToAdd.Title,
                    Url = postToAdd.Url,
                    Username = account.Username,
                    AccountId = postToAdd.AccountId // Associate post with a specific account
                };

                // Add the Post object to the _dbContext.Posts DbSet
                var dbRes = _dbContext.Posts.Add(post);
                Console.WriteLine($"{dbRes}");

                // Save changes to the database
                _dbContext.SaveChanges();
                transaction.Commit();

                return post; // Return the created Post object

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Failed to create post and update account statistics.", ex);
            }
        }

        //public void MapUsernames()
        //{


        //    using(var context = new AccountDbContext())
        //    {
        //        var usernameAcc = context.AccountInfos.Select(x => x.Username);
        //        var varPostUserNameToUpdate = context.Posts;

        //        foreach (var post in varPostUserNameToUpdate)
        //        {
        //            var matchingPostUsername = usernameAcc.FirstOrDefault(u => u == post.Username);
        //            if (matchingPostUsername != null)
        //            {
        //                post.Username = matchingPostUsername;
        //            }

        //        }
        //        context.SaveChanges();

        //    }


        //}

        public PostDTO GetPostById(int id)
        {
       
            var post = _dbContext.Posts.Include(a => a.Account).Include(c => c.Comments).
                FirstOrDefault(p => p.PostId == id);

            if (post == null)
            {
                throw new ArgumentNullException($"Could not get the specified post {post}");
            }
            var postDTO = post.MapPostToDTO();
            return postDTO;
        }

        public PostDTO DeletePost(int id)
        {
            var post = GetPostById(id);
            if (post == null)
            {
                throw new ArgumentNullException($"Selected post with the id {id} doesn't exist");
            }

            // Remove comments
            var comments = _dbContext.Comments.Where(c => c.PostId == id).ToList();
            foreach (var comment in comments)
            {
                _dbContext.Comments.Remove(comment);
            }

            // Remove the post
            var postEntity = _dbContext.Posts.Find(id);
            if (postEntity != null)
            {
                _dbContext.Posts.Remove(postEntity);
            }

            _dbContext.SaveChanges();
            return post;
        }






        public PostDTO UpdatePost(int id, CreateAndUpdatePostDTO postToUpdate)
        {
            // Fetch the entity from the database
            var existingPost = _dbContext.Posts.Find(id);
            if (existingPost == null)
            {
                throw new ArgumentNullException($"Could not find the specified post with the id {id}");
            }

            try
            {
                // Update the entity with values from the DTO
                existingPost.Title = postToUpdate.Title;
                existingPost.Url = postToUpdate.Url;

                // Mark the entity as modified
                _dbContext.Posts.Update(existingPost);

                // Save the changes to the database
                _dbContext.SaveChanges();

                // Map the updated entity back to a DTO
                var updatedPostDTO = existingPost.MapPostToDTO(); // Assuming you have a method to map Post to PostDTO

                return updatedPostDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update post", ex);
            }
        }

        public IEnumerable<PostDTO> GetAllPosts()
        {
            var posts = _dbContext.Posts
                .Include(p => p.Account)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Account)
                .ToList(); // Include Account in Comments

            if (posts == null || posts.Count == 0)
            {
                throw new InvalidOperationException("No posts found in the database.");
            }

            var postDtos = new List<PostDTO>();

            // Map each post to PostDTO
            foreach (var post in posts)
            {
                var postDto = post.MapPostToDTO();
                postDtos.Add(postDto);
                Console.WriteLine($"{post.ToString()}");
            }

            _dbContext.SaveChanges();
            return postDtos;
        }


        //public IEnumerable<PostDTO> GetAllPosts()

        //{

        //    var posts = _dbContext.Posts
        //    .Include(p => p.Account)
        //    .Include(p => p.Comments)
        //    .ThenInclude(c => c.Account).ToList(); // Include Account in Comments

        //    if (posts.Count == 0 || posts == null)
        //    {

        //        throw new InvalidOperationException($"The entities {posts.ToString()} could not be retrieved from the database");
        //    }

        //    var postDtos = new List<PostDTO>();

        //    // Map each post to PostDTO
        //    foreach (var post in posts)
        //    {
        //        var postDto = post.MapPostToDTO();
        //        postDtos.Add(postDto);
        //        Console.WriteLine($"{post.ToString()}");
        //    }


        //    var mappedPosts = new List<Post>();


        //    _dbContext.SaveChanges();
        //    return postDtos;
        //}


        //public void UpvoteRecieved(int id)
        //{
        //    var post = GetPostById(id);
        //    if (post == null)
        //    {
        //        throw new ArgumentNullException($"Selected post with the id {id} doesnt exist");
        //    }
        //    int method = 3;
        //    _accRepo.modifyAccountStats(method, post.AccountId);
        //    _dbContext.SaveChanges();
        //}
    }
}
