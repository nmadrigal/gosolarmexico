using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ServiceApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool UserAuthentication(string email, string password);

        [OperationContract]
        List<Customer> GetReferralsByCustomerEmail(string customerEmail);

        [OperationContract]
        void NewReferral(Customer customer, Address address);

        [OperationContract]
        bool CustomerMailExists(string userMail);

        [OperationContract]
        List<Address> GetUserAddresses(string userEmail);

        [OperationContract]
        List<Address> GetAllAddresses();

        [OperationContract]
        List<string> GetUserRoles(string userMail);

        [OperationContract]
        List<Customer> GetAllCutomers();

        [OperationContract]
        void UpdateCustomerStatus(Dictionary<int, int> customerList);

        [OperationContract]
        void ChangePasswordByUserEmail(string userMail, string password);

        [OperationContract]
        Customer GetCustomerById(int customerId);

        [OperationContract]
        Address GetAddressByOwnerId(int ownerId);
    }
}