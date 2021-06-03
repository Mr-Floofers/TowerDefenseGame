using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense
{
    class Sprite : GameObject
    {
        Texture2D Texture;
        Rectangle? sourceRectangle;
        public Sprite(Texture2D texture)
            :base()
        {
            Texture = texture;
            sourceRectangle = null;
        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, onScreenPosition, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }
    }
}
