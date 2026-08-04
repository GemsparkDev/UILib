using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace UILib.Content;

public class Button : FunctionalWidget
{
    private List<Action> behaviours = [];
    private SpriteFont textFont;
    public Color TextColor { get; set; } = Color.White;
    private float textSize = 10f;
    private Texture2D flipTexture;
    public Window Tooltip { get; private set; }
    private Vector2 size;
    public override Vector2 Size => (size != default) ? size : base.Size;
    public Button(Vector2 _offset, Texture2D _texture)
    {
        offset = _offset;
        Texture = _texture;
        Text = null;
    }
    public Button(Vector2 _offset, Texture2D _texture, SpriteFont _textFont, string _text, Color _textColor, Texture2D _flipTexture = null)
    {
        offset = _offset;
        Texture = _texture;
        textFont = _textFont;
        Text = _text;
        TextColor = _textColor;
        flipTexture = _flipTexture;
    }
    public Button(Vector2 _offset, SpriteFont _textFont, string _text, Color _textColor, float _textSize)
    {
        size = _textFont.MeasureString(_text) * new Vector2(1, 0.5f);
        textSize = _textSize;
        offset = _offset;
        textFont = _textFont;
        Texture = null;
        Text = _text;
        TextColor = _textColor;
    }
    public override void Interact(Vector2 parentPosition)
    {
        ApplyBehaviours();
        if (flipTexture != null)
        {
            (Texture, flipTexture) = (flipTexture, Texture);
        }
    }
    public void AddTooltip(Window _tooltip)
    {
        Tooltip ??= _tooltip;
    }
    public override void ContinuousInteract(Vector2 parentPosition) { base.ContinuousInteract(parentPosition); }
    public override void AddBehaviour(Action func)
    {
        behaviours.Add(func);
    }
    public override void ApplyBehaviours()
    {
        for (int i = 0; i < behaviours.Count; i++)
        {
            behaviours[i]();
        }
    }
    public override void Draw(SpriteBatch _spriteBatch, Vector2 _parentPosition, float _transparency, Vector2 _center)
    {
        base.Draw(_spriteBatch, _parentPosition, _transparency, _center);
        if (Text != null)
        {
            Vector2 textMiddlePoint = textFont.MeasureString(Text ) / 2;
            float textSize = Size.X/(Text .Length * 10);
            if(textSize > 1)
            {
                textSize = 1;
            }
            _spriteBatch.DrawString(textFont, Text, _parentPosition + Offset - _center, TextColor, 0, textMiddlePoint, textSize * UIManager.UIScale * this.textSize / 10, 0, 0);
            if(Text == "Next")
            {
                Debug.WriteLine($"Button: {Size.Y}");
            }
        }
    }
    public override void HoveringDraw(SpriteBatch _spriteBatch, Vector2 _parentPosition, float _transparency, Vector2 _center) 
    {
        if (Tooltip == null)
        {
            return;
        }
        MouseState newState = Mouse.GetState();
        Texture2D tex = Tooltip.texture;
        Tooltip.position = new Vector2(newState.Position.X, newState.Position.Y) + new Vector2(tex.Width, tex.Height) / 2 * UIManager.UIScale;
        Tooltip.Draw(_spriteBatch);
    }
}
