using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (quitButton.Pressed)
        {
            this.Reset();
            GameEnvironment.GameStateManager.SwitchTo("levelMenu");
        }      
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        TimerGameObject timer = this.Find("timer") as TimerGameObject;
        Player player = this.Find("player") as Player;

        // cперевірка на те, що ми померли
        if (!player.IsAlive)
            timer.Running = false;

        // перевірка на те, що ми вичерпали час
        if (timer.GameOver)
            player.Explode();
                       
        // перевірка на те, що ми виграли
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
}
