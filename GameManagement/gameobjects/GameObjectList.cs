using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Клас, що представляє собою лист об'єктів.
/// </summary>
public class GameObjectList : GameObject
{
    #region Поля класу

    /// <summary>
    /// Лист об'єктів.
    /// </summary>
    protected List<GameObject> gameObjects;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void HandleInput(InputHelper inputHelper)
    {
        for (int i = gameObjects.Count - 1; i >= 0; i--)
            gameObjects[i].HandleInput(inputHelper);
    }

    public override void Update(GameTime gameTime)
    {
        foreach (GameObject obj in gameObjects)
            obj.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible)
            return;
        this.Layer = 100;
        List<GameObject>.Enumerator e = gameObjects.GetEnumerator();
        while (e.MoveNext())
            e.Current.Draw(gameTime, spriteBatch);

    }

    public override void Reset()
    {
        base.Reset();
        foreach (GameObject obj in gameObjects)
            obj.Reset();
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу.
    /// </summary>
    public GameObjectList(int layer = 0, string id = "") : base(layer, id)
    {
        gameObjects = new List<GameObject>();
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає лист об'єктів.
    /// </summary>
    public List<GameObject> Objects
    {
        get { return gameObjects; }
    }

    #endregion Властивості

    #region Методи

    /// <summary>
    /// Додає об'єкт.
    /// </summary>
    /// <param name="obj">Об'єкт, який додається.</param>
    public void Add(GameObject obj)
    {
        obj.Parent = this;
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i].Layer > obj.Layer)
            {
                gameObjects.Insert(i, obj);
                return;
            }
        }
        gameObjects.Add(obj);
    }

    /// <summary>
    /// Видаляє об'єкт.
    /// </summary>
    /// <param name="obj">Об'єкт, який видаляється.</param>
    public void Remove(GameObject obj)
    {
        gameObjects.Remove(obj);
        obj.Parent = null;
    }

    /// <summary>
    /// Знаходить об'єкт.
    /// </summary>
    /// <param name="id">Програмне ім'я об'єкту.</param>
    /// <returns>Ігровий об'єкт.</returns>
    public GameObject Find(string id)
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj.ID == id)
                return obj;
            if (obj is GameObjectList)
            {
                GameObjectList objlist = obj as GameObjectList;
                GameObject subobj = objlist.Find(id);
                if (subobj != null)
                    return subobj;
            }
        }
        return null;
    }

    #endregion Методи
}