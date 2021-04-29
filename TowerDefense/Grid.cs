using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public partial class Grid
    {
        GridSquare[,] gridSquares;

        public Map Map { get; }

        Dictionary<Vector4, TileKinds> directions;

        public Vector2 GridSize
            => Map.MapSize;

        public GridSquare this[int x, int y]
            => gridSquares[x, y];


        public Vector2 this[GridSquare gridSquare]
        {
            get
            {
                return new Vector2(gridSquare.GridPosition.X * SquareSize, gridSquare.GridPosition.Y * SquareSize);
            }
        }

        public GridSquare this[Vector2 screenPosition]
        {
            get
            {
                //screenPosition -= new Vector2(TowerDefense.Grid.SquareSize / 2);

                // Return null if off-grid position
                if (screenPosition.X < 0 || (int)screenPosition.X >= (Map.MapSize.X) * SquareSize || screenPosition.Y < 0 || (int)screenPosition.Y >= (Map.MapSize.Y) * SquareSize)
                {
                    return null;
                }
                //if(screenPosition.X > 0)
                //{
                //    screenPosition.X--;
                //}
                //if(screenPosition.Y > 0)
                //{
                //    screenPosition.Y--;
                //}
                return this[(int)screenPosition.X / SquareSize, (int)screenPosition.Y / SquareSize];
            }
        }

        public Vector2 this[int pathIndex]
        {
            get
            {
                return new Vector2(Map.Path[pathIndex].X * SquareSize, Map.Path[pathIndex].Y * SquareSize);
            }
        }

        public Grid(Map map, Dictionary<TileKinds, Texture2D> tileTextures, int squareSize)
        {
            this.Map = map;
            gridSquares = new GridSquare[(int)GridSize.X, (int)GridSize.Y];
            TileTextures = tileTextures;
            directions = new Dictionary<Vector4, TileKinds>
            {
                [new Vector4(0, -1, 1, 0)] = TileKinds.TurnFromEntryAboveToLowerRight,
                [new Vector4(1, 0, 0, -1)] = TileKinds.TurnFromEntryAboveToLowerRight,
                [new Vector4(0, -1, -1, 0)] = TileKinds.TurnFromEntryLeftToUpperRight,
                [new Vector4(-1, 0, 0, -1)] = TileKinds.TurnFromEntryLeftToUpperRight,
                [new Vector4(0, 1, 1, 0)] = TileKinds.TurnFromEntryRightToLowerLeft,
                [new Vector4(1, 0, 0, 1)] = TileKinds.TurnFromEntryRightToLowerLeft,
                [new Vector4(0, 1, -1, 0)] = TileKinds.TurnFromEntryBelowToUpperLeft,
                [new Vector4(-1, 0, 0, 1)] = TileKinds.TurnFromEntryBelowToUpperLeft,
                [new Vector4(-1, 0, 1, 0)] = TileKinds.Horizontal,
                [new Vector4(1, 0, -1, 0)] = TileKinds.Horizontal,
                [new Vector4(0, 1, 0, -1)] = TileKinds.Vertical,
                [new Vector4(0, -1, 0, 1)] = TileKinds.Vertical
            };
            SquareSize = squareSize;

            var lerpFuncs = new Dictionary<TileKinds, Func<Vector2, Vector2, float, Vector2>>()
            {
                [TileKinds.Horizontal] = Vector2.Lerp,
                [TileKinds.Vertical] = Vector2.Lerp
            };
        }

        public int SquareSize { get; set; }

        public void SetGridSquares()
        {
            // Initialize grid
            for (int x = 0; x < GridSize.X; x++)
            {
                for (int y = 0; y < GridSize.Y; y++)
                {
                    gridSquares[x, y] = new GridSquare(new Vector2(x, y));
                }
            }

            // Set path into grid
            for (int i = 1; i < Map.Path.Count - 1; i++)
            {
                TileKinds tileKind = PathSquareDirection(i);
                gridSquares[(int)Map.Path[i].X, (int)Map.Path[i].Y] = new GridSquare(Map.Path[i], tileKind);
            }
        }

        TileKinds PathSquareDirection(int index)
        {
            Vector2 pathSquarePosition = Map.Path[index];
            Vector2 nextPathSquarePosition = Map.Path[index + 1];
            Vector2 prePathSquarePosition = Map.Path[index - 1];

            // Offset by current tile
            nextPathSquarePosition -= pathSquarePosition;
            prePathSquarePosition -= pathSquarePosition;


            Vector4 directionIndex = new Vector4(nextPathSquarePosition.X, nextPathSquarePosition.Y,
                                                 prePathSquarePosition.X, prePathSquarePosition.Y);
            var direction = directions[directionIndex];

            //if(direction == TileKinds.Vertical)
            //{
            //    if(gridSquares[(int)prePathSquarePosition.X, (int)prePathSquarePosition.Y].TileKind == TileKinds.TurnFromEntryRightToLowerLeft)
            //    {
            //        direction = TileKinds.VerticalDrawLeft;
            //    }
            //    else
            //    {
            //        direction = TileKinds.VerticalDrawRight;
            //    }

            //}
            //else if(direction == TileKinds.Horizontal)
            //{
            //    if (gridSquares[(int)prePathSquarePosition.X, (int)prePathSquarePosition.Y].TileKind == TileKinds.TurnFromEntryRightToLowerLeft)
            //    {
            //        direction = TileKinds.HorizontalDrawHigh;
            //    }
            //    else
            //    {
            //        direction = TileKinds.HorizontalDrawLow;
            //    }
            //}
            return direction;
        }

        //TileKinds PathSquareDirection(int index)
        //{
        //    if (index == 0 || index == map.path.Count - 1)
        //    {
        //        return StartAndEndPathSquareDirection(index);
        //    }

        //    Vector2 pathSquarePosition = map.path[index];
        //    Vector2 nextPathSquarePosition = map.path[index + 1];
        //    Vector2 prePathSquarePosition = map.path[index - 1];

        //    if (prePathSquarePosition.X == nextPathSquarePosition.X)
        //    {
        //        return TileKinds.Horizontal;
        //    }
        //    if (prePathSquarePosition.Y == nextPathSquarePosition.Y)
        //    {
        //        return TileKinds.Vertical;
        //    }

        //    if (nextPathSquarePosition.X == prePathSquarePosition.X + 1)
        //    {
        //        if (nextPathSquarePosition.Y == prePathSquarePosition.Y + 1)
        //        {
        //            return TileKinds.TurnFromEntryAboveToLowerRight;
        //        }
        //        if (nextPathSquarePosition.Y == prePathSquarePosition.Y - 1)
        //        {
        //            return TileKinds.TurnFromEntryRightToLowerLeft;
        //        }
        //    }
        //    if (nextPathSquarePosition.X == prePathSquarePosition.X - 1)
        //    {
        //        if (nextPathSquarePosition.Y == prePathSquarePosition.Y + 1)
        //        {
        //            return TileKinds.TurnFromEntryLeftToUpperRight;
        //        }
        //        if (nextPathSquarePosition.Y == prePathSquarePosition.Y - 1)
        //        {
        //            return TileKinds.TurnFromEntryBellowToUpperLeft;
        //        }
        //    }
        //    return TileKinds.None;
        //}

        //TileKinds StartAndEndPathSquareDirection(int index)
        //{
        //    Vector2 pathSquarePosition = map.path[index];
        //    Vector2 otherPathSquarePosition;
        //    if (index == 0)
        //    {
        //        otherPathSquarePosition = map.path[index + 1];
        //    }
        //    else
        //    {
        //        otherPathSquarePosition = map.path[index - 1];
        //    }

        //    if (pathSquarePosition.X == 0)
        //    {
        //        if (otherPathSquarePosition.Y == pathSquarePosition.Y)
        //        {
        //            return TileKinds.Horizontal;
        //        }
        //        if (otherPathSquarePosition.Y == pathSquarePosition.Y + 1)
        //        {
        //            return TileKinds.TurnFromEntryBellowToUpperLeft;
        //        }
        //        if (otherPathSquarePosition.Y == pathSquarePosition.Y - 1)
        //        {
        //            return TileKinds.TurnFromEntryLeftToUpperRight;
        //        }
        //    }
        //    if (pathSquarePosition.X == GridSize.X - 1)
        //    {
        //        if (otherPathSquarePosition.Y == pathSquarePosition.Y)
        //        {
        //            return TileKinds.Horizontal;
        //        }
        //        if (otherPathSquarePosition.Y == pathSquarePosition.Y + 1)
        //        {
        //            return TileKinds.TurnFromEntryRightToLowerLeft;
        //        }
        //        if (otherPathSquarePosition.Y == pathSquarePosition.Y - 1)
        //        {
        //            return TileKinds.TurnFromEntryAboveToLowerRight;
        //        }
        //    }

        //    if (pathSquarePosition.Y == 0)
        //    {
        //        if (otherPathSquarePosition.X == pathSquarePosition.X)
        //        {
        //            return TileKinds.Vertical;
        //        }
        //        if (otherPathSquarePosition.X == pathSquarePosition.X + 1)
        //        {
        //            return TileKinds.TurnFromEntryAboveToLowerRight;
        //        }
        //        if (otherPathSquarePosition.X == pathSquarePosition.X - 1)
        //        {
        //            return TileKinds.TurnFromEntryLeftToUpperRight;
        //        }
        //    }
        //    if (pathSquarePosition.Y == GridSize.Y - 1)
        //    {
        //        if (otherPathSquarePosition.X == pathSquarePosition.X)
        //        {
        //            return TileKinds.Vertical;
        //        }
        //        if (otherPathSquarePosition.X == pathSquarePosition.X + 1)
        //        {
        //            return TileKinds.TurnFromEntryRightToLowerLeft;
        //        }
        //        if (otherPathSquarePosition.X == pathSquarePosition.X - 1)
        //        {
        //            return TileKinds.TurnFromEntryBellowToUpperLeft;
        //        }
        //    }
        //    return TileKinds.None;
        //}

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < GridSize.X; x++)
            {
                for (int y = 0; y < GridSize.Y; y++)
                {
                    gridSquares[x, y].Draw(spriteBatch);
                }
            }
        }

    }
}
