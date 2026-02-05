using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UILib.Content.Main;

public abstract class Widget
{
    public Color color = Color.White;
    protected Vector2 offset;
    public string text;
    public Texture2D texture;
    public Vector2 Offset => offset * UIManager.UIScale;
    public Vector2 Size { get; protected set; }
    public virtual void Update() { }
    public virtual void Draw(SpriteBatch _spriteBatch, Vector2 _parentPositon, float _transparency, Vector2 _center)
    {
        if(texture != null)
        {
            _spriteBatch.Draw(texture, _parentPositon + Offset - _center, null, color * _transparency, 0, Size / 2, UIManager.UIScale, 0, 0);
        }
    }
    public abstract void HoveringDraw(SpriteBatch _spriteBatch);
    public void SetTexture(Texture2D _texture)
    {
        texture = _texture;
        Size = UIManager.DimsOf(_texture);
    }
}
