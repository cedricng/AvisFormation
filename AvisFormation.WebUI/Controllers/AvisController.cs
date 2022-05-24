using AvisFormation.Data;
using AvisFormation.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AvisFormation.Logic;

namespace AvisFormation.WebUI.Controllers
{
    public class AvisController : Controller
    {
        // GET: Avis
        [Authorize]
        public ActionResult LaisserUnAvis(string nomSeo)
        {
            var vm = new LaisserUnAvisViewModel();
            vm.NomSeo=nomSeo;
            using (var context = new AvisEntities())
            {
                var formationEntity = context.Formation.FirstOrDefault(f => f.NomSeo == nomSeo);
                if (formationEntity == null)
                    return RedirectToAction("Accueil", "Home");
                vm.FormationName = formationEntity.Nom;
            }
            return View(vm);
        }
        //public ActionResult Sa
        //veComment(string commentaire, string nom, string note, string nomSeo)
        [HttpPost]
        [Authorize]
        public ActionResult SaveComment(SaveCommentViewModel comment)
        {


            using (var context = new AvisEntities())
            {
                var formationEntity = context.Formation.FirstOrDefault(f => f.NomSeo == comment.NomSeo);
                if (formationEntity == null)
                    return RedirectToAction("Accueil", "Home");
                Avis nouvelAvis = new Avis();
                nouvelAvis.DateAvis = DateTime.Now;

                var userId = User.Identity.GetUserId();

                var mgerUnicite = new UniqueAvisVerification();
                if (!mgerUnicite.estAutoriseACommenter(userId, formationEntity.Id))
                {
                    TempData["Message"] = "Désolé un seul avis par formation";
                    return RedirectToAction("DetailsFormation", "Formation", new { nomSeo = comment.NomSeo });
                }

                var mger = new PersonneManager();
                nouvelAvis.Nom = mger.GetNomFromUserId(userId);
                nouvelAvis.Description = comment.commentaire;


                nouvelAvis.UserId = userId;
                double bNote = 0;
                if (!double.TryParse(comment.Note, NumberStyles.Any, CultureInfo.InvariantCulture, out bNote))
                {
                    throw new Exception("Impossible de parser la note" + comment.Note);
                }

                nouvelAvis.Note = bNote;
                nouvelAvis.IdFormation = formationEntity.Id;

                context.Avis.Add(nouvelAvis);

                context.SaveChanges();
            }
                
              
            return RedirectToAction("DetailsFormation","Formation",new {nomSeo = comment.NomSeo});
        }
    }
}