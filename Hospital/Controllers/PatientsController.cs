using Hospital.DAL;
using Hospital.Models;
using Hospital.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Hospital.Controllers
{
    public class PatientsController : Controller
    {
        private HospitalContext _context;
        private PatientsRepository _patients;
        private DoctorsRepository _doctors;
        public PatientsController()
        {
            _context = new HospitalContext();
            _patients = new PatientsRepository(_context);
            _doctors = new DoctorsRepository(_context);
        }
        // GET: Patients
        public ActionResult Index()
        {
            return View(_patients.GetAll());
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = _patients.Get(id.Value);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            SetViewBagStatusType(Status.Arrived);
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient)
        {
            SetViewBagStatusType(Status.Arrived);
            if (ModelState.IsValid)
            {
                _patients.Add(patient);
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        // GET: Patients/Edit/5

        public ActionResult Edit(int? id)
        {
            SetViewBagStatusType(Status.Arrived);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = _patients.Get(id.Value);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _patients.Update(patient);
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = _patients.Get(id.Value);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = _patients.Get(id);
            _patients.Remove(patient);
            return RedirectToAction("Index");
        }
        public ActionResult Search(string patientName)
        {
            var patients = new List<Patient>();
            if (!string.IsNullOrEmpty(patientName))
            {
                patients = _patients.Find(d => d.Name.Contains(patientName)).ToList();
            }
            return View(patients);
        }

        public ActionResult AssignToDoctor(string patientName, int patientId)
        {
            ViewBag.PatientId = patientId;
            ViewBag.PatientName = patientName;
            var assignedDoctors = _patients.GetAssignedDoctors(patientId);
            var doctorsList = _doctors.GetAll()
                .Select(d => new AssignedDoctor()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Specialization = d.Specialization,
                    IsAssigned = assignedDoctors.Contains(d)
                });
            return View(doctorsList);
        }

        [HttpPost]
        public JsonResult AssignToDoctor(int patientId, List<AssignedDoctor> doctors)
        {
            foreach (var d in doctors)
            {
                if (d.IsAssigned)
                    _patients.AssignToDoctor(patientId, _doctors.Get(d.Id));
                else
                    _patients.RemoveAssignment(patientId, _doctors.Get(d.Id));
            }
            return Json(new object());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _patients.Dispose();
            }
            base.Dispose(disposing);
        }

        private void SetViewBagStatusType(Status selectedStatus)
        {
            IEnumerable<Status> values = Enum.GetValues(typeof(Status)).Cast<Status>();
            IEnumerable<SelectListItem> items = values
                .Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = x.ToString(),
                    Selected = x == selectedStatus
                });
            ViewBag.Status = items;
        }
    }
}
