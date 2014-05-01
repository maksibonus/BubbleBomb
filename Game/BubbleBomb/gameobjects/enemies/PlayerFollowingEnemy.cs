using System;
using Microsoft.Xna.Framework;

/// <summary>
/// Клас полум'я, яке йде за головним персонажем.
/// </summary>
class PlayerFollowingEnemy : PatrollingEnemy
{
    #region Методи

    /// <summary>
    /// Оновлює стан об'єкту.
    /// </summary>
    /// <param name="gameTime">Час, який минув від попереднього до поточного стану гри.</param>
    public override void Update(GameTime gameTime)
    {
        GameObjectList gameWorld = Root as GameObjectList;
        Player player = gameWorld.Find("player") as Player;
        float direction = player.Position.X - position.X;
        if (Math.Sign(direction) != Math.Sign(velocity.X) && player.Velocity.X != 0.0f && velocity.X != 0.0f)
            TurnAround();
        base.Update(gameTime);
    }

    #endregion Методи
}