using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UILib.Content;

public class Decal : Widget
{
    private SpriteFont textFont;
    private float textSize = 1f;
    public Color textColor;
    private Vector2 size;
    public override Vector2 Size => size != default ? size : base.Size;
    public Decal(Vector2 _offset, Texture2D _texture, SpriteFont _textFont, string _text, Color _textColor, float _textSize)
    {
        offset = _offset;
        Texture = _texture;
        textFont = _textFont;
        text = _text;
        textColor = _textColor;
        textSize = _textSize;
    }
    public Decal(Vector2 _offset, Texture2D _texture)
    {
        offset = _offset;
        Texture = _texture;
        text = null;
        textColor = Color.White;

    }
    public Decal(Vector2 _offset, SpriteFont _textFont, string _text, Color _textColor, float _textSize)
    {
        size = new Vector2(_text.Length * 4, 12) * textSize;
        offset = _offset;
        Texture = null;
        textFont = _textFont;
        text = _text;
        textColor = _textColor;
        textSize = _textSize;
    }
    public override void Draw(SpriteBatch _spriteBatch, Vector2 _parentPosition, float _transparency, Vector2 _center)
    {
        base.Draw(_spriteBatch, _parentPosition, _transparency, _center);
        if (text != null)
        {
            Vector2 textMiddlePoint = textFont.MeasureString(text) / 2;
            Vector2 textPosition = _parentPosition + Offset;
            _spriteBatch.DrawString(textFont, text, textPosition - _center, textColor, 0, textMiddlePoint, UIManager.UIScale * textSize / 10, SpriteEffects.None, 0.45f);
        }
    }
    public override void HoveringDraw(SpriteBatch _spriteBatch) { }
}
