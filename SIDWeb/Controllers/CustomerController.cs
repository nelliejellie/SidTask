using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIDWeb.Contracts;
using SIDWeb.Model;

namespace SIDWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // GET: api/customer (Optimized Query)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            try
            {
                var customers = await _customerRepository.GetAllCustomersAsync();
                return Ok(customers);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                if (customer == null)
                    return NotFound();

                return Ok(customer);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        // POST: api/customers (Optimized Insert)
        [HttpPost]
        public async Task<ActionResult<Customer>> AddCustomer(Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _customerRepository.AddCustomerAsync(customer);
                await _customerRepository.SaveAsync();

                return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
