﻿using System;
using System.Collections.Generic;      

namespace SerialGUI
{
    class Race
    {
        private readonly int step = 5;

        public List<Lap> Laps { get; } 

        public Race()
        {              
            Laps = new List<Lap>();
        }

        public void AddLap(Lap lap)
        {              
            Laps.Add(lap);
        }            
                 
        public List<Instruction> GetInstructions()
        {                                         
            List<Instruction> toReturn = new List<Instruction>();

            for (int i = 0; i < Laps[Laps.Count - 1].Records.Count; i++)
            {
                if (Math.Abs((int)(Laps[Laps.Count - 1].Records[i].ZRot)) < 5000)
                {
                    Instruction tmp = new Instruction(Laps[Laps.Count - 1].GetInstructions()[i].GetTacho(),
                        90, 0);
                    toReturn.Add(tmp);
                }
                else
                    toReturn.Add(Laps[Laps.Count - 1].GetInstructions()[i]);
            }                                     

            return toReturn;
        }        
    }
}                                                 