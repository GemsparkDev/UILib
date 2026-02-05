using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace UILib.Content.Main;
public class Screen : Container
{
    private List<(Widget widget, Alignment alignment)> children = [];
    private List<(FunctionalWidget widget, Alignment alignment)> functionalChildren = [];
    public override void AddWidget(Widget _widget, int _alignment)
    {
        if (!children.Contains((_widget, (Alignment)_alignment)))
        {
            children.Add((_widget, (Alignment)_alignment));
        }
    }
    public override void AddWidget(FunctionalWidget _widget, int _alignment)
    {
        if (!functionalChildren.Contains((_widget, (Alignment)_alignment)))
        {
            functionalChildren.Add((_widget, (Alignment)_alignment));
        }
    }
    public override FunctionalWidget GetWidgetOver()
    {
        if (!enabled)
        {
            return new DummyWidget();
        }
        float bestDistance = float.MaxValue;
        float currentDistance;
        FunctionalWidget bestWidget = new DummyWidget();
        foreach (var functionalWidget in functionalChildren)
        {
            Widget widget = functionalWidget.widget as Widget ?? new DummyWidget();
            Vector2 offset = widget.Offset;
            Vector2 halfSize = widget.Size / 2 * UIManager.UIScale;
            Vector2 center = WidgetOrigin(functionalWidget.widget as Widget, functionalWidget.alignment);
            var mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - position + center;
            if (offset.X - halfSize.X <= mousePosition.X && mousePosition.X <= offset.X + halfSize.X &&
                offset.Y - halfSize.Y <= mousePosition.Y && mousePosition.Y <= offset.Y + halfSize.Y)
            {
                currentDistance = Vector2.DistanceSquared(halfSize + offset, mousePosition);
                if (currentDistance < bestDistance)
                {
                    bestDistance = currentDistance;
                    bestWidget = functionalWidget.widget;
                }
            }
        }
        return bestWidget;
    }
    public override bool GetMouseOver()
    {
        return GetWidgetOver() is not DummyWidget;
    }
    public override void Update()
    {
        foreach (var child in children)
        {
            child.widget.Update();
        }
        foreach (var child in functionalChildren)
        {
            (child.widget as Widget).Update();
        }
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!enabled)
        {
            return;
        }
        foreach (var widget in children)
        {
            widget.widget.Draw(spriteBatch, position, 1, WidgetOrigin(widget.widget, widget.alignment));
        }
        foreach (var widgetPair in functionalChildren)
        {
            var widget = widgetPair.widget as Widget;
            widget.Draw(spriteBatch, position, 1, WidgetOrigin(widget, widgetPair.alignment));
        }
    }
    public override Vector2 WidgetOrigin(Widget _widget)
    {
        Alignment alignment = Alignment.Center;
        foreach(var pair in children)
        {
            if(pair.widget == _widget)
            {
                alignment = pair.alignment;
            }
        }
        foreach (var pair in functionalChildren)
        {
            if (pair.widget == _widget)
            {
                alignment = pair.alignment;
            }
        }
        return WidgetOrigin(_widget, alignment);
    }
    private static Vector2 WidgetOrigin(Widget _widget, Alignment _alignment)
    {
        Vector2 size = _widget.Size;
        var origin = -size / 2 * UIManager.UIScale;
        if (_alignment is Alignment.Center or Alignment.Top or Alignment.Bottom)
        {
            origin.X = -UIManager.BackBuffer.X / 2;
        }
        else if (_alignment is Alignment.Right or Alignment.TopRight or Alignment.BottomRight)
        {
            origin.X = size.X / 2 * UIManager.UIScale - UIManager.BackBuffer.X;
        }
        if (_alignment is Alignment.Center or Alignment.Left or Alignment.Right)
        {
            origin.Y = -UIManager.BackBuffer.Y / 2;
        }
        else if (_alignment is Alignment.BottomLeft or Alignment.Bottom or Alignment.BottomRight)
        {
            origin.Y = size.Y / 2 * UIManager.UIScale - UIManager.BackBuffer.Y;
        }
        return origin;
    }

}
