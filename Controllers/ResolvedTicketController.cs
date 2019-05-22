﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JTicket.Controllers;
using JTicket.Models;

namespace JTicket.Views
{
    public class ResolvedTicketController : TicketController
    {

        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.CanManageTickets))
                return View("Index");

            return View("ReadOnlyIndex");
        }





    }
}