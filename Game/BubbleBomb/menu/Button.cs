using Microsoft.Xna.Framework;

/// <summary>
/// Клас, що представляє собою кнопку.
/// </summary>
class Button : SpriteGameObject
{
    #region Поля класу

    /// <summary>
    /// Прапорець, що вказує, чи натиснута клавіша.
    /// </summary>
    protected bool pressed;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void HandleInput(InputHelper inputHelper)
    {
        pressed = inputHelper.MouseLeftButtonPressed() &&
            BoundingBox.Contains((int)inputHelper.MousePosition.X, (int)inputHelper.MousePosition.Y);
    }

    public override void Reset()
    {
        base.Reset();
        pressed = false;
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу початковими значеннями за замовчуванням.
    /// </summary>
    public Button(string imageAsset, int layer = 0, string id = "")
        : base(imageAsset, layer, id)
    {
        pressed = false;
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Вказує, чи натиснута клавіша.
    /// </summary>
    public bool Pressed
    {
        get { return pressed; }
    }

    #endregion Властивості
}