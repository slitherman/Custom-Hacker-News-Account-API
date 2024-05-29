using Custom_Hacker_News_Account_API.Models.DTOS;

namespace Custom_Hacker_News_Account_API.Repository
{
    public interface IAccountRepository
    {
        AccountInfo CreateAccount(CreateAndUpdateAccountDTO accountDTO);
        AccountInfo DeleteAccount(int id);
        AccountInfo GetAccountById(int id);
        AccountInfo GetAccountByName(string accountName);
        IEnumerable<AccountInfoDTO> GetAccounts();
        AccountInfoDTO UpdateAccount(CreateAndUpdateAccountDTO updatedAccount, int accountId);
        AccountInfo ResetPassWord(string email, string password);
    }
}