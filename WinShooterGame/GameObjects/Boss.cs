using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WinShooterGame.GameObjects
{
    internal class Boss
    {
        public Animation BossAnimation;

        public Vector2 Position;

        public bool Active;

        public int Health;

        public int Damage;

        // the amount of the score enemy is worth.
        public int Value;

        public TimeSpan bossMovementTime;
        private TimeSpan previousTime;

        public int Width
        {
            get { return BossAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return BossAnimation.FrameHeight; }
        }

        public float bossMoveSpeed;

        public void Initialize(Animation animation, Vector2 position)
        {
            BossAnimation = animation;
            Position = position;
            Active = true;
            Health = 200;
            Damage = 20;
            bossMoveSpeed = 0;
            Value = 500;
            bossMovementTime = TimeSpan.FromSeconds(1.5f);
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousTime > bossMovementTime)
            {
                previousTime = gameTime.TotalGameTime;
                Position.Y += bossMoveSpeed;
            }
            else
            {
                Position.Y -= bossMoveSpeed;
            }

            BossAnimation.Position = Position;
            BossAnimation.Update(gameTime);
            if (Health <= 0)
            {
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            BossAnimation.Draw(spriteBatch);
        }
    }
}