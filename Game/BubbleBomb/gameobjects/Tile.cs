using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Перерахування типів тайлів.
/// </summary>
enum TileType
{
    /// <summary>
    /// Задній фон гри.
    /// </summary>
    Background,

    /// <summary>
    /// Звичайна платформа.
    /// </summary>
    Normal,

    /// <summary>
    /// Прозора платформа.
    /// </summary>
    Platform
}

/// <summary>
/// Клас, що представляє собою елемент ігрового поля.
/// </summary>
class Tile : SpriteGameObject
{
    #region Поля класу

    /// <summary>
    /// Тип тайлу.
    /// </summary>
    protected TileType type;

    /// <summary>
    /// Прапорець, що вказує, чи тайл гарячий.
    /// </summary>
    protected bool hot;

    /// <summary>
    /// Прапорець, що вказує, чи тайл холодний.
    /// </summary>
    protected bool ice;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        this.Layer = 100;
        if (type == TileType.Background)
            return;
        base.Draw(gameTime, spriteBatch);
    }
    
    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public Tile(string assetname = "", TileType type = TileType.Background, int layer = 0, string id = "")
        : base(assetname, layer, id)
    {
        this.type = type;
        hot = false;
        ice = false;
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає тип тайлу.
    /// </summary>
    public TileType TileType
    {
        get { return type; }
    }

    /// <summary>
    /// Вказує, чи тайл гарячий.
    /// </summary>
    public bool Hot
    {
        get { return hot; }
        set { hot = value; }
    }

    /// <summary>
    /// Вказує, чи тайл холодний.
    /// </summary>
    public bool Ice
    {
        get { return ice; }
        set { ice = value; }
    }

    #endregion Властивості
}