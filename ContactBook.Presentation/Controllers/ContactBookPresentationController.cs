using ContactBook.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ContactBook.Presentation.Controllers
{
    public class ContactBookPresentationController : Controller
    {

        IEnumerable<ContactViewModel> Contacts = null;

        // GET: ContactBookPresentation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllContacts()
        {
            using (var Client = new HttpClient())
            {

                Client.BaseAddress = new Uri("Http://localhost:00000/api/");
                //Called Member default GET All records  
                //GetAsync to send a GET request   
                // PutAsync to send a PUT request 
                var responseTask = Client.GetAsync("Contact");
                responseTask.Wait();
                //To store result of web api response
                var result = responseTask.Result;

                //if success receieved
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ContactViewModel>>();
                    readTask.Wait();

                    Contacts = readTask.Result;

                }
                else
                {
                    //error response received
                    Contacts = Enumerable.Empty<ContactViewModel>();
                    ModelState.AddModelError(string.Empty, "Server Error occured");
                }

            }

            return View(Contacts);
        }

        
    }
}