using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    class Grid
    {
        GridSquare[,] gridSquares;
        public Vector2 GridSize { get; set; }
        Map map;

        public Grid(Map map)
        {
            this.map = map;
            GridSize = map.mapSize;
            gridSquares = new GridSquare[(int)GridSize.X, (int)GridSize.Y];
        }

        public void SetGridSquares()
        {
            for (int x = 0; x < GridSize.X; x++)
            {
                for (int y = 0; y < GridSize.Y; y++)
                {
                    gridSquares[x, y] = new GridSquare(new Vector2(x, y), map.path.Contains(new Vector2(x, y)));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel, int squareSize)
        {
            for (int x = 0; x < GridSize.X; x++)
            {
                for (int y = 0; y < GridSize.Y; y++)
                {
                    gridSquares[x, y].Draw(spriteBatch, squareSize, pixel);
                }
            }
        }


    }
}
