using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace UILib.Content.Main;

public class Slider : FunctionalWidget
{
    private List<Action> behaviours = [];
    private Texture2D knob;
    public float[] Intervals { get; set; } = [0];
    private Vector2 sliderSize;
    public bool visualSlider;
    public Color[] Colors { get; set; }
    private Vector2 size;
    public override Vector2 Size => (size != default) ? size : base.Size;
    public Slider(Texture2D _line, Vector2 _offset, Vector2 _sliderSize, bool _visualSlider, Color[] _colors)
    {
        Texture = _line;
        knob = null;
        sliderSize = _sliderSize;
        size = _sliderSize;
        if (!_visualSlider)
        {
            size += new Vector2(8, 8);
        }
        offset = _offset;
        visualSlider = _visualSlider;
        Colors = _colors;
    }
    public Slider(Texture2D _line, Texture2D _knob, Vector2 _offset, Vector2 _sliderSize, bool _visualSlider, Color[] _colors)
    {
        Texture = _line;
        knob = _knob;
        sliderSize = _sliderSize;
        size = _sliderSize;
        if (!_visualSlider)
        {
            size += UIManager.DimsOf(knob) / 2;
        }
        offset = _offset;
        visualSlider = _visualSlider;
        Colors = _colors;
    }
    public void SetInterval(float _value, float _maxValue, int _index = 0)
    {
        if(_index >= Colors.Length)
        {
            return;
        }
        Intervals[_index] = Math.Clamp(_value/_maxValue, 0, 1);
    }
    public override void Interact(Vector2 parentPosition) { }
    public override void ContinuousInteract(Vector2 parentPosition)
    {
        if (visualSlider)
        {
            return;
        }
        SetInterval(Mouse.GetState().X - Offset.X - parentPosition.X + sliderSize.X * UIManager.UIScale / 2, sliderSize.X * UIManager.UIScale);
        ApplyBehaviours();
    }
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
        _spriteBatch.Draw(Texture, _parentPosition + Offset - _center, new Rectangle(0, 0, (int)(sliderSize.X), (int)(sliderSize.Y)),
    Colors[^1], 0, sliderSize / 2, UIManager.UIScale, 0, 0);
        for (int i = Intervals.Length - 1; i >= 0; i--)
        {
            //_spriteBatch.Draw(texture, _parentPositon + Offset, null, color * _transparency, 0, Size/2, UIManager.UIScale, 0, 0);
            _spriteBatch.Draw(Texture, _parentPosition + Offset - _center, new Rectangle(0, 0, (int)(sliderSize.X * Intervals[i]), (int)(sliderSize.Y)),
                Colors[i], 0, sliderSize / 2, UIManager.UIScale, 0, 0);
        }
        if (!(visualSlider || knob == null))
        {
            Vector2 knobPosition = _parentPosition + Offset - sliderSize / 2 * UIManager.UIScale + new Vector2((int)(sliderSize.X * Intervals[0]), (int)(sliderSize.Y / 2)) * UIManager.UIScale - _center;
            _spriteBatch.Draw(knob, knobPosition, null, Color.White, 0, UIManager.DimsOf(knob) / 2, UIManager.UIScale / 2, 0, 0);
        }
    }
    public override void HoveringDraw(SpriteBatch _spriteBatch) { }
}
