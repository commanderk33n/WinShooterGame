using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WinShooterGame.GameObjects;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace WinShooterGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private const int DEFAULT_SCREEN_WIDTH = 800;
        private const int DEFAULT_SCREEN_HEIGTH = 480;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = DEFAULT_SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = DEFAULT_SCREEN_HEIGTH;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            /* Init our screen manager and add a screens to it. */
            SCREEN_MANAGER.add_screen(new GameMenu(GraphicsDevice, Content));
            SCREEN_MANAGER.add_screen(new GameScreen(GraphicsDevice, Content));
            SCREEN_MANAGER.add_screen(new GameOver(GraphicsDevice, Content));

            /* Set the active screen to the game menu */
            SCREEN_MANAGER.goto_screen("gameMenu");

            /* Init the current screen */
            SCREEN_MANAGER.Init();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            /* Have the active screen initilize itself. */
            SCREEN_MANAGER.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            //laserSoundInstance.Dispose();
            //explosionSoundInstance.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            /* have the actrive screen update */
            SCREEN_MANAGER.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            /* Draw the active screen */
            SCREEN_MANAGER.Draw(gameTime);

            base.Draw(gameTime);
        }


    }
}