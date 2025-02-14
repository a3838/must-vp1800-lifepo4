﻿namespace Must.Modbus
{
    public static class Utils
    {
        public static ushort CalculateCrc(byte[] message, int offset, int count)
        {
            ushort crcFull = 0xFFFF;
            char crcLsb;

            for (int i = 0; i < count; i++)
            {
                crcFull = (ushort)(crcFull ^ message[offset + i]);

                for (int j = 0; j < 8; j++)
                {
                    crcLsb = (char)(crcFull & 0x0001);
                    crcFull = (ushort)(crcFull >> 1 & 0x7FFF);

                    if (crcLsb == 1)
                        crcFull = (ushort)(crcFull ^ 0xA001);
                }
            }
            byte crcHigh = (byte)(crcFull >> 8 & 0xFF);
            byte crcLow = (byte)(crcFull & 0xFF);

            return (ushort)(crcHigh | (uint)crcLow << 8);
        }
    }
}