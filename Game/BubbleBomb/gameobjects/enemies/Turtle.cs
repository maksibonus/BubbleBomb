using Microsoft.Xna.Framework;

/// <summary>
/// Клас черепахи.
/// </summary>
class Turtle : AnimatedGameObject
{
    #region Поля класу

    /// <summary>
    /// Час випускання шипів.
    /// </summary>
    protected float sneezeTime;

    /// <summary>
    /// Час байдикування.
    /// </summary>
    protected float idleTime;

    #endregion Поля класу

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу початковими значеннями за замовчуванням.
    /// </summary>
    public Turtle()
    {
        this.LoadAnimation("Sprites/Turtle/spr_sneeze@9", "sneeze", false);
        this.LoadAnimation("Sprites/Turtle/spr_idle", "idle", true);
        this.PlayAnimation("idle");
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
        if (sneezeTime > 0)
        {
            this.PlayAnimation("sneeze");
            sneezeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (sneezeTime <= 0.0f)
            {
                idleTime = 5.0f;
                sneezeTime = 0.0f;
            }
        }
        else if (idleTime > 0)
        {
            this.PlayAnimation("idle");
            idleTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (idleTime <= 0.0f)
            {
                idleTime = 0.0f;
                sneezeTime = 5.0f;
            }
        }

        CheckPlayerCollision();
    }

    /// <summary>
    /// Повертає стан об'єкту до початкового.
    /// </summary>
    public override void Reset()
    {
        sneezeTime = 0.0f;
        idleTime = 5.0f;
    }

    /// <summary>
    /// Перевіряє, чи є колізії з головним персонажем.
    /// </summary>
    public void CheckPlayerCollision()
    {
        Player player = GameWorld.Find("player") as Player;
        if (!this.CollidesWith(player))
            return;
        if (sneezeTime > 0)
            player.Die(false);
        else if (idleTime > 0 && player.Velocity.Y > 0)
            player.Jump(1500);
    }

    #endregion Методи
}