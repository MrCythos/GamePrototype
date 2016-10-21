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

        float deltaTime;

        Vector2 mapOrigin = new Vector2(400, 200);
        Vector2 positionVector = Vector2.Zero;
        Vector2 accelerationVector = Vector2.Zero;
        int maxSpeed = 4;
        float vectorAccelerationX = 0.3f;
        float vectorAccelerationY = 0.3f;
        float vectorDecelerationX = -0.5f;
        float vectorDecelerationY = -0.5f;
        
        Vector2 jump = Vector2.Zero;
        float jumpDistance = 50f;
        float jumpTimeElapsed = 0f;
        float jumpTime = 0.5f;
        float jumpChargeTime = 0f;
        float jumpMaxCharge = 1.0f;
        float jumpChargeMult;
        bool jumpBool = true;

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
            positionVector = new Vector2(texture.Width / 2, texture.Height / 2);

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
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

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

            //movement of player, replace getstates with more elegant solution, maybe enum
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                accelerationVector.X += vectorAccelerationX;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                accelerationVector.X -= vectorAccelerationX;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                accelerationVector.Y -= vectorAccelerationY;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                accelerationVector.Y += vectorAccelerationY;
            }

            //sets a maximum velocity
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
            //there's probably a more elegant solution here. there is a sort of conflict, if a player wants to slow down quickly, 
            //they must let go of all keys, pressing left while going right is slower than letting go of the keys while going right.
            if (Keyboard.GetState().IsKeyUp(Keys.A) && Keyboard.GetState().IsKeyUp(Keys.D))
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
                    if (Math.Abs(accelerationVector.X) <= Math.Abs(vectorDecelerationX))
                    {
                        accelerationVector.X = 0;
                    }
                }
                if (Keyboard.GetState().IsKeyUp(Keys.W) && Keyboard.GetState().IsKeyUp(Keys.S))
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
                    if (Math.Abs(accelerationVector.Y) <= Math.Abs(vectorDecelerationY))
                    {
                        accelerationVector.Y = 0;
                    }
                }

            jumpTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (jumpTimeElapsed > 10) jumpTimeElapsed = 1 + jumpTime; //prevents the time elapsed counter from constantly rising

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                jumpChargeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (jumpChargeTime > jumpMaxCharge)
                {
                    jumpChargeTime = jumpMaxCharge;
                }
                jumpBool = false;
                jumpChargeMult = jumpChargeTime / 0.25f;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                if (jumpBool == false)
                {
                    float angleOfJump;

                    if (jumpTimeElapsed > jumpTime)
                    {
                            if (accelerationVector.X != 0 || accelerationVector.Y != 0)
                            {
                                angleOfJump = (float)Math.Atan2(accelerationVector.X, accelerationVector.Y);

                                positionVector.X += ((float)Math.Sin(angleOfJump) * jumpDistance * jumpChargeMult);
                                positionVector.Y += ((float)Math.Cos(angleOfJump) * jumpDistance * jumpChargeMult);
                            }
                            else
                            {   //instead of mouse position, use last known vector/acceleration
                                //angleOfJump = (float)Math.Atan2(mousePosition.X - positionVector.X, mousePosition.Y - positionVector.Y);

                                //positionVector.X += ((float)Math.Sin(angleOfJump) * jumpDistance);
                                //positionVector.Y += ((float)Math.Cos(angleOfJump) * jumpDistance);
                            }
                        jumpChargeTime = 0f;
                        jumpTimeElapsed = 0f;
                    }
                }
                jumpBool = true;
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
            spriteBatch.Draw(texture, mapOrigin + positionVector);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
