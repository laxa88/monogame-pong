using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class GameObject : DrawableGameComponent
    {
        protected SpriteBatch _spriteBatch;
        protected Texture2D _texture;
        protected Rectangle _drawRect;
        protected Vector2 _position;

        public GameObject(Game game, SpriteBatch spriteBatch) : base(game)
        {
            _spriteBatch = spriteBatch;
        }
    }
}
