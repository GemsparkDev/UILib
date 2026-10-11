using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace UILib.Content;

public class Window(Vector2 _unitPosition, Texture2D _texture, UIManager _manager) : Container(_unitPosition, _texture, _manager)
{
    private List<Widget> children = [];
    private List<FunctionalWidget> functionalChildren = [];
    public override Vector2 WidgetOrigin(Widget _widget)
    {
        return Position - Origin + Size / 2;
    }
    public override FunctionalWidget GetWidgetOver()
    {
        Vector2 mp = Manager.NewPosition - Position + Center;
        float bestDistance = float.MaxValue;
        float currentDistance;
        FunctionalWidget bestWidget = null;
        foreach (FunctionalWidget widget in functionalChildren)
        {
            Vector2 hs = widget.Size / 2 * UIManager.UIScale;
            if (widget.Offset.X - hs.X <= mp.X && mp.X <= widget.Offset.X + hs.X && 
                widget.Offset.Y - hs.Y <= mp.Y && mp.Y <= widget.Offset.Y + hs.Y)
            {
                currentDistance = Vector2.DistanceSquared(widget.Size / 2 * UIManager.UIScale + widget.Offset, mp);
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
        Vector2 mp = Manager.NewPosition + Center;
        Vector2 hs = Size / 2 * UIManager.UIScale;
        return Position.X - hs.X <= mp.X && mp.X <= Position.X + hs.X && Position.Y - hs.Y <= mp.Y && mp.Y <= Position.Y + hs.Y;
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

