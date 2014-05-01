using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Клас, що зв'язує спрайти з фізикою гри.
/// </summary>
public class SpriteGameObject : GameObject
{
    #region Поля класу

    /// <summary>
    /// Спрайт.
    /// </summary>
    protected SpriteSheet sprite;

    /// <summary>
    /// Задає центр спрайту.
    /// </summary>
    protected Vector2 origin;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        this.Layer = 100;
        if (!visible || sprite == null)
            return;
        sprite.Draw(spriteBatch, this.GlobalPosition, origin);
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public SpriteGameObject(string assetname, int layer = 0, string id = "", int sheetIndex = 0)
        : base(layer, id)
    {
        if (assetname != "")
            sprite = new SpriteSheet(assetname, sheetIndex);
        else
            sprite = null;
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає спрайт.
    /// </summary>
    public SpriteSheet Sprite
    {
        get { return sprite; }
    }

    /// <summary>
    /// Повертає центральну точку спрайту.
    /// </summary>
    public Vector2 Center
    {
        get { return new Vector2(Width, Height) / 2; }
    }

    /// <summary>
    /// Повертає ширину спрайту.
    /// </summary>
    public int Width
    {
        get
        {
            return sprite.Width;
        }
    }

    /// <summary>
    /// Повертає висоту спрайту.
    /// </summary>
    public int Height
    {
        get
        {
            return sprite.Height;
        }
    }

    /// <summary>
    /// Повертає чи задає зеркальне відображення спрайту.
    /// </summary>
    public bool Mirror
    {
        get { return sprite.Mirror; }
        set { sprite.Mirror = value; }
    }

    /// <summary>
    /// Повертає чи задає центр спрайту.
    /// </summary>
    public Vector2 Origin
    {
        get { return this.origin; }
        set { this.origin = value; }
    }

    /// <summary>
    /// Повертає прямокутник, в якому знаходиться спрайт
    /// </summary>
    public override Rectangle BoundingBox
    {
        get
        {
            int left = (int)(GlobalPosition.X - origin.X);
            int top = (int)(GlobalPosition.Y - origin.Y);
            return new Rectangle(left, top, Width, Height);
        }
    }

    #endregion Властивості

    #region Методи

    /// <summary>
    /// Вказує, чи перетинаються об'єкти
    /// </summary>
    /// <param name="obj">Об'єкт, який перевіряється.</param>
    /// <returns>Перетинаються чи не перетинаються.</returns>
    public bool CollidesWith(SpriteGameObject obj)
    {
        if (!this.Visible || !obj.Visible || !BoundingBox.Intersects(obj.BoundingBox))
            return false;
        Rectangle b = Collision.Intersection(BoundingBox, obj.BoundingBox);
        for (int x = 0; x < b.Width; x++)
            for (int y = 0; y < b.Height; y++)
            {
                int thisx = b.X - (int)(GlobalPosition.X - origin.X) + x;
                int thisy = b.Y - (int)(GlobalPosition.Y - origin.Y) + y;
                int objx = b.X - (int)(obj.GlobalPosition.X - obj.origin.X) + x;
                int objy = b.Y - (int)(obj.GlobalPosition.Y - obj.origin.Y) + y;
                if (sprite.GetPixelColor(thisx, thisy).A != 0
                    && obj.sprite.GetPixelColor(objx, objy).A != 0)
                    return true;
            }
        return false;
    }

    #endregion Методи
}