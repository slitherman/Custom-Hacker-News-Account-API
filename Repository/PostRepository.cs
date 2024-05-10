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
       
            var post = _dbContext.Posts.Include(a => a.Account).Include(c => c.Comments).FirstOrDefault(p => p.PostId == id);

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
                throw new ArgumentNullException($"Selected post with the id {id} doesnt exist");
            }
            
            foreach(var commentDTO in post.Comments.ToList())
            {
                var comment = commentDTO.MapDTOToComment();
                _dbContext.Comments.Remove(comment);
            }

            var PostDTO = post.MapDTOToPost();
            _dbContext.Posts.Remove(PostDTO);
            _dbContext.SaveChanges();
            return post;
        }

        public Post UpdatePost(int id, CreateAndUpdatePostDTO postToUpdate)
        {
            var existingPostDTO = GetPostById(id);
            if (existingPostDTO == null)
            {
                throw new ArgumentNullException($"Could not find the specified post with the id {id}");
            }
            try
            {
                existingPostDTO.Title = postToUpdate.Title;
                existingPostDTO.Url = postToUpdate.Url;
                var existingPost = existingPostDTO.MapDTOToPost();
                _dbContext.SaveChanges();
                return existingPost;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed up update post", ex);
            }
        }

        //public Post UpdatePost(int id, CreateAndUpdatePostDTO postToUpdate)
        //{
        //    var existingPost = _dbContext.Posts.Find(id);
        //    if (existingPost == null)
        //    {
        //        throw new ArgumentNullException($"Could not find the specified post with the id {id}");
        //    }
        //    try
        //    {
        //        existingPost.Title = postToUpdate.Title;
        //        existingPost.Url = postToUpdate.Url;
        //        _dbContext.SaveChanges();
        //        return existingPost;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Failed to update post", ex);
        //    }
        //}

        //public IEnumerable<PostDTO> GetAllPosts()

        //{
        //    //_dbContext.ChangeTracker.LazyLoadingEnabled = false;
        //    var dbOutput = _dbContext.Posts
        //   .Include(p => p.Account) // Ensure Account is loaded
        //   .Select(p => p.MapPostToDTO()) // Map each Post entity to PostDTO
        //   .ToList();
        //    if (dbOutput.Count == 0 || dbOutput == null) { 

        //        throw new InvalidOperationException($"The entities {dbOutput.ToString()} could not be retrieved from the database");
        //    }
        //    //dbOutput.MapAccountsToDTOs();
        //    Console.WriteLine($"{dbOutput.ToString()}");
        //    return dbOutput;
        //}


        public IEnumerable<PostDTO> GetAllPosts()

        {

            var posts = _dbContext.Posts.Include(a => a.Account).Include(c=> c.Comments).ToList();
            if (posts.Count == 0 || posts == null)
            {

                throw new InvalidOperationException($"The entities {posts.ToString()} could not be retrieved from the database");
            }

            var postDtos = new List<PostDTO>();

            // Map each post to PostDTO
            foreach (var post in posts)
            {
                var postDto = post.MapPostToDTO();
                postDtos.Add(postDto);
                Console.WriteLine($"{post.ToString()}");
            }

            var mappedPosts = new List<Post>();

            // Map each PostDTO back to Post
            //foreach (var postDto in postDtos)
            //{
            //    var post = postDto.MapDTOToPost();
            //    mappedPosts.Add(post);
            //    Console.WriteLine($"{post.ToString()}");
            //}
            _dbContext.SaveChanges();
            return postDtos;
        }

     
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
