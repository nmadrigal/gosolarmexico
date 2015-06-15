using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class CustomerModel
    {
        [Required]
        [Display(Name = "Nombres")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Movil")]
        public string Mobile { get; set; }

        [Required]
        [Display(Name = "Calle")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Numero Ext")]
        public int ExternalNum { get; set; }

        [Required]
        [Display(Name = "Colonia")]
        public string ResidentialArea { get; set; }

        [Required]
        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Codigo Postal")]
        public int ZipCode { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public string State { get; set; }

        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        //[Display(Name = "Numer Int")]
        //public int InteriorNum { get; set; }
    }
}