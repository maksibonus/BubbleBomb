﻿using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RamGecXNAControls;

class PlayingState : IGameLoopObject
{
    protected List<Level> levels;
    protected int currentLevelIndex;
    protected ContentManager Content;
    GUIManager guiManager = new GUIManager(TickTick.game);       // менеджер контролю усіх елементів
    public bool questionState=false;

    public PlayingState(ContentManager Content)
    {
        
        this.Content = Content;
        currentLevelIndex = -1;
        levels = new List<Level>();
        LoadLevels();
        //LoadLevelsStatus(Content.RootDirectory + "/Levels/levels_status.txt");
    }

    public GUIManager GUIManager
    {
        get
        {
            return guiManager;
        }
    }

    public Level CurrentLevel
    {
        get
        {
            return levels[currentLevelIndex];
        }
    }

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

    public List<Level> Levels
    {
        get
        {
            return levels;
        }
    }

    public virtual void HandleInput(InputHelper inputHelper)
    {
        TimerGameObject timer = this.Find("timer") as TimerGameObject;
        if (!questionState)
            CurrentLevel.HandleInput(inputHelper);
    }

    public virtual void Update(GameTime gameTime)
    {
        if (!questionState)
            CurrentLevel.Update(gameTime);
        foreach (var control in guiManager.Controls)
            control.Update(gameTime);
        if (CurrentLevel.GameOver)
            GameEnvironment.GameStateManager.SwitchTo("gameOverState");
        else if (CurrentLevel.Completed)
        {
            //CurrentLevel.Solved = true;
            GameEnvironment.GameStateManager.SwitchTo("levelFinishedState");
        }    
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

    public void NextLevel()
    {
        CurrentLevel.Reset();
        if (currentLevelIndex >= levels.Count - 1)
            GameEnvironment.GameStateManager.SwitchTo("levelMenu");
        //WriteLevelsStatus(Content.RootDirectory + "/Levels/levels_status.txt");
    }

    public void LoadLevels()
    {
        for (int currLevel = 1; currLevel <= 10; currLevel++)
            levels.Add(new Level(currLevel));
    }

    //public void LoadLevelsStatus(string path)
    //{
    //    List<string> textlines = new List<string>();
    //    StreamReader fileReader = new StreamReader(path);
    //    for (int i = 0; i < levels.Count; i++)
    //    {
    //        string line = fileReader.ReadLine();
    //        string[] elems = line.Split(',');
    //        if (elems.Length == 2)
    //        {
    //            levels[i].Locked = bool.Parse(elems[0]);
    //            levels[i].Solved = bool.Parse(elems[1]);
    //        }
    //    }
    //    fileReader.Close();
    //}

    //public void WriteLevelsStatus(string path)
    //{
    //    // read the lines
    //    List<string> textlines = new List<string>();
    //    StreamWriter fileWriter = new StreamWriter(path, false);
    //    for (int i = 0; i < levels.Count; i++)
    //    {
    //        string line = levels[i].Locked.ToString() + "," + levels[i].Solved.ToString();
    //        fileWriter.WriteLine(line);
    //    }
    //    fileWriter.Close();
    //}
}