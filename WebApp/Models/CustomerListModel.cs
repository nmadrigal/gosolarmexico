using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.ServiceReference1;

namespace WebApp.Models
{
    public class CustomerListModel
    {
        public List<Customer> customerList { get; set; }
    }
}