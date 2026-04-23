using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;

namespace UILib.Content.Main;
public class UIManager
{
    public static bool LockMouseInput { get; private set; } = false;
    public static Vector2 BackBuffer { get; set; }

    private MouseState oldState;
    private readonly List<Container> containers = [];
    public Screen ScreenWindow { get; set; }
    public Container focusedContainer;
    private FunctionalWidget focusedInteract = null;
    private static float sfxVolume = 1;
    public static float SFXVolume { get { return sfxVolume; } set { sfxVolume = Math.Clamp(value, 0, 1); } }
    public IData selectedIcon;
    public static float UIScale { get; set; } = 2f;
    public void Update()
    {
        //Sets the focused container to be the first container that is enabled and that the mouse is over
        //If there are no available containers, sets to either the screen menu or a dummy window depending on if the screen window is enabled
        if(!LockMouseInput || (focusedContainer != null && focusedContainer.enabled == false))
        {
            focusedContainer = containers.Where(c => c.enabled && c.GetMouseOver()).FirstOrDefault() ?? ((ScreenWindow != null && ScreenWindow.enabled) ? ScreenWindow : new DummyWindow());
        }

        MouseState newState = Mouse.GetState();
        if (oldState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released)
        {
            FunctionalWidget widget = focusedContainer.GetWidgetOver();
            widget.Interact(focusedContainer.WidgetOrigin(widget as Widget));
        }
        if (newState.LeftButton == ButtonState.Pressed)
        {
            if(LockMouseInput)
            {
                focusedInteract.ContinuousInteract(focusedContainer.WidgetOrigin(focusedInteract as Widget));
            }
            else
            {
                focusedInteract = null;
                FunctionalWidget widget = focusedContainer.GetWidgetOver();
                widget.ContinuousInteract(focusedContainer.WidgetOrigin(widget as Widget));
                if (LockMouseInput = focusedContainer.GetMouseOver())
                {
                    focusedInteract = widget;
                }
            }
        }
        else if (oldState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released)
        {
            LockMouseInput = false;
        }
        if (selectedIcon != null)
        {
            LockMouseInput = true;
        }
        focusedContainer.Update();
        oldState = newState;
    }
    public IData MoveSelectedIcon()
    {
        if (selectedIcon != null)
        {
            IData icon = selectedIcon;
            selectedIcon = null;
            LockMouseInput = false;
            return icon;
        }
        return null;
    }
    public bool ToggleToMenu(Container _container)
    {
        foreach (Container container in containers)
        {
            if (container.enabled && container != _container)
            {
                container.enabled = false;
                return false;
            }
        }
        _container.enabled = !_container.enabled;
        return true;
    }
    public void DisableAll()
    {
        foreach (var container in containers)
        {
            container.enabled = false;
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
        if(ScreenWindow != null && ScreenWindow.enabled)
        {
            ScreenWindow.Draw(spriteBatch);
        }
        foreach (Container container in containers.Where(c => c.enabled))
        {
            container.Draw(spriteBatch);
        }
        if (selectedIcon != null)
        {
            spriteBatch.Draw(selectedIcon.Texture, new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - DimsOf(selectedIcon.Texture)/2, null, selectedIcon.Color, 0, Vector2.One / 2, UIScale, SpriteEffects.None, 0.35f);
        }
    }
}
