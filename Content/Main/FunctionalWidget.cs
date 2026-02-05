using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace UILib.Content.Main;

public abstract class FunctionalWidget : Widget
{
    public abstract void Interact(Vector2 parentPosition);
    public abstract void ContinuousInteract(Vector2 parentPosition);
    public abstract void AddBehaviour(Action func);
    public abstract void ApplyBehaviours();
}
