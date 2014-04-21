using Microsoft.Xna.Framework;

class LevelButton : GameObjectList
{
    protected TextGameObject text;
    protected SpriteGameObject levels_solved;
    protected bool pressed;
    protected int levelIndex;
    protected Level level;

    public LevelButton(int levelIndex, Level level, int layer = 0, string id = "")
        : base(layer, id)
    {
        this.levelIndex = levelIndex;
        this.level = level;

        levels_solved = new SpriteGameObject("Sprites/spr_level_solved", 0, "", levelIndex - 1);
        this.Add(levels_solved);

        text = new TextGameObject("Fonts/Hud", 1);
        text.Text = levelIndex.ToString();
        this.Add(text);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        pressed = inputHelper.MouseLeftButtonPressed() &&
            levels_solved.BoundingBox.Contains((int)inputHelper.MousePosition.X, (int)inputHelper.MousePosition.Y);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
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
        get { return levels_solved.Width; }
    }

    public int Height
    {
        get { return levels_solved.Height; }
    }
}
