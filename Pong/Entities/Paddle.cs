using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Paddle : GameObject
    {
        protected Texture2D _texture;
        protected Rectangle _drawRect;
        private float _speed;

        public int width
        {
            get { return _drawRect.Width; }
        }

        public int height
        {
            get { return _drawRect.Height; }
        }

        public Paddle(Game game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
            : base(game, graphics, spriteBatch) { }

        public void Initialize(int x, int y, int w, int h)
        {
            _texture = new Texture2D(GraphicsDevice, 1, 1);
            var data = new Color[1];
            data[0] = Color.White;
            _texture.SetData(data);

            _drawRect = new Rectangle(0, 0, w, h);
            _position = new Vector2(x, y);
            _speed = 0.5f;
        }

        public void Update(GameTime gameTime, Court court)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
                _position.Y -= _speed * gameTime.ElapsedGameTime.Milliseconds;

            if (kstate.IsKeyDown(Keys.Down))
                _position.Y += _speed * gameTime.ElapsedGameTime.Milliseconds;

            if (_position.Y > court.height - _drawRect.Height)
                _position.Y = court.height - _drawRect.Height;
            else if (_position.Y < 0f)
                _position.Y = 0f;
        }

        override public void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, _position, _drawRect, Color.White);
        }
    }
}
