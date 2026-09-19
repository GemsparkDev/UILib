using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Linq;

namespace UILib.Content;

public class ItemSlot<T> : FunctionalWidget where T : class, IData
{
    public T daughterItem = default;
    private List<Action> behaviours = [];
    public readonly List<int> ids;
    public ItemSlot(Vector2 _offset, Texture2D _texture, int _id)
    {
        Texture = _texture;
        offset = _offset;
        if(_id != -1)
        {
            ids = [_id];
        }
    }
    public ItemSlot(Vector2 _offset, Texture2D _texture, List<int> _ids)
    {
        Texture = _texture;
        offset = _offset;
        ids = _ids;
    }
    public override void Interact(Vector2 parentPosition)
    {
        if (UIManager.Self.selectedIcon == null)
        {
            (daughterItem, UIManager.Self.selectedIcon) = (null, daughterItem);
        }
        else if(ids == null || ids.Contains(UIManager.Self.selectedIcon.ID))
        {
            if (UIManager.Self.selectedIcon as T == null)
            {
                return;
            }
            (daughterItem, UIManager.Self.selectedIcon) = ((T)UIManager.Self.selectedIcon, daughterItem);
        }
        ApplyBehaviours();
    }
    public override void ContinuousInteract(Vector2 parentPosition) { }
    public override void AddBehaviour(Action func)
    {
        behaviours.Add(func);
    }
    public override void ApplyBehaviours()
    {
        for (int i = 0; i < behaviours.Count; i++)
        {
            behaviours[i]();
        }
    }
    public override void Draw(SpriteBatch _spriteBatch, Vector2 _parentPosition, float _transparency, Vector2 _center)
    {
        base.Draw(_spriteBatch, _parentPosition, _transparency, _center);
        if (daughterItem == null || daughterItem.Texture == null)
        {
            return;
        }
        _spriteBatch.Draw(daughterItem.Texture, _parentPosition + Offset - _center, null, daughterItem.Color, 0, daughterItem.Size / 2, UIManager.UIScale, SpriteEffects.None, 0);
    }
    public override void HoveringDraw(SpriteBatch _spriteBatch, Vector2 _parentPosition, float _transparency, Vector2 _center) 
    {
        base.HoveringDraw(_spriteBatch, _parentPosition, _transparency, _center);
        if (daughterItem == null || daughterItem.Tooltip == null)
        {
            return;
        }
        MouseState newState = Mouse.GetState();
        Texture2D tex = daughterItem.Tooltip.texture;
        daughterItem.Tooltip.position = new Vector2(newState.Position.X, newState.Position.Y) + new Vector2(tex.Width, tex.Height)/2 * UIManager.UIScale;
        daughterItem.Tooltip.Draw(_spriteBatch);
    }
}
