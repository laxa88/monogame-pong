using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _doubleBuffer;
        private Rectangle _renderRectangle;
        private Texture2D _texture;

        private Ball _ball;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _doubleBuffer = new RenderTarget2D(GraphicsDevice, 640, 480);

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            Window.ClientSizeChanged += OnWindowSizeChange;
            OnWindowSizeChange(null, null);

            _ball = new Ball(this, _graphics, _spriteBatch);

            base.Initialize();
        }

        private void OnWindowSizeChange(object sender, EventArgs e)
        {
            var width = Window.ClientBounds.Width;
            var height = Window.ClientBounds.Height;

            if (height < width / (float)_doubleBuffer.Width * _doubleBuffer.Height)
            {
                width = (int)(height / (float)_doubleBuffer.Height * _doubleBuffer.Width);
            }
            else
            {
                height = (int)(width / (float)_doubleBuffer.Width * _doubleBuffer.Height);
            }

            var x = (Window.ClientBounds.Width - width) / 2;
            var y = (Window.ClientBounds.Height - height) / 2;
            _renderRectangle = new Rectangle(x, y, width, height);
        }

        protected override void LoadContent()
        {
            _texture = new Texture2D(GraphicsDevice, 1, 1);
            var data = new Color[1];
            data[0] = Color.White;
            _texture.SetData(data);

            DrawStripes();
        }

        protected override void Update(GameTime gameTime)
        {
            if (
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)
            )
                Exit();

            _ball.Update(gameTime);

            base.Update(gameTime);
        }

        private void DrawStripes()
        {
            GraphicsDevice.SetRenderTarget(_doubleBuffer);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            for (int i = 0; i < 31; i++)
            {
                _spriteBatch.Draw(
                    _texture,
                    new Rectangle(
                        _doubleBuffer.Width / 2,
                        i * _doubleBuffer.Height / 31,
                        2,
                        _doubleBuffer.Height / 62
                    ),
                    Color.White
                );
            }
            _spriteBatch.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            _spriteBatch.Draw(_doubleBuffer, _renderRectangle, Color.White);
            _ball.Draw(gameTime);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
