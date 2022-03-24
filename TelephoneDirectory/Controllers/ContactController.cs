using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.Models;
using TelephoneDirectory.Services;

namespace TelephoneDirectory.Controllers
{
    public class ContactController : Controller
    {
        private readonly IRepositorieContact repositorieContact;

        public ContactController(IRepositorieContact repositorieContact)
        {
            this.repositorieContact = repositorieContact;
        }

        // Creamos index para ejecutar la interfaz
   
        public async Task<IActionResult> Index(string nameContact)
        {
            if (nameContact != null)
            {
                ViewData["nameContact"] = nameContact;

                var contact = await repositorieContact.getContact(nameContact);
                return View(contact);
            }
            else
            {
                var contact = await repositorieContact.getContacts();
                return View(contact);
            }
            
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }
            // Validamos si ya existe antes de registrar
            var contactExist =
               await repositorieContact.Exist(contact.Name);

            if (contactExist)
            {
                ModelState.AddModelError(nameof(contact.Name),
                    $"The contact {contact.Name} already exist.");

                return View(contact);
            }
            await repositorieContact.Create(contact);
            return RedirectToAction("Index");
        }

        // Hace que la validacion se active automaticamente desde el front
        [HttpGet]
        public async Task<IActionResult> VerificaryContact(string Name)
        {
            var contactExist = await repositorieContact.Exist(Name);

            if (contactExist)
            {
                // permite acciones directas entre front y back
                return Json($"The account {Name} already exist");
            }

            return Json(true);
        }

         //Actualizar
        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {
            var contacts = await repositorieContact.getContactById(id);

            if (contacts is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(contacts);
        }
        [HttpPost]
        public async Task<ActionResult> Modify(Contact contact)
        {
       
            var contacts = await repositorieContact.getContactById(contact.Id);

            if (contacts is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieContact.Modify(contact);// el que llega
            return RedirectToAction("Index");
        }

        // Eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            
            var contacts = await repositorieContact.getContactById(id);

            if (contacts is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(contacts);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteContact(int id)
        {
           
            var contacts = await repositorieContact.getContactById(id);

            if (contacts is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieContact.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
