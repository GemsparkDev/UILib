using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UILib.Content;

public class DummyWindow : Container
{
    public DummyWindow()
    {
        position = Vector2.Zero;
        enabled = false;
        transparency = 1;
    }
    public override bool GetMouseOver() { return false; }
    public override FunctionalWidget GetWidgetOver()
    {
        return new DummyWidget();
    }
    public override void Draw(SpriteBatch _spriteBatch) { }
    public override void AddWidget(Widget widget, int tab) { }
    public override void AddWidget(FunctionalWidget widget, int tab) { }
    public override void Update() { }
    public override Vector2 WidgetOrigin(Widget _widget) { return Vector2.One; }
}
