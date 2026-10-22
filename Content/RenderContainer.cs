using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UILib.Content;
public class RenderContainer : Container
{
    private GraphicsDevice device;
    private FunctionalWidget interfaceWidget;
    RenderTarget2D renderBefore, renderAfter;
    public RenderContainer(UIManager _manager, FunctionalWidget _interface, GraphicsDevice _device, RenderTarget2D _renderBefore, RenderTarget2D _renderAfter) 
        : base(new Vector2(-1, -1), null, _manager)
    {
        interfaceWidget = _interface;
        device = _device;
        renderBefore = _renderBefore;
        renderAfter = _renderAfter;

        interfaceWidget.RisingInteract += delegate(Vector2 _clickPosition) { GetWidgetOver(_clickPosition).RisingInteract(); };
    }
    private List<Widget> children = [];
    private List<FunctionalWidget> functionalChildren = [];
    public Vector2 MousePosition { get; set; }
    public override Vector2 WidgetOrigin(Widget _widget) => Position - Origin + Size / 2;
    public FunctionalWidget GetWidgetOver(Vector2 mp)
    {
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
    public override FunctionalWidget GetWidgetOver() => null;
    public override bool IsMouseOver() => false;
    public override void Draw(SpriteBatch _spriteBatch)
    {
        device.SetRenderTarget(renderBefore);
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
        device.SetRenderTarget(renderAfter);
    }
    public override void AddWidget(Widget widget, int index = 0) => children.Add(widget);
    public override void AddWidget(FunctionalWidget widget, int index = 0) => functionalChildren.Add(widget);
}
