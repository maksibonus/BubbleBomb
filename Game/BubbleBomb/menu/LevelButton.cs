using Microsoft.Xna.Framework;

/// <summary>
/// Клас, що представляє собою кнопку рівня.
/// </summary>
class LevelButton : GameObjectList
{
    #region Поля класу

    /// <summary>
    /// Текст.
    /// </summary>
    protected TextGameObject text;

    /// <summary>
    /// Зображення, коли рівень пройдений.
    /// </summary>
    protected SpriteGameObject levels_solved;

    /// <summary>
    /// Зображення, коли рівень не пройдений.
    /// </summary>
    protected SpriteGameObject levels_unsolved;

    /// <summary>
    /// Зображення, коли рівень замкнутий.
    /// </summary>
    protected SpriteGameObject spr_lock;

    /// <summary>
    /// Прапорець, що вказує, чи натиснута клавіша.
    /// </summary>
    protected bool pressed;

    /// <summary>
    /// Номер рівня.
    /// </summary>
    protected int levelIndex;

    /// <summary>
    /// Рівень.
    /// </summary>
    protected Level level;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        spr_lock.Visible = level.Locked;
        levels_solved.Visible = level.Solved;
        levels_unsolved.Visible = !level.Solved;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        pressed = inputHelper.MouseLeftButtonPressed() && !level.Locked &&
            levels_solved.BoundingBox.Contains((int)inputHelper.MousePosition.X, (int)inputHelper.MousePosition.Y);
    }
    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
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

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає номер рівня.
    /// </summary>
    public int LevelIndex
    {
        get { return levelIndex; }
    }
   
    /// <summary>
    /// Вказує, чи натиснута клавіша.
    /// </summary>
    public bool Pressed
    {
        get { return pressed; }
    }

    /// <summary>
    /// Повертає ширину зображення.
    /// </summary>
    public int Width
    {
        get { return spr_lock.Width; }
    }

    /// <summary>
    /// Повертає висоту зображення.
    /// </summary>
    public int Height
    {
        get { return spr_lock.Height; }
    }

    #endregion Властивості
}