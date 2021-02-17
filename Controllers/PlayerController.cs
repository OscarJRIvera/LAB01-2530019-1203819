using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LAB01_2530019_1203819.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using DoubleLinkedListLibrary1;

namespace LAB01_2530019_1203819.Controllers
{
    public class PlayerController : Controller
    {
        public List<Player> PlayerList; //la lista del sistema
        public DoubleLinkedList<Player> List2;//mi lista aqui 
        
        private readonly Models.Data.Singleton J = Models.Data.Singleton.Instance; //base de datos
        private readonly Models.Data.Singleton C;

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
        public IActionResult IndexSearchName() => View();
        
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
                PlayerModel = J.PlayerList.Find(m => m.Id == id);
               // PlayerModel = J.List2.Find(m => m.Id == id);
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
        public delegate int PlayerComp(Player a, Player b); //Crear el tipo de delegado que recibe los dos parametros a comparar
        public ActionResult Search(string Filter, string Param)
        {
            PlayerComp comparador;//Un tipo de apuntador para tener internamente que tipo de comparacion se va a hacer
            Player PlayerModel;
            switch (Filter)
            {
                case "Nombre":
                    comparador = Player.Compare_First_Name;
                    PlayerModel = new Player { First_name = Param };
                    break;
                case "Apellido":
                    comparador = Player.Compare_Last_Name;
                    PlayerModel = new Player {Last_name = Param };
                    break;
                case "Club":
                    comparador = Player.Compare_Club;
                    PlayerModel = new Player { Club = Param };
                    break;
                case "Posición":
                    comparador = Player.Compare_Position;
                    PlayerModel = new Player { Position = Param };
                    break;
                case "Salario Igual A":
                    comparador = Player.Compare_Base_Salary;
                    PlayerModel = new Player { Base_salary = double.Parse(Param) };
                    break;
                case "Salario Mayor A":
                    comparador = Player.Compare_Base_Salary_Upper;
                    PlayerModel = new Player { Base_salary = double.Parse(Param) };
                    break;
                case "Salario Menor A":
                    comparador = Player.Compare_Base_Salary_Lower;
                    PlayerModel = new Player { Base_salary = double.Parse(Param) };
                    break;
                default:
                    comparador = (Player a, Player b) => { return a.Id.CompareTo(b.Id); };//funcion anonima, solo por si no entra en ninguno de los cases.
                    PlayerModel = new Player { Id = int.Parse(Param) };
                    break;
            }

            if (J.TipeList.Value)
            {
                var List = J.PlayerList.FindAll(m => comparador(m, PlayerModel) == 0);
                return View("Index", List);
            }

            else
            {
                var List = J.List2.FindAll(m => comparador(m, PlayerModel) == 0);
                return View("Index", List);
            }
        }

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

                   if (J.List2.Count() == 0)
                   {
                       PlayerModel.Id = 1;
                   }
                   else
                   {
                        var tmp = J.List2.Count()+1;//ya que si se elimina, salta el que falta para seguir. 
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
        public IActionResult Edit(int? Id)
        {
            if (J.TipeList == null)
                return RedirectToAction("Index", "Home");


            if (Id == null)
            {
                return NotFound();
            }

            Player PlayerModel;
            if (J.TipeList.Value)
            {
                PlayerModel = J.PlayerList.Find(m => m.Id == Id);
            }
            else
            {
                PlayerModel = J.PlayerList.Find(m => m.Id == Id);
                //PlayerModel = J.List2.Find(m => m.Id == Id);
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
                        //var index = J.List2.FindIndex(j => j.Id == id); //busca en que posicion del jugador en el array
                        //J.List2[index] = PlayerModel;
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
                PlayerModel = J.PlayerList.Find(m => m.Id == id);
                //PlayerModel = J.List2.Find(m => m.Id == id);
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
                    //var index = J.List2.FindIndex(j => j.Id == id); //busca en que posicion del jugador en el array
                    //J.List2.RemoveAt(index);
                }
               

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            int i ;
            if (J.TipeList.Value)
            {
                i = J.PlayerList.Count()+1;
            }  
            else
            {
                i = J.List2.Count()+1;
            }
                if (file == null || file.Length == 0) return Content("file not selected");
            byte[] byts = new byte[file.Length];
            using (var strm = file.OpenReadStream())
            {
                await strm.ReadAsync(byts, 0, byts.Length);
            }
            string cnt = Encoding.UTF8.GetString(byts);
            string[] lines = cnt.Split('\n');
            
            foreach (string line in lines)
            {
                if (line == "club,last_name,first_name,position,base_salary,guaranteed_compensation") { }
                else if (line == "") { }
                else 
                {
                    string[] parts = line.Split(',');
                    Player nuevo = new Player
                    {
                        Id = i,
                        Club = parts[0],
                        Last_name = parts[1],
                        First_name = parts[2],
                        Position = parts[3],
                        Base_salary = double.Parse(parts[4]),
                        Guaranteed_compensation = double.Parse(parts[5])
                    };
                    if (J.TipeList.Value)
                        J.PlayerList.Add(nuevo);
                    else
                        J.List2.Add(nuevo);

                    
                    i++;
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
