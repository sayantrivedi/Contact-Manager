using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsAPI.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message) { }
        public ItemNotFoundException(string message, Exception ex) : base(message, ex) { }

    }
}