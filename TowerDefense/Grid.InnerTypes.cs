using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense
{
    partial class Grid
    {
        public static Dictionary<TileKinds, Texture2D> TileTextures;

        public enum TileKinds
        {
            None,
            Horizontal,
            HorizontalDrawLow,
            HorizontalDrawHigh,
            Vertical,
            VerticalDrawLeft,
            VerticalDrawRight,
            TurnFromEntryLeftToUpperRight,
            TurnFromEntryBellowToUpperLeft,
            TurnFromEntryRightToLowerLeft,
            TurnFromEntryAboveToLowerRight
        }
    }
}
