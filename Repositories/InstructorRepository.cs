using _4VGymEF.Data;
using _4VGymEF.Dtos;
using _4VGymEF.Models;

namespace _4VGymEF.Repositories
{
    public class InstructorRepository
    {
        private readonly GymDbContext _gymDbContext;

        public InstructorRepository(GymDbContext gymDbContext)
        {
            _gymDbContext = gymDbContext;
        }

        public IEnumerable<InstructorDto> GetAll()
        {
            return _gymDbContext.Instructors
                .Select(i => new InstructorDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Email = i.Email,
                    PhotoUrl = i.PhotoUrl,
                    Phone = i.Phone
                })
                .ToList();
        }

        public InstructorDto? GetById(int id)
        {
            Instructor? instructor = _gymDbContext.Instructors.Find(id);

            if (instructor == null)
                return null;

            return new InstructorDto(instructor.Id, instructor.Name, instructor.Email, instructor.PhotoUrl, instructor.Phone);
        }

        public IEnumerable<InstructorDto> GetByFirstInitial(string? firstInitial = null)
        {
            if (string.IsNullOrWhiteSpace(firstInitial))
            {
                return GetAll();
            }

            return _gymDbContext.Instructors
                .Where(i => i.Name.StartsWith(firstInitial))
                .Select(i => new InstructorDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Email = i.Email,
                    PhotoUrl = i.PhotoUrl,
                    Phone = i.Phone
                })
                .ToList();
        }

        public InstructorDto? Add(InstructorDto instructorNew, out ErrorResponse? errorResponse)
        {
            errorResponse = null;

            bool exists = _gymDbContext.Instructors.Any(i =>
                i.Email == instructorNew.Email || i.Name == instructorNew.Name);

            if (exists)
            {
                errorResponse = new ErrorResponse(34, "Ya existe un instructor con el mismo email o nombre");
                return null;
            }

            if (_gymDbContext.Instructors.Count() == 0)
                instructorNew.Id = 1;
            else
                instructorNew.Id = _gymDbContext.Instructors.Max(i => i.Id) + 1;

            return instructorNew;
        }
    }
}