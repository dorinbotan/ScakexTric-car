namespace SerialGUI
{
    class CRC8
    {
        /*
	     * Generator - x^8 + x^2 + x + 1
	     * @param data - byte array terminated by a null
	     * @return - checksum 
         */
        public static byte Checksum(byte[] data)
        {
            short register = 0;
            short bitMask = 0;
            short poly = 0;
            register = data[0];

            for (int i = 1; i < data.Length; i++)
            {
                register = (short)((register << 8) | (data[i] & 0xff));

                unchecked
                {
                    poly = (short)(0x107 << 7);
                    bitMask = (short)0x8000;
                }

                while (bitMask != 0x80)
                {
                    if ((register & bitMask) != 0)
                        register ^= poly;

                    poly = (short)((poly & 0xffff) >> 1);
                    bitMask = (short)((bitMask & 0xffff) >> 1);
                }
            }

            return (byte)register;
        }

        public static byte[] ShortToByte(short number)
        {
            return new byte[] { (byte)(number & 255), (byte)(number >> 8) };
        }

        public static short ByteToShort(short byte1, short byte2)
        {
            return (short)((byte2 << 8) + byte1);
        }
    }
}                       