using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.Interface;
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


            //var message = "Hola prueba";

           // ViewData["Message"] = message;
            //ViewBag.message = message;
           
          


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
            var contactCount = await repositorieContact.CountContact();

            if (contactCount > 10)
            {

                ModelState.AddModelError(nameof(contact.Name),
                   $"ya hay 10 contactos no puede registrar mas");

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

        public async Task<ActionResult> CountContact()
        {
            var contactCount = await repositorieContact.CountContact();
            
            Console.WriteLine(contactCount);
           /// return View(contactCount);

  
                // permite acciones directas entre front y back
                return Json($"The account already exist", contactCount);
            

            //return Json(true);

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
