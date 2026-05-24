using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UILib.Content;

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
            return Texture != null ? UIManager.DimsOf(Texture) : Vector2.Zero;
        }
    }
}
