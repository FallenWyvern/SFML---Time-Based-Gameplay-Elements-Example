using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Window;

namespace SFML_UI
{
    public static class Controls
    {
        public static void _Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
        }

        public static void _Window_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
        }

        public static void _Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
        }

        public static void _Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
        }

        public static void _Window_KeyReleased(object sender, KeyEventArgs e)
        {            
        }

        public static void _Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.T)
            {
                Console.WriteLine(Environment.NewLine);
                foreach (Tavern t in Data.Taverns)
                {                    
                    t.PayTaxes(5.0);                    
                }
                Console.WriteLine("Town Gold: " + Data.TownGold + Environment.NewLine);
            }
        }
    }
}
