using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class GridSquare
    {
        public Vector2 GridPosition { get; set; }
        bool IsPath => TileKind != Grid.TileKinds.None;
        public Grid.TileKinds TileKind { get; internal set; }

        //put in sprite
        Vector2 onScreenPosition => TowerDefense.Grid[this];
        Vector2 origin;
        Texture2D texture; 
        //square/pic
        float scale;

        //Texture2D pixel;


        public GridSquare(Vector2 gridPosition, Grid.TileKinds tileKind = Grid.TileKinds.None)
        {
            GridPosition = gridPosition;

            TileKind = tileKind;

            texture = Grid.TileTextures[TileKind];
            scale = (float)TowerDefense.Grid.SquareSize / texture.Height;
            // scale /= 2;
            //onScreenPosition = new Vector2(GridPosition.X * squareSize, GridPosition.Y * squareSize);

            //pixel = new Texture2D(GraphicsDevice, 1, 1);
            //pixel.SetData(new[] { Color.White });


            origin = Vector2.Zero; // new Vector2(squareSize, squareSize) / 2f;  // Center origin, RELATIVE TO THE TEXTURE!
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //* (((int)TileKind & 4) == 4 ? 1.5 : 1))/*written by peter*/, (int)(squareSize * (((int)TileKind & 4) == 4 ? 1.5 : 1)));
            //Rectangle positionRectangle = new Rectangle();
            //Color color = Color.Black;
            //if(IsPath)
            //{
            //    color = Color.White;
            //}
            //spriteBatch.Draw(Grid.TileTextures[Grid.TileKinds.None], destinationRect, null, Color.White);

            spriteBatch.Draw(Grid.TileTextures[Grid.TileKinds.None], onScreenPosition, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
            if (TileKind != Grid.TileKinds.None)
            {
                spriteBatch.Draw(texture, onScreenPosition, sourceRectangle: null, Color.White, rotation: 0f, origin, scale, SpriteEffects.None, layerDepth: 0f);
            }
            spriteBatch.Draw(TowerDefense.DebugGridSquare, onScreenPosition, Color.White);
            //spriteBatch.Draw(TowerDefense.Pixel, new Rectangle((int)onScreenPosition.X, (int)onScreenPosition.Y, TowerDefense.Grid.SquareSize, TowerDefense.Grid.SquareSize), Color.Red * 0.25f);
        }
    }
}
