using AvisFormation.Data;
using AvisFormation.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AvisFormation.WebUI.Controllers
{
    public class FormationController : Controller
    {
        // GET: Formation
        public ActionResult ToutesLesFormations()
        {

            var listFormation = new List<Formation>();
            var vm = new AccueilViewModel();
            using (var context = new AvisEntities())
            {
                listFormation = context.Formation.OrderBy(x => Guid.NewGuid()).ToList();
                foreach (var f in listFormation)
                {
                    var dto = new FormationAvecAvisDto();
                    dto.Formation = f;
                    if (f.Avis.Count == 0)
                        dto.Note = 0;
                    else
                        dto.Note = Math.Round(f.Avis.Average(a => a.Note), 2);
                    vm.ListFormations.Add(dto);
                }
            }
            return View(vm);
        }

        public ActionResult DetailsFormation(string nomSeo)
        {
            var vm = new Models.FormationAvecAvisViewModel();
            using (var context = new AvisEntities())
            {
                var formationEntity = context.Formation.Where(f=>f.NomSeo == nomSeo).FirstOrDefault();
                if(formationEntity==null)
                {
                    return RedirectToAction("Accueil", "Home");
                }

                vm.FormationNom = formationEntity.Nom;
                vm.FormationDescription = formationEntity.Description;
                vm.FormationNomSeo = nomSeo;
                vm.FormationUrl = formationEntity.Url;
                if(formationEntity.Avis.Count>0)
                    vm.Note = formationEntity.Avis.Average(a=>a.Note);
                vm.NombreAvis = formationEntity.Avis.Count;
                vm.Avis = formationEntity.Avis.OrderByDescending(a=>a.DateAvis).ToList();
            }

            return View(vm);
        }
    }
}