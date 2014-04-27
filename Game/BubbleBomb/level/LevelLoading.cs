using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

/// <summary>
/// Клас, що представляє собою рівень гри(методи).
/// </summary>
partial class Level : GameObjectList
{
    #region Методи

    /// <summary>
    /// Завантажує тайли.
    /// </summary>
    /// <param name="path">Файл, з якого береться інформація про рівень.</param>
    /// <param name="levelIndex">Номер рівня.</param>
    public void LoadTiles(string path,int levelIndex)
    {
        int width;
        List<string> textlines = new List<string>();
        StreamReader fileReader = new StreamReader(path);
        string line = fileReader.ReadLine();
        width = line.Length;
        while (line != null)
        {
            textlines.Add(line);
            line = fileReader.ReadLine();
        }
        TileField tiles = new TileField(textlines.Count - 1, width, 1, "tiles");
        this.Add(tiles);
        tiles.CellWidth = 72;
        tiles.CellHeight = 55;
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < textlines.Count - 1; ++y)
            {
                Tile t = LoadTile(textlines[y][x], x, y, levelIndex);
                tiles.Add(t, x, y);
            }
        }

        GameObjectList hintfield = new GameObjectList(100);
        this.Add(hintfield);
        string hint = textlines[textlines.Count - 1];
        SpriteGameObject hint_frame = new SpriteGameObject("Overlays/spr_frame_hint", 1);
        hintfield.Position = new Vector2((GameEnvironment.Screen.X - hint_frame.Width) / 2, 10);
        hintfield.Add(hint_frame);
        TextGameObject hintText = new TextGameObject("Fonts/HintFont", 2);
        hintText.Text = textlines[textlines.Count - 1];
        hintText.Position = new Vector2(230, 30);
        hintText.Color = Color.Black;
        hintfield.Add(hintText);
        VisibilityTimer hintTimer = new VisibilityTimer(hintfield, 1, "hintTimer");
        this.Add(hintTimer);
    }

    /// <summary>
    /// Завантажує тайл.
    /// </summary>
    /// <param name="tileType">Символ з файлу.</param>
    /// <param name="x">Координата тайлу по x в файлі.</param>
    /// <param name="y">Координата тайлу по y в файлі.</param>
    /// <param name="levelIndex">Номер рівня.</param>
    /// <returns>Тайл.</returns>
    private Tile LoadTile(char tileType, int x, int y, int levelIndex)
    {
        switch (tileType)
        {
            case '.':
                return new Tile();
            case '-':
                return LoadBasicTile("spr_platform", TileType.Platform);
            case '+':
                return LoadBasicTile("spr_platform_hot", TileType.Platform, true, false);
            case '@':
                return LoadBasicTile("spr_platform_ice", TileType.Platform, false, true);
            case 'X':
                return LoadEndTile(x, y);
            case 'W':
                return LoadWaterTile(x, y,levelIndex);
            case '1':
                return LoadStartTile(x, y);
            case '#':
                return LoadBasicTile("spr_wall", TileType.Normal);
            case '^':
                return LoadBasicTile("spr_wall_hot", TileType.Normal, true, false);
            case '*':
                return LoadBasicTile("spr_wall_ice", TileType.Normal, false, true);
            case 'T':
                return LoadTurtleTile(x, y);
            case 'R':
                return LoadRocketTile(x, y, true);
            case 'r':
                return LoadRocketTile(x, y, false);
            case 'S':
                return LoadSparkyTile(x, y);
            case 'A':
            case 'B':
            case 'C':
                return LoadFlameTile(x, y, tileType);
            default:
                return new Tile("");
        }
    }

    /// <summary>
    /// Завантажує тайл поверхні.
    /// </summary>
    /// <param name="name">Назва тайлу.</param>
    /// <param name="tileType">Тип тайлу.</param>
    /// <param name="hot">Прапорець, що вказує, чи тайл гарячий.</param>
    /// <param name="ice">Прапорець, що вказує, чи тайл холодний.</param>
    /// <returns>Тайл.</returns>
    private Tile LoadBasicTile(string name, TileType tileType, bool hot = false, bool ice = false)
    {
        Tile t = new Tile("Tiles/" + name, tileType);
        t.Hot = hot;
        t.Ice = ice;
        return t;
    }

    /// <summary>
    /// Завантажує тайл головного персонажа.
    /// </summary>
    /// <param name="x">Координата тайлу по x в файлі.</param>
    /// <param name="y">Координата тайлу по y в файлі.</param>
    /// <returns>Тайл.</returns>
    private Tile LoadStartTile(int x, int y)
    {
        TileField tiles = this.Find("tiles") as TileField;
        Vector2 startPosition = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight);
        Player player = new Player(startPosition);
        this.Add(player);
        return new Tile("", TileType.Background);
    }

    /// <summary>
    /// Завантажує тайл полум'я.
    /// </summary>
    /// <param name="x">Координата тайлу по x в файлі.</param>
    /// <param name="y">Координата тайлу по y в файлі.</param>
    /// <param name="enemyType">Тип полум'я.</param>
    /// <returns>Тайл.</returns>
    private Tile LoadFlameTile(int x, int y, char enemyType)
    {
        GameObjectList enemies = this.Find("enemies") as GameObjectList;
        TileField tiles = this.Find("tiles") as TileField;
        GameObject enemy = null;
        switch (enemyType)
        {
            case 'A': enemy = new UnpredictableEnemy(); break;
            case 'B': enemy = new PlayerFollowingEnemy(); break;
            case 'C': 
            default:         enemy = new PatrollingEnemy(); break;
        }
        enemy.Position = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight);
        enemies.Add(enemy);
        return new Tile();
    }

    /// <summary>
    /// Завантажує тайл черепахи.
    /// </summary>
    /// <param name="x">Координата тайлу по x в файлі.</param>
    /// <param name="y">Координата тайлу по y в файлі.</param>
    /// <returns>Тайл.</returns>
    private Tile LoadTurtleTile(int x, int y)
    {
        GameObjectList enemies = this.Find("enemies") as GameObjectList;
        TileField tiles = this.Find("tiles") as TileField;
        Turtle enemy = new Turtle();
        enemy.Position = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight + 25.0f);
        enemies.Add(enemy);
        return new Tile();
    }

    /// <summary>
    /// Завантажує тайл спаркі.
    /// </summary>
    /// <param name="x">Координата тайлу по x в файлі.</param>
    /// <param name="y">Координата тайлу по y в файлі.</param>
    /// <returns>Тайл.</returns>
    private Tile LoadSparkyTile(int x, int y)
    {
        GameObjectList enemies = this.Find("enemies") as GameObjectList;
        TileField tiles = this.Find("tiles") as TileField;
        Sparky enemy = new Sparky((y + 1) * tiles.CellHeight - 100f);
        enemy.Position = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight - 100f);
        enemies.Add(enemy);
        return new Tile();
    }

    /// <summary>
    /// Завантажує тайл спаркі.
    /// </summary>
    /// <param name="x">Координата тайлу по x в файлі.</param>
    /// <param name="y">Координата тайлу по y в файлі.</param>
    /// <param name="moveToLeft">Задає напрям.</param>
    /// <returns>Тайл.</returns>
    private Tile LoadRocketTile(int x, int y, bool moveToLeft)
    {
        GameObjectList enemies = this.Find("enemies") as GameObjectList;
        TileField tiles = this.Find("tiles") as TileField;
        Vector2 startPosition = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight);
        Rocket enemy = new Rocket(moveToLeft, startPosition);
        enemies.Add(enemy);
        return new Tile();
    }

    /// <summary>
    /// Завантажує тайл фінішу.
    /// </summary>
    /// <param name="x">Координата тайлу по x в файлі.</param>
    /// <param name="y">Координата тайлу по y в файлі.</param>
    /// <returns>Тайл.</returns>
    private Tile LoadEndTile(int x, int y)
    {
        TileField tiles = this.Find("tiles") as TileField;
        SpriteGameObject exitObj = new SpriteGameObject("Sprites/spr_goal", 1, "exit");
        exitObj.Position = new Vector2(x * tiles.CellWidth, (y+1) * tiles.CellHeight);
        exitObj.Origin = new Vector2(0, exitObj.Height);
        this.Add(exitObj);
        return new Tile();
    }

    /// <summary>
    /// Завантажує тайл краплі.
    /// </summary>
    /// <param name="x">Координата тайлу по x в файлі.</param>
    /// <param name="y">Координата тайлу по y в файлі.</param>    
    /// <param name="levelIndex">Номер рівня.</param>
    /// <returns>Тайл.</returns>
    private Tile LoadWaterTile(int x, int y,int levelIndex)
    {
        countWaterDrop[levelIndex-1]++;
        GameObjectList waterdrops = this.Find("waterdrops") as GameObjectList;
        TileField tiles = this.Find("tiles") as TileField;
        TextGameObject tw = new TextGameObject("Fonts/Hud");
        tw.Text = countWaterDrop[levelIndex - 1].ToString();
        WaterDrop w = new WaterDrop(tw);
        w.Origin = w.Center;
        w.Position = new Vector2(x * tiles.CellWidth, y * tiles.CellHeight - 10);
        w.Position += new Vector2(tiles.CellWidth, tiles.CellHeight) / 2;
        tw.Position = new Vector2(x * tiles.CellWidth, y * tiles.CellHeight - 40);
        waterdrops.Add(w);
        waterdrops.Add(tw);
        return new Tile();
    }

    #endregion Методи
}