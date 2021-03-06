﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nestor.Business;
using Nestor.Models;
using Nestor.Models.Entities;

namespace Nestor.UI.Controllers
{
    public class HomeController : Controller
    {
        #region Action
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 菜单
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult Menu()
        {
            ColumnBusiness business = new ColumnBusiness();
            var data = business.GetTop().Where(r => r.ShowTop == true && r.Type != (int)ColumnType.Creative);
            return View(data);
        }
        #endregion //Action
    }
}