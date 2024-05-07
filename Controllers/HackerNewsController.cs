using Custom_Hacker_News_Account_API.Manual_Mapping;
using Custom_Hacker_News_Account_API.Models;
using Custom_Hacker_News_Account_API.Models.DTOS;
using Custom_Hacker_News_Account_API.NewFolder;
using Custom_Hacker_News_Account_API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Custom_Hacker_News_Account_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
     

        public readonly AccountRepository _accountRepo;
        public readonly PostRepository _postRepo;
        public readonly CommentRepository _commentRepo;

        public HackerNewsController(AccountRepository accountRepo, PostRepository postRepo, CommentRepository commentRepo)
        {
            _accountRepo = accountRepo;
            _postRepo = postRepo;
            _commentRepo = commentRepo;
          
        }

        [HttpGet("GetAllAccounts")]
        public ActionResult GetAllAccounts()
        {
            var accounts = _accountRepo.GetAccounts();
            if (accounts == null)
            {
                return NotFound();
            }
            return Ok(accounts);

        }

        [HttpGet("AccountStats/{id}")] 
        public ActionResult GetAccStats(int id) {

            var accountStats = _accountRepo.GetAccountStats(id);
            if (accountStats == null)
            {
                return NotFound();
            }
            return Ok(accountStats);        
        }

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

        [HttpDelete("DeleteAccount")]
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

        [HttpPut("UpdateAccount")] 
        public IActionResult UpdateAcc([FromBody] AccountInfoDTO accountInfo, int id)
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
        [HttpPost("CreatedAccount")]
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
        [HttpPost("CreatedPost")]
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
        [HttpPut("UpdatePost")]
        public IActionResult UpdatePost(int id, [FromBody] CreateAndUpdatePostDTO postInfo)
        {
            try
            {
                var existingPost = _postRepo.GetPostById(id);
                if (existingPost == null)
                {
                    return NotFound($"Post with id {id} not found.");
                }
                _postRepo.UpdatePost(id, postInfo);
                return Ok(postInfo);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeletePost")]
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
        [HttpGet("Post/{id}")]
        public IActionResult GetPostById(int id)
        {
            var post = _postRepo.GetPostById(id);

            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpGet("GetAllPosts")]
        public ActionResult GetAllPosts()
        {
            var posts = _postRepo.GetAllPosts();
            if (posts == null)
            {
                return NotFound();
            }
            return Ok(posts);

        }
        [HttpPut("UpdatedComment/{id}")]
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

        [HttpDelete("DeleteComment")]
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

        [HttpPost("CreatedComment")]
        public IActionResult CreateComment([FromBody] CreateAndUpdateCommentDTO commentDTO)
        {
            try
            {
                _commentRepo.CreateComment(commentDTO);
                return Created("api/Comment/GetCommentById", commentDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the comment: {ex.Message}");
            }
        }


    }

}
