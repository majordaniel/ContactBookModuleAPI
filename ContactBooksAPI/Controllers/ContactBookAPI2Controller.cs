using ContactBook.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ContactBooksAPI.Controllers
{
    public class ContactBookAPI2Controller : ApiController
    {

        // Instantiating the Class
        ContactBookDBEntities db = new ContactBookDBEntities();
        [HttpGet]
        public IEnumerable<TbContact> GetAllContacts()
        {
            var ConData = db.TbContacts.ToList();
            return ConData;
        }



        /// <summary>
        /// to select a particular Contact Using an Identifier
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetContactById(int Id)
        {
            var conData = db.TbContacts.FirstOrDefault(e => e.ContactId == Id);
            if (conData !=null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, conData);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Contact Id" + Id.ToString() + "is not Found");
            }
        }

        /// <summary>
        /// To Add new Conttct Details
        /// </summary>
        /// <param name="NewContact"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddNewContact([FromBody] TbContact NewContact)
        {
            try
            {
                db.TbContacts.Add(NewContact);
                db.SaveChanges();

                var msg = Request.CreateResponse(HttpStatusCode.Created, NewContact);

                //embed the ID of the new data to the Header of the request while redirecting
                msg.Headers.Location = new Uri(Request.RequestUri + "/" + NewContact.ContactId.ToString());

                return msg;
            }
            catch (Exception ex )
            {
                //else tell the user that there is an Issue
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
           
        }

        [HttpDelete]
        public HttpResponseMessage DeleteContact( int Id)
        {
            try
            {
                var ConData = db.TbContacts.FirstOrDefault(e => e.ContactId == Id);

                if (ConData !=null)
                {

                    // if you want to change just a column data then do something like this, example like the date of birth
                    //ConData.DateOfBirth = DateTime.Now;

                    db.TbContacts.Remove(ConData);
                    db.SaveChanges();
                    //embed the Id of the Deleted data to the request URL

                    var msg = Request.CreateResponse(HttpStatusCode.OK, Id);
                    return msg;

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The record with Id" + Id.ToString() + "Not Found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex);
            }
        }
        /// <summary>
        /// To Edit a Particular Contact using a Parameter Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="NewContactValue"></param>
        /// <returns></returns>
        [HttpPut]
        public HttpResponseMessage EditContact(int id, [FromBody] TbContact NewContactValue)
        {
            try
            {
                var entity = db.TbContacts.FirstOrDefault(e => e.ContactId == id);
                if (entity != null)
                {
                    entity.Address = NewContactValue.Address;
                    entity.DateOfBirth = NewContactValue.DateOfBirth;
                    entity.ContactName = NewContactValue.ContactName;
                    entity.Email = NewContactValue.Email;
                    entity.PhoneNo = NewContactValue.PhoneNo;
                    //save the Updated record

                    db.SaveChanges();

                    //Append/ embed the updated Data record to the request URL
                    var Msg = Request.CreateResponse(HttpStatusCode.OK, entity);
                    return Msg;
                }

                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "The Contact with the Id" + id.ToString() + "not found");
                }
            }
            catch (Exception ex )
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
