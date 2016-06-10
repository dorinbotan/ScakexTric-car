using System;                

namespace SerialGUI
{
    class OutMessage
    {
        private byte[] message;

        public OutMessage(byte seq, byte tacho, byte speed, byte brake)
        {
            message = new byte[5];
            message[0] = seq;
            message[1] = tacho;
            message[2] = speed;
            message[3] = brake;
            message[4] = (byte)(CRC8.Checksum(message) & 0xff);
        }

        public OutMessage(byte[] values)
        {
            if (values.Length < 4 || values.Length > 5)
                throw new Exception("Wrong input.");

            message = new byte[5];
            for (int i = 0; i < 4; i++)
                message[i] = values[i];

            if (values.Length == 5)
            {
                message[4] = values[4];

                if (CRC8.Checksum(message) != 0)
                    throw new Exception("Wrong CRC.");
            }
            else
                message[4] = (byte)(CRC8.Checksum(message) & 0xff);
        }

        public byte[] GetMessage()
        {
            return message;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", message[0],
                message[1], message[2], message[3], message[4]);
        }
    }
}    