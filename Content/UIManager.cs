using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;

namespace UILib.Content;
public class UIManager
{
    private static Func<Vector2> backBuffer;
    public static Vector2 BackBuffer => backBuffer();

    private MouseState oldState;
    private readonly List<Container> containers = [];
    public Screen ScreenWindow { get; set; }
    public Container focusedContainer;
    private FunctionalWidget focusedWidget = null;
    public bool IsOver => focusedWidget != null;
    private static float sfxVolume = 1;
    public static float SFXVolume { get { return sfxVolume; } set { sfxVolume = Math.Clamp(value, 0, 1); } }
    public IData selectedIcon;
    public static float UIScale { get; set; } = 2f;
    public static UIManager Self { get; private set; }
    public UIManager(Func<Vector2> _backBuffer)
    {
        Self = this;
        backBuffer = _backBuffer;
    }
    public void Update()
    {
        //Sets the focused container to be the first container that is enabled and that the mouse is over
        //If there are no available containers, sets to either the screen menu or a dummy window depending on if the screen window is enabled

        MouseState newState = Mouse.GetState();
        if(focusedWidget == null)
        {
            focusedContainer = containers.Where(c => c.IsEnabled && c.GetMouseOver()).FirstOrDefault() ?? (ScreenWindow != null && ScreenWindow.IsEnabled ? ScreenWindow : new DummyWindow());
            focusedWidget = focusedContainer.GetWidgetOver();
        }
        if(focusedWidget != null)
        {
            Vector2 clickLocation = 2 * (new Vector2(newState.X, newState.Y) - focusedContainer.position + focusedContainer.Center - focusedWidget.Offset) / (focusedWidget.Size * UIScale);
            if (oldState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released)
            {
                focusedWidget.OnFallingInteract(clickLocation);
            }
            if (oldState.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed)
            {
                focusedWidget.OnRisingInteract(clickLocation);
            }
            if (oldState.LeftButton == ButtonState.Pressed)
            {
                focusedWidget.OnContinuousInteract(clickLocation);
            }
        }
        focusedContainer.Update();
        oldState = newState;
        if(newState.LeftButton == ButtonState.Released)
        {
            focusedWidget = null;
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
        if (ScreenWindow != null && ScreenWindow.IsEnabled)
        {
            ScreenWindow.Draw(spriteBatch);
        }
        foreach (Container container in containers.Where(c => c.IsEnabled))
        {
            container.Draw(spriteBatch);
        }
        if (selectedIcon != null && selectedIcon.Texture != null)
        {
            spriteBatch.Draw(selectedIcon.Texture, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), null, selectedIcon.Color, 0, DimsOf(selectedIcon.Texture) / 2, UIScale, SpriteEffects.None, 0.35f);
        }
    }
}
