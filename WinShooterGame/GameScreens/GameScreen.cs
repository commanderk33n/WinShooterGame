using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using WinShooterGame.GameObjects;

namespace WinShooterGame
{
    internal class GameScreen : Screen
    {
        // A movement speed for the player.
        private const float PlayerMoveSpeed = 8;

        private Player _player;

        private SpriteFont font;
        private int score = 0;

        private Texture2D _mainBackground;
        private ParallaxingBackground _bgLayer1;
        private ParallaxingBackground _bgLayer2;
        private Rectangle _rectBackground;
        private const float Scale = 1f;

        // Keyboard states
        private KeyboardState _currentKeyboardState;

        private KeyboardState _prevKeyboardState;

        // Gamepad states
        private GamePadState _currentGamePadState;

        private GamePadState _prevGamePadState;

        // Mouse states
        private MouseState _currentMouseState;

        private MouseState _prevMouseState;

        // texture to hold the laser.
        private Texture2D laserTexture;

        private List<Laser> laserBeams;

        // govern how fast our laser can fire.
        private TimeSpan laserSpawnTime;

        private TimeSpan previousLaserSpawnTime;

        // The rate at which enemies appear.
        private TimeSpan enemySpawnTime;

        private TimeSpan previousSpawnTime;

        //Enemies
        private Texture2D enemyTexture;

        private List<Enemy> enemies;

        private Texture2D bossTexture;
        private Boss _boss;

        // a random number gen
        private Random random;

        // Collections of explosions
        private List<Explosion> explosions;

        //Texture to hold explosion animation.
        private Texture2D explosionTexture;

        //Our Laser Sound and Instance
        private SoundEffect laserSound;

        private SoundEffectInstance laserSoundInstance;

        //Our Explosion Sound.
        private SoundEffect explosionSound;

        private SoundEffectInstance explosionSoundInstance;

        /* Game Music */
        private Song gameMusic;

        public GameScreen(GraphicsDevice device,
            ContentManager content)
            : base(device, content, "gameScreen")
        {
        }

        public override bool Init()
        {
            // TODO: Add your initialization logic here
            _player = new Player();

            _bgLayer1 = new ParallaxingBackground();
            _bgLayer2 = new ParallaxingBackground();
            _rectBackground = new Rectangle(0, 0, _device.Viewport.Width,
                _device.Viewport.Height);

            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            // init our laser
            laserBeams = new List<Laser>();
            const float SECONDS_IN_MINUTE = 60f;
            const float RATE_OF_FIRE = 200f;
            laserSpawnTime = TimeSpan.FromSeconds(SECONDS_IN_MINUTE / RATE_OF_FIRE);
            previousLaserSpawnTime = TimeSpan.Zero;

            // Initialize the enemies list
            enemies = new List<Enemy>();

            //used to determine how fast the enemies will respawn.
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);

            // init our random number generator
            random = new Random();

            explosions = new List<Explosion>();
            _boss = new Boss();

            Rectangle titleSafeArea = _device.Viewport.TitleSafeArea;
            var playerPosition = new Vector2(titleSafeArea.X, titleSafeArea.Y + titleSafeArea.Height / 2);
            font = _content.Load<SpriteFont>("Graphics\\Score");
            Texture2D playerTexture = _content.Load<Texture2D>("Graphics\\shipAnimation");
            Animation playerAnimation = new Animation();
            playerAnimation.Initialize(playerTexture, playerPosition, 115, 69, 8, 30, Color.White, 1, true);
            _player.Initialize(playerAnimation, playerPosition);

            // Load the background.
            _bgLayer1.Initialize(_content, "Graphics/bgLayer1",
                _device.Viewport.Width,
                _device.Viewport.Height, -1);
            _bgLayer2.Initialize(_content, "Graphics/bgLayer2",
                _device.Viewport.Width,
                _device.Viewport.Height, -2);
            _mainBackground = _content.Load<Texture2D>("Graphics/mainbackground");

            bossTexture = _content.Load<Texture2D>("Graphics\\boss");
            // load the enemy texture.
            enemyTexture = _content.Load<Texture2D>("Graphics\\mineAnimation");

            // load th texture to serve as the laser.
            laserTexture = _content.Load<Texture2D>("Graphics\\laser");

            // Load the exploision sprite strip
            explosionTexture = _content.Load<Texture2D>("Graphics\\explosion");

            // Load the laserSound Effect and create the effect Instance
            laserSound = _content.Load<SoundEffect>("Sounds\\laserFire");
            laserSoundInstance = laserSound.CreateInstance();

            // Load the laserSound Effect and create the effect Instance
            explosionSound = _content.Load<SoundEffect>("Sounds\\explosion");
            explosionSoundInstance = explosionSound.CreateInstance();

            // Load the game music
            gameMusic = _content.Load<Song>("Sounds\\gameMusic");

            // Start playing the music.
            MediaPlayer.Play(gameMusic);

            return base.Init();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Shutdown()
        {
            MediaPlayer.Stop();
            laserSoundInstance.Dispose();
            explosionSoundInstance.Dispose();
            base.Shutdown();
        }

        public override void Draw(GameTime gameTime)
        {
            _device.Clear(Color.CornflowerBlue);

            // Start drawing
            _spriteBatch.Begin();

            // Draw background.
            _spriteBatch.Draw(_mainBackground, _rectBackground, Color.White);
            _bgLayer1.Draw(_spriteBatch);
            _bgLayer2.Draw(_spriteBatch);

            // Draw Score
            _spriteBatch.DrawString(font, "Score: " + score, new Vector2(150, 0), Color.Black);
            _spriteBatch.DrawString(font, "Health: " + _player.Health, new Vector2(150, 30), Color.Black);

            // Draw the Player
            _player.Draw(_spriteBatch);
            //  _spriteBatch.Draw(bossTexture, _rectBackground, Color.Blue);
            if (_boss.Active)
            {
                _spriteBatch.Draw(bossTexture, _rectBackground, Color.Beige);
            }

            // Draw the lasers.
            foreach (var l in laserBeams)
            {
                l.Draw(_spriteBatch);
            }

            // draw the enemies
            foreach (var e in enemies)
            {
                e.Draw(_spriteBatch);
            };

            // draw explosions
            foreach (var e in explosions)
            {
                e.Draw(_spriteBatch);
            };

            // Stop drawing
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            // User inputs.
            // Save the previous state of the keyboard and game pad so we can determine single key/button presses
            _prevGamePadState = _currentGamePadState;
            _prevKeyboardState = _currentKeyboardState;
            _prevMouseState = _currentMouseState;
            // Read the current state of the keyboard and gamepad and store it.
            _currentKeyboardState = Keyboard.GetState();
            _currentGamePadState = GamePad.GetState(PlayerIndex.One);
            _currentMouseState = Mouse.GetState();

            UpdatePlayer(gameTime);
            _bgLayer1.Update(gameTime);
            _bgLayer2.Update(gameTime);

            // update lasers
            UpdateLasers(gameTime);

            // update the enemies
            UpdateEnemies(gameTime);

            // update collisons
            UpdateCollision();

            UpdateExplosions(gameTime);
            if (score >= 100 & !_boss.Active)
            {
                AddBoss();
            }
            if (_boss.Active)
            {
                UpdateBoss(gameTime);
                // TODO: if boss.inactive exit -> highscore
            }

            // Check if ESC is pressed or Player is inactive and go to GameOver screen
            if (!_player.Active || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                SCREEN_MANAGER.goto_screen("gameOver");
            }
            base.Update(gameTime);
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            for (var e = 0; e < explosions.Count; e++)
            {
                explosions[e].Update(gameTime);

                if (!explosions[e].Active)
                    explosions.Remove(explosions[e]);
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            _player.Update(gameTime);

            // Touch inputs
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();

                if (gesture.GestureType == GestureType.FreeDrag)
                    _player.Position += gesture.Delta;
            }

            // Get Mouse State
            Vector2 mousePosition = new Vector2(_currentMouseState.X, _currentMouseState.Y);
            if (_currentMouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 posDelta = mousePosition - _player.Position;
                posDelta.Normalize();
                posDelta = posDelta * PlayerMoveSpeed;
                _player.Position = _player.Position + posDelta;
            }

            // Thumbstick controls
            _player.Position.X += _currentGamePadState.ThumbSticks.Left.X * PlayerMoveSpeed;
            _player.Position.Y += _currentGamePadState.ThumbSticks.Left.Y * PlayerMoveSpeed;

            // Keyboard/DPad
            if (_currentKeyboardState.IsKeyDown(Keys.Left) || _currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                _player.Position.X -= +PlayerMoveSpeed;
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Right) || _currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                _player.Position.X += PlayerMoveSpeed;
            }

            if (_currentKeyboardState.IsKeyDown(Keys.Up) || _currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                _player.Position.Y -= PlayerMoveSpeed;
            }
            if (_currentKeyboardState.IsKeyDown(Keys.Down) || _currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                _player.Position.Y += PlayerMoveSpeed;
            }

            if (_currentKeyboardState.IsKeyDown(Keys.Space) || _currentGamePadState.Buttons.X == ButtonState.Pressed)
            {
                FireLaser(gameTime);
            }

            // Make sure that the player does not go out of bounds
            _player.Position.X = MathHelper.Clamp(_player.Position.X, 0, _device.Viewport.Width - _player.Width);
            _player.Position.Y = MathHelper.Clamp(_player.Position.Y, 0, _device.Viewport.Height - _player.Height);
        }

        protected void UpdateLasers(GameTime gameTime)
        {
            // update laserbeams
            for (var i = 0; i < laserBeams.Count; i++)
            {
                laserBeams[i].Update(gameTime);

                // Remove the beam when its deactivated or is at the end of the screen.
                if (!laserBeams[i].Active || laserBeams[i].Position.X > _device.Viewport.Width)
                {
                    laserBeams.Remove(laserBeams[i]);
                }
            }
        }

        protected void AddBoss()
        {
            Animation bossAnimation = new Animation();

            bossAnimation.Initialize(bossTexture,
              Vector2.Zero,
               47,
               61,
               8,
               30,
               Color.White,
               1f,
               true);
            Vector2 position = new Vector2(200, 100);
            // init the boss
            _boss.Initialize(bossAnimation, position);
        }

        protected void UpdateBoss(GameTime gameTime)
        {
            if (_boss.Active)
            {
                _boss.Update(gameTime);
            }
        }

        protected void FireLaser(GameTime gameTime)
        {
            // govern the rate of fire for our lasers
            if (gameTime.TotalGameTime - previousLaserSpawnTime > laserSpawnTime)
            {
                previousLaserSpawnTime = gameTime.TotalGameTime;

                // Add the laer to our list.
                AddLaser();

                // Play the laser sound!
                laserSoundInstance.Play();
            }
        }

        protected void AddLaser()
        {
            Animation laserAnimation = new Animation();

            // initlize the laser animation
            laserAnimation.Initialize(laserTexture,
                _player.Position,
                46,
                16,
                1,
                30,
                Color.White,
                1f,
                true);

            Laser laser = new Laser();

            // Get the starting postion of the laser.
            var laserPostion = _player.Position;
            // Adjust the position slightly to match the muzzle of the cannon.
            laserPostion.Y += 37;
            laserPostion.X += 70;

            // init the laser
            laser.Initialize(laserAnimation, laserPostion);

            laserBeams.Add(laser);

            /* todo: add code to create a laser. */
            //laserSoundInstance.Play();
        }

        protected void UpdateEnemies(GameTime gameTime)
        {
            // spawn a new enemy every 1.5 seconds.
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime & !_boss.Active)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                // add an enemy
                AddEnemy();
            }

            // update the enemies
            for (var i = 0; i < enemies.Count; i++)
            {
                enemies[i].Update(gameTime);
                if (!enemies[i].Active)
                {
                    enemies.Remove(enemies[i]);
                }
            }
        }

        protected void AddEnemy()
        {
            // create the animation object
            Animation enemyAnimation = new Animation();

            // Init the animation with the correct
            // animation information
            enemyAnimation.Initialize(enemyTexture,
                Vector2.Zero,
                47,
                61,
                8,
                30,
                Color.White,
                1f,
                true);

            // randomly generate the postion of the enemy
            Vector2 position = new Vector2(
                _device.Viewport.Width + enemyTexture.Width / 2,
                random.Next(100, _device.Viewport.Height - 100));

            // create an enemy
            Enemy enemy = new Enemy();

            // init the enemy
            enemy.Initialize(enemyAnimation, position);

            // Add the enemy to the active enemies list
            enemies.Add(enemy);
        }

        protected void UpdateCollision()
        {
            // we are going to use the rectangle's built in intersection
            // methods.

            Rectangle playerRectangle;
            Rectangle enemyRectangle;
            Rectangle laserRectangle;

            // create the rectangle for the player
            playerRectangle = new Rectangle(
                (int)_player.Position.X,
                (int)_player.Position.Y,
                _player.Width,
                _player.Height);

            // detect collisions between the player and all enemies.
            for (var i = 0; i < enemies.Count; i++)
            {
                enemyRectangle = new Rectangle(
                   (int)enemies[i].Position.X,
                   (int)enemies[i].Position.Y,
                   enemies[i].Width,
                   enemies[i].Height);

                // determine if the player and the enemy intersect.
                if (playerRectangle.Intersects(enemyRectangle))
                {
                    // kill off the enemy
                    enemies[i].Health = 0;

                    // Show the explosion where the enemy was...
                    AddExplosion(enemies[i].Position);

                    // deal damge to the player
                    _player.Health -= enemies[i].Damage;
                    // if the player has no health destroy it.
                    if (_player.Health <= 0)
                    {
                        //AddExplosion(_player.Position);
                        _player.Active = false;
                        // _player.Position = Vector2.Zero;
                    }
                }

                for (var l = 0; l < laserBeams.Count; l++)
                {
                    // create a rectangle for this laserbeam
                    laserRectangle = new Rectangle(
                        (int)laserBeams[l].Position.X,
                        (int)laserBeams[l].Position.Y,
                        laserBeams[l].Width,
                        laserBeams[l].Height);

                    // test the bounds of the laser and enemy
                    if (laserRectangle.Intersects(enemyRectangle))
                    {
                        // Show the explosion where the enemy was...
                        AddExplosion(enemies[i].Position);

                        // kill off the enemy
                        enemies[i].Health = 0;

                        // kill off the laserbeam
                        laserBeams[l].Active = false;
                        score++;
                    }
                }
            }
        }

        protected void AddExplosion(Vector2 enemyPosition)
        {
            Animation explosionAnimation = new Animation();

            explosionAnimation.Initialize(explosionTexture,
                enemyPosition,
                134,
                134,
                12,
                30,
                Color.White,
                1.0f,
                true);

            Explosion explosion = new Explosion();
            explosion.Initialize(explosionAnimation, enemyPosition);

            explosions.Add(explosion);

            /* play the explosion sound. */
            explosionSound.Play();
            //explosionSoundInstance.Play();
        }
    }
}