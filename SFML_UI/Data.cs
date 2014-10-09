using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFML_UI
{
    public static class Data
    {
        public static List<Tavern> Taverns = new List<Tavern>();
        public static int Population = 0;
        public static int TownGold = 0;

        static public Random rnd = new Random();        
        static public Sky.TimeOfDay lastTime = Sky.TimeOfDay.Dawn;

        public static void AdvanceTown(Sky.TimeOfDay d)
        {                            
            if (lastTime != d)
            {
                switch (d)
                {
                    case Sky.TimeOfDay.Dawn:
                        lastTime = Sky.TimeOfDay.Dawn;                        
                        foreach (Tavern t in Taverns)
                        {
                            ModTravellers(t, false);
                            t.DailyOperations();
                        }
                        if ((Sky.DayOfWeek)Sky.dayName == Sky.DayOfWeek.Unis)
                        {
                            Console.WriteLine("Town Gold: " + Data.TownGold);
                            Console.WriteLine("Town Population: " + Data.Population);
                            Console.WriteLine(Environment.NewLine);
                        }
                        break;
                    case Sky.TimeOfDay.Day:
                        lastTime = Sky.TimeOfDay.Day;
                        foreach (Tavern t in Taverns)
                        {
                            t.DailyOperations();
                            ModPopulation(t, 10, true);
                        }
                        break;
                    case Sky.TimeOfDay.Evening:
                        lastTime = Sky.TimeOfDay.Evening;
                        foreach (Tavern t in Taverns)
                        {
                            ModTravellers(t, true);
                            t.DailyOperations();
                        }
                        break;
                    case Sky.TimeOfDay.Night:
                        lastTime = Sky.TimeOfDay.Night;
                        foreach (Tavern t in Taverns)
                        {                            
                            t.DailyOperations();
                        }
                        break;
                }                
            }            
        }        

        public static void ModTravellers(Tavern Tavern, bool AddRemove)
        {
            if (AddRemove)
            {
                Tavern.AddTravellers(rnd.Next(0, 10));
            }
            else
            {
                Tavern.RemoveTravellers(rnd.Next(0, Tavern.Travellers));
            }
        }

        public static void ModPopulation(Tavern Tavern, int percentChance, bool AddRemove)
        {
            if (Tavern.Travellers > 1)
            {
                if (rnd.Next(0, 100) < percentChance)
                {
                    if (AddRemove)
                    {
                        int pop = rnd.Next(1, (int)Math.Ceiling(Tavern.Travellers * 0.05));
                        Tavern.RemoveTravellers(pop);
                        Population += pop;
                    }
                    else
                    {
                    }
                }
            }
        }
    }

    public class Tavern
    {
        public int Name = 0;
        public int Travellers = 0;
        public int MaxTravellers = 10;
        public int Gold = 0;

        public string TownStats()
        {
            return Name + ": Current Occupants - " + Travellers + " and Gold: " + Gold;
        }

        public Tavern()
        {
            Name = Data.rnd.Next(1, 999);
        }

        public void AddTravellers(int NumberOfTravellers)
        {
            Travellers += NumberOfTravellers;
            if (Travellers > MaxTravellers) Travellers = MaxTravellers;            
        }

        public void RemoveTravellers(int NumberOfTravellers)
        {
            Travellers -= NumberOfTravellers;
            if (Travellers < 0) Travellers = 0;            
        }

        public void DailyOperations()
        {
            if (Data.lastTime == Sky.TimeOfDay.Dawn)
            {
                for (int i = 0; i < Travellers; i++)
                {
                    Gold -= (int)(Travellers * 0.4);
                }
            }
            else
            {
                for (int i = 0; i < Travellers; i++)
                {
                    Gold += Data.rnd.Next(0, 3);
                }
            }
            if ((Sky.DayOfWeek)Sky.dayName == Sky.DayOfWeek.Unis && Data.lastTime == Sky.TimeOfDay.Dawn)
            {                        
                PayTaxes(35.0);                                
            }
        }

        public double PayTaxes(double percentage)
        {
            double taxes = ((double)Gold * (percentage/100));
            if (taxes < 0) taxes = 0;
            Gold -= (int)Math.Ceiling(taxes);
            if (Gold < 0) Gold = 0;  
          
            Data.TownGold += (int)Math.Ceiling(taxes);
            Console.WriteLine(Name + ": Paying Taxes (%" + percentage + ") " + taxes + " leaving " + Gold + " gold.");            
            return taxes;
        }
    }
}
