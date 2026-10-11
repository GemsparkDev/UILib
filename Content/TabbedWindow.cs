using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace UILib.Content;

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
    public TabbedWindow(Vector2 _unitPosition, Texture2D _texture, Texture2D _tabTexture, Texture2D _selectedTabTexture, SoundEffect _selectSound, int? _tabs) : base(_unitPosition, _texture)
    {
        tabTexture = _tabTexture;
        selectedTabTexture = _selectedTabTexture;
        selectSound = _selectSound;
        totalTabs = _tabs?? 0;
        RecalculateTabs();
    }
    public override Vector2 WidgetOrigin(Widget _widget)
    {
        return Position - Origin + Size / 2;
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
        Vector2 mp = UIManager.NewPosition + Center;
        Vector2 hs = Size / 2 * UIManager.UIScale;
        return Position.X - hs.X <= mp.X && mp.X <= Position.X + hs.X 
             && Position.Y - hs.Y - UIManager.DimsOf(tabTexture).Y * UIManager.UIScale <= mp.Y && mp.Y <= Position.Y + hs.Y;
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
        Vector2 mp = UIManager.NewPosition - Position + Center;
        float bestDistance = float.MaxValue;
        float currentDistance;
        FunctionalWidget bestWidget = null;
        if (UIManager.NewInput[0])
        {
            for (int i = 0; i < totalTabs; i++)
            {
                Vector2 offset = tabList[i].Size / 2 * UIManager.UIScale;
                if (tabList[i].Offset.X - offset.X <= mp.X && mp.X <= tabList[i].Offset.X + offset.X
                 && tabList[i].Offset.Y - offset.Y <= mp.Y && mp.Y <= tabList[i].Offset.Y + offset.Y)
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
                    return null;
                }
            }
        }
        var funcMatches = from val in functionalChildren where val.tab == currentTab select val.widget;
        foreach (var functionalWidget in funcMatches)
        {
            var widget = functionalWidget;
            Vector2 halfSize = widget.Size / 2 * UIManager.UIScale;
            if (widget.Offset.X - halfSize.X <= mp.X && mp.X <= widget.Offset.X + halfSize.X &&
                widget.Offset.Y - halfSize.Y <= mp.Y && mp.Y <= widget.Offset.Y + halfSize.Y)
            {
                currentDistance = Vector2.DistanceSquared(widget.Size/2 + widget.Offset, mp);
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
                tabList[i]=new Decal(tabOffset, selectedTabTexture);
            }
            tabOffset.X += UIManager.DimsOf(tabTexture).X + 4;
        }
    }
    public override void Draw(SpriteBatch _spriteBatch)
    {
        for (int i = 0; i < totalTabs; i++)
        {
            if (tabList[i].Texture != null)
            {
                tabList[i].Draw(_spriteBatch, Position, Transparency, Center);
            }
            if(i < icons.Count)
            {
                if (icons[i] != null)
                {
                    Vector2 placementPosition = Position + tabList[i].Offset; 
                    if(i == currentTab)
                    {
                        placementPosition += new Vector2(-icons[i].Width / 2 * UIManager.UIScale, -icons[i].Height / 4 * UIManager.UIScale);
                    }
                    else
                    {
                        placementPosition += new Vector2(-icons[i].Width / 2 * UIManager.UIScale, icons[i].Height / 4 * UIManager.UIScale);
                    }
                    _spriteBatch.Draw(icons[i], placementPosition - Center, null, Color.White * Transparency, 0, Vector2.Zero, UIManager.UIScale, SpriteEffects.None, 0.4f);
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
                    widget.Draw(_spriteBatch, Position, Transparency, Center);
                }
            }
        }
        if (children.Count > 0)
        {
            var matches = from val in children where val.tab == currentTab select val.widget;
            foreach (var widget in matches)
            {
                widget.Draw(_spriteBatch, Position, Transparency, Center);
            }
        }
        GetWidgetOver()?.HoveringDraw(_spriteBatch, Position, Transparency, Center);
    }
}
