using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelGoSolar
{
    public static class DBContext
    {
        public static bool UserAuthentication(string email, string password)
        {
            try
            {
                using (GoSolarDataContext dbContext = new GoSolarDataContext())
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

        public static List<Customer> GetReferralsByCustomerEmail(string customerEmail)
        {
            try
            {
                using (GoSolarDataContext dbContext = new GoSolarDataContext())
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
    }
}
