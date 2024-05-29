using Custom_Hacker_News_Account_API.Models.DTOS;
using Custom_Hacker_News_Account_API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Custom_Hacker_News_Account_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {


        public readonly IAccountRepository _accountRepo;
        public readonly IPostRepository _postRepo;
        public readonly ICommentRepository _commentRepo;

        public HackerNewsController(IAccountRepository accountRepo, IPostRepository postRepo, ICommentRepository commentRepo)
        {
            _accountRepo = accountRepo;
            _postRepo = postRepo;
            _commentRepo = commentRepo;

        }

        [HttpGet("Accounts")]
        public IActionResult GetAllAccounts()
        {
            var accounts = _accountRepo.GetAccounts();
            if (accounts == null)
            {
                return NotFound();
            }
            return Ok(accounts);

        }

        [HttpGet("ByName")]
        public IActionResult GetAccountByName([FromQuery] string name) 
        { 
            var account = _accountRepo.GetAccountByName(name);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }


        //[HttpGet("AccountStats/{id}")] 
        //public ActionResult GetAccStats(int id) {

        //    var accountStats = _accountRepo.GetAccountStats(id);
        //    if (accountStats == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(accountStats);        
        //}

        [HttpGet("Account/{id}")]
        public IActionResult GetAccById(int id)
        {
            var account = _accountRepo.GetAccountById(id);
           
            if(account  == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpDelete("DeleteAccount{id}")]
        public IActionResult DeleteAcc(int id)
        {
          try
            {
                var account = _accountRepo.DeleteAccount(id);
                return Ok(account);
            }
            catch(ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("UpdateAccount/{id}")] 
        public IActionResult UpdateAcc(int id, [FromBody] CreateAndUpdateAccountDTO accountInfo)
        {
            try
            {
                var existingAccount = _accountRepo.GetAccountById(id);
                if (existingAccount == null)
                {
                    return NotFound($"Account with id {id} not found.");
                }
                _accountRepo.UpdateAccount(accountInfo, id);
                return Ok(accountInfo);

            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("CreateAccount")]
        public IActionResult CreateAcc([FromBody] CreateAndUpdateAccountDTO accountDTO)
        {
            try
            {
                _accountRepo.CreateAccount(accountDTO);
                return Created("api/Account/GetAccount", accountDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while creating the account: {ex.Message}");
            }
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("CreatePost")]
        public IActionResult CreatePost([FromBody] CreateAndUpdatePostDTO postDTO)
        {
            try
            {
                _postRepo.CreatePost(postDTO);
                return Created("api/Post/GetPostById", postDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while creating the post {postDTO}: {ex.Message}");
            }
        }
        [HttpPut("UpdatePost/{id}")]
        public IActionResult UpdatePost(int id, [FromBody] CreateAndUpdatePostDTO postInfo)
        {
            try
            {
                var existingPost = _postRepo.GetPostById(id);
                if (existingPost == null)
                {
                    return NotFound($"Post with id {id} not found.");
                }

                var updatedPost = _postRepo.UpdatePost(id, postInfo);
                return Ok(updatedPost); 

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeletePost/{id}")]
        public IActionResult DeletePost(int id)
        {
            try
            {
                var post = _postRepo.DeletePost(id);
                return Ok(post);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("Post")]
        public IActionResult GetPostById([FromQuery] int id)
        {
            var post = _postRepo.GetPostById(id);

            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpGet("Posts")]
        public IActionResult GetAllPosts()
        {
            var posts = _postRepo.GetAllPosts();
            if (posts == null)
            {
                return NotFound();
            }
            return Ok(posts);

        }
        [HttpGet("PopularPosts")]
        public IActionResult PopularPosts([FromQuery] int MinComments)
        {
            var posts = _postRepo.GetMostPopularPosts(MinComments);
            if(posts == null)
            {
                return NotFound();
            }
            return Ok(posts);
        }

        [HttpPut("UpdateComment/{id}")]
        public IActionResult UpdateComment(int id, [FromBody] CreateAndUpdateCommentDTO commentToUpdate)
        {
            try
            {
                var updatedComment = _commentRepo.UpdateComment(id, commentToUpdate);
                return Ok(updatedComment);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteComment/{id}")]
        public IActionResult DeleteComment(int id)
        {
            try
            {
                var comment = _commentRepo.DeleteCommentById(id);
                return Ok(comment);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Comment/{id}")]
        public IActionResult GetCommentById(int id)
        {
            var comment = _commentRepo.GetCommentById(id);

            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }
     // Upvote a comment
     [HttpGet("AddUpvoteFromComment/{id}")]
     public IActionResult UpvoteComment(int id)
     {
         try
         {
             _commentRepo.UpvoteRecieved(id);
             return Ok($"Upvote added to comment ID {id}");
         }
         catch (ArgumentNullException ex)
         {
             return NotFound(ex.Message);
         }
         catch (Exception ex)
         {
             return StatusCode(500, $"Internal server error: {ex.Message}");
         }
     }
     // Upvote a post
     [HttpGet("AddUpvoteFromPost/{id}")]
     public IActionResult UpvotePost(int id)
     {
         try
         {
             var post = _postRepo.UpvoteRecieved(id);
             if(post == null)
             {
                 return NotFound($"Cannot find post with the id:{id}");
             }
             return Ok(post);
         }
         catch (ArgumentNullException ex)
         {
             return NotFound(ex.Message);
         }
         catch (Exception ex)
         {
             return StatusCode(500, $"Internal server error: {ex.Message}");
         }
     }

     // Remove upvote from a comment
     [HttpGet("RemoveUpvoteFromComment/{id}")]
     public IActionResult RemoveUpvoteFromComment(int id)
     {
         try
         {
             _commentRepo.UpvoteRemoved(id);
             return Ok($"Upvote removed from comment ID {id}");
         }
         catch (ArgumentNullException ex)
         {
             return NotFound(ex.Message);
         }
         catch (Exception ex)
         {
             return StatusCode(500, $"Internal server error: {ex.Message}");
         }
     }

     // Remove upvote from a post
     [HttpGet("RemoveUpvoteFromPost/{id}")]
     public IActionResult RemoveUpvoteFromPost(int id)
     {
         try
         {
             _postRepo.UpvoteRemoved(id);
             return Ok($"Upvote removed from post ID {id}");
         }
         catch (ArgumentNullException ex)
         {
             return NotFound(ex.Message);
         }
         catch (Exception ex)
         {
             return StatusCode(500, $"Internal server error: {ex.Message}");
         }
     }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("CreateComment/{accountId}/{postId}")]
        public IActionResult CreateComment(int accountId, int postId ,[FromBody] CreateAndUpdateCommentDTO commentDTO)
        {
            try
            {
                _commentRepo.CreateComment(accountId, postId,commentDTO);
                return Created("api/Comment/GetCommentById", commentDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while creating the comment: {ex.Message} {ex.InnerException}");
            }
        }


    }

}
