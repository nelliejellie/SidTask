using SIDWeb.Model;

namespace SIDWeb.Contracts
{
    public interface ICustomerRawsqlRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<int> AddCustomerAsync(Customer customer);
        Task<int> UpdateCustomerAsync(Customer customer);
        Task<int> DeleteCustomerAsync(int id);
    }
}
