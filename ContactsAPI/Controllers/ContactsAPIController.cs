using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ContactsAPI.Repository;
using ContactsAPI.Models.DBModel;
using System.Web.Http.Results;
using ContactsAPI.Exceptions;
using ContactsAPI.Models;

namespace ContactsAPI.Controllers
{
    [CustomNotFoundExceptionFilter]
    public class ContactsAPIController : ApiController
    {
        // GET api/contacsapi

        static readonly IContactsRepository repository = new ContactsRepository();


        public JsonResult<IEnumerable<Contact>> GetAllContacts()
        {
            var contacts = repository.GetAll();
            if (contacts == null)
            {
                throw new ItemNotFoundException("No contact found in database");
            }
            
            return Json<IEnumerable<Contact>>(contacts);

        }


        public HttpResponseMessage GetContact(int id)
        {
            Contact item = repository.Get(id);
            if (item == null)
            {
                throw new ItemNotFoundException("Contact not found");
            }
            return Request.CreateResponse<Contact>(HttpStatusCode.OK, item);
        }

        
        public HttpResponseMessage PostContact(Contact item)
        {
            var response = new HttpResponseMessage();
            var dupStatus = repository.CheckDuplicates(item); 
            if (dupStatus==0)
            {
                if (repository.Add(item))
                {
                    response = Request.CreateResponse<Contact>(HttpStatusCode.Created, item);
                    string uri = Url.Link("DefaultApi", new { id = item.Id });
                    response.Headers.Location = new Uri(uri);

                }

                else
                {
                    response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Internal server error occurred. Could not save data."),
                        ReasonPhrase = "Error"
                    };
                    throw new HttpResponseException(response);
                }
                return response;
            }




            else if (dupStatus == 1 || dupStatus ==2)
            {
                string message = "";
                if(dupStatus == 1)
                    message = "Email already exists!";
                else if(dupStatus ==2)
                    message = "Phone no already exists";
                response = new HttpResponseMessage(HttpStatusCode.Conflict)
                    {
                        Content = new StringContent(message),
                        ReasonPhrase = "Error"
                    };
                throw new HttpResponseException(response);
            }

            else
            {
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Internal server error occurred. Could not save data."),
                    ReasonPhrase = "Error"
                };
                throw new HttpResponseException(response);
            }
            
        }

       
        public HttpResponseMessage PutContact(Contact Contact)
        {
             var response = new HttpResponseMessage();
             var dupStatus = repository.CheckDuplicates(Contact); 
            if (dupStatus==0)
            {
                if (!repository.Update(Contact))
                {
                    throw new ItemNotFoundException("Contact Not Found");
                }
                string uri = Url.Link("DefaultApi", new { id = Contact.Id });
                response = Request.CreateResponse<Contact>(HttpStatusCode.OK, Contact);
                response.Headers.Location = new Uri(uri);
                return response;
            }
            else if (dupStatus == 1 || dupStatus == 2)
            {
                string message = "";
                if (dupStatus == 1)
                    message = "Email already exists!";
                else if (dupStatus == 2)
                    message = "Phone no already exists!";
                response = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent(message),
                    ReasonPhrase = "Error"
                };
                throw new HttpResponseException(response);
            }

            else
            {
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Internal server error occurred. Could not save data."),
                    ReasonPhrase = "Error"
                };
                throw new HttpResponseException(response);
            }

        }

        
        public HttpResponseMessage DeleteContact(int id)
        {
            if (repository.Remove(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                throw new ItemNotFoundException("Contact not found");
            }
        }
    }
}
