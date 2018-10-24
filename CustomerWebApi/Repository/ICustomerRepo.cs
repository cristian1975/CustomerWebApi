using CustomerWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerWebApi.Repository
{
    public interface ICustomerRepo
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> Get(string id);
        Task<Customer> GetByEmail(string email);
        Task<Customer> Create(Customer customer );
        Task<Customer> Update(Customer customer);
        Task<bool> Delete(string id);
    }
}
