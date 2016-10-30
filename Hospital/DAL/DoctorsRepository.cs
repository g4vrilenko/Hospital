using Hospital.Models;

namespace Hospital.DAL
{
    public class DoctorsRepository : Repository<Doctor>
    {
        public DoctorsRepository(HospitalContext context) : base(context)
        {
        }
    }
}