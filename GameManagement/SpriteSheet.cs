using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Клас, що обробляє зображення і спрайти.
/// </summary>
public class SpriteSheet
{
    #region Поля класу

    /// <summary>
    /// Зображення.
    /// </summary>
    protected Texture2D sprite;

    /// <summary>
    /// Номер спрайту в зображенні.
    /// </summary>
    protected int sheetIndex;

    /// <summary>
    /// Кількість стовпців в зображенні.
    /// </summary>
    protected int sheetColumns;

    /// <summary>
    /// Кількість рядків в зображенні.
    /// </summary>
    protected int sheetRows;

    /// <summary>
    /// Відбиття.
    /// </summary>
    protected bool mirror;

    #endregion Поля класу

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу початковими значеннями за замовчуванням.
    /// </summary>
    public SpriteSheet(string assetname, int sheetIndex = 0)
    {
        sprite = GameEnvironment.AssetManager.GetSprite(assetname);
        this.sheetIndex = sheetIndex;
        this.sheetColumns = 1;
        this.sheetRows = 1;

        //перевіряємо, чи зможемо отримати спрайти з assetname
        string[] assetSplit = assetname.Split('@');
        if (assetSplit.Length <= 1)
            return;

        string sheetNrData = assetSplit[assetSplit.Length - 1];
        string[] colrow = sheetNrData.Split('x');
        this.sheetColumns = int.Parse(colrow[0]);
        if (colrow.Length == 2)
            this.sheetRows = int.Parse(colrow[1]);
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає зображення.
    /// </summary>
    public Texture2D Sprite
    {
        get { return sprite; }
    }

    /// <summary>
    /// Повертає центр зображення.
    /// </summary>
    public Vector2 Center
    {
        get { return new Vector2(Width, Height) / 2; }
    }

    /// <summary>
    /// Повертає ширину спрайту в зображенні.
    /// </summary>
    public int Width
    {
        get
        {
            return sprite.Width / sheetColumns;
        }
    }
     
    /// <summary>
    /// Повертає висоту спрайту в зображенні.
    /// </summary>
    public int Height
    {
        get
        {
            return sprite.Height / sheetRows;
        }
    }

    /// <summary>
    /// Повертає чи задає відбиття.
    /// </summary>
    public bool Mirror
    {
        get { return mirror; }
        set { mirror = value; }
    }

    /// <summary>
    /// Повертає чи задає номер спрайту в зображенні.
    /// </summary>
    public int SheetIndex
    {
        get
        {
            return this.sheetIndex;
        }
        set
        {
            if (value < this.sheetColumns * this.sheetRows && value >= 0)
                this.sheetIndex = value;
        }
    }

    /// <summary>
    /// Повертає кількість спрайтів в зображенні.
    /// </summary>
    public int NumberSheetElements
    {
        get { return this.sheetColumns * this.sheetRows; }
    }

    #endregion Властивості

    #region Методи

    /// <summary>
    /// Відображає об'єкт на екрані.
    /// </summary>
    /// <param name="spriteBatch">Включає в себе групу спрайтів з однаковими параметрами.</param>
    /// <param name="position">Позиція спрайту.</param>
    /// <param name="origin">Позиція центру зображення.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin)
    {
        int columnIndex = sheetIndex % sheetColumns;
        int rowIndex = sheetIndex / sheetColumns % sheetRows;
        Rectangle spritePart = new Rectangle(columnIndex * this.Width, rowIndex * this.Height, this.Width, this.Height);
        SpriteEffects spriteEffects = SpriteEffects.None;
        if (mirror)
            spriteEffects = SpriteEffects.FlipHorizontally;
        spriteBatch.Draw(sprite, position, spritePart, Color.White,
            0.0f, origin, 1.0f, spriteEffects, 0.0f);
    }

    /// <summary>
    /// Отримати колір пікселю.
    /// </summary>
    /// <param name="x">Позиція по осі ОХ.</param>
    /// <param name="y">Позиція по осі ОY.</param>
    /// <returns>Колір</returns>
    public Color GetPixelColor(int x, int y)
    {
        int column_index = sheetIndex % sheetColumns;
        int row_index = sheetIndex / sheetColumns % sheetRows;
        Rectangle sourceRectangle = new Rectangle(column_index * this.Width + x, row_index * this.Height + y, 1, 1);
        Color[] retrievedColor = new Color[1];
        sprite.GetData<Color>(0, sourceRectangle, retrievedColor, 0, 1);
        return retrievedColor[0];
    }

    #endregion Методи
}