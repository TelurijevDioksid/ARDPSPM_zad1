using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Span.Culturio.Api.Models.CultureObject;
using Span.Culturio.Api.Services.CultureObject;
using System.ComponentModel.DataAnnotations;

namespace Span.Culturio.Api.Controllers
{
    [Route("/culture-objects")]
    [ApiController]
    [Tags("Culture objects")]
    public class CultureObjectController : ControllerBase
    {
        private readonly IValidator<CreateCultureObjectDto> _validator;
        private readonly ICultureObjectService _cultureObjectService;


        public CultureObjectController(IValidator<CreateCultureObjectDto> validator, ICultureObjectService cultureObjectService)
        {
            _validator = validator;
            _cultureObjectService = cultureObjectService;
        }

        /// <summary>
        /// Create new culture object
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<string>> Post([Required]CreateCultureObjectDto req)
        {
            FluentValidation.Results.ValidationResult result = await _validator.ValidateAsync(req);
            if (!result.IsValid) return BadRequest("ValidationError");

            var cultureObjectDto = await _cultureObjectService.CreateCultureObjectAsync(req);
            if (cultureObjectDto is null) return BadRequest("ValidationError");

            return Ok("Successful response");

        }

        /// <summary>
        /// Get culture object by Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CultureObjectDto>> GetCultureObjectsById(int id)
        {
            var cultureObjectDto = await _cultureObjectService.GetCultureObjectAsync(id);
            if (cultureObjectDto is null) return NotFound("Nema");

            return Ok(cultureObjectDto);
        }

        /// <summary>
        /// Get culture objects
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CultureObjectDto>>> GetCultureObjects()
        {
            var cultureObjectsDto = await _cultureObjectService.GetCultureObjectsAsync();
            return Ok(cultureObjectsDto);
        }
    }
}
