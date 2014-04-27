using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Клас, що представляє собою стан перебування у меню.
/// </summary>
class TitleMenuState : GameObjectList
{
    #region Поля класу

    /// <summary>
    /// Кнопка переходу в меню вибору рівня.
    /// </summary>
    protected Button playButton;

    /// <summary>
    /// Кнопка переходу в стан допомоги.
    /// </summary>
    protected Button helpButton;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (playButton.Pressed)
            GameEnvironment.GameStateManager.SwitchTo("levelMenu");
        else if (helpButton.Pressed)
            GameEnvironment.GameStateManager.SwitchTo("helpState");
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public TitleMenuState()
    {
        // Завантаження початкового екрану.
        SpriteGameObject title_screen = new SpriteGameObject("Backgrounds/spr_title", 0, "background");
        this.Add(title_screen);

        // Додавання кнопки переходу в меню вибору рівня.
        playButton = new Button("Sprites/spr_button_play", 1);
        playButton.Position = new Vector2((GameEnvironment.Screen.X - playButton.Width) / 2, 540);
        this.Add(playButton);

        // Додавання кнопки переходу в стан допомоги.
        helpButton = new Button("Sprites/spr_button_help", 1);
        helpButton.Position = new Vector2((GameEnvironment.Screen.X - helpButton.Width) / 2, 600);
        this.Add(helpButton);
    }

    #endregion Конструктори
}