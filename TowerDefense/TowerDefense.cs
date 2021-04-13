using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TowerDefense
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TowerDefense : GridBasedGame
    {
        Texture2D pixel;

        //TimeSpan timeToKillSomeRAM = TimeSpan.Zero;
        //TimeSpan whenToKillSomeRam = TimeSpan.FromMilliseconds(100);
        //List<Random> theseAreUseless = new List<Random>();


        // Debug
        private TimeSpan moveTimer = TimeSpan.Zero;
        private TimeSpan moveTimerTarget = TimeSpan.FromMilliseconds(500);
        private int debugMapTraverserPositionIndex = 0;
        int mouseX;
        int mouseY;
        Vector2 testGridPosition;

        public TowerDefense()
            : base(@"MapFiles\mapFileFormat.json", squareSize: 140) { }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
           
            //tileTextures = new Dictionary<Grid.TileKinds, Texture2D>
            //{

            //    [Grid.TileKinds.None] = Content.Load<Texture2D>("Tiles\\land"),
            //    [Grid.TileKinds.Vertical] = Content.Load<Texture2D>(@"Tiles\vertical340"),//(@"Tiles\road_6_centered"),
            //    [Grid.TileKinds.Horizontal] = Content.Load<Texture2D>(@"Tiles\horizontalTest340"),//(@"Tiles\road_5_centered"),
            //    [Grid.TileKinds.TurnFromEntryAboveToLowerRight] = Content.Load<Texture2D>(@"Tiles\road_3_centered"),
            //    [Grid.TileKinds.TurnFromEntryBellowToUpperLeft] = Content.Load<Texture2D>(@"Tiles\road_2_centered"),
            //    [Grid.TileKinds.TurnFromEntryLeftToUpperRight] = Content.Load<Texture2D>(@"Tiles\road_4_centered"),
            //    [Grid.TileKinds.TurnFromEntryRightToLowerLeft] = Content.Load<Texture2D>(@"Tiles\road_1_centered")
            //};

           
            //Grid.Map.MapFileFormatTest(@"C:\Users\denni\source\repos\TowerDefense\TowerDefense\MapFiles\vector2JsonFormat.json");

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
            //timeToKillSomeRAM += gameTime.ElapsedGameTime;
            //for (int i = 0; i < 1000; i++)
            //{
            //    theseAreUseless.Add(new Random());

            //}
            //if(timeToKillSomeRAM > whenToKillSomeRam)
            //{
            //    theseAreUseless.Clear();
            //    timeToKillSomeRAM = TimeSpan.Zero;
            //}

            if (mouseX != Mouse.GetState().X || mouseY != Mouse.GetState().Y)
            {
                Debug.WriteLine($"{mouseX}, {mouseY}");
                Debug.WriteLine($"{mouseX/Grid.SquareSize}, {mouseY/Grid.SquareSize}");
                testGridPosition = new Vector2(mouseX / Grid.SquareSize, mouseY / Grid.SquareSize);
            }
            mouseX = Mouse.GetState().X;
            mouseY = Mouse.GetState().Y;

            moveTimer += gameTime.ElapsedGameTime;
            if (moveTimer < moveTimerTarget) return;

            if (debugMapTraverserPositionIndex == Grid.Map.Path.Count - 1) return;

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
            GraphicsDevice.Clear(Color.Green);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            Grid.Draw(spriteBatch, pixel);

            spriteBatch.Draw(pixel, new Rectangle((testGridPosition*Grid.SquareSize).ToPoint()/*(Grid.Map.Path[debugMapTraverserPositionIndex] * Grid.SquareSize).ToPoint()*/, new Point(Grid.SquareSize, Grid.SquareSize)), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
