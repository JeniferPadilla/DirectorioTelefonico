using TelephoneDirectory.Models;

namespace TelephoneDirectory.Interface
{
  
        public interface IRepositorieContact
        {
            Task Create(Contact contact);
            Task<bool> Exist(string Name);
            Task<IEnumerable<Contact>> getContacts();
            Task<IEnumerable<Contact>> getContact(string nameContact);
            Task Modify(Contact contact);
            Task<Contact> getContactById(int id); // para el modify
            Task Delete(int id);
            Task<int> CountContact();
            //void Create(Contact contact);
        }
}
