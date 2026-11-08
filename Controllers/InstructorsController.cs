using _4VGymEF.Dtos;
using _4VGymEF.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace _4VGymEF.Controllers
{
    [ApiController]
    [Route("instructors")]
    public class InstructorsController : ControllerBase
    {
        private readonly InstructorRepository _repository;

        public InstructorsController(InstructorRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InstructorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<InstructorDto>> GetInstructors([FromQuery] int? id, [FromQuery(Name = "first_initial")] string? firstInitial)
        {
            try
            {
                if (id.HasValue)
                {
                    InstructorDto? instructor = _repository.GetById(id.Value);
                    if (instructor == null)
                    {
                        return NotFound(new ErrorResponse(2, $"No existe el instructor con ID {id.Value}"));
                    }
                    return Ok(new[] { instructor });
                }

                if (!string.IsNullOrWhiteSpace(firstInitial))
                {
                    IEnumerable<InstructorDto> instructorsByInitial = _repository.GetByFirstInitial(firstInitial);
                    return Ok(instructorsByInitial);
                }

                IEnumerable<InstructorDto> allInstructors = _repository.GetAll();
                return Ok(allInstructors);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ErrorResponse(1, $"Error interno: {ex.Message}")
                );
            }
        }

        private static readonly Regex SoloNumerosRegex = new Regex(@"^[0-9]+$", RegexOptions.Compiled);
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@gmail.com", RegexOptions.Compiled);

        [HttpPost]
        [ProducesResponseType(typeof(InstructorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public ActionResult<InstructorDto> CreateInstructor([FromBody] InstructorDto instructorNew)
        {
            try
            {
                if (instructorNew.isEmpty())
                {
                    return BadRequest(new ErrorResponse(3, "Los datos del instructor no son válidos"));
                }

                if (string.IsNullOrWhiteSpace(instructorNew.Name))
                {
                    return BadRequest(new ErrorResponse(4, "El nombre del instructor es obligatorio"));
                }

                if (string.IsNullOrWhiteSpace(instructorNew.Phone) || !SoloNumerosRegex.IsMatch(instructorNew.Phone))
                {
                    return BadRequest(new ErrorResponse(4, "El teléfono del instructor es obligatorio y solo puede contener números"));
                }

                if (string.IsNullOrWhiteSpace(instructorNew.Email) || !EmailRegex.IsMatch(instructorNew.Email))
                {
                    return BadRequest(new ErrorResponse(4, "El email es obligatorio y debe ser una dirección de gmail válida"));
                }

                if (string.IsNullOrWhiteSpace(instructorNew.PhotoUrl) ||
                    !Uri.TryCreate(instructorNew.PhotoUrl, UriKind.Absolute, out Uri? uriResult) ||
                    (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                {
                    return BadRequest(new ErrorResponse(4, "La URL de la foto no es válida. Debe empezar por http:// o https://"));
                }

                InstructorDto? instructor = _repository.Add(instructorNew, out ErrorResponse? errorResponse);
                if (errorResponse != null)
                {
                    return BadRequest(errorResponse);
                }

                return StatusCode(StatusCodes.Status201Created, instructor);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    new ErrorResponse(1, "Se ha producido un error interno en el servidor"));
            }
        }
    }
}