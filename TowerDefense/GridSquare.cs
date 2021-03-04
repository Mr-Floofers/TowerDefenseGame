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
        bool IsPath { get; set; }

        public GridSquare(Vector2 gridPosition, bool isPath = false)
        {
            GridPosition = gridPosition;
            IsPath = isPath;
        }

        public void Draw(SpriteBatch spriteBatch, int squareSize, Texture2D pixel)
        {
            Vector2 position = new Vector2(GridPosition.X * squareSize, GridPosition.Y * squareSize);
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, squareSize, squareSize);
            Color color = Color.Black;
            if(IsPath)
            {
                color = Color.White;
            }
            spriteBatch.Draw(pixel, position, rectangle, color);
        }
    }
}
