using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TowerDefense
{
    public class GridBasedGame : Game 
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;

        private readonly string mapFilePath;
        private readonly int squareSize;

        public static Grid Grid;

        public GridBasedGame(string mapFilePath, int squareSize)
        {
            this.mapFilePath = mapFilePath;
            this.squareSize = squareSize;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            var map = Map.ImportMap(mapFilePath);

            var tileTextures = new Dictionary<Grid.TileKinds, Texture2D>();
            foreach (var pair in map.TileTextures)
            {
                tileTextures.Add(pair.Key, Content.Load<Texture2D>(pair.Value));
            }

            int screenSize = (int)map.MapSize.X * squareSize;

            graphics.PreferredBackBufferHeight = screenSize;
            graphics.PreferredBackBufferWidth = screenSize;
            graphics.ApplyChanges();

            Grid = new Grid(map, tileTextures, squareSize);
            Grid.SetGridSquares();
        }
    }
}
