using Microsoft.Xna.Framework;

/// <summary>
/// Клас, що представляє собою рівень гри(реалізація інтерфейсів).
/// </summary>
partial class Level : GameObjectList
{
    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (quitButton.Pressed)
        {
            this.Reset();
            GameEnvironment.AssetManager.StopMusic();
            GameEnvironment.AssetManager.PlayMusic("Sounds/Legend Of Zelda - Zelda's lullaby (Original)");
            GameEnvironment.GameStateManager.SwitchTo("levelMenu");
        }      
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        TimerGameObject timer = this.Find("timer") as TimerGameObject;
        Player player = this.Find("player") as Player;

        // Перевірка на те, що ми померли
        if (!player.IsAlive)
            timer.Running = false;

        // Перевірка на те, що ми вичерпали час
        if (timer.GameOver)
            player.Explode();
                       
        // Перевірка на те, що ми виграли
        if (this.Completed && timer.Running)
        {
            player.LevelFinished();
            timer.Running = false;
        }
    }

    public override void Reset()
    {
        base.Reset();
        VisibilityTimer hintTimer = this.Find("hintTimer") as VisibilityTimer;
        hintTimer.StartVisible();
    }

    #endregion Реалізація інтерфейсів
}