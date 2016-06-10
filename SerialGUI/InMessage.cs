using System;                

namespace SerialGUI
{
    class InMessage
    {
        private byte[] message;

        public InMessage(byte seq, byte tacho, short XAcceleration,
            short YAcceleration, short ZAcceleration, short XRotation,
            short YRotation, short ZRotation, byte crc)
        {
            message = new byte[15];
            message[0] = seq;
            message[1] = tacho;
            message[2] = CRC8.ShortToByte(XAcceleration)[0];
            message[3] = CRC8.ShortToByte(XAcceleration)[1];
            message[4] = CRC8.ShortToByte(YAcceleration)[0];
            message[5] = CRC8.ShortToByte(YAcceleration)[1];
            message[6] = CRC8.ShortToByte(ZAcceleration)[0];
            message[7] = CRC8.ShortToByte(ZAcceleration)[1];
            message[8] = CRC8.ShortToByte(XRotation)[0];
            message[9] = CRC8.ShortToByte(XRotation)[1];
            message[10] = CRC8.ShortToByte(XRotation)[0];
            message[11] = CRC8.ShortToByte(XRotation)[1];
            message[12] = CRC8.ShortToByte(XRotation)[0];
            message[13] = CRC8.ShortToByte(XRotation)[1];
            message[14] = (byte)(CRC8.Checksum(message) & 0xff);
        }

        public InMessage(byte[] values)
        {
            if (values.Length < 14 || values.Length > 15)
                throw new Exception("Wrong input.");

            message = new byte[15];
            for (int i = 0; i < 14; i++)
                message[i] = values[i];

            if (values.Length == 15)
            {
                message[14] = values[14];

                if (CRC8.Checksum(message) != 0)
                    throw new Exception("Wrong CRC.");
            }
            else
                message[14] = (byte)(CRC8.Checksum(message) & 0xff);
        }

        public byte[] GetMessage()
        {
            return message;
        }

        public override string ToString()
        {
            string toReturn = string.Format("{0, -5} {1, -4} ", message[0], message[1]);

            for (int i = 2; i < 14; i+=2)
                toReturn += string.Format("{0,-10} ", CRC8.ByteToShort(message[i + 1], message[i]));

            toReturn += string.Format("{0, -5}", message[14]);

            return toReturn;
        }
    }
}              