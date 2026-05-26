using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace UILib.Content;

public class ItemSlot<T> : FunctionalWidget where T : class, IData
{
    public T daughterItem;
    private UIManager UIManager;
    private List<Action> behaviours = [];
    public readonly int id;
    public ItemSlot(Vector2 _offset, Texture2D _texture, UIManager _UIManager, int _id)
    {
        Texture = _texture;
        offset = _offset;
        daughterItem = default;
        UIManager = _UIManager;
        id = _id;
    }
    public ItemSlot(Vector2 _offset, Texture2D _texture, T _daughterItem, UIManager _UImanager, int _id)
    {
        Texture = _texture;
        offset = _offset;
        daughterItem = _daughterItem;
        UIManager = _UImanager;
        id = _id;
    }
    public override void Interact(Vector2 parentPosition)
    {
        if (UIManager.selectedIcon == null)
        {
            (daughterItem, UIManager.selectedIcon) = (null, daughterItem);
        }
        else if(id == UIManager.selectedIcon.ID || id == -1)
        {
            if (UIManager.selectedIcon as T == null)
            {
                return;
            }
            (daughterItem, UIManager.selectedIcon) = ((T)UIManager.selectedIcon, daughterItem);
        }
        for (int i = 0; i < behaviours.Count; i++)
        {
            ApplyBehaviours();
        }
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
        if (daughterItem == null)
        {
            return;
        }
        _spriteBatch.Draw(daughterItem.Texture, _parentPosition + Offset - _center, null, daughterItem.Color, 0, daughterItem.Size / 2, UIManager.UIScale, SpriteEffects.None, 0);
    }
    public override void HoveringDraw(SpriteBatch _spriteBatch) 
    {
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
