using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WinShooterGame
{
    internal class GameMenu : Screen
    {
        protected Texture2D texture;
        protected Rectangle destinationRectagle;
        protected Song menuMusic;
        private SpriteFont font;

        public GameMenu(GraphicsDevice device,
            ContentManager content)
            : base(device, content, "gameMenu")
        {
        }

        public override bool Init()
        {
            texture = _content.Load<Texture2D>("Graphics\\mainMenu");
            menuMusic = _content.Load<Song>("Sounds\\menuMusic");
            font = _content.Load<SpriteFont>("Graphics\\Score");
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
            _spriteBatch.DrawString(font, "<Enter>Start | <ESC>End", new Vector2(200, 280), Color.Black);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // Check if Enter is pressed to start game
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                SCREEN_MANAGER.goto_screen("gameScreen");
            }
            base.Update(gameTime);
        }
    }
}