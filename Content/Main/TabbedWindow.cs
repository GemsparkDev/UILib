using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace UILib.Content.Main;

public class TabbedWindow : Container
{
    private List<(int tab, FunctionalWidget widget)> functionalChildren = [];
    private List<(int tab, Widget widget)> children = [];
    private List<Decal> tabList = [];
    private int currentTab = 0;
    public int CurrentTab { get { return currentTab; } set { if (value >= 0 && value < totalTabs) { currentTab = value; } } }
    private int prevTab = 0;
    private int totalTabs = 0;
    public List<Texture2D> icons = [];
    public Texture2D tabTexture;
    public Texture2D selectedTabTexture;
    public SoundEffect selectSound;
    public TabbedWindow(Vector2 _position, Texture2D _texture, Texture2D _tabTexture, Texture2D _selectedTabTexture, SoundEffect _selectSound, int? _tabs, float _transparency = 1)
    {
        Size = UIManager.DimsOf(_texture);
        texture = _texture;
        tabTexture = _tabTexture;
        selectedTabTexture = _selectedTabTexture;
        selectSound = _selectSound;
        position = _position;
        totalTabs = _tabs?? 0;
        transparency = _transparency;
        RecalculateTabs();
    }
    public override Vector2 WidgetOrigin(Widget _widget)
    {
        return position - Origin + Size / 2;
    }
    public override void AddWidget(Widget widget, int tab = 0)
    {
        children.Add((tab, widget));
        if (tab >= totalTabs)
        {
            totalTabs = tab + 1;
            RecalculateTabs();
        }
    }
    public override void AddWidget(FunctionalWidget widget, int tab = 0)
    {
        functionalChildren.Add((tab, widget));
        if (tab >= totalTabs)
        {
            totalTabs = tab + 1;
            RecalculateTabs();
        }
    }
    public FunctionalWidget GetFuncWidget(int index, int tab)
    {
        return (from val in functionalChildren where val.tab == tab select val.widget).ToList()[index];
    }
    public override bool GetMouseOver()
    {
        Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) + Center;
        Vector2 halfSize = Size / 2 * UIManager.UIScale;
        return (position.X - halfSize.X <= mousePosition.X && mousePosition.X <= position.X + halfSize.X 
             && position.Y - halfSize.Y - UIManager.DimsOf(tabTexture).Y * UIManager.UIScale <= mousePosition.Y && mousePosition.Y <= position.Y + halfSize.Y);
    }
    public override void Update()
    {
        foreach (var (tab, widget) in children)
        {
            if (tab != currentTab)
            {
                continue;
            }
            widget.Update();
        }
        foreach (var (tab, widget) in functionalChildren)
        {
            if (tab != currentTab)
            {
                continue;
            }
            widget.Update();
        }
    }
    public override FunctionalWidget GetWidgetOver()
    {
        var state = Mouse.GetState();
        Vector2 mousePosition = new Vector2(state.X, state.Y) - position + Center;
        float bestDistance = float.MaxValue;
        float currentDistance;
        FunctionalWidget bestWidget = new DummyWidget();
        if (state.LeftButton == ButtonState.Pressed)
        {
            for (int i = 0; i < totalTabs; i++)
            {
                Vector2 offset = tabList[i].Size / 2 * UIManager.UIScale;
                if (tabList[i].Offset.X - offset.X <= mousePosition.X && mousePosition.X <= tabList[i].Offset.X + offset.X
                 && tabList[i].Offset.Y - offset.Y <= mousePosition.Y && mousePosition.Y <= tabList[i].Offset.Y + offset.Y)
                {
                    prevTab = currentTab;
                    if (currentTab != i)
                    {
                        currentTab = i;
                        tabList[prevTab].Texture = tabTexture;
                        tabList[currentTab].Texture = selectedTabTexture;
                        var sound = selectSound.CreateInstance();
                        sound.Volume = UIManager.SFXVolume;
                        sound.Play();
                    }
                    return new DummyWidget();
                }
            }
        }
        var funcMatches = from val in functionalChildren where val.tab == currentTab select val.widget;
        foreach (var functionalWidget in funcMatches)
        {
            Widget widget = functionalWidget as Widget ?? new DummyWidget();
            Vector2 halfSize = widget.Size / 2 * UIManager.UIScale;
            if (widget.Offset.X - halfSize.X <= mousePosition.X && mousePosition.X <= widget.Offset.X + halfSize.X &&
                widget.Offset.Y - halfSize.Y <= mousePosition.Y && mousePosition.Y <= widget.Offset.Y + halfSize.Y)
            {
                currentDistance = Vector2.DistanceSquared(widget.Size/2 + widget.Offset, mousePosition);
                if (currentDistance < bestDistance)
                {
                    bestDistance = currentDistance;
                    bestWidget = functionalWidget;
                }
            }
        }
        return bestWidget;
    }
    private void RecalculateTabs()
    {
        Vector2 tabOffset = -Size / 2 + new Vector2(UIManager.DimsOf(tabTexture).X/2, -UIManager.DimsOf(tabTexture).Y / 2);
        for (int i = 0; i < totalTabs; i++)
        {
            if (i >= tabList.Count)
            {
                tabList.Add(new Decal(tabOffset, tabTexture));
            }
            else
            {
                tabList[i] = new Decal(tabOffset, tabTexture);
            }
            if (i == currentTab)
            {
                tabList[i]=(new Decal(tabOffset, selectedTabTexture));
            }
            tabOffset.X += (UIManager.DimsOf(tabTexture).X + 4);
        }
    }
    public override void Draw(SpriteBatch _spriteBatch)
    {
        for (int i = 0; i < totalTabs; i++)
        {
            if (tabList[i].Texture != null)
            {
                tabList[i].Draw(_spriteBatch, position, transparency, Center);
            }
            if(i < icons.Count)
            {
                if (icons[i] != null)
                {
                    Vector2 placementPosition; 
                    if(i == currentTab)
                    {
                        placementPosition = position + tabList[i].Offset + new Vector2(-icons[i].Width / 2 * UIManager.UIScale, -icons[i].Height / 4 * UIManager.UIScale);
                    }
                    else
                    {
                        placementPosition = position + tabList[i].Offset + new Vector2(-icons[i].Width / 2 * UIManager.UIScale, icons[i].Height / 4 * UIManager.UIScale);
                    }
                    _spriteBatch.Draw(icons[i], placementPosition - Center, null, Color.White * transparency, 0, Vector2.Zero, UIManager.UIScale, SpriteEffects.None, 0.4f);
                }
            }
        }
        base.Draw(_spriteBatch);

        if(functionalChildren.Count > 0)
        {
            var funcMatches = from val in functionalChildren where val.tab == currentTab select val.widget;
            foreach (var functionalWidget in funcMatches)
            {
                if (functionalWidget is Widget widget)
                {
                    widget.Draw(_spriteBatch, position, transparency, Center);
                }
            }
        }
        if (children.Count > 0)
        {
            var matches = from val in children where val.tab == currentTab select val.widget;
            foreach (var widget in matches)
            {
                widget.Draw(_spriteBatch, position, transparency, Center);
            }
        }
        if (GetWidgetOver() != null)
        {
            (GetWidgetOver() as Widget).HoveringDraw(_spriteBatch);
        }
    }
}
