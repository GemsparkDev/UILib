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
    public RenderContainer(Vector2 unitPosition, Texture2D texture, UIManager _manager) : base(unitPosition, texture, _manager)
    {
    }

    public override void AddWidget(Widget widget, int tab = 0)
    {
        throw new NotImplementedException();
    }

    public override void AddWidget(FunctionalWidget widget, int tab = 0)
    {
        throw new NotImplementedException();
    }

    public override FunctionalWidget GetWidgetOver()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    public override Vector2 WidgetOrigin(Widget _widget)
    {
        throw new NotImplementedException();
    }
}
