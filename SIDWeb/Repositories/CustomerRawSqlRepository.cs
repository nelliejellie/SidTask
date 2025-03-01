using Microsoft.EntityFrameworkCore;
using SIDWeb.Contracts;
using SIDWeb.Data;
using SIDWeb.Model;

namespace SIDWeb.Repositories
{
    public class CustomerRawSqlRepository : ICustomerRawsqlRepository
    {
        private readonly AppDbContext _context;

        public CustomerRawSqlRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddCustomerAsync(Customer customer)
        {
            return await _context.Database.ExecuteSqlRawAsync(
                "INSERT INTO Customers (Name, Email, PhoneNumber, CreatedAt) VALUES ({0}, {1}, {2}, {3})",
                customer.Name, customer.Email, customer.PhoneNumber, customer.CreatedAt
            );
        }

        public async Task<int> DeleteCustomerAsync(int id)
        {
            return await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Customers WHERE Id = {0}", id
            );
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .FromSqlRaw("SELECT * FROM Customers")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers
                .FromSqlRaw("SELECT * FROM Customers WHERE Id = {0}", id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            return await _context.Database.ExecuteSqlRawAsync(
                "UPDATE Customers SET Name = {0}, Email = {1}, PhoneNumber = {2} WHERE Id = {3}",
                customer.Name, customer.Email, customer.PhoneNumber, customer.Id
            );
        }
    }
}
