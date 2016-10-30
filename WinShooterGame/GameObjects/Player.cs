using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WinShooterGame.GameObjects
{
    internal class Player
    {
        public Animation PlayerAnimation;

        public Vector2 Position;

        // State of the player
        public bool Active;

        // Amount of hit points the player has
        public int Health;

        public int Width
        { get { return PlayerAnimation.FrameWidth; } }

        public int Height
        { get { return PlayerAnimation.FrameHeight; } }

        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;

            Position = position;
            Active = true;
            Health = 100;
        }

        public void Update(GameTime gameTime)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
            /* Players health reaches 0 then deactivate it. */
            if (Health <= 0)
            {
                //deactivate the player
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}