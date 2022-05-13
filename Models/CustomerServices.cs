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
    public class CustomerServices : ICustomer
    {


        
        private readonly ConnectionStrings _cs;
        public CustomerServices(IOptions<ConnectionStrings> cs)
        {

            _cs = cs.Value;

        }
        public List<Country> GetCountryNames()
        {

            List<Country> Country = new List<Country>();
            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                 Country = con.Query<Country>("GetCountrys").ToList();



            }

            return Country;
        }




        public List<State> GetStateNames(int countryid)
        {
            List<State> states = new List<State>();

            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();




                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@countryid", countryid);

                states = con.Query<State>("GetStatebyCountryid", parameters, commandType: CommandType.StoredProcedure).ToList();


            }
            return states;
        }



        public IEnumerable<Customer> GetCustomers(FilterDto filter)
        {
            List<Customer> Custlist = new List<Customer>();

            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@createdby", filter.CreatedBy);
                parameters.Add("@firstname", filter.FirstName);
                parameters.Add("@lastname", filter.LastName);
                parameters.Add("@email", filter.Email);
                parameters.Add("@mobile", filter.Mobile);
                parameters.Add("@isactive", filter.IsActive);
                parameters.Add("@countryid", filter.Countryid);
                parameters.Add("@stateid", filter.Stateid);

    

                Custlist = con.Query<Customer>("GetCustomers", parameters, commandType: CommandType.StoredProcedure).ToList();

            }


            return Custlist;
        }


        public Customer GetCustomerById(int? id, int userid)
        {
            Customer Customer = new Customer();
            if (id == null)
                return Customer;

            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@id", id);
                parameter.Add("@createdby", userid);
                Customer = con.Query<Customer>("GetCustomerById", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

            }

            Customer.StateName = GetStateNameById((int)Customer.Stateid);
            Customer.CountryName = GetCountryNameById((int)Customer.Countryid);


            return Customer;
        }








        public int AddCustomer(Customer Customer)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();



                string pass = BCrypt.Net.BCrypt.HashPassword(Customer.Password);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@firstname", Customer.FirstName);
                parameters.Add("@lastname", Customer.LastName);
                parameters.Add("@email", Customer.Email);
                parameters.Add("@password", pass);
                parameters.Add("@mobile", Customer.Mobile);
                parameters.Add("@createdby", Customer.CreatedBy);
                parameters.Add("@isactive", Customer.IsActive);
                parameters.Add("@address", Customer.Address);
                parameters.Add("@countryid", Customer.Countryid);
                parameters.Add("@stateid", Customer.Stateid);
                rowAffected = con.Execute("InsertCustomer", parameters, commandType: CommandType.StoredProcedure);
                con.Close();
            }

            return rowAffected;
        }



        public int DeleteCustomer(int id, int userid)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@createdby", userid);
                rowAffected = con.Execute("DeleteCustomer", parameters, commandType: CommandType.StoredProcedure);
                con.Close();
            }

            return rowAffected;
        }





        public int UpdateCustomer(Customer Customer)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();





                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", Customer.CustomerId);
                parameters.Add("@firstname", Customer.FirstName);
                parameters.Add("@lastname", Customer.LastName);
                parameters.Add("@email", Customer.Email);

                parameters.Add("@mobile", Customer.Mobile);
                parameters.Add("@modifyby", Customer.ModifiedBy);
                parameters.Add("@isactive", Customer.IsActive);
                parameters.Add("@address", Customer.Address);
                parameters.Add("@countryid", Customer.Countryid);
                parameters.Add("@stateid", Customer.Stateid);
                rowAffected = con.Execute("UpdateCustomer", parameters, commandType: CommandType.StoredProcedure);

            }

            return rowAffected;
        }


        public string GetStateNameById(int StateId)
        {
            string StateName = null;
            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@stateid", StateId);

                State state = con.Query<State>("GetStateNamebyId", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                StateName = state.Statename;

            }

            return StateName;
        }

        public string GetCountryNameById(int CountryId)
        {
            string Countryname = null;
            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@countryid", CountryId);

                Country country = con.Query<Country>("GetCountryNamebyId", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                Countryname = country.Countryname;

            }

            return Countryname;
        }

        public int UploadFile(Attachments attachment)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();




                DynamicParameters parameters = new DynamicParameters();
              
                parameters.Add("@customerid",attachment.CustomerId);
                parameters.Add("@documentname",attachment.DocumentName);
                parameters.Add("@documenttype",attachment.DocumentType);
                parameters.Add("@attachment",attachment.Attachment);
                parameters.Add("@uploadedbyid", attachment.UploadedBy);
                rowAffected = con.Execute("UploadFile", parameters, commandType: CommandType.StoredProcedure);
                con.Close();
            }

            return rowAffected;
        }

        public List<Attachments> GetAttachments(int customerid)
        {
            List<Attachments> Attachment = new List<Attachments>();

            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@customerId", customerid);

                Attachment = con.Query<Attachments>("GetCustomerDocuments", parameters, commandType: CommandType.StoredProcedure).ToList();

            }

            return Attachment;
        }
        public int DeleteCustomerDocument(int id)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                rowAffected = con.Execute("DeleteDocument", parameters, commandType: CommandType.StoredProcedure);
                con.Close();
            }

            return rowAffected;
        }


        public Attachments GetDocumentById(int id)
        {
            Attachments document = new Attachments();
            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
             document = con.Query<Attachments>("GetDocument", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return document;
        }

        public List<Chart> GetChartData(int userid)
        {
            List<Chart> chartdata = new List<Chart>();
            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@userid", userid);
                chartdata = con.Query<Chart>("GetChartData", parameters, commandType: CommandType.StoredProcedure).ToList();
            }

            return chartdata;
        }




       public List<Attachments> GetALLAttachments(int userid)
        {
            List<Attachments> Attachment = new List<Attachments>();

            using (IDbConnection con = new SqlConnection(_cs.DefaultConnection))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@userid", userid);

                Attachment = con.Query<Attachments>("GetAllDocumentsforUser", parameters, commandType: CommandType.StoredProcedure).ToList();

            }

            return Attachment;
        }



    }
}
