using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace UILib.Content;

public class Window(Vector2 _unitPosition, Texture2D _texture) : Container(_unitPosition, _texture)
{
    private List<Widget> children = [];
    private List<FunctionalWidget> functionalChildren = [];
    public override Vector2 WidgetOrigin(Widget _widget)
    {
        return Position - Origin + Size / 2;
    }
    public override FunctionalWidget GetWidgetOver()
    {
        Vector2 mousePosition = new Vector2(Mouse.GetState().X - Position.X, Mouse.GetState().Y - Position.Y) + Center;
        float bestDistance = float.MaxValue;
        float currentDistance;
        FunctionalWidget bestWidget = null;
        foreach (FunctionalWidget widget in functionalChildren)
        {
            Vector2 halfSize = widget.Size / 2 * UIManager.UIScale;
            if (widget.Offset.X - halfSize.X <= mousePosition.X && mousePosition.X <= widget.Offset.X + halfSize.X && 
                widget.Offset.Y - halfSize.Y <= mousePosition.Y && mousePosition.Y <= widget.Offset.Y + halfSize.Y)
            {
                currentDistance = Vector2.DistanceSquared(widget.Size / 2 * UIManager.UIScale + widget.Offset, mousePosition);
                if (currentDistance < bestDistance)
                {
                    bestDistance = currentDistance;
                    bestWidget = widget;
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
            child.Update();
        }
    }
    public override bool GetMouseOver()
    {
        Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) + Center;
        Vector2 halfSize = Size / 2 * UIManager.UIScale;
        return Position.X - halfSize.X <= mousePosition.X && mousePosition.X <= Position.X + halfSize.X && Position.Y - halfSize.Y <= mousePosition.Y && mousePosition.Y <= Position.Y + halfSize.Y;
    }
    public override void Draw(SpriteBatch _spriteBatch)
    {
        base.Draw(_spriteBatch);
        foreach (var widget in children)
        {
            widget.Draw(_spriteBatch, Position, Transparency, Center);
        }
        foreach (var functionalWidget in functionalChildren)
        {
            if (functionalWidget is Widget widget)
            {
                widget.Draw(_spriteBatch, Position, Transparency, Center);
            }
        }
        GetWidgetOver()?.HoveringDraw(_spriteBatch, Position, Transparency, Center);
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

