namespace SerialGUI
{
    struct Instruction
    {
        byte[] instruction;

        public Instruction(byte tacho, byte speed, byte brake)
        {
            instruction = new byte[] { tacho, speed, brake };
        }

        public byte[] GetInstruction()
        {
            return instruction;
        }

        public byte GetTacho()
        {
            return instruction[0];
        }

        public byte GetSpeed()
        {
            return instruction[1];
        }

        public byte GetBrake()
        {
            return instruction[2];
        }
    }
}    