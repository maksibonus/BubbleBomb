using System.Collections.Generic;
using Microsoft.Xna.Framework;

/// <summary>
/// Клас спаркі.
/// </summary>
class Sparky : AnimatedGameObject
{
    #region Поля класу

    /// <summary>
    /// Час байдикування.
    /// </summary>
    protected float idleTime;

    /// <summary>
    /// Інтервал падіння.
    /// </summary>
    protected float yoffset;

    /// <summary>
    /// Початкова висота.
    /// </summary>
    protected float initialY;

    #endregion Поля класу

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public Sparky(float initialY)
    {
        this.LoadAnimation("Sprites/Sparky/spr_electrocute@6x5", "electrocute", false);
        this.LoadAnimation("Sprites/Sparky/spr_idle", "idle", true);
        this.PlayAnimation("idle");
        this.initialY = initialY;
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
        this.Layer = 100;
        if (idleTime <= 0)
        {
            this.PlayAnimation("electrocute");
            if (this.velocity.Y != 0)
            {
                // falling down
                yoffset -= this.velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (yoffset <= 0)
                    this.velocity.Y = 0;
                else if (yoffset >= 120.0f)
                    this.Reset();
            }
            else if (Current.AnimationEnded)
                this.velocity.Y = -60;
        }
        else
        {
            this.PlayAnimation("idle");
            idleTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (idleTime <= 0.0f)
                this.velocity.Y = 300;

        }

        CheckPlayerCollision();
    }

    /// <summary>
    /// Повертає стан об'єкту до початкового.
    /// </summary>
    public override void Reset()
    {
        idleTime = (float)GameEnvironment.Random.NextDouble() * 5;
        this.position.Y = initialY;
        yoffset = 120;
        velocity = Vector2.Zero;
    }

    /// <summary>
    /// Перевіряє, чи є колізії з головним персонажем.
    /// </summary>
    public void CheckPlayerCollision()
    {
        Player player = GameWorld.Find("player") as Player;
        if (this.CollidesWith(player) && idleTime <= 0.0f)
            player.Die(false);
    }

    #endregion Методи
}