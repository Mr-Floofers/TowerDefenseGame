using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace TowerDefense
{
    struct Frame
    {
        internal Texture2D Texture { get; set; }
        public string AssetName { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Vector2 Origin { get; set; }
    }


    class AnimatedSprite<T> : Sprite
        where T : Enum
    {
        public AnimatedSprite(string file)
            : base(null)
        {

        }

        private Dictionary<T, List<Frame>> animationFrames { get; set; }

        public static void AnimationFileFormatTest(string filePath)
        {
            AnimatedSprite<PlayerAnimationStates> test = new AnimatedSprite<PlayerAnimationStates>(filePath)
            {
                animationFrames = new Dictionary<PlayerAnimationStates, List<Frame>>
                {
                    [PlayerAnimationStates.Running] = new List<Frame>()
                    {
                        new Frame() { AssetName = "test1", SourceRectangle = Rectangle.Empty, Origin = Vector2.Zero },
                        new Frame() { AssetName = "test2", SourceRectangle = Rectangle.Empty, Origin = Vector2.Zero }
                    },
                    [PlayerAnimationStates.Idle] = new List<Frame>()
                    {
                        new Frame() { AssetName = "test3", SourceRectangle = Rectangle.Empty, Origin = Vector2.Zero },
                        new Frame() { AssetName = "test4", SourceRectangle = Rectangle.Empty, Origin = Vector2.Zero }
                    }
                }
            };

            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();

            var options = new JsonSerializerOptions { IncludeFields = true, WriteIndented = true };
            var jsonText = JsonSerializer.Serialize(test.animationFrames, options); // Formatting.Indented;
            File.WriteAllText(filePath, jsonText);
            var fileContents = File.ReadAllText(filePath);
            var readingBackTest = JsonSerializer.Deserialize<Dictionary<PlayerAnimationStates, List<Frame>>>(fileContents, options);
        }

        public override void Draw(SpriteBatch sb)
        {
            //do the animation stuff
        }
    }
    enum PlayerAnimationStates
    {
        Idle,
        Running,
        Attacking,
        Dying
    };

    enum EnemyAnimationStates
    {
        Idle,
        Flying,
        Attacking,
        Dying
    };

}
