using System;
using Microsoft.Xna.Framework;

class UnpredictableEnemy : PatrollingEnemy
{
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (waitTime > 0 || GameEnvironment.Random.NextDouble() > 0.01)
            return;
        TurnAround();
        velocity.X = Math.Sign(velocity.X) * (float)GameEnvironment.Random.NextDouble() * 5.0f;       
    }
}

