namespace _4VGymEF.Models
{
    public class Activity
    {
        public int Id { get; set; }

        public int MaxParticipants { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; } 


        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

        public int? ActivityTypeId { get; set; }

        public ActivityType? ActivityType { get; set; }
    }

}
