using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    [DebuggerDisplay("X:{X}, Y:{Y}")]
    struct BasicVector2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public BasicVector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector2(BasicVector2 value)
            => value.ToVector2();

        public static implicit operator BasicVector2(Vector2 value)
            => new BasicVector2((int)value.X, (int)value.Y);

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
    }
}
