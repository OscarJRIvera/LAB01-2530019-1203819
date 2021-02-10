using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LAB01_2530019_1203819.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LAB01_2530019_1203819.Controllers
{
    public class PlayerController : Controller
    {
        private readonly Models.Data.Singleton J = Models.Data.Singleton.Instance; //base de datos

        // GET: PlayerController
        public ActionResult Index()
        {
            if (J.TipeList == null)
                return RedirectToAction("Index", "Home");

            if (J.TipeList.Value) //si es verdadero se usa la lista del sistema
            {
                return View(J.PlayerList);
            }
            return View(J.List2);

        }

        // GET: PlayerController/Details/5
        public ActionResult Details(int? id)//? si existe o no existe 
        {
            if (J.TipeList == null)
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return NotFound();
            }

            Player PlayerModel;
            if(J.TipeList.Value)
            {
                 PlayerModel = J.PlayerList.Find(m => m.Id == id);
            }
            else
            {
                PlayerModel = J.List2.Find(m => m.Id == id);
            }

            if (PlayerModel == null)
            {
                return NotFound();
            }

            return View(PlayerModel);
        }

        // GET: PlayerController/Create
        public ActionResult Create()
        {
            if (J.TipeList == null)
                return RedirectToAction("Index", "Home");
            return View();
        }

       // public ActionResult Search(string? x)
       //{

       // }

        // POST: PlayerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id, Club,Last_name,First_name,Position,Base_salary,Guaranteed_compensation")] Player PlayerModel)
        {
            if (J.TipeList == null) 
                return RedirectToAction("Index", "Home");
            try
            {
               
                if (J.TipeList.Value)
                {
                    if(J.PlayerList.Count == 0 )
                    {
                        PlayerModel.Id = 1;
                    }
                    else
                    {
                        var tmp = J.PlayerList.Max(i => i.Id);//ya que si se elimina, salta el que falta para seguir. 
                        PlayerModel.Id = tmp;
                    }
                    J.PlayerList.Add(PlayerModel);
                }
                else
                {
                    if (J.List2.Count == 0)
                    {
                        PlayerModel.Id = 1;
                    }
                    else
                    {
                        var tmp = J.List2.Max(i => i.Id);//ya que si se elimina, salta el que falta para seguir. 
                        PlayerModel.Id = tmp;
                    }
                    J.List2.Add(PlayerModel);
                   
                }
               
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlayerController/Edit/5
        public IActionResult Edit(int? id)
        {
            if (J.TipeList == null)
                return RedirectToAction("Index", "Home");


            if (id == null)
            {
                return NotFound();
            }

            Player PlayerModel;
            if (J.TipeList.Value)
            {
                PlayerModel = J.PlayerList.Find(m => m.Id == id);
            }
            else
            {
                PlayerModel = J.List2.Find(m => m.Id == id);
            }
           
            if (PlayerModel == null)
            {
                return NotFound();
            }
            return View(PlayerModel);
        }

        // POST: PlayerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Id, Club,Last_name,First_name,Position,Base_salary,Guaranteed_compensation")] Player PlayerModel)
        {

            if (J.TipeList == null)
                return RedirectToAction("Index", "Home");
            if (id != PlayerModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    if (J.TipeList.Value)
                    {
                        var index = J.PlayerList.FindIndex(j => j.Id == id); //busca en que posicion del jugador en el array
                        J.PlayerList[index] = PlayerModel;
                    }
                    else
                    {
                        var index = J.List2.FindIndex(j => j.Id == id); //busca en que posicion del jugador en el array
                        J.List2[index] = PlayerModel;
                    }


                   
                }
                catch
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(PlayerModel);
        }


        // GET: PlayerController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (J.TipeList == null)
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return NotFound();
            }

            Player PlayerModel;
            if (J.TipeList.Value)
            {
                PlayerModel = J.PlayerList.Find(m => m.Id == id);
            }
            else
            {
                PlayerModel = J.List2.Find(m => m.Id == id);
            }

            if (PlayerModel == null)
            {
                return NotFound();
            }

            return View(PlayerModel);
        }

        // POST: PlayerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            if (J.TipeList == null)
                return RedirectToAction("Index", "Home");

            try
            {

                if (J.TipeList.Value)
                {
                    var index = J.PlayerList.FindIndex(j => j.Id == id); //busca en que posicion del jugador en el array
                    J.PlayerList.RemoveAt(index);
                }
                else
                {
                    var index = J.List2.FindIndex(j => j.Id == id); //busca en que posicion del jugador en el array
                    J.List2.RemoveAt(index);
                }
               

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
