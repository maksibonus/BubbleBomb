using Microsoft.Xna.Framework;

class LevelButton : GameObjectList
{
    protected TextGameObject text;
    protected SpriteGameObject levels_solved, levels_unsolved, spr_lock;
    protected bool pressed;
    protected int levelIndex;
    protected Level level;

    public LevelButton(int levelIndex, Level level, int layer = 0, string id = "")
        : base(layer, id)
    {
        this.levelIndex = levelIndex;
        this.level = level;

        levels_solved = new SpriteGameObject("Sprites/spr_level_solved", 0, "", levelIndex - 1);
        levels_unsolved = new SpriteGameObject("Sprites/spr_level_unsolved");
        spr_lock = new SpriteGameObject("Sprites/spr_level_locked", 2);
        this.Add(levels_solved);
        this.Add(levels_unsolved);
        this.Add(spr_lock);

        text = new TextGameObject("Fonts/Hud", 1);
        text.Text = levelIndex.ToString();
        text.Position = new Vector2(spr_lock.Width - text.Size.X - 10, 5);
        this.Add(text);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        pressed = inputHelper.MouseLeftButtonPressed() && !level.Locked &&
            levels_solved.BoundingBox.Contains((int)inputHelper.MousePosition.X, (int)inputHelper.MousePosition.Y);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        spr_lock.Visible = level.Locked;
        levels_solved.Visible = level.Solved;
        levels_unsolved.Visible = !level.Solved;
    }

    public int LevelIndex
    {
        get { return levelIndex; }
    }

    public bool Pressed
    {
        get { return pressed; }
    }

    public int Width
    {
        get { return spr_lock.Width; }
    }

    public int Height
    {
        get { return spr_lock.Height; }
    }
}
