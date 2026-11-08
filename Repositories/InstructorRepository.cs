using _4VGymEF.Data;
using _4VGymEF.Dtos;

namespace _4VGymEF.Repositories
{
    public class InstructorRepository
    {
        private readonly GymDbContext _context;

        public InstructorRepository(GymDbContext context)
        {
            _context = context;
        }

        public IEnumerable<InstructorDto> GetAll()
        {
            return _context.Instructors
                .Select(i => new InstructorDto(i.Id, i.Name, i.Email, i.PhotoUrl, i.Phone))
                .ToList();
        }

        public InstructorDto? GetById(int id)
        {
            return _context.Instructors
                .Where(i => i.Id == id)
                .Select(i => new InstructorDto(i.Id, i.Name, i.Email, i.PhotoUrl, i.Phone))
                .FirstOrDefault();
        }

        public IEnumerable<InstructorDto> GetByFirstInitial(string? firstInitial = null)
        {
            if (string.IsNullOrWhiteSpace(firstInitial))
            {
                return GetAll();
            }

            return _context.Instructors
                .Where(i => i.Name.StartsWith(firstInitial))
                .Select(i => new InstructorDto(i.Id, i.Name, i.Email, i.PhotoUrl, i.Phone))
                .ToList();
        }

        public List<InstructorDto> GetByIds(List<int> ids)
        {
            if (ids == null || !ids.Any())
                return new List<InstructorDto>();

            return _context.Instructors
                .Where(i => ids.Contains(i.Id))
                .Select(i => new InstructorDto(i.Id, i.Name, i.Email, i.PhotoUrl, i.Phone))
                .ToList();
        }

        public InstructorDto? Add(InstructorDto instructorNew, out ErrorResponse? errorResponse)
        {
            errorResponse = null;

            bool exists = _context.Instructors.Any(i =>
                i.Email == instructorNew.Email || i.Name == instructorNew.Name);

            if (exists)
            {
                errorResponse = new ErrorResponse(34, "An instructor with the same email or name already exists");
                return null;
            }

            int maxId = _context.Instructors.Max(i => (int?)i.Id) ?? 0;
            InstructorDto instructor = new InstructorDto(maxId + 1, instructorNew.Name, instructorNew.Email, instructorNew.PhotoUrl, instructorNew.Phone);

            return instructor;
        }
    }
}