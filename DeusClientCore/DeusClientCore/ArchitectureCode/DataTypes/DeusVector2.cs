using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeusClientCore.Packets;

namespace DeusClientCore
{
    public class DeusVector2 : ISerializable
    {
        public float X
        {
            get
            {
                return m_x / m_precision;
            }

            set
            {
                m_x = (int)(value * m_precision);
            }
        }

        public float Y
        {
            get
            {
                return m_y / m_precision;
            }

            set
            {
                m_y = (int)(value * m_precision);
            }
        }

        private int m_x = 0;
        private int m_y = 0;
        private float m_precision = 100000.0f; // 5

        public DeusVector2()
        {

        }

        public DeusVector2(float x, float y, long precision = 5)
        {
            m_precision = (long)Math.Pow(10, precision);
            X = x;
            Y = y;
        }

        public static bool operator ==(DeusVector2 a, DeusVector2 b)
        {
            return a.m_x == b.m_x && a.m_y == b.m_y;
        }

        public static bool operator !=(DeusVector2 a, DeusVector2 b)
        {
            return a.m_x != b.m_x || a.m_y != b.m_y;
        }

        public static DeusVector2 operator +(DeusVector2 a, DeusVector2 b)
        {
            DeusVector2 result = new DeusVector2();
            result.m_x = a.m_x + b.m_x;
            result.m_y = a.m_y + b.m_y;
            return result;
        }

        public static DeusVector2 operator -(DeusVector2 a, DeusVector2 b)
        {
            DeusVector2 result = new DeusVector2();
            result.m_x = a.m_x - b.m_x;
            result.m_y = a.m_y - b.m_y;
            return result;
        }

        public static DeusVector2 operator *(DeusVector2 a, float scalar)
        {
            DeusVector2 result = new DeusVector2();
            result.m_x = (int)(a.m_x * scalar);
            result.m_y = (int)(a.m_y * scalar);
            return result;
        }

        public static DeusVector2 Zero { get { return new DeusVector2(); } }

        public static double SqrtMagnitude(DeusVector2 lhs, DeusVector2 rhs)
        {
            return Math.Sqrt(Magnitude(lhs, rhs));
        }

        public static double Magnitude(DeusVector2 lhs, DeusVector2 rhs)
        {
            float precision = Math.Min(lhs.m_precision, rhs.m_precision);
            return Math.Pow((rhs.m_x - lhs.m_x) / precision, 2) +
                Math.Pow((rhs.m_y - lhs.m_y) / precision, 2);
        }

        public override bool Equals(object obj)
        {
            var vector = obj as DeusVector2;
            return m_x == vector.m_x &&
                   m_y == vector.m_y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode += hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public byte[] Serialize()
        {
            List<byte> results = new List<byte>();

            // 1 - X
            results.AddRange(Serializer.SerializeData(m_x));

            // 2 - Y
            results.AddRange(Serializer.SerializeData(m_y));

            // 3 - Precision -> cast in long
            results.AddRange(Serializer.SerializeData((long)m_precision));

            return results.ToArray();
        }

        public void Deserialize(byte[] packetsBuffer, ref int index)
        {
            // 1 - X
            int tmpX = 0;
            Serializer.DeserializeData(packetsBuffer, ref index, out tmpX);
            m_x = tmpX;

            // 2 - Y
            int tmpY = 0;
            Serializer.DeserializeData(packetsBuffer, ref index, out tmpY);
            m_y = tmpY;

            // 3 - Precision -> cast in long
            long tmpPrecision = 0;
            Serializer.DeserializeData(packetsBuffer, ref index, out tmpPrecision);
            m_precision = tmpPrecision;
        }

        public ushort EstimateCurrentSerializedSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(long);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
