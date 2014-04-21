using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RamGecXNAControls;
class LevelFinishedState : GameObjectList
{
    protected IGameLoopObject playingState;
    GUIManager guiManager = new GUIManager(TickTick.game);       // менеджер контролю усіх елементів
    GUIControl myControl;                                                                               //вікно
    Label myAnotherControl;                                                                        //елементи управління
    RamGecXNAControls.Button buttonOK;

    public LevelFinishedState()
    {
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        //SpriteGameObject overlay = new SpriteGameObject("Overlays/spr_welldone");
        //overlay.Position = new Vector2(GameEnvironment.Screen.X, GameEnvironment.Screen.Y) / 2 - overlay.Center;
        //this.Add(overlay);

        //створюємо вікно
        myControl = new Window(new Rectangle(400, 400, GameEnvironment.Screen.X - 450, GameEnvironment.Screen.Y - 450), "Результат");
        guiManager.Controls.Add(myControl);

        //створюємо кнопку "Підтвердити"
        buttonOK = new RamGecXNAControls.Button(new Rectangle(myControl.Bounds.Width / 2 - 180, myControl.Bounds.Height - 120, 120, 60), "Підтвердити", "OK");
        buttonOK.Hint = "Відповісти на питання";
        buttonOK.OnClick += (sender) =>
        {

            GameEnvironment.GameStateManager.SwitchTo("playingState");
            (playingState as PlayingState).NextLevel();
            myControl.Visible = false;
        };
        myControl.Controls.Add(buttonOK);

        //результат
        myAnotherControl = new Label(new Rectangle(30, 30, 0, 0), "Ваш результат: "+Result.result, "Question");
        myControl.Controls.Add(myAnotherControl);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (!inputHelper.KeyPressed(Keys.Space))
            return;
        GameEnvironment.GameStateManager.SwitchTo("playingState");
        (playingState as PlayingState).NextLevel();
        base.HandleInput(inputHelper);
    }
    public override void Update(GameTime gameTime)
    {
        playingState.Update(gameTime);
        foreach (var control in guiManager.Controls)
            control.Update(gameTime);
        myAnotherControl.Text = "Ваш результат: " + Result.result;
        
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        playingState.Draw(gameTime, spriteBatch);
        base.Draw(gameTime, spriteBatch);
        guiManager.Draw(spriteBatch);
    }
}