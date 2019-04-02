using System;
using System.Collections.Generic;
using RESTFundamentals.Models;

namespace RESTFundamentals
{
    public interface ICustomerStore
    {
        List<Customer> CustomerList { get; }

    }
}
