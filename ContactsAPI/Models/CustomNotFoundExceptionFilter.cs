using ContactsAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using ContactsAPI.Models.DBModel;
using ContactsAPI.Repository;

namespace ContactsAPI.Models
{
    public class CustomNotFoundExceptionFilterAttribute : ExceptionFilterAttribute  
    {
        static readonly IContactsRepository repository = new ContactsRepository();

        public override void OnException(HttpActionExecutedContext context)  
        {
            Log log = new Log();
            log.Desc = context.Exception.Message;
            log.StackTrace = context.Exception.StackTrace;
            log.Date = DateTime.Now;
            if (context.Exception.InnerException != null)
                log.Inner_Exception = context.Exception.InnerException.Message;
            repository.LogException(log);


            if (context.Exception is ItemNotFoundException)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "ItemNotFound"
                };
                
                throw new HttpResponseException(resp);
            } 
        }  
    }  
}  