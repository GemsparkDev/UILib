using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;

namespace UILib.Content.Main;

public class Button : FunctionalWidget
{
    private List<Action> behaviours = [];
    private SpriteFont textFont;
    public Color textColor = Color.White;
    private Texture2D flipTexture;
    public Window Tooltip { get; private set; }
    public Button(Vector2 _offset, Texture2D _texture)
    {
        Size = UIManager.DimsOf(_texture);
        offset = _offset;
        texture = _texture;
        text = null;
    }
    public Button(Vector2 _offset, Texture2D _texture, SpriteFont _textFont, string _text, Color _textColor, Texture2D _flipTexture = null)
    {
        Size = UIManager.DimsOf(_texture);
        offset = _offset;
        texture = _texture;
        textFont = _textFont;
        text = _text;
        textColor = _textColor;
        flipTexture = _flipTexture;
    }
    public Button(Vector2 _offset, string _text, Color _textColor)
    {
        Size = new Vector2(_text.Length * 4, 12);
        offset = _offset;
        texture = null;
        text = _text;
        textColor = _textColor;
    }
    public override void Interact(Vector2 parentPosition)
    {
        ApplyBehaviours();
        if (flipTexture != null)
        {
            (texture, flipTexture) = (flipTexture, texture);
        }
    }
    public void AddTooltip(Window _tooltip)
    {
        Tooltip ??= _tooltip;
    }
    public override void ContinuousInteract(Vector2 parentPosition) { }
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
        if (text != null)
        {
            Vector2 textMiddlePoint = textFont.MeasureString(text) / 2;
            Vector2 textPosition = (_parentPosition + Offset);
            float textSize = Size.X/(text.Length * 12);
            if(textSize > 1)
            {
                textSize = 1;
            }
            _spriteBatch.DrawString(textFont, text, textPosition - _center, textColor, 0, textMiddlePoint, textSize * UIManager.UIScale, SpriteEffects.None, 0.45f);
        }
    }
    public override void HoveringDraw(SpriteBatch _spriteBatch) 
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
