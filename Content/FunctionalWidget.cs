using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace UILib.Content;

public abstract class FunctionalWidget : Widget
{
    public abstract void Interact(Vector2 parentPosition);
    public virtual void ContinuousInteract(Vector2 clickPosition) 
    {
        //offset = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y) / UIManager.UIScale + UIManager.Self.ScreenWindow.WidgetOrigin(this) / 2;
        //Debug.WriteLine(offset);
    }
    public abstract void AddBehaviour(Action func);
    public abstract void ApplyBehaviours();
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
