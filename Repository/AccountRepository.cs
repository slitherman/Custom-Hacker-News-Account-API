using Custom_Hacker_News_Account_API.Manual_Mapping;
using Custom_Hacker_News_Account_API.Models.DTOS;

namespace Custom_Hacker_News_Account_API.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public readonly AccountDbContext _dbContext;

        public AccountRepository(AccountDbContext DbContext)
        {
            _dbContext = DbContext;
        }

        public IEnumerable<AccountInfoDTO> GetAccounts()
        {
            return _dbContext.AccountInfos.ToList().MapAccountsToDTOs();
        }

        public string HashPassword(string password) {

            //the workfactor 13 results in 8,192 iterations 
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password,13);
        }

  

        public AccountInfo GetAccountByName(string accountName)
        {
            AccountInfo? account = _dbContext.AccountInfos.FirstOrDefault(x => x.Username == accountName);
            if (account == null)
            {
                throw new ArgumentNullException($"Account {accountName} could not be found");
            }
            return account;
        }

        public AccountInfo GetAccountById(int id)
        {
            AccountInfo? account_Id = _dbContext.AccountInfos.FirstOrDefault(x => x.AccountId == id);

            if (account_Id == null)
            {
                throw new ArgumentException($"Could not get the specified account id{id}");
            }
            return account_Id;
        }

        public AccountInfo ResetPassWord(string email, string password) 
        { 
            var account = _dbContext.AccountInfos.FirstOrDefault(x=>x.Email == email);
            if (account == null)
            {
                throw new ArgumentException($"Could not find an account with the email {email}");
            }
            account.Password = password;
            _dbContext.SaveChanges();
            return account;
           

        }
        public AccountInfoDTO UpdateAccount(CreateAndUpdateAccountDTO updatedAccount, int accountId)
        {

            var existingAccount = _dbContext.AccountInfos.FirstOrDefault(x => x.AccountId == accountId);

            if (existingAccount == null)
            {

                throw new Exception($"Account with ID {updatedAccount.AccountId} not found.");
            }


            try
            {
                existingAccount.Firstname = updatedAccount.Firstname;
                existingAccount.Lastname = updatedAccount.Lastname;
                existingAccount.Email = updatedAccount.Email;
                existingAccount.BirthDate = updatedAccount.BirthDate;
                existingAccount.Username = updatedAccount.Username;
                existingAccount.Password = updatedAccount.Password;

                var accountUpdated = existingAccount.MapAccountToDTO();

                _dbContext.SaveChanges();

                return accountUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed up update account", ex);
            }
        }

        public AccountInfo DeleteAccount(int id)
        {

            var account = GetAccountById(id);
            if (account == null)
            {
                throw new ArgumentNullException("The specified account id either couldn't be found or it doesn't exist");
            }
            _dbContext.Remove(account);
            _dbContext.SaveChanges();
            return account;
        }


        public AccountInfo CreateAccount(CreateAndUpdateAccountDTO accountDTO)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                AccountInfoDTO accountInfoDTO = ManualMapper.MapCreateAccountToDTO(accountDTO);

                // Map AccountInfoDTO to AccountInfo entity
                AccountInfo accountInfo = accountInfoDTO.MapDTOToAccount();

               
                _dbContext.AccountInfos.Add(accountInfo);
                _dbContext.SaveChanges();
                transaction.Commit();
                return accountInfo;

            }

            catch (Exception ex)
            {

                transaction.Rollback();
                throw new Exception("Failed to add account to the database.", ex);
            }
        }

    }
}
