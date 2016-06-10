namespace SerialGUI
{          
    public struct Record
    {
        public int tacho { get; }
        public short XAcc { get; }
        public short YAcc { get; }
        public short ZAcc { get; }
        public short XRot { get; }
        public short YRot { get; }
        public short ZRot { get; }

        // work on this
        //public byte Step { set; get; }

        public Record(int tacho, short XAcc, short YAcc,
            short ZAcc, short XRot, short YRot, short ZRot)
        {
            this.tacho = tacho;
            this.XAcc = XAcc;
            this.XRot = XRot;
            this.YAcc = YAcc;
            this.YRot = YRot;
            this.ZAcc = ZAcc;
            this.ZRot = ZRot;
        }

        public override string ToString()
        {
            return string.Format("{0, -7} {1, -13} {2, -13} {3, -13} {4, -13} {5, -13} {6, -13}", 
                tacho, XAcc, YAcc, ZAcc, XRot, YRot, ZRot);
        }
    } 
}                        