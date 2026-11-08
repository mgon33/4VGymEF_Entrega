using _4VGymEF.Data;
using _4VGymEF.Dtos;

namespace _4VGymEF.Repositories
{
    public class ActivityRepository
    {
        private readonly GymDbContext _gymDbContext;
        private readonly ActivityTypeRepository _repositoryActivityType;
        private readonly InstructorRepository _repositoryInstructor;

        public ActivityRepository(GymDbContext gymDbContext, ActivityTypeRepository repositoryActivityType, InstructorRepository repositoryInstructor)
        {
            _gymDbContext = gymDbContext;
            _repositoryActivityType = repositoryActivityType;
            _repositoryInstructor = repositoryInstructor;
        }

        public ActivityDto? Add(ActivityDto activity, out List<ErrorResponse> errorResponse)
        {
            errorResponse = new List<ErrorResponse>();

            if (_repositoryActivityType.GetById(activity.ActivityTypeId) == null)
            {
                errorResponse.Add(new ErrorResponse(34, $"El tipo de actividad: {activity.ActivityTypeId} no existe"));
            }

            var missingInstructors = activity.InstructorsId
                .Where(instructorId => _repositoryInstructor.GetById(instructorId) == null);

            foreach (var instructorId in missingInstructors)
            {
                errorResponse.Add(new ErrorResponse(35, $"El instructor: {instructorId} no existe"));
            }

            if (errorResponse.Count > 0)
            {
                return null;
            }

            if (_gymDbContext.Activities.Count() == 0)
                activity.Id = 1;
            else
                activity.Id = _gymDbContext.Activities.Max(a => a.Id) + 1;

            return activity;
        }
    }
}