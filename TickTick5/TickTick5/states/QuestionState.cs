using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RamGecXNAControls;
using RamGecXNAControlsExtensions;

class QuestionState : GameObjectList
{
    Window myWindow;
    RamGecXNAControls.Button myButton;
    GUIManager guiManager = new GUIManager(TickTick.game, "Themes", "Default");

    public QuestionState()
    {
        //створюємо вікно
        myWindow = new Window(new Rectangle(50, 50, 650, 650), "Питання");
        guiManager.Controls.Add(myWindow);

        //створюємо кнопки
        myButton = new RamGecXNAControls.Button(new Rectangle(60, 60, 40, 30), "Cancel","Cancel");
        myButton.Parent = myWindow;
        myButton.OnClick += (sender) =>
        {
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        };
        guiManager.Controls.Add(myButton);
    }

    //public override void HandleInput(InputHelper inputHelper)
    //{
    //    if (!inputHelper.KeyPressed(Keys.Space))
    //        return;
    //    GameEnvironment.GameStateManager.SwitchTo("playingState");
    //    (playingState as PlayingState).NextLevel();
    //}

    public override void Update(GameTime gameTime)
    {
        foreach (var control in guiManager.Controls)
            control.Update(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var control in guiManager.Controls)
            control.Draw(spriteBatch);
        base.Draw(gameTime, spriteBatch);
    }
}

