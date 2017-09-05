using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ycombinator.data
{
    
        public class UserRepository
        {
            private string _connectionString;
            public UserRepository(string connectionString)
            {
                _connectionString = connectionString;
            }

            public string AddUser(User user)
            {
                if (GetUserByEmail(user.Email) != null)
                {
                    return null;
                }
                //this was done in controller
                //user.PasswordSalt = PasswordHelper.GenerateSalt();
                //user.PasswordHash = PasswordHelper.HashPassword(password, user.PasswordSalt);
                using (DBContextDataContext context = new DBContextDataContext(_connectionString))
                {
                    context.Users.InsertOnSubmit(user);
                    context.SubmitChanges();
                    return "User Added";
                }

            }

            public User GetUser(string Email, string Password)
            {
                User user = GetUserByEmail(Email);
                if (user == null)
                {
                    return null;
                }
                if (!PasswordHelper.PasswordMatch(Password, user.PasswordSalt, user.PasswordHash))
                {
                    return null;
                }
                return user;
            }
            public User GetUserByEmail(string email)
            {
                using (DBContextDataContext context = new DBContextDataContext(_connectionString))
                {
                    return context.Users.FirstOrDefault(U => U.Email == email);
                }
            }

        }
    }
    
