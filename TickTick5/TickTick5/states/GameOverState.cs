using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RamGecXNAControls;

class GameOverState : GameObjectList
{
    protected IGameLoopObject playingState;
    GUIManager guiManager = new GUIManager(TickTick.game);       // менеджер контролю усіх елементів
    GUIControl myControl;                                                                               //вікно
    Label myAnotherControl;       

    public GameOverState()
    {
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        SpriteGameObject overlay = new SpriteGameObject("Overlays/spr_gameover");
        overlay.Position = new Vector2(GameEnvironment.Screen.X, GameEnvironment.Screen.Y) / 2 - overlay.Center;
        this.Add(overlay);

        //створюємо вікно
        myControl = new Window(new Rectangle(GameEnvironment.Screen.X / 2 - 125, GameEnvironment.Screen.Y / 2 + 25, GameEnvironment.Screen.X / 2 - 350, GameEnvironment.Screen.Y / 2 - 250), "Результат");
        guiManager.Controls.Add(myControl);

        //результат
        string text = "Ваш результат: " + Result.result + " из " + Level.countWaterDrop + "балов.";
        myAnotherControl = new Label(new Rectangle(30, 30, 0, 0), text, "Question");
        myControl.Controls.Add(myAnotherControl);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (!inputHelper.KeyPressed(Keys.Space))
            return;
        playingState.Reset();
        GameEnvironment.GameStateManager.SwitchTo("playingState");
    }

    public override void Update(GameTime gameTime)
    {
        playingState.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        playingState.Draw(gameTime, spriteBatch);
        base.Draw(gameTime, spriteBatch);
    }
}