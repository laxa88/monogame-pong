using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class PongGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderBuffer;
        private Rectangle _renderRectangle;

        private Court _court;
        private Ball _ball;
        private Paddle _paddleLeft;
        private Paddle _paddleRight;

        public PongGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = true;

            // Don't initialize local variables here because Monogame stuff
            // hasn't been initialized yet, e.g. GraphicsDevice
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Note: Doesn't work if you set in constructor
            Window.Title = "My Pong Game";

            // Initialize variables first
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _renderBuffer = new RenderTarget2D(GraphicsDevice, 640, 480);

            _court = new Court(this, _graphics, _spriteBatch, 640, 480);
            _court.Initialize();

            _ball = new Ball(this, _graphics, _spriteBatch);
            _ball.Initialize(_court.width / 2, _court.height / 2);
            _ball.BallExitedLeft += OnLeftPlayerWin;
            _ball.BallExitedRight += OnRightPlayerWin;

            _paddleLeft = new Paddle(this, _graphics, _spriteBatch);
            _paddleLeft.Initialize(10, (_court.height / 2) - 50, 10, 100);
            _paddleRight = new Paddle(this, _graphics, _spriteBatch);
            _paddleRight.Initialize(_court.width - 20, (_court.height / 2) - 50, 10, 100);

            // Add callbacks

            Window.ClientSizeChanged += OnWindowSizeChange;
            OnWindowSizeChange(null, null);
        }

        private void OnLeftPlayerWin(object sender, EventArgs e)
        {
            Debug.WriteLine("Left player win");
        }

        private void OnRightPlayerWin(object sender, EventArgs e)
        {
            Debug.WriteLine("Right player win");
        }

        private void OnWindowSizeChange(object sender, EventArgs e)
        {
            var width = Window.ClientBounds.Width;
            var height = Window.ClientBounds.Height;

            if (height < width / (float)_renderBuffer.Width * _renderBuffer.Height)
            {
                width = (int)(height / (float)_renderBuffer.Height * _renderBuffer.Width);
            }
            else
            {
                height = (int)(width / (float)_renderBuffer.Width * _renderBuffer.Height);
            }

            var x = (Window.ClientBounds.Width - width) / 2;
            var y = (Window.ClientBounds.Height - height) / 2;
            _renderRectangle = new Rectangle(x, y, width, height);
        }

        protected override void LoadContent()
        {
            // Load global content here if necessary

            // Note that this is protected, so LoadContent within GameObjects
            // should be invoked within themselves whenever necessary.
        }

        protected override void Update(GameTime gameTime)
        {
            if (
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)
            )
            {
                Exit();
            }

            _paddleLeft.Update(gameTime, _court);
            _paddleRight.Update(gameTime, _court);
            _ball.Update(gameTime, _court);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Target buffer screen (original resolution)
            GraphicsDevice.SetRenderTarget(_renderBuffer);
            GraphicsDevice.Clear(Color.Black);

            // Draw everything game-related on this buffer screen.
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            _court.Draw(gameTime);
            _paddleLeft.Draw(gameTime);
            _paddleRight.Draw(gameTime);
            _ball.Draw(gameTime);
            _spriteBatch.End();

            // Target main window, reset background colour
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the buffer screen onto the main window,
            // resized with PointClamp to create pixel-perfect effect.
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            _spriteBatch.Draw(_renderBuffer, _renderRectangle, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
