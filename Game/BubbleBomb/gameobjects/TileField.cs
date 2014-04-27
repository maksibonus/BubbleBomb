/// <summary>
/// ����, �� ����������� ����� ������ ����.
/// </summary>
class TileField : GameObjectGrid
{
    #region ������������

    /// <summary>
    /// �������� ���� ����� ����������� ���������� �� �������������.
    /// </summary>
    public TileField(int rows, int columns, int layer = 0, string id = "")
        : base(rows, columns, layer, id)
    {
    }

    #endregion ������������

    #region ������

    /// <summary>
    /// �� ���������� ��� ��� �����.
    /// </summary>
    /// <param name="x">���������� ��'���� �� x � �����.</param>
    /// <param name="y">���������� ��'���� �� y � �����.</param>
    /// <returns>��� �����.</returns>
    public TileType GetTileType(int x, int y)
    {
        if (x < 0 || x >= Columns)
            return TileType.Normal;
        if (y < 0 || y >= Rows)
            return TileType.Background;
        Tile current = this.Objects[x, y] as Tile;
        return current.TileType;
    }

    #endregion ������
}