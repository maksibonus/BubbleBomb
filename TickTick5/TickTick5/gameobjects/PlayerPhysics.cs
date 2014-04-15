using System;
using Microsoft.Xna.Framework;

partial class Player :  AnimatedGameObject
{
    public void Jump(float speed = 1100)
    {
        velocity.Y = -speed;
        GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_jump");
    }
    
    private void DoPhysics()
    {
        if (!exploded)
            velocity.Y += 55;
        if (isAlive)
            HandleCollisions();
    }

    private void HandleCollisions()
    {
        isOnTheGround = false;
        walkingOnIce = false;
        walkingOnHot = false;

        TileField tiles = GameWorld.Find("tiles") as TileField;
        int x_floor = (int)position.X / tiles.CellWidth;
        int y_floor = (int)position.Y / tiles.CellHeight;

        for (int y = y_floor - 2; y <= y_floor + 1; ++y)
            for (int x = x_floor - 1; x <= x_floor + 1; ++x)
            {
                TileType tileType = tiles.GetTileType(x, y);
                if (tileType == TileType.Background)
                    continue;
                Tile currentTile = tiles.Get(x, y) as Tile;
                Rectangle tileBounds = new Rectangle(x * tiles.CellWidth, y * tiles.CellHeight,
                                                        tiles.CellWidth, tiles.CellHeight);
                Rectangle boundingBox = this.BoundingBox;
                boundingBox.Height += 1;
                if (((currentTile != null && !currentTile.CollidesWith(this)) || currentTile == null) && !tileBounds.Intersects(boundingBox))
                    continue;
                Vector2 depth = Collision.CalculateIntersectionDepth(boundingBox, tileBounds);
                if (Math.Abs(depth.X) < Math.Abs(depth.Y))
                {
                    if (tileType == TileType.Normal)
                        position.X += depth.X;
                    continue;
                }
                if (previousYPosition <= tileBounds.Top && tileType != TileType.Background)
                {
                    isOnTheGround = true;
                    velocity.Y = 0;
                    if (currentTile != null)
                    {
                        walkingOnIce = walkingOnIce || currentTile.Ice;
                        walkingOnHot = walkingOnHot || currentTile.Hot;
                    }
                }
                if (tileType == TileType.Normal || isOnTheGround)
                    position.Y += depth.Y + 1;             
            }
        position = new Vector2((float)Math.Floor(position.X), (float)Math.Floor(position.Y));
        previousYPosition = position.Y;
    }
}