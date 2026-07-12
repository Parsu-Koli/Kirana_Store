using DAL.Models;
using DAL.Repository.Interface;

namespace BLL.Services
{
    public class GstRegistrationService
    {
        private readonly IGSTRegistrationRepository _gstRepo;

        public GstRegistrationService(IGSTRegistrationRepository gstRepo)
        {
            _gstRepo = gstRepo;
        }

        public IEnumerable<GstRegistration> GetAll()
        {
            return _gstRepo.GetAll();
        }

        public GstRegistration? GetById(int id)
        {
            return _gstRepo.GetById(id);
        }

        public GstRegistration? GetActiveRegistration()
        {
            return _gstRepo.GetActiveRegistration();
        }

        public void Add(GstRegistration registration)
        {
            if (string.IsNullOrWhiteSpace(registration.GstNumber))
                throw new Exception("GST Number is required.");

            if (string.IsNullOrWhiteSpace(registration.BusinessName))
                throw new Exception("Business Name is required.");

            _gstRepo.Add(registration);
        }

        public void Update(GstRegistration registration)
        {
            var existing = _gstRepo.GetById(registration.GstRegistrationId);

            if (existing == null)
                throw new Exception("GST Registration not found.");

            existing.GstNumber = registration.GstNumber;
            existing.BusinessName = registration.BusinessName;
            existing.OwnerName = registration.OwnerName;
            existing.Address = registration.Address;
            existing.State = registration.State;
            existing.StateCode = registration.StateCode;
            existing.PinCode = registration.PinCode;
            existing.Email = registration.Email;
            existing.MobileNumber = registration.MobileNumber;
            existing.IsGstEnabled = registration.IsGstEnabled;

            _gstRepo.Update(existing);
        }

        public void Delete(int id)
        {
            _gstRepo.Remove(id);
        }

        public void EnableDisableGST(int id, bool status)
        {
            var gst = _gstRepo.GetById(id);

            if (gst == null)
                throw new Exception("GST Registration not found.");

            gst.IsGstEnabled = status;

            _gstRepo.Update(gst);
        }

        public void ToggleGST(bool enabled)
        {
            var gst = _gstRepo.GetFirstRegistration();

            if (gst == null)
                throw new Exception("GST Registration not found.");

            gst.IsGstEnabled = enabled;

            _gstRepo.Update(gst);
        }
    }
}