using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

/// <summary>
/// Клас, що відповідає за відображення тексту
/// </summary>
public class TextGameObject : GameObject
{
    #region Поля класу

    /// <summary>
    /// Текстура шрифту.
    /// </summary>
    protected SpriteFont spriteFont;

    /// <summary>
    /// Колір тексту.
    /// </summary>
    protected Color color;

    /// <summary>
    /// Текст.
    /// </summary>
    protected string text;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        this.Layer = 100;
        if (visible)
            spriteBatch.DrawString(spriteFont, text, this.GlobalPosition, color);
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public TextGameObject(string assetname, int layer = 0, string id = "")
        : base(layer, id)
    {
        spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(assetname);
        color = Color.White;
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає чи задає колір тексту.
    /// </summary>
    public Color Color
    {
        get { return color; }
        set { color = value; }
    }

    /// <summary>
    /// Повертає чи задає текст.
    /// </summary>
    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    /// <summary>
    /// Повертає розмір тексту.
    /// </summary>
    public Vector2 Size
    {
        get
        {
            return spriteFont.MeasureString(text);
        }
    }

    #endregion Властивості
}

