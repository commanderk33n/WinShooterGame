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

        public int Width
        {
            get { return BossAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return BossAnimation.FrameHeight; }
        }

        public float bossMoveSpeed;
        private bool tem;

        public void Initialize(Animation animation, Vector2 position)
        {
            BossAnimation = animation;
            Position = position;
            Active = true;
            Health = 200;
            Damage = 20;
            bossMoveSpeed = 0;
            Value = 500;
            tem = false;
        }

        public void Update(GameTime gameTime)
        {
            var i = 0;
            if (!tem)
            {
                Position.Y += bossMoveSpeed;
                i++;
                if (i >= 5)
                {
                    tem = true;
                }
            }
            else
            {
                Position.Y -= bossMoveSpeed;
                i--;
                if (i <= 5)
                {
                    tem = false;
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