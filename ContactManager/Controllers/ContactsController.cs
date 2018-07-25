using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using ContactManager.Models;
using ContactsAPI;
using System.Threading.Tasks;

namespace ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        HttpClient client;
        public ContactsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:64974/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        List<SelectListItem> status = new List<SelectListItem> { new SelectListItem { Text = "Active", Value = "Active" }, new SelectListItem { Text = "Inactive", Value = "Inactive" } };
        //
        // GET: /Contacts/
        public ActionResult Index()
        {
            HttpResponseMessage resp = null;
            var status = TempData["Status"];
            ViewBag.Status = status;
            try
            {
                var gettask = client.GetAsync("ContactsAPI");
                gettask.Wait();
                resp = gettask.Result;
                if (resp.IsSuccessStatusCode)
                {
                    var getResult = resp.Content.ReadAsAsync<List<ContactsAPI.Models.DBModel.Contact>>();
                    getResult.Wait();
                    List<ContactsAPI.Models.DBModel.Contact> cnts = getResult.Result;
                    List<Contact> contacts = new List<Contact>();
                    foreach (var c in cnts)
                    {
                        contacts.Add(TranslateForGet(c));
                    }

                    return View(contacts);

                }

                if (resp != null)
                {
                    //ModelState.AddModelError(string.Empty, resp.Content.ReadAsStringAsync().Result);
                    if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return View("NoDataAvailable");
                }

                return View("InternalError");
            }

            catch
            {
                return View("InternalError");
            }


        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            HttpResponseMessage response = null;
            try
            {
                var selectedIds = form.GetValues("checked");
                
                if (selectedIds != null)
                {
                    foreach (var id in selectedIds)
                    {
                        if (Convert.ToString(id).ToUpper() != "FALSE")
                        {
                            Task<HttpResponseMessage> deleteTask = client.DeleteAsync("ContactsAPI/" + id);
                            deleteTask.Wait();
                            response = deleteTask.Result;
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["Status"] = "Successfully deleted";
                                return RedirectToAction("Index");
                            }

                            if (response != null)
                            {
                                //ModelState.AddModelError(string.Empty, resp.Content.ReadAsStringAsync().Result);
                                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                                    return View("NotFound");
                                else
                                    return View("InternalError");
                            }

                            return View("InternalError");
                        }

                        
                    }

                    return View("InternalError");
                        
                }

                return View("InternalError");
            }

            catch
            {
                return View("InternalError");
            }

        }


        //
        // GET: /Contacts/Create
        public ActionResult Create()
        {
            ViewBag.StatusList = status;
            return View();
        }

        //
        // POST: /Contacts/Create
        [HttpPost]
        public ActionResult Create(Contact contact)
        {
            ViewBag.StatusList = status;
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = null;
                try
                {
                    ViewBag.StatusList = status;
                    ContactsAPI.Models.DBModel.Contact c = TranslateForPost(contact);
                    Task<HttpResponseMessage> postTask = client.PostAsJsonAsync<ContactsAPI.Models.DBModel.Contact>("ContactsAPI", c);
                    postTask.Wait();
                    response = postTask.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Status"] = "Successfully added";
                        return RedirectToAction("Index");
                    }

                    if (response != null)
                    {
                        ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                    }
                    return View(contact);


                }
                catch (Exception e)
                {
                    if (response != null)
                    {
                        ModelState.AddModelError(string.Empty, "Some internal error occurred");
                    }

                    
                    return View(contact);
                }
            }
            else
            {
                return View(contact);
            }
        }

        private ContactsAPI.Models.DBModel.Contact TranslateForPost(Contact source)
        {
            ContactsAPI.Models.DBModel.Contact target = new ContactsAPI.Models.DBModel.Contact();
            foreach (var src in source.GetType().GetProperties())
            {
                foreach (var trgt in target.GetType().GetProperties())
                {
                    if (src.Name.ToUpper() == trgt.Name.ToUpper())
                    {
                        trgt.SetValue(target, src.GetValue(source));
                    }
                }
            }

            return target;
        }

        private Contact TranslateForGet(ContactsAPI.Models.DBModel.Contact source)
        {
            Contact target = new Contact();
            foreach (var src in source.GetType().GetProperties())
            {
                foreach (var trgt in target.GetType().GetProperties())
                {
                    if (src.Name.ToUpper() == trgt.Name.ToUpper())
                    {
                        trgt.SetValue(target, src.GetValue(source));
                    }
                }
            }

            return target;
        }

        //
        // GET: /Contacts/Edit/5
        [HttpGet]
        public ActionResult Edit(Contact contact)
        {
            ViewBag.StatusList = status;
            return View(contact);
        }

        //
        // POST: /Contacts/Edit/5
        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(Contact contact)
        {
             ViewBag.StatusList = status;
             if (ModelState.IsValid)
             {
                 HttpResponseMessage response = null;
                 try
                 {
                     ViewBag.StatusList = status;
                     ContactsAPI.Models.DBModel.Contact c = TranslateForPost(contact);
                     Task<HttpResponseMessage> putTask = client.PutAsJsonAsync<ContactsAPI.Models.DBModel.Contact>("ContactsAPI", c);
                     putTask.Wait();
                     response = putTask.Result;
                     if (response.IsSuccessStatusCode)
                     {
                         TempData["Status"] = "Successfully edited";
                         return RedirectToAction("Index");
                     }

                     if (response != null)
                     {
                         if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                             ModelState.AddModelError(string.Empty, "item Not Found");
                         else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                             ModelState.AddModelError(string.Empty, response.Content.ReadAsStringAsync().Result);
                         else
                             ModelState.AddModelError(string.Empty, "Some internal error occurred");
                     }
                     return View(contact);


                 }
                 catch (Exception)
                 {
                     if (response != null)
                     {
                         ModelState.AddModelError(string.Empty, "Some internal error occurred. Please try again.");
                     }
                     return View(contact);
                 }
             }
             else
             {
                 return View(contact);
             }
        }

        //
        // GET: /Contacts/Delete/5

    }
}
