using Microsoft.Xna.Framework;
using System;

class PatrollingEnemy : AnimatedGameObject
{
    protected float waitTime;

    public PatrollingEnemy()
    {
        waitTime = 0.0f;
        velocity.X = 120;
        this.LoadAnimation("Sprites/Flame/spr_flame@9", "default", true);
        this.PlayAnimation("default");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (waitTime > 0)
        {
            waitTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (waitTime <= 0.0f)
                TurnAround();
        }
        else
        {
            TileField tiles = GameWorld.Find("tiles") as TileField;
            float posX = this.BoundingBox.Left;
            if (!Mirror)
                posX = this.BoundingBox.Right;
            int tileX = (int)Math.Floor(posX / tiles.CellWidth);
            int tileY = (int)Math.Floor(position.Y / tiles.CellHeight);
            if (tiles.GetTileType(tileX, tileY - 1) == TileType.Normal ||
                tiles.GetTileType(tileX, tileY) == TileType.Background)
            {
                waitTime = 0.5f;
                velocity.X = 0.0f;
            }
        }
        this.CheckPlayerCollision();
    }

    public void CheckPlayerCollision()
    {
        Player player = GameWorld.Find("player") as Player;
        if (this.CollidesWith(player))
            player.Die(false);
    }

    public void TurnAround()
    {
        Mirror = !Mirror;
        this.velocity.X = 120;
        if (Mirror)
            this.velocity.X = -this.velocity.X;
    }
}
