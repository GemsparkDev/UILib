using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace UILib.Content;

public abstract class FunctionalWidget : Widget
{
    public event InteractHandler RisingInteract;
    public event InteractHandler FallingInteract;
    public event InteractHandler ContinuousInteract;
    public delegate void InteractHandler(object sender, Vector2 _clickPosition);
    public virtual void OnContinuousInteract(Vector2 _clickPosition)
    {
        ContinuousInteract?.Invoke(this, _clickPosition);
    }
    public virtual void OnRisingInteract(Vector2 _clickPosition)
    {
        RisingInteract?.Invoke(this, _clickPosition);
    }
    public virtual void OnFallingInteract(Vector2 _clickPosition)
    {
        FallingInteract?.Invoke(this, _clickPosition);
    }
    public override void HoveringDraw(SpriteBatch _spriteBatch, Vector2 _parentPosition, float _transparency, Vector2 _center)
    {
        /* Figure out outlines
        if (Texture != null)
        {
            _spriteBatch.Draw(Texture, _parentPosition + Offset - _center + new Vector2(0, UIManager.UIScale), null, Color.Black, 0, Size / 2, UIManager.UIScale, 0, 0);
            _spriteBatch.Draw(Texture, _parentPosition + Offset - _center + new Vector2(0, -UIManager.UIScale), null, Color.Black, 0, Size / 2, UIManager.UIScale, 0, 0);
            _spriteBatch.Draw(Texture, _parentPosition + Offset - _center + new Vector2(UIManager.UIScale, 0), null, Color.Black, 0, Size / 2, UIManager.UIScale, 0, 0);
            _spriteBatch.Draw(Texture, _parentPosition + Offset - _center + new Vector2(-UIManager.UIScale, 0), null, Color.Black, 0, Size / 2, UIManager.UIScale, 0, 0);
            _spriteBatch.Draw(Texture, _parentPosition + Offset - _center, null, color*_transparency, 0, Size / 2, UIManager.UIScale, 0, 0);
        }
        */
    }
}