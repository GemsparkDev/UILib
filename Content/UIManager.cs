using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;

namespace UILib.Content;
public class UIManager
{
    public static Func<Vector2> GetBackBuffer { get; set; }
    public static Vector2 BackBuffer => GetBackBuffer();
    //Always use this for UI mouse input.
    public Vector2 NewPosition { get; private set; } = Vector2.Zero;
    public Vector2 OldPosition { get; private set; } = Vector2.Zero;
    public bool[] NewInput { get; private set; } = [false, false];
    public bool[] OldInput { get; private set; } = [false, false];
    private readonly List<Container> containers = [];
    public Container focusedContainer;
    public FunctionalWidget FocusedWidget { get; private set; } = null;
    public bool IsOver => FocusedWidget != null;
    private static float sfxVolume = 1;
    public static float SFXVolume { get { return sfxVolume; } set { sfxVolume = Math.Clamp(value, 0, 1); } }
    public IData selectedIcon;
    public static float UIScale { get; set; } = 2f;
    public void Update(Vector2 _newPosition, bool[] _newInput)
    {
        OldPosition = NewPosition;
        OldInput = NewInput;
        NewPosition = _newPosition;
        NewInput = _newInput;
        //Sets the focused container to be the first container that is enabled and that the mouse is over
        //If there are no available containers, sets to either the screen menu or a dummy window depending on if the screen window is enabled
        if (FocusedWidget == null)
        {
            focusedContainer = containers.Where(c => c.IsEnabled && c.GetMouseOver()).FirstOrDefault();
            FocusedWidget = focusedContainer?.GetWidgetOver();
        }
        if (FocusedWidget != null)
        {
            Vector2 clickLocation = 2 * (_newPosition - focusedContainer.Position + focusedContainer.Center - FocusedWidget.Offset) / (FocusedWidget.Size * UIScale);
            if (OldInput[0] && !_newInput[0])
            {
                FocusedWidget.OnFallingInteract(clickLocation);
            }
            if (!OldInput[0] && _newInput[0])
            {
                FocusedWidget.OnRisingInteract(clickLocation);
            }
            if (NewInput[0])
            {
                FocusedWidget.OnContinuousInteract(clickLocation);
            }
        }
        foreach(var container in containers)
        {
            container.Update();
        }
        if (!_newInput[0])
        {
            FocusedWidget = null;
        }
    }
    public bool ToggleToMenu(Container _container)
    {
        foreach (Container container in containers)
        {
            if (container.IsEnabled && container != _container)
            {
                container.IsEnabled = false;
                return false;
            }
        }
        _container.IsEnabled = !_container.IsEnabled;
        return true;
    }
    public void DisableAll()
    {
        foreach (var container in containers)
        {
            container.IsEnabled = false;
        }
    }
    public void AddContainer(Container container)
    {
        containers.Add(container);
    }
    public static Vector2 DimsOf(Texture2D _texture)
    {
        if(_texture == null)
        {
            return Vector2.Zero;
        }
        return new Vector2(_texture.Width, _texture.Height);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Container container in containers.Where(c => c.IsEnabled))
        {
            container.Draw(spriteBatch);
        }
        if (selectedIcon != null && selectedIcon.Texture != null)
        {
            spriteBatch.Draw(selectedIcon.Texture, NewPosition, null, selectedIcon.Color, 0, DimsOf(selectedIcon.Texture) / 2, UIScale, SpriteEffects.None, 0.35f);
        }
    }
}
