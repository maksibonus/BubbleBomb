using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Клас, що відповідає за обробку клавіш.
/// </summary>
public class InputHelper
{
    #region Поля класу

    /// <summary>
    /// Поточний стан миші.
    /// </summary>
    protected MouseState currentMouseState;

    /// <summary>
    /// Попередній стан миші.
    /// </summary>
    protected MouseState previousMouseState;

    /// <summary>
    /// Поточний стан клавіатури.
    /// </summary>
    protected KeyboardState currentKeyboardState;

    /// <summary>
    /// Попередній стан клавіатури.
    /// </summary>
    protected KeyboardState previousKeyboardState;

    /// <summary>
    /// Масштабування.
    /// </summary>
    protected Vector2 scale;

    #endregion Поля класу

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу початковими значеннями за замовчуванням.
    /// </summary>
    public InputHelper()
    {
        scale = Vector2.One;
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає чи задає масштабування
    /// </summary>
    public Vector2 Scale
    {
        get { return scale; }
        set { scale = value; }
    }

    /// <summary>
    /// Повертає позицію миші
    /// </summary>
    public Vector2 MousePosition
    {
        get { return new Vector2(currentMouseState.X, currentMouseState.Y) / scale; }
    }

    /// <summary>
    /// Вказує, чи була натиснута клавіша клавіатури
    /// </summary>
    public bool AnyKeyPressed
    {
        get { return currentKeyboardState.GetPressedKeys().Length > 0 && previousKeyboardState.GetPressedKeys().Length == 0; }
    }

    #endregion Властивості

    #region Методи

    /// <summary>
    /// Оновлює стан миші і клавіатури.
    /// </summary>
    public void Update()
    {
        previousMouseState = currentMouseState;
        previousKeyboardState = currentKeyboardState;
        currentMouseState = Mouse.GetState();
        currentKeyboardState = Keyboard.GetState();
    }

    /// <summary>
    /// Вказує, чи була натиснута ліва клавіша миші
    /// </summary>
    /// <returns>Натиснута чи не натиснута</returns>
    public bool MouseLeftButtonPressed()
    {
        return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
    }

    /// <summary>
    /// Вказує, чи була затиснута ліва клавіша миші
    /// </summary>
    /// <returns>Затиснута чи не затиснута</returns>
    public bool MouseLeftButtonDown()
    {
        return currentMouseState.LeftButton == ButtonState.Pressed;
    }

    /// <summary>
    /// Вказує, чи була натиснута клавіша клавіатури
    /// </summary>
    /// <param name="k">Ідентифікатор клавіші.</param>
    /// <returns>Натиснута чи не натиснута</returns>
    public bool KeyPressed(Keys k)
    {
        return currentKeyboardState.IsKeyDown(k) && previousKeyboardState.IsKeyUp(k);
    }

    /// <summary>
    /// Вказує, чи була затиснута клавіша клавіатури
    /// </summary>
    /// <param name="k">Ідентифікатор клавіші.</param>
    /// <returns>Затиснута чи не затиснута</returns>
    public bool IsKeyDown(Keys k)
    {
        return currentKeyboardState.IsKeyDown(k);
    }

    #endregion Методи
}