using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Клас, що представляє собою стан перебування у меню вибору рівня.
/// </summary>
class LevelMenuState : GameObjectList
{
    #region Поля класу

    /// <summary>
    /// Кнопка переходу до головного меню.
    /// </summary>
    protected Button backButton;

    /// <summary>
    /// Поточний рівень.
    /// </summary>
    static public int curLevel;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);

        if (LevelSelected != -1)
        {
            PlayingState playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState;
            playingState.CurrentLevelIndex = LevelSelected - 1;
            curLevel = playingState.CurrentLevelIndex;
            GameEnvironment.AssetManager.StopMusic();
            GameEnvironment.AssetManager.PlayMusic("Sounds/Chipzel - Focus");
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        }
        else if (backButton.Pressed)
            GameEnvironment.GameStateManager.SwitchTo("titleMenu");
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу початковими значеннями за замовчуванням.
    /// </summary>
    public LevelMenuState()
    {
        PlayingState playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState;
        List<Level> levels = playingState.Levels;

        // Додаємо задній фон.
        SpriteGameObject background = new SpriteGameObject("Backgrounds/spr_levelselect", 0, "background");
        this.Add(background);

        // Додаємо кнопки рівнів.
        for (int i = 0; i < 10; i++)
        {
            int row = i / 4;
            int column = i % 4;
            LevelButton level = new LevelButton(i + 1, levels[i], 1);
            level.Position = new Vector2(column * (level.Width + 20), row * (level.Height + 20)) + new Vector2(390, 180);
            this.Add(level);
        }

        // Додаємо кнопку переходу до головного меню.
        backButton = new Button("Sprites/spr_button_back", 1);
        backButton.Position = new Vector2((GameEnvironment.Screen.X - backButton.Width) / 2, 750);
        this.Add(backButton);
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає номер вибраного рівня.
    /// </summary>
    public int LevelSelected
    {
        get
        {
            foreach (GameObject obj in this.Objects)
            {
                LevelButton levelButton = obj as LevelButton;
                if (levelButton != null && levelButton.Pressed)
                    return levelButton.LevelIndex;
            }
            return -1;
        }
    }

    #endregion Властивості
}