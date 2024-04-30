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


        //public IEnumerable<AccountInfoDTO> GetAccounts()
        //{
        //    var accounts = _dbContext.AccountInfos.Include(ai => ai.AccountAddresses).ToList();

        //    var accountDTOs = accounts.Select(account =>
        //        new AccountInfoDTO
        //        {
        //            AccountId = account.AccountId,
        //            Username = account.Username,
        //            Firstname = account.Firstname,
        //            Lastname = account.Lastname,
        //            BirthDate = account.BirthDate,
        //            Email = account.Email,
        //            Password = account.Password,
        //            IsBanned = account.IsBanned,
        //            Addresses = account.AccountAddresses.Select(address =>
        //                new AccountAddressDTO
        //                {
        //                    AddressId = address.AddressId,
        //                    City = address.City,
        //                    Zip = address.Zip,
        //                    Street = address.Street,
        //                    Country = address.Country
        //                }).ToList()
        //        });

        //    return accountDTOs;
        //}


        public IEnumerable<AccountInfoDTO> GetAccounts()
        {
         return _dbContext.AccountInfos.Include(a=> a.AccountStatistic).Select(a => a.AccountInfoAsDTO()).ToList();   
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
            var accountDTO = account.AccountInfoAsDTO();

            return accountDTO;
        }


        public AccountInfo GetAccountById(int id)
        {
            AccountInfo? account_Id =_dbContext.AccountInfos.FirstOrDefault( x=> x.AccountId == id);
            if (account_Id == null) 
            {
                throw new ArgumentException($"Could not get the specified account id{id}");
            }
            return account_Id;
        }

        public AccountInfo UpdateAccount(AccountInfoDTO updatedAccount, int accountId) 
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

                AccountInfo accountInfo =  updatedAccount.AccountInfoAsDTOReverse();

                _dbContext.SaveChanges();

                return accountInfo;
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
       

        public void CreateAccount(AccountInfoDTO accountDTO)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                AccountInfo accountInfo = accountDTO.AccountInfoAsDTOReverse();
            
            _dbContext.AccountInfos.Add(accountInfo);
            _dbContext.SaveChanges();
            transaction.Commit();

            }
            
            catch (Exception ex)
            {
              
                transaction.Rollback();
                throw new Exception("Failed to add account to the database.", ex);
            }
        }

        public void CreatePostAndUpdateAccountStatistics(Post post)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                
                _dbContext.Posts.Add(post);
                _dbContext.SaveChanges();
                var account = _dbContext.AccountInfos.Include(a =>
                a.AccountStatistic).FirstOrDefault(
                    a => a.AccountId == post.AccountId);


                if (account != null)
                {
                    account.AccountStatistic.SubmissionCount++;
                    account.AccountStatistic.UpvotesReceived++;
                    account.AccountStatistic.Karma = account.AccountStatistic.UpvotesReceived * 2;
                    _dbContext.SaveChanges();

                 
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Failed to create post and update account statistics.", ex);
            }
        }


    }
}
