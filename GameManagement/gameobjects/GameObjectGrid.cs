using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Клас, що представляє собою таблицю об'єктів.
/// </summary>
public class GameObjectGrid : GameObject
{
    #region Поля класу

    /// <summary>
    /// Масив об'єктів.
    /// </summary>
    protected GameObject[,] grid;

    /// <summary>
    /// Ширина комірки.
    /// </summary>
    protected int cellWidth;
        
    /// <summary>
    /// Висота комірки.
    /// </summary>
    protected int cellHeight;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void HandleInput(InputHelper inputHelper)
    {
        foreach (GameObject obj in grid)
            obj.HandleInput(inputHelper);
    }

    public override void Update(GameTime gameTime)
    {
        foreach (GameObject obj in grid)
            obj.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        this.Layer = 100;
        foreach (GameObject obj in grid)
            obj.Draw(gameTime, spriteBatch);
    }

    public override void Reset()
    {
        base.Reset();
        foreach (GameObject obj in grid)
            obj.Reset();
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public GameObjectGrid(int rows, int columns, int layer = 0, string id = "")
        : base(layer, id)
    {
        grid = new GameObject[columns, rows];
        for (int x = 0; x < columns; x++)
            for (int y = 0; y < rows; y++)
                grid[x, y] = null;
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає масив об'єктів.
    /// </summary>
    public GameObject[,] Objects
    {
        get
        {
            return grid;
        }
    }

    /// <summary>
    /// Повертає кількість рядків масиву.
    /// </summary>
    public int Rows
    {
        get { return grid.GetLength(1); }
    }

    /// <summary>
    /// Повертає кількість стовпців масиву.
    /// </summary>
    public int Columns
    {
        get { return grid.GetLength(0); }
    }

    /// <summary>
    /// Повертає чи задає ширину комірки.
    /// </summary>
    public int CellWidth
    {
        get { return cellWidth; }
        set { cellWidth = value; }
    }

    /// <summary>
    /// Повертає чи задає висоту комірки.
    /// </summary>
    public int CellHeight
    {
        get { return cellHeight; }
        set { cellHeight = value; }
    }

    #endregion Властивості

    #region Методи

    /// <summary>
    /// Додає об'єкт.
    /// </summary>
    /// <param name="obj">Об'єкт, який додається.</param>
    /// <param name="x">Координата об'єкту по x в масиві.</param>
    /// <param name="y">Координата об'єкту по y в масиві.</param>
    public void Add(GameObject obj, int x, int y)
    {
        grid[x, y] = obj;
        obj.Parent = this;
        obj.Position = new Vector2(x * cellWidth, y * cellHeight);
    }

    /// <summary>
    /// Повертає ігровий об'єкт.
    /// </summary>
    /// <param name="x">Координата об'єкту по x в масиві.</param>
    /// <param name="y">Координата об'єкту по y в масиві.</param>
    /// <returns>Ігровий об'єкт.</returns>
    public GameObject Get(int x, int y)
    {
        if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
            return grid[x, y];
        else
            return null;
    }

    /// <summary>
    /// Повертає позицію об'єкту по відношенню к іншому об'єкту.
    /// </summary>
    /// <param name="obj">Об'єкт по відношенню к якому шукається позиція.</param>
    /// <returns>Позиція об'єкту.</returns>
    public Vector2 GetAnchorPosition(GameObject obj)
    {
        for (int x = 0; x < Columns; x++)
            for (int y = 0; y < Rows; y++)
                if (grid[x, y] == obj)
                    return new Vector2(x * cellWidth, y * cellHeight);
        return Vector2.Zero;
    }

    #endregion Методи
}
