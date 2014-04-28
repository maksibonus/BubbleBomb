using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using RamGecXNAControls;
using RamGecXNAControls.ExtendedControls;

/// <summary>
/// Клас, що представляє собою краплю з питанням.
/// </summary>
class WaterDrop : SpriteGameObject
{
    #region Поля класу

    /// <summary>
    /// Текст.
    /// </summary>
    TextGameObject textAboveWater;

    /// <summary>
    /// Елементи вікна.
    /// </summary>
    GUIControl myAnotherControl;

    /// <summary>
    /// Кнопка "Підтвердити".
    /// </summary>
    RamGecXNAControls.Button buttonOK;

    /// <summary>
    /// Лист, де зберігаються елементи поточного вікна, які відповідають за питання.
    /// </summary>
    List<GUIControl> elements;

    /// <summary>
    /// Відповідає за рух.
    /// </summary>
    protected float bounce;

    /// <summary>
    /// Вікно.
    /// </summary>
    public static GUIControl myControl;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void Update(GameTime gameTime)
    {
        double t = gameTime.TotalGameTime.TotalSeconds * 20.0f + Position.X;
        bounce = (float)Math.Sin(t) * 0.6f;
        position.Y += bounce;

        Player player = GameWorld.Find("player") as Player;
        PlayingState playingState = (GameEnvironment.GameStateManager.CurrentGameState as PlayingState);
        if (playingState != null)
        {
            var guiManager = playingState.GUIManager;
            guiManager.SetMatrix(GameEnvironment.spriteScale);
            if (this.visible && this.CollidesWith(player) && playingState.questionState==false)
            {
                playingState.questionState = true;
                myControl = new Window(new Rectangle(100, 100, GameEnvironment.Screen.X - 150, GameEnvironment.Screen.Y - 150), "Питання");
                guiManager.Controls.Add(myControl);

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
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public WaterDrop(TextGameObject textAboveWater, int layer = 0, string id = "")
        : base("Sprites/spr_water", layer, id)
    {
        this.textAboveWater = textAboveWater;
        this.elements = new List<GUIControl>();
    }

    #endregion Конструктори

    #region Методи

    /// <summary>
    /// Створює питання
    /// </summary>
    private void CreateQuestion()
    {
        //створюємо питання
        GameTests.Question question = GameTests.TestManager.GetRandomQuestion();
        myAnotherControl = new Label(new Rectangle(30, 30, 0, 0), question.Text, "Question");
        myControl.Controls.Add(myAnotherControl);

        //створюємо відповіді на питання
        int i = 1;
        elements.Clear();
        if (question.Answers.RightCount == 1)
        {
            foreach (GameTests.Answer answer in question.Answers)
            {
                myAnotherControl = new RadioButton(new Rectangle(40, 70 * i, 20, 20), answer.Text, "answer" + i);
                elements.Add(myAnotherControl);
                myControl.Controls.Add(myAnotherControl);
                i++;
            }
        }
        else
        {
            foreach (GameTests.Answer answer in question.Answers)
            {
                myAnotherControl = new CheckBox(new Rectangle(40, 70 * i, 20, 20), answer.Text, "answer" + i);
                elements.Add(myAnotherControl);
                myControl.Controls.Add(myAnotherControl);
                i++;
            }
        }
       
        buttonOK.OnClick += (sender) =>
        {
            List<int> answersIndexes = new List<int>();
            PlayingState playingState = (GameEnvironment.GameStateManager.CurrentGameState as PlayingState);
            if (playingState != null)
            {
                if (question.Answers.RightCount == 1)
                {
                    for (i = 1; i <= elements.Count; i++)
                    {
                        if (elements.Exists(q => q.Name == "answer" + i))
                        {
                            if ((elements.Find(q => q.Name == "answer" + i) as RadioButton).Checked)
                            {
                                answersIndexes.Add(i - 1);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    int j = 0;
                    for (i = 1; i <= elements.Count; i++)
                    {
                        if (elements.Exists(q => q.Name == "answer" + i))
                        {
                            if ((elements.Find(q => q.Name == "answer" + i) as CheckBox).Checked)
                            {
                                answersIndexes.Add(i - 1);
                                j++;
                            }
                        }
                    }
                }
                GameTests.AnswerInfo info = question.AreRightAnswers(answersIndexes.ToArray());
                if (info.RightAnswersCount == question.Answers.RightCount && info.WrongAnswersCount == 0)
                    Result.result++;
                
            }
            elements.Clear();
        };
        myControl.Controls.Add(buttonOK);
    }

    #endregion Методи
}