using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace UILib.Content;

public abstract class FunctionalWidget : Widget
{
    public abstract void Interact(Vector2 parentPosition);
    public virtual void ContinuousInteract(Vector2 parentPosition) 
    {
        offset = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y) / UIManager.UIScale + UIManager.Self.ScreenWindow.WidgetOrigin(this) / 2;
        Debug.WriteLine(offset);
    }
    public abstract void AddBehaviour(Action func);
    public abstract void ApplyBehaviours();
}
