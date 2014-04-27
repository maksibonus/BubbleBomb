using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Клас, що відповідає за роботу зі станами гри.
/// </summary>
public class GameStateManager : IGameLoopObject
{
    #region Поля класу

    /// <summary>
    /// Словник станів гри
    /// </summary>
    Dictionary<string, IGameLoopObject> gameStates;

    /// <summary>
    /// Поточний стан гри
    /// </summary>
    IGameLoopObject currentGameState;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public void HandleInput(InputHelper inputHelper)
    {
        if (currentGameState != null)
            currentGameState.HandleInput(inputHelper);
    }

    public void Update(GameTime gameTime)
    {
        if (currentGameState != null)
            currentGameState.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentGameState != null)
            currentGameState.Draw(gameTime, spriteBatch);
    }

    public void Reset()
    {
        if (currentGameState != null)
            currentGameState.Reset();
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу початковими значеннями за замовчуванням.
    /// </summary>
    public GameStateManager()
    {
        gameStates = new Dictionary<string, IGameLoopObject>();
        currentGameState = null;
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає поточний стан гри
    /// </summary>
    public IGameLoopObject CurrentGameState
    {
        get
        {
            return currentGameState;
        }
    }

    #endregion Властивості

    #region Методи

    /// <summary>
    /// Додає стан до словника.
    /// </summary>
    /// <param name="name">Назва стану.</param>
    /// <param name="state">Об'єкт стану.</param>
    public void AddGameState(string name, IGameLoopObject state)
    {
        gameStates[name] = state;
    }

    /// <summary>
    /// Повертає стан гри за ім'ям.
    /// </summary>
    /// <param name="name">Назва стану.</param>
    /// <returns>Об'єкт стану</returns>
    public IGameLoopObject GetGameState(string name)
    {
        return gameStates[name];
    }

    /// <summary>
    /// Переходить на інший стан.
    /// </summary>
    /// <param name="name">Назва стану.</param>
    public void SwitchTo(string name)
    {
        if (gameStates.ContainsKey(name))
            currentGameState = gameStates[name];
        else
            throw new KeyNotFoundException("Could not find game state: " + name);
    }

    #endregion Методи
}