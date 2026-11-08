using _4VGymEF.Data;
using _4VGymEF.Dtos;
using _4VGymEF.Models;

namespace _4VGymEF.Repositories
{
    public class ActivityTypeRepository
    {
        private readonly GymDbContext _gymDbContext;

        public ActivityTypeRepository(GymDbContext gymDbContext)
        {
            _gymDbContext = gymDbContext;
        }

        public IEnumerable<ActivityTypeDto> GetAll()
        {
            return _gymDbContext.ActivityTypes
                .Select(at => new ActivityTypeDto
                {
                    Id = at.Id,
                    Name = at.Name,
                    NumberMonitors = at.NumberMonitors
                })
                .ToList();
        }

        public ActivityTypeDto? GetById(int? id)
        {
            if (id == null)
                return null;

            ActivityType? activityType = _gymDbContext.ActivityTypes.Find(id.Value);

            if (activityType == null)
                return null;

            return new ActivityTypeDto(activityType.Id, activityType.Name, activityType.NumberMonitors);
        }

        public ActivityTypeDto? GetByName(string name)
        {
            return _gymDbContext.ActivityTypes
                .Where(at => at.Name == name)
                .Select(at => new ActivityTypeDto
                {
                    Id = at.Id,
                    Name = at.Name,
                    NumberMonitors = at.NumberMonitors
                })
                .FirstOrDefault();
        }

        public ActivityTypeDto Add(ActivityTypeDto activityType)
        {
            if (_gymDbContext.ActivityTypes.Count() == 0)
                activityType.Id = 1;
            else
                activityType.Id = _gymDbContext.ActivityTypes.Max(at => at.Id) + 1;

            return activityType;
        }
    }
}