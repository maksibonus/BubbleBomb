using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RamGecXNAControls;

class GameOverState : GameObjectList
{
    protected PlayingState playingState;
    GUIManager guiManager = new GUIManager(TickTick.game);       // менеджер контролю усіх елементів
    GUIControl myControl;                                                                               //вікно
    Label myAnotherControl;       

    public GameOverState()
    {
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState;
        SpriteGameObject overlay = new SpriteGameObject("Overlays/spr_gameover");
        overlay.Position = new Vector2(GameEnvironment.Screen.X, GameEnvironment.Screen.Y) / 2 - overlay.Center;
        this.Add(overlay);

        //створюємо вікно
        myControl = new Window(new Rectangle(GameEnvironment.Screen.X / 2 - 175, GameEnvironment.Screen.Y / 2 + 150, GameEnvironment.Screen.X / 2 - 350, GameEnvironment.Screen.Y / 2 - 300), "Результат");
        guiManager.Controls.Add(myControl);
        //результат
        myAnotherControl = new Label(new Rectangle(30, 30, 0, 0), "Ваш результат: " + Result.result + " з " + Level.countWaterDrop[LevelMenuState.curLevel] + " балів.", "Question");
        myControl.Controls.Add(myAnotherControl);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (!inputHelper.KeyPressed(Keys.Space))
            return;

        if (playingState != null)
        {
            //PlayingState playingState = (GameEnvironment.GameStateManager.CurrentGameState as PlayingState);
            if (WaterDrop.myControl != null)
            {
                WaterDrop.myControl.Visible = false;
            }
            playingState.questionState = false;
        }
        Result.result = 0;
        playingState.Reset();
        GameEnvironment.GameStateManager.SwitchTo("playingState");
    }

    public override void Update(GameTime gameTime)
    {
        playingState.Update(gameTime);
        foreach (var control in guiManager.Controls)
            control.Update(gameTime);
        myAnotherControl.Text = "Ваш результат: " + Result.result + " з " + Level.countWaterDrop[LevelMenuState.curLevel] + " балів.";
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        playingState.Draw(gameTime, spriteBatch);
        base.Draw(gameTime, spriteBatch);
        guiManager.Draw(spriteBatch);
    }
}