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
     

        public readonly AccountRepository _repo;

        public AccountController(AccountRepository repo)
        {
            _repo = repo;
          
        }

        [HttpGet]
        public ActionResult Get()
        {
            var accounts = _repo.GetAccounts();
            if (accounts == null)
            {
                return NotFound();
            }
            return Ok(accounts);

        }

        [HttpGet("Stats/{id}")] 
        public ActionResult GetStats(int id) {

            var accountStats = _repo.GetAccountStats(id);
            if (accountStats == null)
            {
                return NotFound();
            }
            return Ok(accountStats);        
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var account = _repo.GetAccountById(id);
           
            if(account  == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
          try
            {
                var account = _repo.DeleteAccount(id);
                return Ok(account);
            }
            catch(ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut] 
        public IActionResult Update([FromBody] AccountInfoDTO accountInfo, int id)
        {
            try
            {
                var existingAccount = _repo.GetAccountById(id);
                if (existingAccount == null)
                {
                    return NotFound($"Account with id {id} not found.");
                }
                _repo.UpdateAccount(accountInfo, id);
                return Ok(accountInfo);

            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

      

        [HttpPost("AddAccount")]
        public IActionResult CreateAccount([FromBody] AccountInfoDTO accountDTO)
        {
            try
            {
                _repo.CreateAccount(accountDTO);
                return Created("api/Account/GetAccountById", accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the account: {ex.Message}");
            }
        }

    }

}
