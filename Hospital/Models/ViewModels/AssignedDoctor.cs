
namespace Hospital.Models.ViewModels
{
    public class AssignedDoctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public bool IsAssigned { get; set; }
    }
}