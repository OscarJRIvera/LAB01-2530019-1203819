using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB01_2530019_1203819.Models.Data
{
    public sealed class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public bool? TipeList;
        public List<Player> PlayerList; //la lista del sistema
        public List<Player> List2;//mi lista aqui 

        private Singleton()
        {
            PlayerList = new List<Player>();
        }

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
