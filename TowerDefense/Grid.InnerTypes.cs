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
            None = 0,                           //0b000
            Horizontal,                     //0b001
            Vertical,                       //0b010
            TurnFromEntryLeftToUpperRight = 4,  //0b100   
            TurnFromEntryBellowToUpperLeft, //0b101
            TurnFromEntryRightToLowerLeft,  //0b110
            TurnFromEntryAboveToLowerRight  //0b111
        }
    }
}
