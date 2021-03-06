﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Nestor.Business;
using Nestor.Common;
using Nestor.Models;
using Nestor.Models.Entities;
using Nestor.UI.Models;
using Nestor.UI.Services;
using Nestor.UI.Filters;

namespace Nestor.UI.Controllers
{
    /// <summary>
    /// 账户控制器
    /// </summary>
    public class AccountController : Controller
    {
        #region Field
        /// <summary>
        /// 认证服务
        /// </summary>
        private IFormsAuthenticationService formsService;

        /// <summary>
        /// 账户服务
        /// </summary>
        private UserBusiness userBusiness;
        #endregion //Field

        #region Function
        protected override void Initialize(RequestContext requestContext)
        {
            if (formsService == null)
            {
                formsService = new FormsAuthenticationService();
            }
            if (userBusiness == null)
            {
                userBusiness = new UserBusiness();
            }
            base.Initialize(requestContext);
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion //Function

        #region Action
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// POST: 用户登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                formsService.SignOut();
                HttpContext.Session.Clear();

                ErrorCode result = this.userBusiness.Login(model.UserName, model.Password);
                if (result == ErrorCode.Success)
                {
                    User user = this.userBusiness.Get(model.UserName);
                    HttpCookie cookie = formsService.SignIn(user.UserName, user.UserTypeName(), model.RememberMe);
                    Response.Cookies.Add(cookie);

                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", result.DisplayName());
                }
            }

            return View(model);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            formsService.SignOut();
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        [EnhancedAuthorize]
        public ActionResult Info()
        {
            var user = this.userBusiness.Get(User.Identity.Name);
            return View(user);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [EnhancedAuthorize]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnhancedAuthorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                ErrorCode result = this.userBusiness.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);

                if (result == ErrorCode.Success)
                {
                    TempData["Message"] = "修改密码成功";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "修改密码失败，" + result.DisplayName());
                }
            }

            return View(model);
        }
        #endregion //Action
    }
}