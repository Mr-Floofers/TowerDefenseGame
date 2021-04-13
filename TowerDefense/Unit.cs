using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    class Unit
    {
        Grid grid;
        Vector2 onScreenPosition;
        Vector2 gridPosition => grid[onScreenPosition].GridPosition; //make it so that this is calculated from onScreenPosition
        float movementSpeed; //gridSquares per second?
        Texture2D texture; //?

        public Unit(Grid grid, float speed, Texture2D texture)
        {
            this.grid = grid;
            movementSpeed = speed;
            this.texture = texture;
        }

        public void Move()
        {

        }

        
    }
}
