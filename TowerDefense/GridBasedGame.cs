using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TowerDefense
{
    public class GridBasedGame : Game 
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;

        private readonly string mapFilePath;
        private readonly int squareSize;

        public static Grid Grid { get; private set; }
        public static Texture2D DebugGridSquare { get; private set; }
        public static Texture2D Pixel { get; protected set; }

        public GridBasedGame(string mapFilePath, int squareSize)
        {
            this.mapFilePath = mapFilePath;
            this.squareSize = squareSize;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });

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

            CreateDebugGridSquare(squareSize);

            base.Initialize();
        }

        private void CreateDebugGridSquare(int squareSize)
        {
            RenderTarget2D debugGridSquare = new RenderTarget2D(GraphicsDevice, squareSize, squareSize);
            GraphicsDevice.SetRenderTarget(debugGridSquare);
            GraphicsDevice.Clear(Color.Transparent);
            int width = 5;
            spriteBatch.Begin();

            spriteBatch.Draw(TowerDefense.Pixel, new Rectangle(0, 0, squareSize, width), Color.Black);
            spriteBatch.Draw(TowerDefense.Pixel, new Rectangle(0, 0, width, squareSize), Color.Black);
            spriteBatch.Draw(TowerDefense.Pixel, new Rectangle(squareSize-width, 0, width, squareSize), Color.Black);
            spriteBatch.Draw(TowerDefense.Pixel, new Rectangle(0, squareSize-width, squareSize, width), Color.Black);

            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            DebugGridSquare = debugGridSquare;
        }
    }
}
