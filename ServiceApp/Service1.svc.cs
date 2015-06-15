using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ServiceApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public bool UserAuthentication(string email, string password)
        {
            try
            {
                using (SolarContextDataContext dbContext = new SolarContextDataContext())
                {
                    Customer customer = new Customer();

                    customer = dbContext.Customers.Where(c => c.Email.Equals(email)).FirstOrDefault();

                    if (customer == null)
                    {
                        return false;
                    }

                    if (customer.Password == password)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Customer GetCustomerById(int customerId)
        {
            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                var customer = dc.Customers.Where(c => c.Id.Equals(customerId)).FirstOrDefault();

                return customer;
            }
        }

        public void ChangePasswordByUserEmail(string userMail, string password)
        {
            int userId = 0;
            userId = this.GetUserIdByUserMail(userMail);

            if (userId != 0)
            {
                try
                {
                    using (SolarContextDataContext dc = new SolarContextDataContext())
                    {
                        Customer customer = new Customer();

                        customer = dc.Customers.Where(c => c.Id.Equals(userId)).FirstOrDefault();

                        if (customer.Password != null)
                        {
                            customer.Password = password;

                            dc.SubmitChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<Customer> GetReferralsByCustomerEmail(string customerEmail)
        {
            try
            {
                using (SolarContextDataContext dbContext = new SolarContextDataContext())
                {
                    int customerId = dbContext.Customers.Where(c => c.Email.Equals(customerEmail)).FirstOrDefault().Id;

                    return dbContext.Customers.Where(c => c.ReferencedById.Equals(customerId)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void NewReferral(Customer customer, Address address)
        {
            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                if (!String.IsNullOrEmpty(customer.ReferencedByEmail))
                {
                    customer.ReferencedById = this.GetUserIdByUserMail(customer.ReferencedByEmail);
                    this.ConvertCustomerIntoAmbassador((int)customer.ReferencedById);
                }

                dc.Customers.InsertOnSubmit(customer);
                try
                {
                    dc.SubmitChanges();

                    address.OwnerId = customer.Id;

                    dc.Addresses.InsertOnSubmit(address);

                    dc.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void ConvertCustomerIntoAmbassador(int userId)
        {
            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                Customer customer = dc.Customers.Where(c => c.Id.Equals(userId)).FirstOrDefault();
                customer.IsAmbassador = true;

                Address address = dc.Addresses.Where(a => a.OwnerId.Equals(userId)).FirstOrDefault();
                address.Type = 0;
                try
                {
                    dc.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool CustomerMailExists(string userMail)
        {
            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                return dc.Customers.Any(c => c.Email.Equals(userMail));
            }
        }

        private int GetUserIdByUserMail(string userMail)
        {
            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                return dc.Customers.Where(c => c.Email.Equals(userMail)).FirstOrDefault().Id;
            }
        }

        private string GetUserEmailById(int userId)
        {
            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                return dc.Customers.Where(c => c.Id.Equals(userId)).FirstOrDefault().Email;
            }
        }

        public List<Address> GetUserAddresses(string userEmail)
        {
            int userId = 0;
            List<Customer> referralsList = new List<Customer>();
            List<Address> addressesList = new List<Address>();

            userId = this.GetUserIdByUserMail(userEmail);

            var ambassadorAddress = this.GetUserAddressByUserId(userId);

            if (ambassadorAddress != null)
            {
                addressesList.Add(ambassadorAddress);
            }

            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                referralsList = dc.Customers.Where(c => c.ReferencedById.Equals(userId)).ToList();

                foreach (Customer customer in referralsList)
                {
                    var address = dc.Addresses.Where(a => a.OwnerId.Equals(customer.Id)).FirstOrDefault();
                    if (address != null)
                    {
                        addressesList.Add(address);
                    }
                }

                return addressesList;
            }
        }

        private Address GetUserAddressByUserId(int userId)
        {
            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                return dc.Addresses.Where(a => a.OwnerId.Equals(userId)).FirstOrDefault();
            }
        }

        public List<Address> GetAllAddresses()
        {
            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                return dc.Addresses.ToList();
            }
        }

        public Address GetAddressByOwnerId(int ownerId)
        {
            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                return dc.Addresses.Where(a => a.OwnerId.Equals(ownerId)).FirstOrDefault();
            }
        }

        public List<string> GetUserRoles(string userMail)
        {
            var userId = 0;

            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                userId = this.GetUserIdByUserMail(userMail);

                var roles = from u in dc.UserInRoles
                            join r in dc.Roles on u.RoleId equals r.Id
                            where u.UserId.Equals(userId)
                            select r.Name;

                return roles.ToList();

            }
        }

        public List<Customer> GetAllCutomers()
        {
            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                return dc.Customers.ToList();
            }
        }

        public void UpdateCustomerStatus(Dictionary<int, int> customerList)
        {
            using (SolarContextDataContext dc = new SolarContextDataContext())
            {
                foreach (KeyValuePair<int, int> entry in customerList)
                {
                    var customer = dc.Customers.Where(c => c.Id.Equals(entry.Key)).FirstOrDefault();

                    customer.Status = entry.Value;
                }
                dc.SubmitChanges();
            }
        }
    }
}
