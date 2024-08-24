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
    public class LanguageController : ControllerBase
    {
        private ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LanguageDTO>>> GetLanguages()
        {
            return new ObjectResult( await _languageService.GetAllLanguages());
        }

        // GET: LanguageController/GetCountry/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LanguageDTO>> GetLanguage(int id)
        {
            var language = await _languageService.GetLanguage(id);
            if (language == null)
            {
                return NotFound();
            }
            return new ObjectResult(language);
        }


        // GET: LanguageController/GetByCountryName/5
        [HttpGet("getbylanguagename/{languageName}")]
        public async Task<ActionResult<LanguageDTO>> GetByLanguageName(string languageName)
        {
            var language = await _languageService.GetLanguageByName(languageName);
            if (language == null)
            {
                return NotFound();
            }
            return new ObjectResult(language);
        }

        // GET: LanguageController/Create
        [HttpPost("add")]
        public async Task<ActionResult<LanguageDTO>> AddLanguage(LanguageDTO languageDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _languageService.AddLanguage(languageDTO);

            return Ok(languageDTO);
        }



        // GET: LanguageController/Edit/5
        [HttpPut("update")]
        public async Task<ActionResult<LanguageDTO>> UpdateLanguage(LanguageDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            LanguageDTO language = await _languageService.GetLanguage(u.Id);
            if (language == null)
            {
                return NotFound();
            }

            LanguageDTO language_new = await _languageService.UpdateLanguage(u);

            return Ok(language_new);
        }

        // GET: LanguageController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LanguageDTO>> DeleteCountry(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LanguageDTO language = await _languageService.GetLanguage(id);
            if (language == null)
            {
                return NotFound();
            }

            await _languageService.DeleteLanguage(id);

            return Ok(language);
        }


    }
}
