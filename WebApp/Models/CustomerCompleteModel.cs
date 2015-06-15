using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class CustomerCompleteModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ExtNum { get; set; }
        public string IntNum { get; set; }
        public string ResidentialArea { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}