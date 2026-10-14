using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UILib.Content;

public abstract class Container
{
    public Container(Vector2 unitPosition, Texture2D texture, UIManager _manager)
    {
        Manager = _manager;
        UnitPosition = unitPosition;
        Texture = texture;
        Manager.AddContainer(this);
    }
    public UIManager Manager { get; }
    public Vector2 UnitPosition { get; set; }
    public Vector2 Position { 
        get => (UnitPosition + Vector2.One) * UIManager.BackBuffer / 2; 
        set => UnitPosition = value * 2 / UIManager.BackBuffer - Vector2.One; } //Using unit position means UI elements scale properly when changing the screen width.
    public Texture2D Texture { get; set; }
    public bool IsEnabled { get; set; } = false;
    public float Transparency { get; set; } = 1;
    public virtual Vector2 Size => UIManager.DimsOf(Texture);
    public abstract void AddWidget(Widget widget, int tab = 0);
    public abstract void AddWidget(FunctionalWidget widget, int tab = 0);
    public Alignment Alignment { get; set; } = Alignment.Center;
    protected Vector2 Origin 
    { 
        get 
        {
            var origin = Vector2.Zero;
            if (Alignment is Alignment.Center or Alignment.Top or Alignment.Bottom)
            {
                origin.X = Size.X / 2;
            }
            else if (Alignment is Alignment.Right or Alignment.TopRight or Alignment.BottomRight)
            {
                origin.X = Size.X;
            }
            if (Alignment is Alignment.Center or Alignment.Left or Alignment.Right)
            {
                origin.Y = Size.Y / 2;
            }
            else if (Alignment is Alignment.BottomLeft or Alignment.Bottom or Alignment.BottomRight)
            {
                origin.Y = Size.Y;
            }
            return origin; 
        } 
    }
    public abstract Vector2 WidgetOrigin(Widget _widget);
    public abstract void Update();
    public Vector2 Center => (Origin - Size / 2) * UIManager.UIScale;
    public virtual bool GetMouseOver() => Position.X <= Manager.NewPosition.X && Manager.NewPosition.X <= Position.X + Size.X* UIManager.UIScale && Position.Y <= Manager.NewPosition.Y && Manager.NewPosition.Y <= Position.Y + Size.Y* UIManager.UIScale;
    public abstract FunctionalWidget GetWidgetOver();
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if(Texture == null)
        {
            return;
        }
        spriteBatch.Draw(Texture, Position, null, Color.White * Transparency, 0, Origin, UIManager.UIScale, SpriteEffects.None, 0.35f);
    }
}
public enum Alignment
{
    TopLeft = 0,
    Top = 1,
    TopRight = 2,
    Left = 3,
    Center = 4,
    Right = 5,
    BottomLeft = 6,
    Bottom = 7,
    BottomRight = 8,
}