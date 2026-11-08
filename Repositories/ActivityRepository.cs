using _4VGymEF.Data;
using _4VGymEF.Dtos;

namespace _4VGymEF.Repositories
{
    public class ActivityRepository
    {
        private readonly GymDbContext _context;

        public ActivityRepository(GymDbContext context)
        {
            _context = context;
        }

        public ActivityDto? Add(ActivityDto activity, out List<ErrorResponse> errorResponse)
        {
            errorResponse = new List<ErrorResponse>();

            if (!_context.ActivityTypes.Any(at => at.Id == activity.ActivityTypeId))
            {
                errorResponse.Add(new ErrorResponse(34, "El tipo de actividad " + activity.ActivityTypeId + " no existe"));
            }

            List<int> existingIds = _context.Instructors
                .Where(i => activity.InstructorsId.Contains(i.Id))
                .Select(i => i.Id)
                .ToList();

            errorResponse.AddRange(
                activity.InstructorsId
                        .Where(instructorId => !existingIds.Contains(instructorId))
                        .Select(instructorIdNotFound =>
                            new ErrorResponse(35, "El instructor: " + instructorIdNotFound + " no existe")));

            if (errorResponse.Count > 0)
            {
                return null;
            }

            int maxId = _context.Activities.Max(a => (int?)a.Id) ?? 0;
            activity.Id = maxId + 1;

            return activity;
        }
    }
}