using System.Collections.Generic;

namespace testmvc.Models
{
    public interface IUser
    {



        public  IEnumerable<User> GetUsers();
       
        public  User GetUserById(int? id);
       
        public  int AddUser(User User);
       
        public  int UpdateUser(User User);
       
        public  int DeleteUser(int id);

        public List<User> GetUserByUserName(string username);

        public bool VerifyUser(string password, string dbpassword);

        public bool ChangePassWord(string password, int id); 
      
    }
}
