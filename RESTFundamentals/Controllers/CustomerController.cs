using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTFundamentals.Models;

namespace RESTFundamentals.Controllers
{
    [Route("/api/customers")]
    public class CustomerController : Controller
    {
        SampleDBContext _dbContext;

        public CustomerController(SampleDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [ProducesResponseType(typeof(List<Customer>), StatusCodes.Status200OK)]
        [HttpGet()]
        public async Task<IActionResult> GetCustomers()
        {
            var customerEntities = await _dbContext.Customers.ToArrayAsync();
            var customers = customerEntities.Select(Mapper.Map<CustomerEntity, Customer>);

            return Json(customers);
        }

        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {

            var customerEntity = await _dbContext.Customers.Where(c => c.CustomerID == customerId).FirstOrDefaultAsync<CustomerEntity>();

            if (customerEntity == null)
            {
                return NotFound();
            }

            var customer = Mapper.Map<CustomerEntity, Customer>(customerEntity);
            return Json(customer);

        }

        [HttpPost()]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = _dbContext.Customers.Select(c => c.CustomerID == customer.CustomerID);
            if (query.Any())
            {
                return BadRequest("Customer ID exists");
            }

            var customerEntity = Mapper.Map<Customer, CustomerEntity>(customer);

            var entry = await _dbContext.AddAsync(customerEntity);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByCustomerId), new { id = customer.CustomerID }, customer);
        }

        [HttpPut("{customerId}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer([FromRoute]string customerId, [FromBody]Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerEntity = await _dbContext.Customers.Where(c => c.CustomerID == customerId).FirstOrDefaultAsync<CustomerEntity>();

            if (customerEntity == null)
            {
                return NotFound();
            }

            Mapper.Map<Customer, CustomerEntity>(customer,customerEntity);
            customerEntity.CustomerID = customerId;
            await _dbContext.SaveChangesAsync();

            return Json(customer);

        }

        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(string customerId)
        {

            var customerEntity = await _dbContext.Customers.Where(c => c.CustomerID == customerId).FirstOrDefaultAsync<CustomerEntity>();

            if (customerEntity == null)
            {
                return NotFound();
            }

            _dbContext.Remove(customerEntity);
            await _dbContext.SaveChangesAsync();

            return Ok();

        }
    }
}
