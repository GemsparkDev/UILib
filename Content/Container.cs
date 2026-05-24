using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace UILib.Content;

public abstract class Container
{
    public Vector2 position;
    //private Vector2 prevMousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
    public bool enabled = false;
    public Texture2D texture;
    public float transparency = 1;
    public Vector2 Size { get; protected set; } = Vector2.Zero;
    public abstract void AddWidget(Widget widget, int tab = 0);
    public abstract void AddWidget(FunctionalWidget widget, int tab = 0);
    public Alignment alignment = Alignment.Center;
    protected Vector2 Origin 
    { 
        get 
        {
            var origin = Vector2.Zero;
            if (alignment is Alignment.Center or Alignment.Top or Alignment.Bottom)
            {
                origin.X = Size.X / 2;
            }
            else if (alignment is Alignment.Right or Alignment.TopRight or Alignment.BottomRight)
            {
                origin.X = Size.X;
            }
            if (alignment is Alignment.Center or Alignment.Left or Alignment.Right)
            {
                origin.Y = Size.Y / 2;
            }
            else if (alignment is Alignment.BottomLeft or Alignment.Bottom or Alignment.BottomRight)
            {
                origin.Y = Size.Y;
            }
            return origin; 
        } 
    }
    public abstract Vector2 WidgetOrigin(Widget _widget);
    public abstract void Update();
    protected Vector2 Center => (Origin - Size / 2) * UIManager.UIScale;
    public virtual bool GetMouseOver()
    {
        Vector2 mousePosition = new(Mouse.GetState().X, Mouse.GetState().Y);
        return position.X <= mousePosition.X && mousePosition.X <= position.X + Size.X * UIManager.UIScale && position.Y <= mousePosition.Y && mousePosition.Y <= position.Y + Size.Y * UIManager.UIScale;
    }
    public abstract FunctionalWidget GetWidgetOver();
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if(texture == null)
        {
            return;
        }
        spriteBatch.Draw(texture, position, null, Color.White * transparency, 0, Origin, UIManager.UIScale, SpriteEffects.None, 0.35f);
    }
}
public enum Alignment
{
    Center = 0,
    Left = 1,
    TopLeft = 2,
    Top = 3,
    TopRight = 4,
    Right = 5,
    BottomRight = 6,
    Bottom = 7,
    BottomLeft = 8,
}