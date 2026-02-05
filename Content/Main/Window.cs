using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace UILib.Content.Main;

public class Window : Container
{
    private List<Widget> children = [];
    private List<FunctionalWidget> functionalChildren = [];
    public Window(Vector2 _position, Texture2D _texture, float _transparency = 1)
    {
        Size = UIManager.DimsOf(_texture);
        texture = _texture;
        position = _position;
        transparency = _transparency;
    }
    public override Vector2 WidgetOrigin(Widget _widget)
    {
        return position - Origin * 2 + Size;
    }
    public override FunctionalWidget GetWidgetOver()
    {
        Vector2 mousePosition = new Vector2(Mouse.GetState().X - position.X, Mouse.GetState().Y - position.Y) + Center;
        float bestDistance = float.MaxValue;
        float currentDistance;
        FunctionalWidget bestWidget = new DummyWidget();
        foreach (FunctionalWidget functionalWidget in functionalChildren)
        {
            Widget widget = functionalWidget as Widget ?? new DummyWidget();
            Vector2 halfSize = widget.Size / 2 * UIManager.UIScale;
            if (widget.Offset.X - halfSize.X <= mousePosition.X && mousePosition.X <= widget.Offset.X + halfSize.X && 
                widget.Offset.Y - halfSize.Y <= mousePosition.Y && mousePosition.Y <= widget.Offset.Y + halfSize.Y)
            {
                currentDistance = Vector2.DistanceSquared(widget.Size / 2 * UIManager.UIScale + widget.Offset, mousePosition);
                if (currentDistance < bestDistance)
                {
                    bestDistance = currentDistance;
                    bestWidget = functionalWidget;
                }
            }
        }
        return bestWidget;
    }
    public override void Update()
    {
        foreach (var child in children)
        {
            child.Update();
        }
        foreach (var child in functionalChildren)
        {
            (child as Widget).Update();
        }
    }
    public override bool GetMouseOver()
    {
        Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) + Center;
        Vector2 halfSize = Size / 2 * UIManager.UIScale;
        return (position.X - halfSize.X <= mousePosition.X && mousePosition.X <= position.X + halfSize.X && position.Y - halfSize.Y <= mousePosition.Y && mousePosition.Y <= position.Y + halfSize.Y);
    }
    public override void Draw(SpriteBatch _spriteBatch)
    {
        base.Draw(_spriteBatch);
        foreach (var widget in children)
        {
            widget.Draw(_spriteBatch, position, transparency, Center);
        }
        foreach (var functionalWidget in functionalChildren)
        {
            if (functionalWidget is Widget widget)
            {
                widget.Draw(_spriteBatch, position, transparency, Center);
            }
        }
        if (GetWidgetOver() != null)
        {
            (GetWidgetOver() as Widget).HoveringDraw(_spriteBatch);
        }
    }
    public override void AddWidget(Widget widget, int index = 0)
    {
        children.Add(widget);
    }
    public override void AddWidget(FunctionalWidget widget, int index = 0)
    {
        functionalChildren.Add(widget);
    }
}

