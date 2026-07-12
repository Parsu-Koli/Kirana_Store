using DAL.Data;
using DAL.Models;
using DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Implimentation
{
    public class GstRegistrationRepository(AppDbContext context) : IGSTRegistrationRepository
    {
        private readonly AppDbContext _context = context;

        public IEnumerable<GstRegistration> GetAll() {
            return _context.GstRegistrations.ToList();
        }

        public void Add(GstRegistration registration)
        {
            try
            {
                _context.GstRegistrations.Add(registration);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public void Remove(int Id)
        {
            var result = _context.GstRegistrations.Find(Id);
            if (result != null)
            {
                _context.Remove(result);
                _context.SaveChanges();
            }
        }
        public GstRegistration GetById(int Id)
        {
            var result = _context.GstRegistrations.Find(Id);
            return result;
           
        }

        public GstRegistration? GetActiveRegistration()
        {
            return _context.GstRegistrations.FirstOrDefault();
        }

        public void Update(GstRegistration registration)
        {
            _context.GstRegistrations.Update(registration);
            _context.SaveChanges();
        }

        public GstRegistration? GetFirstRegistration()
        {
            return _context.GstRegistrations.FirstOrDefault();
        }
    }
}
