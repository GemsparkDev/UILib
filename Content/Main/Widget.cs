using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UILib.Content.Main;

public abstract class Widget
{
    public Color color = Color.White;
    protected Vector2 offset;
    public string text;
    public Texture2D Texture { get; set; }
    public Vector2 Offset => offset * UIManager.UIScale;
    public virtual Vector2 Size => (Texture != null) ? UIManager.DimsOf(Texture) : Vector2.Zero;
    public virtual void Update() { }
    public virtual void Draw(SpriteBatch _spriteBatch, Vector2 _parentPositon, float _transparency, Vector2 _center)
    {
        if(Texture != null)
        {
            _spriteBatch.Draw(Texture, _parentPositon + Offset - _center, null, color * _transparency, 0, Size / 2, UIManager.UIScale, 0, 0);
        }
    }
    public abstract void HoveringDraw(SpriteBatch _spriteBatch);
}
