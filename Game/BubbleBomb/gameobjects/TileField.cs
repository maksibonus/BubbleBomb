/// <summary>
/// Клас, що представляє собою ігрове поле.
/// </summary>
class TileField : GameObjectGrid
{
    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу початковими значеннями за замовчуванням.
    /// </summary>
    public TileField(int rows, int columns, int layer = 0, string id = "")
        : base(rows, columns, layer, id)
    {
    }

    #endregion Конструктори

    #region Методи

    /// <summary>
    /// Дає інформацію про тип тайлу.
    /// </summary>
    /// <param name="x">Координата об'єкту по x в масиві.</param>
    /// <param name="y">Координата об'єкту по y в масиві.</param>
    /// <returns>Тип тайлу.</returns>
    public TileType GetTileType(int x, int y)
    {
        if (x < 0 || x >= Columns)
            return TileType.Normal;
        if (y < 0 || y >= Rows)
            return TileType.Background;
        Tile current = this.Objects[x, y] as Tile;
        return current.TileType;
    }

    #endregion Методи
}