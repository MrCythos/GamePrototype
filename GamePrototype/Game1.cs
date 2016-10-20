using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GamePrototype
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;

        Vector2 positionVector = Vector2.Zero;
        Vector2 accelerationVector = Vector2.Zero;
        int maxSpeed = 4;
        float vectorAccelerationX = 0.1f;
        float vectorAccelerationY = 0.1f;
        float vectorDecelerationX = -0.2f;
        float vectorDecelerationY = -0.2f;

        Vector2 jump = Vector2.Zero;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here

            base.Initialize();
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = this.Content.Load<Texture2D>("Sprites/StandingTest");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState ms = Mouse.GetState();
            var mousePosition = new Vector2(ms.X, ms.Y);

            //if (positionVector.Length() >= maxSpeed)
            //{
            //    positionVector.Normalize() = maxSpeed;
            //}

            //resets the player at the origin
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                positionVector.X = 0;
                positionVector.Y = 0;
                accelerationVector.X = 0;
                accelerationVector.Y = 0;
            }

            //allows acceleration of player movement
            positionVector.X += accelerationVector.X;
            positionVector.Y += accelerationVector.Y;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                accelerationVector.X += vectorAccelerationX;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                accelerationVector.X -= vectorAccelerationX;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                accelerationVector.Y -= vectorAccelerationY;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                accelerationVector.Y += vectorAccelerationY;
            }

            if (accelerationVector.X > maxSpeed)
            {
                accelerationVector.X = maxSpeed;
            }
            if (accelerationVector.X < -maxSpeed)
            {
                accelerationVector.X = -maxSpeed;
            }
            if (accelerationVector.Y > maxSpeed)
            {
                accelerationVector.Y = maxSpeed;
            }
            if (accelerationVector.Y < -maxSpeed)
            {
                accelerationVector.Y = -maxSpeed;
            }

            //slows x movement if player lets go of x movement keys
            if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right))
            {
                if (Math.Abs(accelerationVector.X) > Math.Abs(vectorDecelerationX))
                {
                    if (accelerationVector.X > vectorAccelerationX)
                    {
                        accelerationVector.X += vectorDecelerationX;
                    }
                    if (accelerationVector.X < -vectorAccelerationX)
                    {
                        accelerationVector.X -= vectorDecelerationX;
                    }
                }
                if (Math.Abs(accelerationVector.X) < Math.Abs(vectorDecelerationX))
                {
                    accelerationVector.X = 0;
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Down))
            {
                if (Math.Abs(accelerationVector.Y) > Math.Abs(vectorDecelerationY))
                {
                    if (accelerationVector.Y > vectorAccelerationY)
                    {
                        accelerationVector.Y += vectorDecelerationY;
                    }
                    if (accelerationVector.Y < -vectorAccelerationY)
                    {
                        accelerationVector.Y -= vectorDecelerationY;
                    }
                }
                if (Math.Abs(accelerationVector.Y) < Math.Abs(vectorDecelerationY))
                {
                    accelerationVector.Y = 0;
                }
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                jump = mousePosition - positionVector;
                jump.Normalize();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, positionVector);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
