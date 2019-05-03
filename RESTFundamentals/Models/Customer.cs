using System;
using System.ComponentModel.DataAnnotations;

namespace RESTFundamentals.Models
{
    public class Customer
    {
        public string CustomerID { get; set; }
        public string FirstName {get; set;}
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}
