using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using RamGecXNAControls;

/// <summary>
/// Клас запуску гри.
/// </summary>
class BubbleBomb : GameEnvironment
{
    #region Поля класу

    /// <summary>
    /// Обробляє конфігурацію и управління графічними пристроями.
    /// </summary>
    static public BubbleBomb game;

    #endregion Поля класу

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу початковими значеннями за замовчуванням.
    /// </summary>
    public BubbleBomb()
    {
        Content.RootDirectory = "Content";
        this.IsMouseVisible = true;
    }

    #endregion Конструктори

    #region Методи

    /// <summary>
    /// Задає основні параметри гри.
    /// </summary>
    protected override void LoadContent()
    {
        base.LoadContent();

        screen = new Point(1440, 825);//создание точки
        this.SetFullScreen(false);
        gameStateManager.AddGameState("titleMenu", new TitleMenuState());
        gameStateManager.AddGameState("helpState", new HelpState());
        gameStateManager.AddGameState("playingState", new PlayingState(Content));
        gameStateManager.AddGameState("levelMenu", new LevelMenuState());
        gameStateManager.AddGameState("gameOverState", new GameOverState());
        gameStateManager.AddGameState("levelFinishedState", new LevelFinishedState());
        gameStateManager.SwitchTo("titleMenu");

        AssetManager.PlayMusic("Sounds/Legend Of Zelda - Zelda's lullaby (Original)");
    }

    /// <summary>
    /// Точка входу в гру.
    /// </summary>
    static void Main()
    {
        GameTests.TestManager.Initialize();
        game = new BubbleBomb();
        game.Run();
    }

    #endregion Методи
}