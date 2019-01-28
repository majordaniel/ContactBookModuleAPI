using ContactBook.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace ContactBook.Presentation.Controllers
{
    public class ContactBookPresentation2Controller : Controller
    {
        HttpClient client = new HttpClient();

        public ContactBookPresentation2Controller(HttpClient client)
        {
            client.BaseAddress = new Uri("Http://localhost:12844/api/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        // GET: ContactBookPresentation2
        public ActionResult Index()
        {
            List<ContactViewModel> con = new List<ContactViewModel>();
            HttpResponseMessage resp = client.GetAsync("Contact").Result;
            if (resp.IsSuccessStatusCode)
            {
                con = resp.Content.ReadAsAsync<List<ContactViewModel>>().Result;
            }

            return View();
        }

        public ActionResult Create()
        {

            return View();
        }
        public ActionResult Create(ContactViewModel IncData)
        {
            client.PostAsJsonAsync<ContactViewModel>("Contact", IncData).ContinueWith((e => e.Result.EnsureSuccessStatusCode()));

            return RedirectToAction("Index");
        }

        /// <summary>
        /// first get the details of the data to edit using Id as a parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var ContactDetail = client.GetAsync("Contact/" + id.ToString()).Result;
            return View(ContactDetail.Content.ReadAsAsync<ContactViewModel>().Result);
        }


        /// <summary>
        /// post back the edited data to the system
        /// </summary>
        /// <param name="EditedContact"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ContactViewModel OldContact)
        {

            var NewContactDetails = client.PutAsJsonAsync<ContactViewModel>("Contacts/" + OldContact.ContactId, OldContact).Result;
            return RedirectToAction("Index");
        }
        /// <summary>
        /// To Delete a Particular contact detail
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Delete(int Id)
        {
            var ContactDetail = client.DeleteAsync("Contacts/" + Id.ToString()).Result;
            return RedirectToAction("Index");
        }
    }
}