using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Ball : GameObject
    {
        protected Texture2D _texture;
        protected Rectangle _drawRect;
        private Vector2 _velocity;
        private float _speed;

        public Ball(Game game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
            : base(game, graphics, spriteBatch) { }

        public void Initialize(int x, int y)
        {
            _drawRect = new Rectangle(0, 0, 10, 10);
            _position = new Vector2(x, y);
            _velocity = new Vector2(1f, 1f);
            _speed = 300f;

            LoadContent();
        }

        override protected void LoadContent()
        {
            _texture = this.Game.Content.Load<Texture2D>("ball");
        }

        public void Update(GameTime gameTime, Court court)
        {
            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
                _position.Y -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                _position.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                _position.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Right))
                _position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_position.X > court.width - _texture.Width / 2)
                _position.X = court.width - _texture.Width / 2;
            else if (_position.X < _texture.Width / 2)
                _position.X = _texture.Width / 2;

            if (_position.Y > court.height - _texture.Height / 2)
                _position.Y = court.height - _texture.Height / 2;
            else if (_position.Y < _texture.Height / 2)
                _position.Y = _texture.Height / 2;

            // move the ball within bounds
            // var newVelocity = _velocity;
            // _velocity.Normalize();
            // _velocity *= _speed;
            // _position += _velocity;

            // _drawRect.X = (int)_position.X;
            // _drawRect.Y = (int)_position.Y;

            // check if new position is out of top/bottom bounds
            // if yes, bounce it back within bounds

            // check if new position is out of left/right bounds
            // if yes, signal round end
        }

        override public void Draw(GameTime gameTime)
        {
            // _spriteBatch.Draw(_texture, _drawRect, Color.White);

            _spriteBatch.Draw(
                _texture,
                _position,
                null,
                Color.White,
                0f,
                new Vector2(_texture.Width / 2, _texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
        }
    }
}
