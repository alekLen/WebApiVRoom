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
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService countryService)
        {
            _categoryService = countryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            return new ObjectResult( await _categoryService.GetAllCategories());
        }

        // GET: CategoryController/GetCountry/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return new ObjectResult(category);
        }

        // GET: CategoryController/GetByCountryName/5
        [HttpGet("getbycategoryname/{categoryName}")]
        public async Task<ActionResult<CategoryDTO>> GetByCategoryName(string categoryName)
        {
            var category = await _categoryService.GetCategoryByName(categoryName);
            if (category == null)
            {
                return NotFound();
            }
            return new ObjectResult(category);
        }

        // GET: CategoryController/Create
        [HttpPost("add")]
        public async Task<ActionResult<CategoryDTO>> AddCategory(CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _categoryService.AddCategory(categoryDTO);

            return Ok(categoryDTO);
        }



        // GET: CategoryController/Edit/5
        [HttpPut("update")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(CategoryDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CategoryDTO category = await _categoryService.GetCategory(u.Id);
            if (category == null)
            {
                return NotFound();
            }

            CategoryDTO category_new = await _categoryService.UpdateCategory(u);

            return Ok(category_new);
        }

        // GET: CategoryController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryDTO>> DeleteCategory(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CategoryDTO country = await _categoryService.GetCategory(id);
            if (country == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteCategory(id);

            return Ok(country);
        }


    }
}
