using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense
{
    class AnimatedSprite<T> : Sprite
        where T: Enum
    {
        struct Frame
        {
            Texture2D frame;
            int frameNum;
        }

        public AnimatedSprite(string file)
            : base(GridBasedGame.Pixel)
        {

        }

        private Dictionary<T, List<Frame>> animationFrames { get; set; }
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
