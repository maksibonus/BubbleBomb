using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using RamGecXNAControls;
using RamGecXNAControls.ExtendedControls;


class WaterDrop : SpriteGameObject
{
    protected float bounce;
    TextGameObject textAboveWater;
    
    GUIControl myControl;                                                                               //вікно
    GUIControl myAnotherControl;                                                                        //елементи управління

    public WaterDrop(TextGameObject textAboveWater, int layer = 0, string id = "")
        : base("Sprites/spr_water", layer, id) 
    {
        this.textAboveWater = textAboveWater;
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
                //створюємо вікно
                myControl = new Window(new Rectangle(50, 50, GameEnvironment.Screen.X - 100, GameEnvironment.Screen.Y - 100), "Питання   іыяхёї");
                //myControl.Enabled = true;
                //myControl.Focused = true;
                //myControl.Visible = true;



                guiManager.Controls.Add(myControl);

                //створюємо кнопку "Підтвердити"
                myAnotherControl = new RamGecXNAControls.Button(new Rectangle(myControl.Bounds.Width / 2 - 180, myControl.Bounds.Height - 120, 120, 60), "Підтвердити", "OK");
                myAnotherControl.Hint = "Tooltip text";
                myControl.Controls.Add(myAnotherControl);

                //створюємо кнопку "Закрити вікно"
                myAnotherControl = new RamGecXNAControls.Button(new Rectangle(myControl.Bounds.Width / 2 + 60, myControl.Bounds.Height - 120, 120, 60), "Закрити вікно", "Cancel");
                myAnotherControl.OnClick += (sender) =>
                {
                    myControl.Visible = false;
                };
                myControl.Controls.Add(myAnotherControl);
                CreateQuestion();

                //GameEnvironment.GameStateManager.SwitchTo("questionState");
                textAboveWater.Visible = false;
                this.visible = false;
                GameEnvironment.AssetManager.PlaySound("Sounds/snd_watercollected");
            }

        }
        //if (myControl != null)
        //{
        //    int z = -1;
        //    foreach (GUIControl control in guiManager.Controls)
        //        if (control.ZIndex > z)
        //            z = control.ZIndex;
        //    myControl.ZIndex = z + 1;
        //    guiManager.Controls.Sort();
        //}
       

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
        myControl.ZIndex = 101;
        for (i = 0; i < myControl.Controls.Count; i++)
            myControl.Controls[i].ZIndex = 99;
        myControl.Controls.Sort();
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
    }
}
