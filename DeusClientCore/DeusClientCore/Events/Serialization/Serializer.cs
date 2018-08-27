using DeusClientCore.Exceptions;
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
            byte[] result = BitConverter.GetBytes(value);
            Array.Reverse(result);
            return result;
        }

        public static byte[] SerializeData(ushort value)
        {
            byte[] result = BitConverter.GetBytes(value);
            Array.Reverse(result);
            return result;
        }

        public static byte[] SerializeData(uint value)
        {
            byte[] result = BitConverter.GetBytes(value);
            Array.Reverse(result);
            return result;
        }

        public static byte[] SerializeData(ulong value)
        {
            byte[] result = BitConverter.GetBytes(value);
            Array.Reverse(result);
            return result;
        }

        public static byte[] SerializeData(short value)
        {
            byte[] result = BitConverter.GetBytes(value);
            Array.Reverse(result);
            return result;
        }

        public static byte[] SerializeData(int value)
        {
            byte[] result = BitConverter.GetBytes(value);
            Array.Reverse(result);
            return result;
        }

        public static byte[] SerializeData(long value)
        {
            byte[] result = BitConverter.GetBytes(value);
            Array.Reverse(result);
            return result;
        }

        public static byte[] SerializeData(string value)
        {
            return Encoding.ASCII.GetBytes(value + '\0');
        }

        public static byte[] SerializeData(ISerializable value)
        {
            return value.Serialize();
        }

        public static byte[] SerializeData<T>(T value)
        {
            if (value is uint)
                return SerializeData(Convert.ToUInt32(value));
            else if (value is int)
                return SerializeData(Convert.ToInt32(value));
            else if (value is ushort)
                return SerializeData(Convert.ToUInt16(value));
            else if (value is short)
                return SerializeData(Convert.ToInt16(value));
            else if (value is ulong)
                return SerializeData(Convert.ToUInt64(value));
            else if (value is long)
                return SerializeData(Convert.ToInt64(value));
            else if (value is string)
                return SerializeData(Convert.ToString(value));
            else if (value is bool)
                return SerializeData(Convert.ToBoolean(value));
            else if (value is ISerializable)
                return SerializeData(value as ISerializable);

            throw new DeusException("Cannot serialize this type");
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
            value = "";
            value = Encoding.ASCII.GetString(buffer, index, Math.Max(0, sizeStr - 1));
            index += sizeStr;
        }

        public static void DeserializeData(byte[] buffer, ref int index, ISerializable value)
        {
            value.Deserialize(buffer, ref index);
        }

        public static void DeserializeData<T>(byte[] buffer, ref int index, out T value)
        {
            value = Activator.CreateInstance<T>();

            if (value is uint)
            {
                uint tmpData = 0;
                DeserializeData(buffer, ref index, out tmpData);
                value = (T)Convert.ChangeType(tmpData, typeof(T));
            }
            else if (value is int)
            {
                int tmpData = 0;
                DeserializeData(buffer, ref index, out tmpData);
                value = (T)Convert.ChangeType(tmpData, typeof(T));
            }
            else if (value is ushort)
            {
                ushort tmpData = 0;
                DeserializeData(buffer, ref index, out tmpData);
                value = (T)Convert.ChangeType(tmpData, typeof(T));
            }
            else if (value is short)
            {
                short tmpData = 0;
                DeserializeData(buffer, ref index, out tmpData);
                value = (T)Convert.ChangeType(tmpData, typeof(T));
            }
            else if (value is ulong)
            {
                ulong tmpData = 0;
                DeserializeData(buffer, ref index, out tmpData);
                value = (T)Convert.ChangeType(tmpData, typeof(T));
            }
            else if (value is long)
            {
                long tmpData = 0;
                DeserializeData(buffer, ref index, out tmpData);
                value = (T)Convert.ChangeType(tmpData, typeof(T));
            }
            else if (value is string)
            {
                string tmpData = "";
                DeserializeData(buffer, ref index, out tmpData);
                value = (T)Convert.ChangeType(tmpData, typeof(T));
            }
            else if (value is bool)
            {
                bool tmpData = false;
                DeserializeData(buffer, ref index, out tmpData);
                value = (T)Convert.ChangeType(tmpData, typeof(T));
            }
            else if (typeof(ISerializable).IsAssignableFrom(typeof(T)))
            {
                DeserializeData(buffer, ref index, (value as ISerializable));
            }
            else
                throw new DeusException("Cannot serialize this type");
        }

        #endregion
    }

}
