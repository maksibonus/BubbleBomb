using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Інтерфейс, що задає стиль побудови програми.
/// </summary>
public interface IGameLoopObject
{
    #region Методи

    /// <summary>
    /// Обробляє натискання клавіш.
    /// </summary>
    /// <param name="inputHelper">Зберігає у собі інформацію про стан клавіш.</param>
    void HandleInput(InputHelper inputHelper);

    /// <summary>
    /// Оновлює стан об'єкту.
    /// </summary>
    /// <param name="gameTime">Час, який минув від попереднього до поточного стану гри.</param>
    void Update(GameTime gameTime);

    /// <summary>
    /// Відображає об'єкт на екрані.
    /// </summary>
    /// <param name="gameTime">Час, який минув від попереднього до поточного стану гри.</param>
    /// <param name="spriteBatch">Включає в себе групу спрайтів з однаковими параметрами.</param>
    void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    /// <summary>
    /// Повертає стан об'єкту до початкового.
    /// </summary>
    void Reset();

    #endregion Методи
}
