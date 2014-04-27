using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RamGecXNAControls;

/// <summary>
/// Клас, що представляє собою стан гри.
/// </summary>
class PlayingState : IGameLoopObject
{
    #region Поля класу

    /// <summary>
    /// Менеджер контролю усіх елементів.
    /// </summary>
    GUIManager guiManager;

    /// <summary>
    /// Лист рівнів гри.
    /// </summary>
    protected List<Level> levels;

    /// <summary>
    /// Номер поточного рівня.
    /// </summary>
    protected int currentLevelIndex;

    /// <summary>
    /// Компонент, який завантажує об'єкти з бінарних файлів.
    /// </summary>
    protected ContentManager Content;

    /// <summary>
    /// Прапорець, що вказує, чи знаходиться гра в стані питання.
    /// </summary>
    public bool questionState = false;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public virtual void Update(GameTime gameTime)
    {
        TimerGameObject timer = this.CurrentLevel.Find("timer") as TimerGameObject;
        if (!questionState)
            CurrentLevel.Update(gameTime);
        timer.Update(gameTime);
        guiManager.SetMatrix(GameEnvironment.spriteScale);
        guiManager.Update(gameTime);
        foreach (var control in guiManager.Controls)
            control.Update(gameTime);
        if (CurrentLevel.GameOver)
        {
            GameEnvironment.GameStateManager.SwitchTo("gameOverState");
        }
        else if (CurrentLevel.Completed)
        {
            CurrentLevel.Solved = true;
            GameEnvironment.GameStateManager.SwitchTo("levelFinishedState");
        }
    }

    public virtual void HandleInput(InputHelper inputHelper)
    {
        if (!questionState)
            CurrentLevel.HandleInput(inputHelper);
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        CurrentLevel.Draw(gameTime, spriteBatch);
        guiManager.Draw(spriteBatch);
    }

    public virtual void Reset()
    {
        CurrentLevel.Reset();
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public PlayingState(ContentManager Content)
    {
        this.Content = Content;
        guiManager = new GUIManager(BubbleBomb.game);
        currentLevelIndex = -1;
        levels = new List<Level>();
        LoadLevels();
        LoadLevelsStatus(Content.RootDirectory + "/Levels/levels_status.txt");
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає менеджер контролю усіх елементів.
    /// </summary>
    public GUIManager GUIManager
    {
        get
        {
            return guiManager;
        }
    }

    /// <summary>
    /// Повертає поточний рівень.
    /// </summary>
    public Level CurrentLevel
    {
        get
        {
            return levels[currentLevelIndex];
        }
    }

    /// <summary>
    /// Повертає номер поточного рівня.
    /// </summary>
    public int CurrentLevelIndex
    {
        get
        {
            return currentLevelIndex;
        }
        set
        {
            if (value >= 0 && value < levels.Count)
            {
                currentLevelIndex = value;
                CurrentLevel.Reset();
            }
        }
    }

    /// <summary>
    /// Повертає лист рівнів гри
    /// </summary>
    public List<Level> Levels
    {
        get
        {
            return levels;
        }
    }

    #endregion Властивості

    #region Методи

    /// <summary>
    /// Перехід на наступний рівень.
    /// </summary>
    public void NextLevel()
    {
        CurrentLevel.Reset();
        if (currentLevelIndex >= levels.Count - 1)
            GameEnvironment.GameStateManager.SwitchTo("levelMenu");
        else
        {
            CurrentLevelIndex++;
            levels[currentLevelIndex].Locked = false;
        }

        WriteLevelsStatus(Content.RootDirectory + "/Levels/levels_status.txt");
    }

    /// <summary>
    /// Додавання рівнів в лист.
    /// </summary>
    public void LoadLevels()
    {
        for (int currLevel = 1; currLevel <= 10; currLevel++)
            levels.Add(new Level(currLevel));
    }

    /// <summary>
    /// Визначення статусу рівня.
    /// </summary>
    /// <param name="path">Файл.</param>
    public void LoadLevelsStatus(string path)
    {
        List<string> textlines = new List<string>();
        StreamReader fileReader = new StreamReader(path);
        for (int i = 0; i < levels.Count; i++)
        {
            string line = fileReader.ReadLine();
            string[] elems = line.Split(',');
            if (elems.Length == 2)
            {
                levels[i].Locked = bool.Parse(elems[0]);
                levels[i].Solved = bool.Parse(elems[1]);
            }
        }
        fileReader.Close();
    }

    /// <summary>
    /// Зміна статусу рівня.
    /// </summary>
    /// <param name="path">Файл.</param>
    public void WriteLevelsStatus(string path)
    {
        // read the lines
        List<string> textlines = new List<string>();
        StreamWriter fileWriter = new StreamWriter(path, false);
        for (int i = 0; i < levels.Count; i++)
        {
            string line = levels[i].Locked.ToString() + "," + levels[i].Solved.ToString();
            fileWriter.WriteLine(line);
        }
        fileWriter.Close();
    }

    #endregion Методи
}