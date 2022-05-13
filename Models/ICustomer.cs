using System.Collections.Generic;

namespace testmvc.Models
{
    public interface ICustomer
    {
        int AddCustomer(Customer Customer);
        int DeleteCustomer(int id, int userid);
        string GetCountryNameById(int CountryId);
        List<Country> GetCountryNames();
        Customer GetCustomerById(int? id, int userid);
        IEnumerable<Customer> GetCustomers(FilterDto filter);
        string GetStateNameById(int StateId);
        List<State> GetStateNames(int countryid);
        int UpdateCustomer(Customer Customer);
        int UploadFile(Attachments attachment);
        List<Attachments> GetAttachments(int customerid);
        int DeleteCustomerDocument(int id);
        Attachments GetDocumentById(int id);
       List<Chart> GetChartData(int userid);
        List<Attachments> GetALLAttachments(int userid);


    }
}