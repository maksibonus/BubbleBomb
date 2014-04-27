using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Клас, що представляє собою об'єкт гри.
/// </summary>
public abstract class GameObject : IGameLoopObject
{
    #region Поля класу

    /// <summary>
    /// Об'єкт-батько.
    /// </summary>
    protected GameObject parent;

    /// <summary>
    /// Позиція об'єкту.
    /// </summary>
    protected Vector2 position;
    
     /// <summary>
    /// Швидкість об'єкту.
    /// </summary>
    protected Vector2 velocity;

    /// <summary>
    /// Рівень відображення.
    /// </summary>
    protected int layer;

    /// <summary>
    /// Програмне ім'я об'єкту.
    /// </summary>
    protected string id;

    /// <summary>
    /// Прапорець, що вказує, чи видимий об'єкт.
    /// </summary>
    protected bool visible;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public virtual void HandleInput(InputHelper inputHelper)
    {
    }

    public virtual void Update(GameTime gameTime)
    {
        position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
    }

    public virtual void Reset()
    {
        visible = true;
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public GameObject(int layer = 0, string id = "")
    {
        this.layer = layer;
        this.id = id;
        this.position = Vector2.Zero;
        this.velocity = Vector2.Zero; 
        this.visible = true;
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає чи задає позицію об'єкту.
    /// </summary>
    public virtual Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }

    /// <summary>
    /// Повертає чи задає швидкість об'єкту.
    /// </summary>
    public virtual Vector2 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    /// <summary>
    /// Повертає глобальну позицію об'єкту.
    /// </summary>
    public virtual Vector2 GlobalPosition
    {
        get
        {
            if (parent != null)
                return parent.GlobalPosition + this.Position;
            else
                return this.Position;
        }
    }

    /// <summary>
    /// Повертає головний об'єкт.
    /// </summary>
    public GameObject Root
    {
        get
        {
            if (parent != null)
                return parent.Root;
            else
                return this;
        }
    }

    /// <summary>
    /// Повертає список об'єктів.
    /// </summary>
    public GameObjectList GameWorld
    {
        get
        {
            return Root as GameObjectList;
        }
    }

    /// <summary>
    /// Повертає чи задає рівень відображення.
    /// </summary>
    public virtual int Layer
    {
        get { return layer; }
        set { layer = value; }
    }

    /// <summary>
    /// Повертає чи задає об'єкт-батько.
    /// </summary>
    public virtual GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    /// <summary>
    /// Повертає програмне ім'я об'єкту.
    /// </summary>
    public string ID
    {
        get { return id; }
    }

    /// <summary>
    /// Повертає чи задає видимість об'єкту.
    /// </summary>
    public bool Visible
    {
        get { return visible; }
        set { visible = value; }
    }

    /// <summary>
    /// Повертає обмежувальний прямокутник, в якому знаходиться об'єкт.
    /// </summary>
    public virtual Rectangle BoundingBox
    {
        get
        {
            return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, 0, 0);
        }
    }

    #endregion Властивості
}