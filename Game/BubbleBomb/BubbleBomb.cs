using Microsoft.Xna.Framework;
<<<<<<< HEAD:TickTick5/TickTick5/TickTick.cs
=======
using Microsoft.Xna.Framework.Media;
using RamGecXNAControls;
using System.Collections.Generic;
>>>>>>> 759e6163e580fb573d79fcbbade0aea5bbfaa3da:Game/BubbleBomb/BubbleBomb.cs

/// <summary>
/// Клас запуску гри.
/// </summary>
class BubbleBomb : GameEnvironment
{
<<<<<<< HEAD:TickTick5/TickTick5/TickTick.cs
    static public TickTick game;
    static void Main()
    {
        game = new TickTick();
        game.Run();
    }
=======
    #region Поля класу
>>>>>>> 759e6163e580fb573d79fcbbade0aea5bbfaa3da:Game/BubbleBomb/BubbleBomb.cs

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
        IsMouseVisible = true;
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
        SetFullScreen(false);
        gameStateManager.AddGameState("titleMenu", new TitleMenuState());
        gameStateManager.AddGameState("helpState", new HelpState());
        gameStateManager.AddGameState("playingState", new PlayingState(Content));
        gameStateManager.AddGameState("levelMenu", new LevelMenuState());
        gameStateManager.AddGameState("gameOverState", new GameOverState());
        gameStateManager.AddGameState("levelFinishedState", new LevelFinishedState());
        gameStateManager.SwitchTo("titleMenu");

        //AssetManager.PlayMusic("Sounds/Legend Of Zelda - Zelda's lullaby (Original)");
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