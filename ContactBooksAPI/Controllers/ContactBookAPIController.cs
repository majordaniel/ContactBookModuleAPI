using ContactBook.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ContactBooksAPI.Controllers
{
    public class ContactBookAPIController : ApiController
    {
        // Instantiating the Class
        ContactBookDBEntities db = new ContactBookDBEntities();


        // Action Method to list all the contacts
        //GET api/<controller>
        [HttpGet]
        public IEnumerable<TbContact> Get()
        {
            //returning all the content of the table as list
            return db.TbContacts.ToList().AsEnumerable();
        }


        //Action Method to fetcha and filter a particular record
        //GET api/<controller>/<id>
        [HttpGet]
        public HttpResponseMessage Get(int Id)
        {
            var ContactDetail = (from a in db.TbContacts
                                 where a.ContactId == Id
                                 select a).FirstOrDefault();

            if (ContactDetail !=null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ContactDetail);
            }
            else
            {
                //sending response as error status code NOT FOUND with meaningful message.  

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Contact with the Specified Id is not Found");
            }

        }

        // action method to add new contact record
        //POST api/<controller>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]TbContact _contact)
        {
            try
            {
                //to add a new record 
                db.TbContacts.Add(_contact);
                // save the added data
                db.SaveChanges();

                //return response status as successful
                var msg = Request.CreateResponse(HttpStatusCode.Created, _contact);

                //send the added data to the URi for Check Purpose
                msg.Headers.Location = new Uri(Request.RequestUri + _contact.ContactId.ToString());
                return msg;

            }
            catch (Exception ex)
            {
                // return response as bad request with exception message.

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // To Update contact record
        //PUT api/<controller>/id

        public HttpResponseMessage Put(int Id, [FromBody] TbContact _ContactToEdit)
        {
            // first fetch the details of the record to edit
            var ExisitngRecord = (from a in db.TbContacts
                          where a.ContactId == Id
                          select a
               ).FirstOrDefault();
            //when the record is gotten
            if (ExisitngRecord !=null)
            {
                //set the incoming records to the exitising record
                ExisitngRecord.ContactName = _ContactToEdit.ContactName;
                ExisitngRecord.Address = _ContactToEdit.Address;
                ExisitngRecord.DateOfBirth = _ContactToEdit.DateOfBirth;
                ExisitngRecord.PhoneNo = _ContactToEdit.PhoneNo;

                //save the changes
                db.SaveChanges();
                //return response status as successfully updated with member entity  

                return Request.CreateResponse(HttpStatusCode.OK, ExisitngRecord);

            }
            else
            {
                //return response error as NOT FOUND  with message.  

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not found to edit");

            }

        }
        //DELETE api/<controller>/id
        public HttpResponseMessage Delete(int Id)
        {
            try
            {
                //fetching the particular record to delete
                var _RecordToBeDeleted = (from a in db.TbContacts
                                          where a.ContactId == Id

                                          select a
                                         ).SingleOrDefault();

                // after getting the record 
                if (_RecordToBeDeleted !=null)
                {
                    db.TbContacts.Remove(_RecordToBeDeleted);
                    db.SaveChanges();

                    //then return a response status as successfully deleted
                    return Request.CreateResponse(HttpStatusCode.OK, Id);
                }
                else
                {
                    //return response error as Not Found  with exception message.  
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "record Not Found or Invalid " + Id.ToString());
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record with " + Id.ToString() + "not found to be deleted",ex);
            }
        }
    }
}
