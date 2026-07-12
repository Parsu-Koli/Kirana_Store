using DAL.Models;

namespace DAL.Repository.Interface
{
    public interface IGSTRegistrationRepository
    {
        IEnumerable<GstRegistration> GetAll();

        GstRegistration? GetById(int id);

        GstRegistration? GetActiveRegistration();

        void Add(GstRegistration registration);

        void Update(GstRegistration registration);

        void Remove(int id);

        GstRegistration? GetFirstRegistration();
    }
}
