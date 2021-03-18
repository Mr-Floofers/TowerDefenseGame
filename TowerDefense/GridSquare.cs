using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    class GridSquare
    {
        Vector2 GridPosition { get; set; }
        bool IsPath => TileKind != Grid.TileKinds.None;
        public Grid.TileKinds TileKind { get; internal set; }

        public GridSquare(Vector2 gridPosition, Grid.TileKinds tileKind)
        {
            GridPosition = gridPosition;
            TileKind = tileKind;
        }

        public void Draw(SpriteBatch spriteBatch, int squareSize, Texture2D pixel)
        {
            Vector2 position = new Vector2(GridPosition.X * squareSize, GridPosition.Y * squareSize);
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, (int)(squareSize * (((int)TileKind & 4) == 4 ? 1.5 : 1))/*written by peter*/, (int)(squareSize * (((int)TileKind & 4) == 4 ? 1.5 : 1)));
            //Rectangle positionRectangle = new Rectangle();
            //Color color = Color.Black;
            //if(IsPath)
            //{
            //    color = Color.White;
            //}
            spriteBatch.Draw(Grid.TileTextures[TileKind], rectangle, null, Color.White);
        }
    }
}
