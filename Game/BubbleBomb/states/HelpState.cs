using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Клас, що представляє собою стан допомоги.
/// </summary>
class HelpState : GameObjectList
{
    #region Поля класу

    /// <summary>
    /// Кнопка переходу до головного меню.
    /// </summary>
    protected Button backButton;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (backButton.Pressed)
            GameEnvironment.GameStateManager.SwitchTo("titleMenu");
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу початковими значеннями за замовчуванням.
    /// </summary>
    public HelpState()
    {
        SpriteGameObject background = new SpriteGameObject("Backgrounds/spr_help", 0, "background");
        this.Add(background);
        backButton = new Button("Sprites/spr_button_back", 1);
        backButton.Position = new Vector2((GameEnvironment.Screen.X - backButton.Width) / 2, 750);
        this.Add(backButton);
    }

    #endregion Конструктори
}