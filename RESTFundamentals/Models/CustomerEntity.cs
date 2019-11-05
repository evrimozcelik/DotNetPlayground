using System;
using System.ComponentModel.DataAnnotations;

namespace RESTFundamentals.Models
{
    public class CustomerEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
