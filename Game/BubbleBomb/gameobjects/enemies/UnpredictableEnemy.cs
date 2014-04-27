using System;
using Microsoft.Xna.Framework;

/// <summary>
/// Клас полум'я, яке непередбачуване.
/// </summary>
class UnpredictableEnemy : PatrollingEnemy
{
    #region Методи

    /// <summary>
    /// Оновлює стан об'єкту.
    /// </summary>
    /// <param name="gameTime">Час, який минув від попереднього до поточного стану гри.</param>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (waitTime > 0 || GameEnvironment.Random.NextDouble() > 0.01)
            return;
        TurnAround();
        velocity.X = Math.Sign(velocity.X) * (float)GameEnvironment.Random.NextDouble() * 5.0f;
    }

    #endregion Методи
}