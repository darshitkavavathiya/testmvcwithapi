using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using System;
using BCrypt.Net;
using Microsoft.Extensions.Options;

namespace testmvc.Models
{
    public class UserServices : IUser
    {
        


     

        private readonly ConnectionStrings _cs;
        public readonly string ConnectionString = null;
        public UserServices(IOptions<ConnectionStrings> cs)
        {
            
           _cs = cs.Value;

            ConnectionString = _cs.DefaultConnection;

        }

        







        public IEnumerable<User> GetUsers()
        {
            

            List<User> UsersList = new List<User>();

            using (IDbConnection con = new SqlConnection(ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                UsersList = con.Query<User>("GetAllUsers").ToList();
                con.Close();
            }

            return UsersList;
        }

        public User GetUserById(int? id)
        {
            User user = new User();
            if (id == null)
                return user;

            using (IDbConnection con = new SqlConnection(ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@id", id);
                user = con.Query<User>("GetUserById", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return user;
        }





        public int AddUser(User User)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();



                string pass = BCrypt.Net.BCrypt.HashPassword(User.Password);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@username ", User.UserName);
                parameters.Add("@firstname", User.FirstName);
                parameters.Add("@lastname", User.LastName);
                parameters.Add("@email", User.Email);
                parameters.Add("@password", pass);
                parameters.Add("@mobile", User.Mobile);
                parameters.Add("@isactive", User.IsActive);
                rowAffected = con.Execute("InsertUser", parameters, commandType: CommandType.StoredProcedure);
                con.Close();
            }

            return rowAffected;
        }

        public int UpdateUser(User User)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string pass = BCrypt.Net.BCrypt.HashPassword(User.Password);
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", User.UserId);
                parameters.Add("@username ", User.UserName);
                parameters.Add("@firstname", User.FirstName);
                parameters.Add("@lastname", User.LastName);
                parameters.Add("@email", User.Email);
                parameters.Add("@password", pass);
                parameters.Add("@mobile", User.Mobile);
                parameters.Add("@isactive", User.IsActive);
                rowAffected = con.Execute("UpdateUser", parameters, commandType: CommandType.StoredProcedure);

                con.Close();
            }

            return rowAffected;
        }

        public int DeleteUser(int id)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                rowAffected = con.Execute("DeleteUser", parameters, commandType: CommandType.StoredProcedure);
                con.Close();
            }

            return rowAffected;
        }

        public List<User> GetUserByUserName(string username)
        {
            List<User> user = new List<User>();

            if (string.IsNullOrEmpty(username))
                return user;

            using (IDbConnection con = new SqlConnection(ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@username", username);
                user = con.Query<User>("GetUserByUserName", parameters, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return user;
        }


        public bool VerifyUser(string password, string dbpassword)
        {
            if (password == null)
                return false;


            return BCrypt.Net.BCrypt.Verify(password, dbpassword);
        }

        public bool ChangePassWord(string password, int id)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(ConnectionString))
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                string pass = BCrypt.Net.BCrypt.HashPassword(password);
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@password", pass);
                rowAffected = con.Execute("UpdateUserPassword", parameters, commandType: CommandType.StoredProcedure);
                con.Close();
            }
            if (rowAffected == 0) { return false; }


            return true;

        }









    }
}
