using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerWebApi.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ApiContext _context;
      
        public CustomerRepo(ApiContext context)
        {
            _context = context;
        }

        public async Task<Customer> Get(string id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> GetByEmail(string email)
        {
            return await _context.Customers.Where(c=>c.Email == email).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _context.Customers.ToListAsync();                             
        }

        public async Task<Customer> Create(Customer customer)
        {
            _context.Customers.Add(customer);
             await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> Update( Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
             await _context.SaveChangesAsync();
            return customer;

        }

        public async Task<bool> Delete(string id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }


        }
    }
}
