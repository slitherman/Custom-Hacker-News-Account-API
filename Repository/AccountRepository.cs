using Custom_Hacker_News_Account_API.Manual_Mapping;
using Custom_Hacker_News_Account_API.Models;
using Custom_Hacker_News_Account_API.Models.DTOS;
using Custom_Hacker_News_Account_API.NewFolder;
using Microsoft.EntityFrameworkCore;

namespace Custom_Hacker_News_Account_API.Repository
{
    public class AccountRepository: IAccountRepo
    {
        public readonly AccountDbContext _dbContext;

        public AccountRepository(AccountDbContext DbContext) { 
            _dbContext = DbContext;
        }

        public IEnumerable<AccountInfoDTO> GetAccounts()
        {
         return _dbContext.AccountInfos.Include(a=> a.AccountStatistic).Select(a => a.MapAccountToDTO()).ToList();   
        }   

        public AccountInfoDTO GetAccountStats(int id)
        {
            var account = _dbContext.AccountInfos
                .Include(a => a.AccountStatistic)  
                .FirstOrDefault(a => a.AccountId == id);

            if (account == null)
            {
                throw new ArgumentException($"Could not find the specified account with id {id}");
            }
            var accountDTO = account.MapAccountToDTO();

            return accountDTO;
        }


        public AccountInfo GetAccountById(int id)
        {
            AccountInfo? account_Id =_dbContext.AccountInfos.FirstOrDefault( x => x.AccountId == id);
            if (account_Id == null) 
            {
                throw new ArgumentException($"Could not get the specified account id{id}");
            }
            return account_Id;
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

        public void CalculateKarma(AccountInfo account)
        {
            account.AccountStatistic.Karma = (account.AccountStatistic.UpvotesReceived +
                                              account.AccountStatistic.SubmissionCount + 
                                              account.AccountStatistic.CommentCount) * 2;
        }
        public void modifyAccountStats(int method, int accId)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                _dbContext.SaveChanges();
                var account = _dbContext.AccountInfos.Include(a =>
                a.AccountStatistic).FirstOrDefault(
                    a => a.AccountId == accId);


                if (account != null)
                {
                    switch (method)
                    {
                        case 1: //Creating a post increases these parameters
                            account.AccountStatistic.SubmissionCount++;
                            account.AccountStatistic.UpvotesReceived++;
                            CalculateKarma(account);
                            break;
                        case 2: //Removing a Post reduces the users SubmissionCount and Upvotes recieved by 1 - To prevent Upvoteboosting
                            account.AccountStatistic.SubmissionCount--;
                            account.AccountStatistic.UpvotesReceived--;
                            CalculateKarma(account);
                            break;
                        case 3: //This case gets used when a user upvotes a post - Ups upvotesrecieves with 1, and calculates the new karma
                            account.AccountStatistic.UpvotesReceived++;
                            CalculateKarma(account);
                            break;
                        case 4: //Creating a comment increases these parameters
                            account.AccountStatistic.CommentCount++;
                            account.AccountStatistic.UpvotesReceived++;
                            CalculateKarma(account);
                            break;
                        case 5: //Removing a Comment reduces the users CommentCount and Upvotes recieved by 1 - To prevent Upvoteboosting
                            account.AccountStatistic.CommentCount--;
                            account.AccountStatistic.UpvotesReceived--;
                            CalculateKarma(account);
                            break;

                        default:
                            throw new ArgumentException("Invalid case: " + method);
                            
                    }
                    _dbContext.SaveChanges();
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Failed update account statistics.", ex);
            }
        }


    }
}
