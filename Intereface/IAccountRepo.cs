using Custom_Hacker_News_Account_API.Models.DTOS;

namespace Custom_Hacker_News_Account_API.NewFolder
{
    public interface IAccountRepo
    {

        //int AddAccount(AccountInfo accountInfo, AccountAddress accountAddress);

        //IEnumerable<AccountInfoDTO> GetAccounts();
        public IEnumerable<AccountInfoDTO> GetAccounts();
    }
}
