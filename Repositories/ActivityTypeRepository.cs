using _4VGymEF.Data;
using _4VGymEF.Dtos;

namespace _4VGymEF.Repositories
{
    public class ActivityTypeRepository
    {
        private readonly GymDbContext _context;

        public ActivityTypeRepository(GymDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ActivityTypeDto> GetAll()
        {
            return _context.ActivityTypes
                .Select(at => new ActivityTypeDto(at.Id, at.Name, at.NumberMonitors))
                .ToList();
        }

        public ActivityTypeDto? GetById(int? id)
        {
            if (id == null)
                return null;

            return _context.ActivityTypes
                .Where(at => at.Id == id)
                .Select(at => new ActivityTypeDto(at.Id, at.Name, at.NumberMonitors))
                .FirstOrDefault();
        }

        public ActivityTypeDto? GetByName(string name)
        {
            return _context.ActivityTypes
                .Where(at => at.Name == name)
                .Select(at => new ActivityTypeDto(at.Id, at.Name, at.NumberMonitors))
                .FirstOrDefault();
        }

        public ActivityTypeDto Add(ActivityTypeDto activityType)
        {
            int maxId = _context.ActivityTypes.Max(at => (int?)at.Id) ?? 0;
            activityType.Id = maxId + 1;

            return activityType;
        }
    }
}