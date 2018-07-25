using ContactsAPI.Models.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsAPI.Repository
{
    interface IContactsRepository
    {
        IEnumerable<Contact> GetAll();
        Contact Get(int id);
        bool Add(Contact item);
        bool Remove(int id);
        bool Update(Contact item);
        int CheckDuplicates(Models.DBModel.Contact item);
        bool LogException(Models.DBModel.Log log);
    }
}
