using FidenzLevelOne.Data;
using Microsoft.AspNetCore.Mvc;
using FidenzLevelOne.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace FidenzLevelOne.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("User")]
        public IActionResult Customer()
        {
            return View();
        }

        // Endpoint: api/customer/edit/{customerId}
        [HttpPut("edit/{customerId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> EditCustomer(string customerId, [FromBody] CustomerUpdateModel editModel)
        {
            var customer = await _context.customers.FindAsync(customerId);

            if (customer == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(editModel.Name))
            {
                customer.Name = editModel.Name;
            }

            if (!string.IsNullOrEmpty(editModel.Email))
            {
                if(!new EmailAddressAttribute().IsValid(editModel.Email))
                {
                    return BadRequest("Error - Invalid email");
                }
                customer.Email = editModel.Email;

            }

            if (!string.IsNullOrEmpty(editModel.Phone))
            {
                if (!editModel.Phone.All(char.IsDigit) || editModel.Phone.Length < 10)
                {
                    return BadRequest("Error - Invalid phone number");
                }
                customer.Phone = editModel.Phone;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Success - Data updated");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error updating customer");
            }
        }


        // Endpoint: api/customer/getdistance/{customerId}
        [HttpGet("getdistance/{customerId}")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult GetDistance(string customerId, [FromBody] LocationModel location)
        {
            var customer = _context.customers.Find(customerId);

            if (customer == null)
            {
                return NotFound();
            }

            // Calculate distance using latitude and longitude
            double customerLatitude = Convert.ToDouble(customer.Latitude);
            double customerLongitude = Convert.ToDouble(customer.Longitude);
            double latitude = Convert.ToDouble(location.Latitude);
            double longitude = Convert.ToDouble(location.Longitude);
            double distance = CalculateDistance(customerLatitude, customerLongitude, latitude, longitude);

            return Ok(distance);
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Haversine formula 
            double R = 6371; // Earth's radius in kilometers
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;
            return Math.Round(distance, 3);
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        // Endpoint: api/customer/search
        [HttpGet("search")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult SearchCustomers([FromQuery] string searchText)
        {
            int searchNumber = Int32.Parse(searchText);
            decimal searchDecimal = decimal.Parse(searchText);
            var customers = _context.customers.Where(c =>
                c.Name.Contains(searchText) ||
                c.Email.Contains(searchText) ||
                c.Phone.Contains(searchText) ||
                c.Company.Contains(searchText) ||
                c.AddressCity.Contains(searchText) ||
                c.AddressState.Contains(searchText) ||
                c.Tags1.Contains(searchText) ||
                c.Tags2.Contains(searchText) ||
                c.Tags3.Contains(searchText) ||
                c.Tags4.Contains(searchText) ||
                c.Tags5.Contains(searchText) ||
                c.Tags6.Contains(searchText) ||
                c.About.Contains(searchText) ||
                c.number.Equals(searchNumber) ||
                c.Age.Equals(searchNumber) ||
                c.AddressNumber.Equals(searchNumber) ||
                c.AddressZipCode.Equals(searchNumber) ||
                c.Latitude.Equals(searchDecimal) ||
                c.Longitude.Equals(searchDecimal)
            ).ToList();

            return Ok(customers);
        }

        // Endpoint: api/customer/groupedbyzipcode
        [HttpGet("groupedbyzipcode")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult GetCustomersGroupedByZipCode()
        {
            var customersGrouped = _context.customers.GroupBy(c => c.AddressZipCode).ToList();

            return Ok(customersGrouped);
        }
    }
}
