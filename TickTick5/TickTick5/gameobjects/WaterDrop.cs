using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using RamGecXNAControls;
using RamGecXNAControls.ExtendedControls;


class WaterDrop : SpriteGameObject
{
    
    TextGameObject textAboveWater;
    GUIControl myControl;                                                                               //вікно
    GUIControl myAnotherControl;                                                                        //елементи управління
    RamGecXNAControls.Button buttonOK;
    int[] answers;

    protected float bounce;
    public int result;

    public WaterDrop(TextGameObject textAboveWater, int layer = 0, string id = "")
        : base("Sprites/spr_water", layer, id) 
    {
        this.textAboveWater = textAboveWater;
        this.result = 0;
    }

    public override void Update(GameTime gameTime)
    {
        double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + Position.X;
        bounce = (float)Math.Sin(t) * 0.2f;
        position.Y += bounce;
        Player player = GameWorld.Find("player") as Player;
        PlayingState playingState = (GameEnvironment.GameStateManager.CurrentGameState as PlayingState);
        if (playingState != null)
        {
            var guiManager = playingState.GUIManager;

            if (this.visible && this.CollidesWith(player))
            {
                playingState.questionState = true;
                
                //створюємо вікно
                myControl = new Window(new Rectangle(100, 100, GameEnvironment.Screen.X - 150, GameEnvironment.Screen.Y - 150), "Питання   іыяхёї");
                guiManager.Controls.Add(myControl);

                //створюємо кнопку "Підтвердити"
                buttonOK = new RamGecXNAControls.Button(new Rectangle(myControl.Bounds.Width / 2 - 180, myControl.Bounds.Height - 120, 120, 60), "Підтвердити", "OK");
                buttonOK.Hint = "Відповісти на питання";
                buttonOK.OnClick += (sender) =>
                {
                    myControl.Visible = false;
                    playingState.questionState = false;
                };

                //створюємо кнопку "Закрити вікно"
                myAnotherControl = new RamGecXNAControls.Button(new Rectangle(myControl.Bounds.Width / 2 + 60, myControl.Bounds.Height - 120, 120, 60), "Закрити вікно", "Cancel");
                myAnotherControl.OnClick += (sender) =>
                {
                    myControl.Visible = false;
                    playingState.questionState = false;
                };
                myControl.Controls.Add(myAnotherControl);
                CreateQuestion();

                textAboveWater.Visible = false;
                this.visible = false;
                GameEnvironment.AssetManager.PlaySound("Sounds/snd_watercollected");
            }
        }
        base.Update(gameTime);
    }

    private void CreateQuestion()
    {
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
        this.answers = new int[i];
        buttonOK.OnClick += (sender) =>
        {
            if (question.Answers.RightCount == 1)
            {
                for (i = 0; i < myAnotherControl.Controls.Count; i++) 
                {
                    if ((myAnotherControl.GetControl("answer" + i) as RadioButton).Checked)
                    {
                        answers[0] = i;
                        break;
                    }
                }
            }
            else 
            {
                int j=0;
                for (i = 0; i < myAnotherControl.Controls.Count; i++)
                {
                    if ((myAnotherControl.GetControl("answer" + i) as CheckBox).Checked)
                    {
                        answers[j] = i;
                        j++;
                    }
                }
            }
            GameTests.AnswerInfo info = question.AreRightAnswers(answers);
            if (info.RightAnswersCount == question.Answers.RightCount)
                result++;
        };
        myControl.Controls.Add(buttonOK);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
    }
}
