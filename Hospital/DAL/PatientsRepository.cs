using Hospital.Models;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.DAL
{
    public class PatientsRepository : Repository<Patient>
    {
        public PatientsRepository(HospitalContext context) : base(context)
        {
        }

        public void AssignToDoctor(int patientId, Doctor doctor)
        {
            var patient = Get(patientId);
            patient.Doctors.Add(doctor);
            Context.SaveChanges();
        }

        public void RemoveAssignment(int patientId, Doctor doctor)
        {
            var patient = Get(patientId);
            patient.Doctors.Remove(doctor);
            Context.SaveChanges();
        }

        public IEnumerable<Doctor> GetAssignedDoctors(int patientId)
        {
            var patient = Get(patientId);
            return patient.Doctors.AsEnumerable();
        }
    }
}