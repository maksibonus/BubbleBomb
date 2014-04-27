using Microsoft.Xna.Framework;

/// <summary>
/// Клас ракети.
/// </summary>
class Rocket : AnimatedGameObject
{
    #region Поля класу

    /// <summary>
    /// Час перебування за межами екрану.
    /// </summary>
    protected double spawnTime;

    /// <summary>
    /// Стартова позиція.
    /// </summary>
    protected Vector2 startPosition;

    #endregion Поля класу

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public Rocket(bool moveToLeft, Vector2 startPosition)
    {
        this.LoadAnimation("Sprites/Rocket/spr_rocket@3", "default", true, 0.2f);
        this.PlayAnimation("default");
        this.Mirror = moveToLeft;
        this.startPosition = startPosition;
        Reset();
    }

    #endregion Конструктори

    #region Методи

    /// <summary>
    /// Оновлює стан об'єкту.
    /// </summary>
    /// <param name="gameTime">Час, який минув від попереднього до поточного стану гри.</param>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (spawnTime > 0)
        {
            spawnTime -= gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }
        this.Visible = true;
        this.velocity.X = 600;
        if (Mirror)
            this.velocity.X *= -1f;
        CheckPlayerCollision();
        // перевірка, якщо ракета за межами екрану
        Rectangle screenBox = new Rectangle(0, 0, GameEnvironment.Screen.X, GameEnvironment.Screen.Y);
        if (!screenBox.Intersects(this.BoundingBox))
            this.Reset();
    }

    /// <summary>
    /// Повертає стан об'єкту до початкового.
    /// </summary>
    public override void Reset()
    {
        this.Visible = false;
        this.position = startPosition;
        this.velocity = Vector2.Zero;
        this.spawnTime = GameEnvironment.Random.NextDouble() * 5;
    }

    /// <summary>
    /// Перевіряє, чи є колізії з головним персонажем.
    /// </summary>
    public void CheckPlayerCollision()
    {
        Player player = GameWorld.Find("player") as Player;
        if (this.CollidesWith(player) && this.Visible)
            player.Die(false);
    }

    #endregion Методи
}