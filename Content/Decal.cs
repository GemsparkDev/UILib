using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace UILib.Content;

public class Decal : Widget
{
    private SpriteFont textFont;
    private float textSize = 10f;
    public Color textColor;
    public override Vector2 Size => ((Texture != null) ? UIManager.DimsOf(Texture) : textFont.MeasureString(Text) * new Vector2(1.1f, 0.5f));
    public override Vector2 Center
    {
        get
        {
            var o = Size / 2;
            if (WidgetAlignment is Alignment.Center or Alignment.Top or Alignment.Bottom)
            {
                o.X -= Size.X / 2;
            }
            else if (WidgetAlignment is Alignment.Right or Alignment.TopRight or Alignment.BottomRight)
            {
                o.X -= Size.X;
            }
            if (WidgetAlignment is Alignment.Center or Alignment.Left or Alignment.Right)
            {
                o.Y -= Size.Y / 2;
            }
            else if (WidgetAlignment is Alignment.BottomLeft or Alignment.Bottom or Alignment.BottomRight)
            {
                o.Y -= Size.Y;
            }
            return o;
        }
    }
    public Decal(Vector2 _offset, Texture2D _texture, SpriteFont _textFont, string _text, Color _textColor, float _textSize, Container _container, int[] _index) : base(_offset, _container, _index)
    {
        Texture = _texture;
        textFont = _textFont;
        Text = _text;
        textColor = _textColor;
        textSize = _textSize;
    }
    public Decal(Vector2 _offset, Texture2D _texture, Container _container, int[] _index) : base(_offset, _container, _index)
    {
        Texture = _texture;
        Text = null;
        textColor = Color.White;
    }
    public Decal(Vector2 _offset, SpriteFont _textFont, string _text, Color _textColor, float _textSize, Container _container, int[] _index) : base(_offset, _container, _index)
    {
        Texture = null;
        textFont = _textFont;
        Text = _text;
        textColor = _textColor;
        textSize = _textSize;
    }
    public override void Draw(SpriteBatch _spriteBatch, Vector2 _parentPosition, float _transparency, Vector2 _center)
    {
        base.Draw(_spriteBatch, _parentPosition, _transparency, _center);
        if (Text != null)
        {
            Vector2 textMiddlePoint = textFont.MeasureString(Text) / 2;
            _spriteBatch.DrawString(textFont, Text, _parentPosition + Offset - _center, textColor, 0, textMiddlePoint, UIManager.UIScale * textSize / 10, 0, 0);
            if (Text == "Borderless Window")
            {
                //Debug.WriteLine($"Decal: {Size.Y}");
            }
        }
    }
    public override void HoveringDraw(SpriteBatch _spriteBatch, Vector2 _parentPosition, float _transparency, Vector2 _center) { }
}
