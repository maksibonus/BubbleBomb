using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RamGecXNAControls;

/// <summary>
/// Клас, що представляє собою стан пройденого рівня.
/// </summary>
class LevelFinishedState : GameObjectList
{
    #region Поля класу

    /// <summary>
    /// Менеджер контролю усіх елементів.
    /// </summary>
    GUIManager guiManager;

    /// <summary>
    /// Вікно.
    /// </summary>
    GUIControl myControl;

    /// <summary>
    /// Елементи вікна.
    /// </summary>
    Label myAnotherControl;

    /// <summary>
    /// Інформація про ігровий стан.
    /// </summary>
    protected IGameLoopObject playingState;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void Update(GameTime gameTime)
    {
        playingState.Update(gameTime);
        foreach (var control in guiManager.Controls)
            control.Update(gameTime);
        myAnotherControl.Text = "Ваш результат: " + Result.result + " з " + Level.countWaterDrop[LevelMenuState.curLevel] + " балів.";
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (!inputHelper.KeyPressed(Keys.Space))
            return;
        Result.result = 0;
        (playingState as PlayingState).NextLevel();
        GameEnvironment.GameStateManager.SwitchTo("playingState");
        base.HandleInput(inputHelper);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        playingState.Draw(gameTime, spriteBatch);
        base.Draw(gameTime, spriteBatch);
        guiManager.Draw(spriteBatch);
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу початковими значеннями за замовчуванням.
    /// </summary>
    public LevelFinishedState()
    {
        guiManager = new GUIManager(BubbleBomb.game);
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        SpriteGameObject overlay = new SpriteGameObject("Overlays/spr_welldone");
        overlay.Position = new Vector2(GameEnvironment.Screen.X, GameEnvironment.Screen.Y) / 2 - overlay.Center;
        this.Add(overlay);

        //Створюємо вікно.
        myControl = new Window(new Rectangle(GameEnvironment.Screen.X / 2 - 175, GameEnvironment.Screen.Y / 2 + 150, GameEnvironment.Screen.X / 2 - 350, GameEnvironment.Screen.Y / 2 - 300), "Результат");
        guiManager.Controls.Add(myControl);

        //Результат рівня.
        myAnotherControl = new Label(new Rectangle(30, 30, 0, 0), "Ваш результат: " + Result.result + " з " + Level.countWaterDrop[LevelMenuState.curLevel] + " балів.", "Question");
        myControl.Controls.Add(myAnotherControl);
    }

    #endregion Конструктори
}