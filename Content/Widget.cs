using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UILib.Content;

public abstract class Widget
{
    public Widget(Vector2 _offset, Container _container, int[] _index)
    {
        Container = _container;
        Register(_index);
        offset = _offset;
    }
    public virtual void Register(int[] _index)
    {
        foreach (var index in _index)
        {
            Container.AddWidget(this, index); //Autointialization
        }
    }
    public Container Container { get; protected set; }
    public Color color = Color.White;
    private Vector2 offset;
    public virtual string Text { get; set; }
    public Texture2D Texture { get; set; }
    public Vector2 Offset => (offset) * UIManager.UIScale + Center;
    public virtual Vector2 Size => Texture != null ? UIManager.DimsOf(Texture) : Vector2.Zero;
    public Alignment WidgetAlignment { get; set; } = Alignment.Center;
    public Vector2 Center
    { 
        get 
        {
            var o = Size/2;
            if (WidgetAlignment is Alignment.Center or Alignment.Top or Alignment.Bottom)
            {
                o.X -= Size.X/2;
            }
            else if (WidgetAlignment is Alignment.Right or Alignment.TopRight or Alignment.BottomRight)
            {
                o.X -= Size.X;
            }
            if (WidgetAlignment is Alignment.Center or Alignment.Left or Alignment.Right)
            {
                o.Y -= Size.Y/2;
            }
            else if (WidgetAlignment is Alignment.BottomLeft or Alignment.Bottom or Alignment.BottomRight)
            {
                o.Y -= Size.Y;
            }
            return o;
        } 
    }
    public virtual void Update() { }
    public virtual void Draw(SpriteBatch _spriteBatch, Vector2 _parentPositon, float _transparency, Vector2 _center)
    {
        if(Texture != null)
        {
            _spriteBatch.Draw(Texture, _parentPositon + Offset - _center, null, color * _transparency, 0, Size / 2, UIManager.UIScale, 0, 0);
        }
    }
    public abstract void HoveringDraw(SpriteBatch _spriteBatch, Vector2 _parentPosition, float _transparency, Vector2 _center);
}
