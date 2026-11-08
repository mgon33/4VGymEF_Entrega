using _4VGymEF.Dtos;
using _4VGymEF.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace _4VGymEF.Controllers
{
    [ApiController]
    [Route("activity-types")]
    public class ActivityTypesController : ControllerBase
    {
        private readonly ActivityTypeRepository _repository;

        public ActivityTypesController(ActivityTypeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ActivityTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<ActivityTypeDto>> FindActivityTypes()
        {
            try
            {
                IEnumerable<ActivityTypeDto> activityTypes = _repository.GetAll();

                if (activityTypes == null || activityTypes.Count() == 0)
                {
                    return BadRequest(new ErrorResponse(2, "No se han encontrado tipos de actividad"));
                }

                return Ok(activityTypes);
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponse(1, "Se ha producido un problema en el servidor"));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ActivityTypeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<ActivityTypeDto> CreateActivityType([FromBody] ActivityTypeDto activityType)
        {
            try
            {
                if (activityType == null)
                {
                    return BadRequest(new ErrorResponse(3, "Los datos del tipo de actividad no son válidos"));
                }
                if (String.IsNullOrEmpty(activityType.Name))
                {
                    return BadRequest(new ErrorResponse(4, "El nombre del tipo de actividad es obligatorio"));
                }

                if (_repository.GetByName(activityType.Name) != null)
                {
                    return BadRequest(new ErrorResponse(5, "Ya existe un tipo de actividad con ese nombre"));
                }

                ActivityTypeDto createdActivityType = _repository.Add(activityType);

                return StatusCode(StatusCodes.Status201Created, createdActivityType);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new ErrorResponse(1, "Se ha producido un error interno en el servidor"));
            }
        }
    }
}