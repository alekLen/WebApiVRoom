using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;

namespace WebApiVRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDTO>>> GetCountries()
        {
            return new ObjectResult( await _countryService.GetAllCountries());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(int id)
        {
            var country = await _countryService.GetCountry(id);
            if (country == null)
            {
                return NotFound();
            }
            return new ObjectResult(country);
        }

        [HttpGet("getbycountrycode/{countryCode}")]
        public async Task<ActionResult<CountryDTO>> GetCountryByCode(string countryCode)
        {
            var country = await _countryService.GetCountryByCountryCode(countryCode);
            if (country == null)
            {
                return NotFound();
            }
            return new ObjectResult(country);
        }

        [HttpGet("getbycountryname/{countryName}")]
        public async Task<ActionResult<CountryDTO>> GetByCountryName(string countryName)
        {
            var country = await _countryService.GetByName(countryName);
            if (country == null)
            {
                return NotFound();
            }
            return new ObjectResult(country);
        }

        [HttpPost("add")]
        public async Task<ActionResult<CountryDTO>> AddCountry(CountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _countryService.AddCountry(countryDTO);

            return Ok(countryDTO);
        }


        [HttpPut("update")]
        public async Task<ActionResult<CountryDTO>> UpdateCountry(CountryDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CountryDTO country = await _countryService.GetCountry(u.Id);
            if (country == null)
            {
                return NotFound();
            }

            CountryDTO country_new = await _countryService.UpdateCountry(u);

            return Ok(country_new);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CountryDTO>> DeleteCountry(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CountryDTO country = await _countryService.GetCountry(id);
            if (country == null)
            {
                return NotFound();
            }

            await _countryService.DeleteCountry(id);

            return Ok(country);
        }


    }
}
