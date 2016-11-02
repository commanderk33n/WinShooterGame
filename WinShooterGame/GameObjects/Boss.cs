using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public bool turnAround;

        public int distance;
        public float bossMoveSpeed;

        public int Width
        {
            get { return BossAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return BossAnimation.FrameHeight; }
        }

        public void Initialize(Animation animation, Vector2 position)
        {
            BossAnimation = animation;
            Position = position;
            Active = true;
            Health = 350;
            Damage = 20;
            bossMoveSpeed = 2;
            Value = 500;
            turnAround = true;
            distance = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (turnAround)
            {
                Position.Y -= bossMoveSpeed;
                distance++;
                if (distance > 120)
                {
                    turnAround = false;
                    distance = 0;
                }
            }
            else
            {
                Position.Y += bossMoveSpeed;
                distance++;
                if (distance > 120)
                {
                    turnAround = true;
                    distance = 0;
                }
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