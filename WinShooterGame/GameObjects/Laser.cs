using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WinShooterGame.GameObjects
{
    public class Laser
    {
        // animation the represents the laser animation.
        public Animation LaserAnimation;

        // the speed the laser traves
        private float laserMoveSpeed = 30f;

        // postion of the laser
        public Vector2 Position;

        // The damage the laser deals.
        private int Damage = 10;

        // set the laser to active
        public bool Active;

        // Range of the laser.
        private int Range;

        // the width of the player image.
        public int Width
        {
            get { return LaserAnimation.FrameWidth; }
        }

        // the height of the player image.
        public int Height
        {
            get { return LaserAnimation.FrameHeight; }
        }

        public void Initialize(Animation animation, Vector2 position)
        {
            LaserAnimation = animation;
            Position = position;
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            Position.X += laserMoveSpeed;

            LaserAnimation.Position = Position;
            LaserAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            LaserAnimation.Draw(spriteBatch);
        }
    }
}