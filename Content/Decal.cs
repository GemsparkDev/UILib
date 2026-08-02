using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace UILib.Content;

public class Decal : Widget
{
    private SpriteFont textFont;
    private float textSize = 10f;
    public Color textColor;
    private Vector2 size;
    public override Vector2 Size => size;
    public Decal(Vector2 _offset, Texture2D _texture, SpriteFont _textFont, string _text, Color _textColor, float _textSize)
    {
        offset = _offset;
        Texture = _texture;
        textFont = _textFont;
        Text {get; set;} = _text;
        textColor = _textColor;
        textSize = _textSize;
    }
    public Decal(Vector2 _offset, Texture2D _texture)
    {
        offset = _offset;
        Texture = _texture;
        Text {get; set;} = null;
        textColor = Color.White;
    }
    public Decal(Vector2 _offset, SpriteFont _textFont, string _text, Color _textColor, float _textSize)
    {
        size = _textFont.MeasureString(_text) * new Vector2(1, 0.5f);
        offset = _offset;
        Texture = null;
        textFont = _textFont;
        Text {get; set;} = _text;
        textColor = _textColor;
        textSize = _textSize;
    }
    public override void Draw(SpriteBatch _spriteBatch, Vector2 _parentPosition, float _transparency, Vector2 _center)
    {
        base.Draw(_spriteBatch, _parentPosition, _transparency, _center);
        if (Text {get; set;} != null)
        {
            Vector2 textMiddlePoint = textFont.MeasureString(Text {get; set;}) / 2;
            _spriteBatch.DrawString(textFont, Text {get; set;}, _parentPosition + Offset - _center, textColor, 0, textMiddlePoint, UIManager.UIScale * textSize / 10, 0, 0);
            if (Text {get; set;} == "Borderless Window")
            {
                Debug.WriteLine($"Decal: {Size.Y}");
            }
        }
    }
    public override void HoveringDraw(SpriteBatch _spriteBatch, Vector2 _parentPosition, float _transparency, Vector2 _center) { }
}
