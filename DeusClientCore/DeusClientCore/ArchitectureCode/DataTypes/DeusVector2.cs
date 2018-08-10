using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class DeusVector2
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
        private float m_precision = 100000000.0f; // 8

        public DeusVector2()
        {

        }

        public DeusVector2(float x, float y, long precision = 8)
        {
            X = x;
            Y = y;
            m_precision = (long)Math.Pow(10, precision);
        }

        public static bool operator== (DeusVector2 a, DeusVector2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator!= (DeusVector2 a, DeusVector2 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public static DeusVector2 Zero { get { return new DeusVector2(); } }

        public override bool Equals(object obj)
        {
            var vector = obj as DeusVector2;
            return X == vector.X &&
                   Y == vector.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
