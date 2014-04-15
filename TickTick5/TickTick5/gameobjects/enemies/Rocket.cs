using Microsoft.Xna.Framework;

class Rocket : AnimatedGameObject
{
    protected double spawnTime;
    protected Vector2 startPosition;

    public Rocket(bool moveToLeft, Vector2 startPosition)
    {
        this.LoadAnimation("Sprites/Rocket/spr_rocket@3", "default", true, 0.2f);
        this.PlayAnimation("default");
        this.Mirror = moveToLeft;
        this.startPosition = startPosition;
        Reset();
    }

    public override void Reset()
    {
        this.Visible = false;
        this.position = startPosition;
        this.velocity = Vector2.Zero;
        this.spawnTime = GameEnvironment.Random.NextDouble() * 5;
    }

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
        // check if we are outside the screen
        Rectangle screenBox = new Rectangle(0, 0, GameEnvironment.Screen.X, GameEnvironment.Screen.Y);
        if (!screenBox.Intersects(this.BoundingBox))
            this.Reset();
    }

    public void CheckPlayerCollision()
    {
        Player player = GameWorld.Find("player") as Player;
        if (this.CollidesWith(player) && this.Visible)
            player.Die(false);
    }
}
