using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WinShooterGame
{
    internal class GameOver : Screen
    {
        protected Texture2D texture;
        protected Rectangle destinationRectagle;
        protected Song menuMusic;

        public GameOver(GraphicsDevice device,
            ContentManager content)
            : base(device, content, "gameOver")
        {
        }

        public override bool Init()
        {
            texture = _content.Load<Texture2D>("graphics\\endMenu");
            menuMusic = _content.Load<Song>("sounds\\menuMusic");

            destinationRectagle =
              new Rectangle(0, 0,
             texture.Width,
              _device.Viewport.Height);

            MediaPlayer.Play(menuMusic);
            return base.Init();
        }

        public override void Shutdown()
        {
            MediaPlayer.Stop();
            base.Shutdown();
        }

        public override void Draw(GameTime gameTime)
        {
            _device.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, destinationRectagle, Color.White);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // Check if m is pressed and go to screen2
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                SCREEN_MANAGER.goto_screen("gameMenu");
            }
            base.Update(gameTime);
        }
    }
}