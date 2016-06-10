using System.Collections.Generic;        

namespace SerialGUI
{
    class Lap
    {
        public List<Record> Records { get; }
        private List<Instruction> instructions;

        public Lap()
        {
            Records = new List<Record>();
            instructions = new List<Instruction>();         
        }

        public void AddRecord(Record record)
        {
            Records.Add(record);
        }    

        public void AddInsructions(List<Instruction> instructions)
        {   
            this.instructions.AddRange(instructions);
        }

        public List<Instruction> GetInstructions()
        {
            if (instructions.Count == 0)                         
                for (int i = 0; i < 300; i++)
                    instructions.Add(new Instruction(4, 75, 0)); 

            return instructions;
        }
    }
}