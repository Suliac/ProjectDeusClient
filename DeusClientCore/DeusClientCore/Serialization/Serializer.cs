using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class Serializer
    {
        #region Serialize
        public static byte[] SerializeData(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] SerializeData(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] SerializeData(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] SerializeData(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] SerializeData(short value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] SerializeData(int value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] SerializeData(long value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] SerializeData(string value)
        {
            return Encoding.ASCII.GetBytes(value + '\0');
        }

        #endregion

        #region Deserialize
        public static void DeserializeData(byte[] buffer, ref int index, out bool value)
        {
            value = Convert.ToBoolean(buffer[index++]);
        }

        public static void DeserializeData(byte[] buffer, ref int index, out ushort value)
        {
            value = (ushort)((buffer[index++] << 8) | buffer[index++]);
        }

        public static void DeserializeData(byte[] buffer, ref int index, out uint value)
        {
            value = (uint)((buffer[index++] << 24) | (buffer[index++] << 16) | (buffer[index++] << 8) | buffer[index++]);
        }

        public static void DeserializeData(byte[] buffer, ref int index, out ulong value)
        {
            value = (ulong)((buffer[index++] << 56) | (buffer[index++] << 48) | (buffer[index++] << 40) | (buffer[index++] << 32)
                | (buffer[index++] << 24) | (buffer[index++] << 16) | (buffer[index++] << 8) | buffer[index++]);
        }
        public static void DeserializeData(byte[] buffer, ref int index, out short value)
        {
            value = (short)((buffer[index++] << 8) | buffer[index++]);
        }

        public static void DeserializeData(byte[] buffer, ref int index, out int value)
        {
            value = (int)((buffer[index++] << 24) | (buffer[index++] << 16) | (buffer[index++] << 8) | buffer[index++]);
        }

        public static void DeserializeData(byte[] buffer, ref int index, out long value)
        {
            value = (long)((buffer[index++] << 56) | (buffer[index++] << 48) | (buffer[index++] << 40) | (buffer[index++] << 32)
               | (buffer[index++] << 24) | (buffer[index++] << 16) | (buffer[index++] << 8) | buffer[index++]);
        }

        public static void DeserializeData(byte[] buffer, ref int index, out string value, int sizeStr)
        {
            value = Encoding.ASCII.GetString(buffer, index, sizeStr - 1);
            index += sizeStr;
        }

        #endregion
    }

}
