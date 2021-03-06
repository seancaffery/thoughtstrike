﻿using System.Linq;
using System.Web.Mvc;
using Ideastrike.Helpers.Attributes;
using Ideastrike.Models;
using Ideastrike.Models.Repositories;

namespace Ideastrike.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdeaRepository _ideas;
        private readonly IUserRepository _users;
        private readonly ISettingsRepository _settings;

        public HomeController(IIdeaRepository ideas, IUserRepository users, ISettingsRepository settings)
        {
            _ideas = ideas;
            _users = users;
            _settings = settings;
        }

        public ActionResult Index()
        {
            ViewBag.Selected = SelectedTab.Popular;
            ViewBag.Items = _ideas.GetAll();

            return View();
        }

        [IdeastrikeAuthorize(Roles = "Administrators")] // this should match up with a claim on the user 
        public ActionResult Secured()
        {
            return View("Index");
        }

        public ActionResult Top()
        {
            ViewBag.Selected = SelectedTab.Top; 
            ViewBag.Items = _ideas.GetAll().OrderByDescending(i => i.Votes.Sum(s => s.Value));

            return View("Index");
        }

        public ActionResult New()
        {
            ViewBag.Selected = SelectedTab.New;
            ViewBag.Items = _ideas.GetAll().OrderByDescending(i => i.Time);

            return View("Index");
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.WelcomeMessage = _settings.WelcomeMessage;
            ViewBag.Title = _settings.SiteTitle;

            base.OnActionExecuted(filterContext);
        }

    }
}
