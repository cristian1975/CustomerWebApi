using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerWebApi;
using CustomerWebApi.Models;
using CustomerWebApi.Repository;
using System.Net;

namespace CustomerWebApi.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        
        private readonly ICustomerRepo _repository;

        public CustomersController( ICustomerRepo repository)
        {
            _repository = repository;
        }

        ///<summary>Get customers</summary>
        ///<remarks>
        ///  GET: api/Customers
        ///</remarks>
        [ProducesResponseType(typeof(IEnumerable<Customer>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var customers = await _repository.GetAll();
                return Ok(customers);
            }
            catch
            {
                return BadRequest("Could not get all customers");
            }
        }

        ///<summary>Get customer</summary>
        ///<remarks>
        ///  GET: api/Customers/d12cf36a-061b-47bd-9504-90fa8c6198d6
        ///</remarks>
        ///<param name="id"></param>
        [ProducesResponseType(typeof(Customer), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _repository.Get(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // 
        ///<summary>Update customer</summary>
        ///<remarks>
        ///Sample request
        ///PUT: api/Customers/d12cf36a-061b-47bd-9504-90fa8c6198d6
        ///
        ///        {
        ///                 "id":"d12cf36a-061b-47bd-9504-90fa8c6198d6"
        ///                 "name":"John Doe",
        ///                 "email":"john.doe@acme.com",
        ///                 "address":"Main Street 146 ,NY"
        ///        }
        ///</remarks>
        ///<param name="customer"></param>
        ///<returns>Customer model</returns>               
        [ProducesResponseType(typeof(Customer), 200)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] string id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }
          
            
            var updatedCustomer = await _repository.Update(id,customer);
            if (updatedCustomer != null)
            {
                return Ok(updatedCustomer);
            }
            else
            {
                return NotFound();
            }
        

        }
        ///<summary>Create or update customer</summary>
        ///<remarks>
        ///Sample request
        ///POST: api/Customers
        ///
        ///           {
        ///                       "name":"John Doe",
        ///                       "email":"john.doe@acme.com",
        ///                       "address":"Main Street 146 ,NY"
        ///           }
        ///</remarks>
        ///<param name="customer"></param>
        ///<returns>Customer model</returns>       
        [HttpPost]
        [ProducesResponseType(typeof(Customer),200)]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCustomer = await _repository.GetByEmail(customer.Email);
            if (existingCustomer != null)
            {

              
                    //could use mapper 
                    existingCustomer.Name = customer.Name;
                    existingCustomer.Address = customer.Address;

                    var cust = await _repository.Update(existingCustomer.Id,existingCustomer);
                    if (cust != null)
                    {
                        return Ok(cust);
                    }
                    else
                    {
                        return NotFound();
                    }


            }
            else
            {
                var cust = await _repository.Create(customer);
                return Ok(cust);
            }
        }
        ///<summary>Delete customer</summary>
        ///<remarks>
        ///  DELETE: api/Customers/d12cf36a-061b-47bd-9504-90fa8c6198d6
        ///</remarks>
        ///<param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool ret = await _repository.Delete(id);
            if (ret)
            {
                return Ok("Customer was deleted");
            }
            else
            {
                return NotFound();
            }
        }

       
    }
}