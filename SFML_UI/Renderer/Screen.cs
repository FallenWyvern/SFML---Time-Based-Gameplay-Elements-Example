using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

namespace SFML_UI
{
    static public class Screen
    {
        static int X = 0;
        static int Y = 0;
        public static uint Width = 800;
        public static uint Height = 600;

        public static RenderWindow _Window { get; set; }               
        
        public static void Open(int x = 0, int y = 0, uint w = 800, uint h = 600, string Title = "")
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
            
            Sky.Sun.Origin = new Vector2f(Sky.Sun.GetLocalBounds().Width / 2, Sky.Sun.GetLocalBounds().Height / 2);
            Sky.Sun.Position = new Vector2f(Sky.Sun.GetLocalBounds().Width / 2, Sky.Sun.GetLocalBounds().Height / 2);

            Sky.Moon.Origin = new Vector2f(Sky.Moon.GetLocalBounds().Width / 2, Sky.Moon.GetLocalBounds().Height / 2);
            Sky.Moon.Position = new Vector2f(Screen.Width - Sky.Sun.GetLocalBounds().Width / 2, Screen.Height - Sky.Sun.GetLocalBounds().Height / 2);

            Sky.Line.FillColor = new Color(50, 150, 50);
            Sky.Line.Position = new Vector2f(0, 600 / 2);
            Sky.WeekTime.Color = Color.Red;            

            _Window = new RenderWindow(new VideoMode(Width, Height), Title);
            _Window.Position = new Vector2i(X, Y);

            for (int i = 0; i < 1; i++)
            {
                Data.Taverns.Add(new Tavern());
            }
        }

        public static void GameLoop()
        {
            if (_Window != null)
            {
                while (_Window.IsOpen())
                {
                    _Window.DispatchEvents();
                    Data.AdvanceTown((Sky.TimeOfDay)Sky.timeOfDay);
                    _Window.Clear(Sky.SkyColor);
                    Sky.Draw();
                    _Window.Display();
                }
            }
        }        

        public static void Events()
        {
            // Window Stuff
            _Window.Closed += (sender, e) =>
            {                
                _Window.Close();
            };
            _Window.Resized += new EventHandler<SizeEventArgs>(_Window_Resized);
         
            // Controls
            _Window.KeyPressed += new EventHandler<KeyEventArgs>(Controls._Window_KeyPressed);
            _Window.KeyReleased += new EventHandler<KeyEventArgs>(Controls._Window_KeyReleased);
            _Window.MouseButtonPressed += new EventHandler<MouseButtonEventArgs>(Controls._Window_MouseButtonPressed);
            _Window.MouseButtonReleased += new EventHandler<MouseButtonEventArgs>(Controls._Window_MouseButtonReleased);
            _Window.MouseWheelMoved += new EventHandler<MouseWheelEventArgs>(Controls._Window_MouseWheelMoved);
            _Window.MouseMoved += new EventHandler<MouseMoveEventArgs>(Controls._Window_MouseMoved);
        }
        
        static void _Window_Resized(object sender, SizeEventArgs e)
        {            
        }
    }

    public static class Sky
    {
        public static Sprite Sun = new Sprite(new Texture("sun.png"));
        public static Sprite Moon = new Sprite(new Texture("moon.png"));

        public static RectangleShape Line = new RectangleShape(new Vector2f(800, 400));
        public static Text WeekTime = new Text("", new Font(@".\Fonts\18cents.ttf"), 42);
        public static float sunSpeed = 0.0000025F;
        public static Vector2f SunCenter = new Vector2f(800 / 2, 600 / 2);

        static DateTime sundelay = DateTime.Now.ToUniversalTime();
        public static Color SkyColor = new Color(95, 95, 155);        

        public static void Draw()
        {           
            MoveSun(sunSpeed);            
            Screen._Window.Draw(Sun);
            Screen._Window.Draw(Moon);
            Screen._Window.Draw(Line);
            Screen._Window.Draw(WeekTime);
        }

        public static void MoveSun(float delay)
        {            
            if (DateTime.Now.ToUniversalTime() > sundelay)
            {
                sundelay = DateTime.Now.AddSeconds(delay).ToUniversalTime();                
                Sun.Position = RotateAroundPoint(Sun.Position.X, Sun.Position.Y, SunCenter.X, SunCenter.Y, 0.2);
                Moon.Position = RotateAroundPoint(Moon.Position.X, Moon.Position.Y, SunCenter.X, SunCenter.Y, 0.2);                                

                WeekTime.DisplayedString = FormatTime(Sun.Position.Y);
                WeekTime.Position = new Vector2f((float)SunCenter.X - WeekTime.GetLocalBounds().Width / 2, 0);
            }
        }

        static public Vector2f RotateAroundPoint(double OriginX, double OriginY, double CenterX, double CenterY, double Angle)
        {
            double angle = Angle * (Math.PI / 180);
            Vector2f returnValue = new Vector2f(
                (float)(Math.Cos(angle) * (OriginX - CenterX) - Math.Sin(angle) * (OriginY - CenterY) + CenterX),
                (float)(Math.Sin(angle) * (OriginX - CenterX) + Math.Cos(angle) * (OriginY - CenterY) + CenterY)
                );
                        
            return returnValue;
        }


        public enum Month { SnowThaw, DawnEdge, MistHollow, NewBloom, SunCore, HarvestsEdge, SeedFall, SnowBirth, YearsEnd };
        public enum DayOfWeek { Unis, Erik, Silje, Henk, Deyus, Prestus, Rupert };
        public enum TimeOfDay { Dawn, Day, Evening, Night };

        public static int timeOfDay = 0;
        public static int dayName = 0;
        public static int day = 1;
        public static int year = 1352;
        public static int month = 0;        
        
        static bool reset = false;

        static public string FormatTime(float time)
        {
            if (Sun.Position.Y < Screen.Height / 2)
            {
                if (Sun.Position.X < Screen.Width / 3)
                {
                    timeOfDay = 0;
                }

                if (Sun.Position.X > Screen.Width / 3 && Sun.Position.X < Screen.Width / 3 * 2)
                {
                    timeOfDay = 1;
                }

                if (Sun.Position.X > Screen.Width / 3 * 2)
                {
                    timeOfDay = 2;
                }
            }
            else
            {
                if (Sun.Position.X < Screen.Width / 3 && Sun.Position.Y < Screen.Height / 3 + Screen.Height / 2)
                {
                    timeOfDay = 0;
                }
                else
                {
                    timeOfDay = 3;
                }
            }

            if (!reset && time > Screen.Height / 2)
            {
                reset = true;

                day = (day + 1) % 31;                                
                dayName = (dayName + 1) % 7;

                if (day == 0)
                {
                    day++;
                    month = (month + 1) % Enum.GetNames(typeof(Month)).Length;
                }

                if (month == 0)
                {
                    year++;
                }
            }

            if (reset && time < 180)
            {
                reset = false;
            }

            return year + " " + (Month)month + " " + day + " (" + (DayOfWeek)dayName + ") " + (TimeOfDay)timeOfDay;
        }

        static float lerp(float a, float b, float f)
        {
            return a + f * (b - a);
        }
    }
}
