using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class GameObject : DrawableGameComponent
    {
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;
        protected Texture2D _texture;
        protected Rectangle _drawRect;
        protected Vector2 _position;

        public GameObject(Game game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
            : base(game)
        {
            _graphics = graphics;
            _spriteBatch = spriteBatch;
        }
    }
}
