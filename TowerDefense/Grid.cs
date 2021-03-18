using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    partial class Grid
    {
        GridSquare[,] gridSquares;
        public Vector2 GridSize { get; set; }
        Map map;
        Dictionary<Vector4, TileKinds> directions;

        public Grid(Map map, Dictionary<TileKinds, Texture2D> tileTextures)
        {
            this.map = map;
            GridSize = map.mapSize;
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
                [new Vector4(0, 1, -1, 0)] = TileKinds.TurnFromEntryBellowToUpperLeft,
                [new Vector4(-1, 0, 0, 1)] = TileKinds.TurnFromEntryBellowToUpperLeft,
                [new Vector4(-1, 0, 1, 0)] = TileKinds.Horizontal,
                [new Vector4(1, 0, -1, 0)] = TileKinds.Horizontal,
                [new Vector4(0, 1, 0, -1)] = TileKinds.Vertical,
                [new Vector4(0, -1, 0, 1)] = TileKinds.Vertical
            };
        }

        public void SetGridSquares()
        {
            for (int x = 0; x < GridSize.X; x++)
            {
                for (int y = 0; y < GridSize.Y; y++)
                {
                    TileKinds tilekind = TileKinds.None;
                    if (map.path.Contains(new Vector2(x, y)))
                    {
                        tilekind = PathSquareDirection(map.path.IndexOf(new Vector2(x, y)));
                    }
                    gridSquares[x, y] = new GridSquare(new Vector2(x, y), tilekind);
                }
            }
        }

        TileKinds PathSquareDirection(int index)
        {
            Vector2 pathSquarePosition = map.path[index];
            Vector2 nextPathSquarePosition;
            Vector2 prePathSquarePosition;
            if (index == 0)
            {
                return TileKinds.None;
                //prePathSquarePosition = map.WalkInFrom;
            }
            else
            {
                //return TileKinds.None;
                prePathSquarePosition = map.path[index - 1];
            }
            if (index == map.path.Count - 1)
            {
                return TileKinds.None;
                //nextPathSquarePosition = map.WalkOutFrom;
            }
            else
            {
                nextPathSquarePosition = map.path[index + 1];
            }
            nextPathSquarePosition -= pathSquarePosition;
            prePathSquarePosition -= pathSquarePosition;

            Vector4 final = new Vector4(nextPathSquarePosition.X, nextPathSquarePosition.Y, prePathSquarePosition.X, prePathSquarePosition.Y);
            return directions[final];
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
