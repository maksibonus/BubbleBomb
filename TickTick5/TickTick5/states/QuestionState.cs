using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RamGecXNAControls;
using RamGecXNAControlsExtensions;

class QuestionState : GameObjectList
{
    GUIManager guiManager = new GUIManager(TickTick.game, "Themes", "Default");                         // менеджер контролю усіх елементів
    GUIControl myControl;                                                                               //вікно
    GUIControl myAnotherControl;                                                                        //елементи управління

    public QuestionState()
    {
        //створюємо вікно
        myControl = new Window(new Rectangle(50, 50, GameEnvironment.Screen.X-100, GameEnvironment.Screen.Y-100), "Питання   іыяхёї");
        guiManager.Controls.Add(myControl);

        //створюємо кнопку "Підтвердити"
        myAnotherControl = new RamGecXNAControls.Button(new Rectangle(myControl.Bounds.Width / 2 - 180, myControl.Bounds.Height - 120, 120, 60), "Підтвердити", "OK");
        myAnotherControl.Hint = "Tooltip text";
        myControl.Controls.Add(myAnotherControl);

        //створюємо кнопку "Закрити вікно"
        myAnotherControl = new RamGecXNAControls.Button(new Rectangle(myControl.Bounds.Width / 2 + 60, myControl.Bounds.Height - 120, 120, 60), "Закрити вікно", "Cancel");
        myAnotherControl.OnClick += (sender) =>
        {
            GameEnvironment.GameStateManager.SwitchTo("playingState");
        };
        myControl.Controls.Add(myAnotherControl);

        //створюємо питання
        GameTests.Question question = GameTests.TestManager.GetRandomQuestion();
        myAnotherControl = new Label(new Rectangle(30, 30, 0, 0), question.Text, "Question");
        myControl.Controls.Add(myAnotherControl);

        //створюємо відповіді на питання
        int i = 1;
        if (question.Answers.RightCount == 1)
        {
            foreach (GameTests.Answer answer in question.Answers)
            {
                myAnotherControl = new RadioButton(new Rectangle(40, 70 * i, 20, 20), answer.Text, "answer" + i);
                myControl.Controls.Add(myAnotherControl);
                i++;
            }
        }
        else
        {
            foreach (GameTests.Answer answer in question.Answers)
            {
                myAnotherControl = new CheckBox(new Rectangle(40, 70 * i, 20, 20), answer.Text, "answer" + i);
                myControl.Controls.Add(myAnotherControl);
                i++;
            }
        }
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

