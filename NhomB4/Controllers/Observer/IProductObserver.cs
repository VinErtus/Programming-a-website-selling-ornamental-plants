using NhomB4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers.Observer
{
    public interface IProductObserver
    {
        void ProductUpdated(Product product);
    }
}