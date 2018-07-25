using ContactsAPI.Models.DBModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.Entity.Migrations;
using ContactsAPI.Models.DBModel;

namespace ContactsAPI.Repository
{
    public class ContactsRepository : IContactsRepository
    {
        ContactsDBContext conDB = new ContactsDBContext();
        public IEnumerable<Models.DBModel.Contact> GetAll()
        {
            return conDB.Contacts;
        }

        public Models.DBModel.Contact Get(int id)
        {
            return conDB.Contacts.SingleOrDefault(c => c.Id == id);
        }

        public bool Add(Models.DBModel.Contact item)
        {
            try
            {
                conDB.Contacts.Add(item);
                conDB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Remove(int id)
        {
            try
            {
                this.conDB.Contacts.Remove(conDB.Contacts.Where(c => c.Id == id).Select(c => c).SingleOrDefault());
                this.conDB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Models.DBModel.Contact item)
        {
            try
            {
                //conDB.Entry(item).State = EntityState.Modified;
                conDB.Contacts.AddOrUpdate(item); //requires using System.Data.Entity.Migrations;
                conDB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int CheckDuplicates(Models.DBModel.Contact item)
        {
            try
            {
                var e = conDB.Contacts.FirstOrDefault(c => c.Email == item.Email);
                var p = conDB.Contacts.FirstOrDefault(c => c.PhoneNumber == item.PhoneNumber);
                if (e != null && e.Id != item.Id)
                    return 1;
                else if (p != null && p.Id != item.Id)
                    return 2;
                else
                    return 0;
            }
            catch
            {
                return 3;
            }
        }

        public bool LogException(Models.DBModel.Log log)
        {
            try
            {
                conDB.Logs.Add(log);
                conDB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}