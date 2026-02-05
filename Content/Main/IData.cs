using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UILib.Content.Main;

public interface IData
{
    public Texture2D Texture { get; }
    public Window Tooltip { get; }
    public int ID { get; }
    public Color Color { get; }
    public Vector2 Size
    {
        get 
        { 
            if(Texture != null)
            {
                return UIManager.DimsOf(Texture);
            }
            return Vector2.Zero;
        }
    }
}
