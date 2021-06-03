using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public class GameObject 
    {
        public Vector2 onScreenPosition;
        protected Vector2 origin;
        protected Vector2 scale;
        protected Color color;
        protected float rotation;
        protected SpriteEffects effects;
        protected float layerDepth;

        public GameObject()
        {

        }

        public virtual void Draw(SpriteBatch sb) { }
    }
}
