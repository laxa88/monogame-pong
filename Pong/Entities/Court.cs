using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class Court : GameObject
    {
        private RenderTarget2D _renderTarget;
        private Texture2D _texture;

        public int width
        {
            get { return _renderTarget.Width; }
        }

        public int height
        {
            get { return _renderTarget.Height; }
        }

        public Court(
            Game game,
            GraphicsDeviceManager graphics,
            SpriteBatch spriteBatch,
            int width,
            int height
        ) : base(game, graphics, spriteBatch)
        {
            _renderTarget = new RenderTarget2D(GraphicsDevice, width, height);
        }

        override public void Initialize()
        {
            _texture = new Texture2D(GraphicsDevice, 1, 1);
            var data = new Color[1];
            data[0] = Color.White;
            _texture.SetData(data);

            DrawStripesOnCourt();
        }

        /// <summary>
        /// Use this once during initialization. Uses a square texture to draw
        /// a striped line on a black background.
        /// </summary>
        public void DrawStripesOnCourt()
        {
            GraphicsDevice.GetRenderTargets();
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            for (int i = 0; i < 31; i++)
            {
                _spriteBatch.Draw(
                    _texture,
                    new Rectangle(
                        _renderTarget.Width / 2 - 1,
                        i * _renderTarget.Height / 31,
                        2,
                        _renderTarget.Height / 62
                    ),
                    Color.White
                );
            }
            _spriteBatch.End();
        }

        override public void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(
                _renderTarget,
                new Rectangle(0, 0, _renderTarget.Width, _renderTarget.Height),
                Color.White
            );
        }
    }
}
