using _4VGymEF.Dtos;
using _4VGymEF.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace _4VGymEF.Controllers
{
    [ApiController]
    [Route("activities")]
    public class ActivityController : ControllerBase
    {
        private readonly ActivityRepository _repository;

        public ActivityController(ActivityRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ActivityDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<ActivityDto> CreateActivity([FromBody] ActivityDto activity)
        {
            try
            {
                ActivityDto? activityCreated = _repository.Add(activity, out List<ErrorResponse> errors);

                if (activityCreated == null && errors.Count > 0)
                {
                    return BadRequest(errors);
                }

                return StatusCode(StatusCodes.Status201Created, activityCreated);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new ErrorResponse(1, "Ha ocurrido un error en el servidor"));
            }
        }
    }
}