using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFML_UI
{
    class Program
    {
        static void Main(string[] args)
        {            
            Screen.Open(0, 0, 800, 600, "");
            Screen.Events();            
            Screen.GameLoop();            
        }               
    }
}
