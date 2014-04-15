using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RamGecXNAControls;
using RamGecXNAControlsExtensions;

class QuestionState : GameObjectList
{
    GUIManager guiManager = new GUIManager(TickTick.game, "Themes", "Default");                         // менеджер контролю усіх елементів

    public QuestionState()
    {
        //створюємо вікно
        GUIControl myControl = new Window(new Rectangle(50, 50, GameEnvironment.Screen.X-100, GameEnvironment.Screen.Y-100), "Питання");
        guiManager.Controls.Add(myControl);

        //створюємо кнопку Підтвердити
        GUIControl myAnotherControl = new RamGecXNAControls.Button(new Rectangle(myControl.Bounds.Width / 2 - 180, myControl.Bounds.Height - 120, 120, 60), "OK", "OK");
        myAnotherControl.Hint = "Tooltip text";
        myControl.Controls.Add(myAnotherControl);

        //створюємо кнопку Cancel
        myAnotherControl = new RamGecXNAControls.Button(new Rectangle(myControl.Bounds.Width / 2 + 60, myControl.Bounds.Height - 120, 120, 60), "Cancel", "Cancel");
        myAnotherControl.OnClick += (sender) =>
        {
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        };
        myControl.Controls.Add(myAnotherControl);
        
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

