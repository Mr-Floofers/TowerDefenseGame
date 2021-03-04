using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TowerDefense
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map testMap;
        Texture2D pixel;
        int squareSize;
        Grid grid;

        // Debug
        private TimeSpan moveTimer = TimeSpan.Zero;
        private TimeSpan moveTimerTarget = TimeSpan.FromMilliseconds(500);
        private int debugMapTraverserPositionIndex = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            testMap = new Map();
            testMap.ImportMap(@"C:\Users\denni\source\repos\TowerDefense\TowerDefense\MapFiles\mapFileFormat.json");
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            
            grid = new Grid(testMap);
            grid.SetGridSquares();
            squareSize = 700 /(int)grid.GridSize.X;
            graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 700;
            graphics.ApplyChanges();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            moveTimer += gameTime.ElapsedGameTime;
            if (moveTimer < moveTimerTarget) return;

            if (debugMapTraverserPositionIndex == testMap.path.Count - 1) return;

            moveTimer = TimeSpan.Zero;
            debugMapTraverserPositionIndex++;

            //testMap.ImportMap(@"C:\Users\denni\source\repos\TowerDefense\TowerDefense\MapFiles\map1.txt");
            //testMap.MapFileFormatTest(@"C:\Users\denni\source\repos\TowerDefense\TowerDefense\MapFiles\mapFileFormat.txt");
            //testMap.ImportMap(@"C:\Users\denni\source\repos\TowerDefense\TowerDefense\MapFiles\mapFileFormat.txt");
            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            grid.Draw(spriteBatch, pixel, squareSize);

            spriteBatch.Draw(pixel, new Rectangle((testMap.path[debugMapTraverserPositionIndex] * squareSize).ToPoint(), new Point(squareSize, squareSize)), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
