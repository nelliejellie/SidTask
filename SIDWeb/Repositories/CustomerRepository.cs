using Microsoft.EntityFrameworkCore;
using SIDWeb.Contracts;
using SIDWeb.Data;
using SIDWeb.Model;

namespace SIDWeb.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .AsNoTracking() // Improves performance for read operations
                .OrderBy(c => c.Name) // Optimized ordering
                .ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers
                .AsNoTracking() // Avoids unnecessary tracking
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
