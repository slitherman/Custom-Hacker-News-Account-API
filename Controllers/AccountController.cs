using Custom_Hacker_News_Account_API.Manual_Mapping;
using Custom_Hacker_News_Account_API.Models.DTOS;
using Custom_Hacker_News_Account_API.NewFolder;
using Custom_Hacker_News_Account_API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Custom_Hacker_News_Account_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
     

        public readonly AccountRepository _accountRepo;
        public readonly PostRepository _postRepo;
        //public readonly CommentRepository _commentRepo;

        public AccountController(AccountRepository accountRepo, PostRepository postRepo)
        {
            _accountRepo = accountRepo;
            _postRepo = postRepo;
          
        }

        [HttpGet]
        public ActionResult GetAllAccounts()
        {
            var accounts = _accountRepo.GetAccounts();
            if (accounts == null)
            {
                return NotFound();
            }
            return Ok(accounts);

        }

        [HttpGet("Stats/{id}")] 
        public ActionResult GetAccStats(int id) {

            var accountStats = _accountRepo.GetAccountStats(id);
            if (accountStats == null)
            {
                return NotFound();
            }
            return Ok(accountStats);        
        }

        [HttpGet("{id}")]
        public IActionResult GetAccById(int id)
        {
            var account = _accountRepo.GetAccountById(id);
           
            if(account  == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpDelete]
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

        [HttpPut] 
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

      

        [HttpPost("Created")]
        public IActionResult CreateAcc([FromBody] CreateAccountDTO accountDTO)
        {
            try
            {
                _accountRepo.CreateAccount(accountDTO);
                return Created("api/Account/GetAccountById", accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the account: {ex.Message}");
            }
        }

    }

}
