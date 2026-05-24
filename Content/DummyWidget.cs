using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace UILib.Content;

public class DummyWidget : FunctionalWidget
{
    public DummyWidget()
    {
        offset = Vector2.Zero;
        Texture = null;
    }
    public override void Interact(Vector2 parentPosition) { }
    public override void ContinuousInteract(Vector2 parentPosition) { }
    public override void AddBehaviour(Action func) { }
    public override void ApplyBehaviours() { }
    public override void Draw(SpriteBatch _spriteBatch, Vector2 _parentPositon, float _transparency, Vector2 _center) { }
    public override void HoveringDraw(SpriteBatch _spriteBatch) { }
}
