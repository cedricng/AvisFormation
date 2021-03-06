using AvisFormation.Data;
using AvisFormation.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AvisFormation.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Accueil()
        {
            var listFormation = new List<Formation>();
            var vm= new AccueilViewModel();
            using(var context=new AvisEntities())
            {
                listFormation = context.Formation.OrderBy(x=>Guid.NewGuid()).Take(4).ToList();
                foreach (var f in listFormation)
                {
                    var dto = new FormationAvecAvisDto();
                    dto.Formation = f;
                    if (f.Avis.Count == 0)
                        dto.Note = 0;
                    else
                        dto.Note = Math.Round(f.Avis.Average(a => a.Note),2);
                    vm.ListFormations.Add(dto);
                }
            }
            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}