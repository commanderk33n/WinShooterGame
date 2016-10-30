using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WinShooterGame
{
    public class Animation
    {
        // The image representing the collection of images used for animation
        private Texture2D spriteStrip;

        // The scale used to display the sprite strip
        private float scale;

        // The time since we last updated the frame
        private int elapsedTime;

        // The time we display a frame until the next one
        private int frameTime;

        // The number of frames that the animation contains
        private int frameCount;

        // The index of the current frame we are displaying
        private int currentFrame;

        // The color of the frame we will be displaying
        private Color color;

        // The area of the image strip we want to display
        private Rectangle sourceRect = new Rectangle();

        // The area where we want to display the image strip in the game
        private Rectangle destinationRect = new Rectangle();

        // Width of a given frame
        public int FrameWidth;

        // Height of a given frame
        public int FrameHeight;

        // The state of the Animation
        public bool Active;

        // Determines if the animation will keep playing or deactivate after one run
        public bool Looping;

        // Width of a given frame
        public Vector2 Position;

        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;

            Looping = looping;
            Position = position;
            spriteStrip = texture;

            elapsedTime = 0;
            currentFrame = 0;

            // Set the Animation to active by default.
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (Active == false) return;

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime > frameTime)
            {
                currentFrame++;

                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    if (Looping == false)
                        Active = false;
                }

                elapsedTime = 0;
            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the Frame width
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            // Grab the frame in the image strip by multiplying the currentFrame index by the frame width.
            destinationRect = new Rectangle((int)Position.X,
                                            (int)Position.Y,
                                            (int)(FrameWidth * scale),
                                            (int)(FrameHeight * scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
        }
    }
}