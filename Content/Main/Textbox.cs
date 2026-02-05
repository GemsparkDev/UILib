using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;


namespace UILib.Content.Main;
public class Textbox : FunctionalWidget
{
    private List<Action> behaviours = [];
    private SpriteFont textFont;
    public Color textColor = Color.White;
    private bool isActive;
    Keys[] prevState = [];
    public Textbox(Vector2 _offset, Texture2D _texture, SpriteFont _textFont)
    {
        Size = UIManager.DimsOf(_texture);
        offset = _offset;
        texture = _texture;
        textFont = _textFont;
        text = "";
    }
    public override void Interact(Vector2 parentPosition)
    {
        ApplyBehaviours();
        isActive = !isActive;
    }
    public override void ContinuousInteract(Vector2 parentPosition) { }
    public override void Update() 
    {
        if (!isActive)
        {
            return;
        }
        Keys[] input = Keyboard.GetState().GetPressedKeys();
        bool caps = input.Contains(Keys.LeftShift) || input.Contains(Keys.RightShift);
        var keys = new Dictionary<Keys, string>()
        {
            { Keys.Tab, "   " },
            { Keys.Space, " " },
            { Keys.OemPlus, "=" },
            { Keys.OemMinus, "-" },
            { Keys.OemSemicolon, ";" },
            { Keys.OemComma, "," },
            { Keys.OemPeriod, "." },
            { Keys.OemQuestion, "/"},
            { Keys.OemTilde, "`" },
            { Keys.OemOpenBrackets, "[" },
            { Keys.OemPipe, "\\" },
            { Keys.OemCloseBrackets, "]" },
            { Keys.OemQuotes, "\'" },
            { Keys.OemBackslash, "\\" },
            { Keys.D0, "0" },
            { Keys.D1, "1" },
            { Keys.D2, "2" },
            { Keys.D3, "3" },
            { Keys.D4, "4" },
            { Keys.D5, "5" },
            { Keys.D6, "6" },
            { Keys.D7, "7" },
            { Keys.D8, "8" },
            { Keys.D9, "9" },
        };
        var specialUppercase = new Dictionary<Keys, string>()
        {
            { Keys.Tab, "   " },
            { Keys.Space, " " },
            { Keys.OemPlus, "+" },
            { Keys.OemMinus, "_" },
            { Keys.OemSemicolon, ":" },
            { Keys.OemComma, "<" },
            { Keys.OemPeriod, ">" },
            { Keys.OemQuestion, "?"},
            { Keys.OemTilde, "~" },
            { Keys.OemOpenBrackets, "{" },
            { Keys.OemPipe, "|" },
            { Keys.OemCloseBrackets, "}" },
            { Keys.OemQuotes, "\"" },
            { Keys.OemBackslash, "|" },
            { Keys.D0, ")" },
            { Keys.D1, "!" },
            { Keys.D2, "@" },
            { Keys.D3, "#" },
            { Keys.D4, "$" },
            { Keys.D5, "%" },
            { Keys.D6, "^" },
            { Keys.D7, "&" },
            { Keys.D8, "*" },
            { Keys.D9, "(" },
        };
        foreach (var key in input)
        {
            if (prevState.Contains(key))
            {
                continue;
            }
            if (key == Keys.Back && text.Length > 0)
            {
                text = text[0..^1];
            }
            else if ((key is >= Keys.A and <= Keys.Z) && (text.Length < 20))
            {
                string c = key.ToString();
                text += caps ? c.ToUpper() : c.ToLower();
            }
            else if ((caps ? specialUppercase : keys).TryGetValue(key, out string c) && (text.Length < 20))
            {
                text += c;
            }
        }
        if (input.Length > 0)
        {
            ApplyBehaviours();
        }
        prevState = input;
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
        base.Draw(_spriteBatch, _parentPosition, _transparency, _center);
        if (text == "")
        {
            return;
        }
        Vector2 textMiddlePoint = textFont.MeasureString(text) / 2;
        Vector2 textPosition = (_parentPosition + Offset);
        float textSize = Size.X / (text.Length * 12);
        if (textSize > 1)
        {
            textSize = 1;
        }
        _spriteBatch.DrawString(textFont, text, textPosition - _center, textColor, 0, textMiddlePoint, textSize * UIManager.UIScale, SpriteEffects.None, 0.45f);
    }
    public override void HoveringDraw(SpriteBatch _spriteBatch) { }
}
