﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ContactModel
    {
        [Required]
        [Display(Name = "Apellido paterno")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Nombre")]        
        public string Name { get; set; }

        [Required]
        [Display(Name = "Correo")]        
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        ////[Display(Name = "Estado")]
        public StateList State { get; set; }

        public class StateList 
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        //[Display(Name = "Telefono")]
        public string Phone { get; set; }

        public string Message { get; set; }
    }
}