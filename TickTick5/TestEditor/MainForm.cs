using System;
using System.Drawing;
using System.Windows.Forms;
using GameTests;

namespace TestEditor
{
    /// <summary>
    /// Форма редагування тестових запитань.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Поля класу

        /// <summary>
        /// Прапорець, що визначає, чи були внесені зміни в області редагування.
        /// </summary>
        private bool hasChanges;

        /// <summary>
        /// Індекс поточного рівня.
        /// </summary>
        private int currentLevelIndex;

        /// <summary>
        /// Індекс поточного запитання.
        /// </summary>
        private int currentQuestionIndex;

        #endregion Поля класу

        #region Конструктори

        /// <summary>
        /// Ініціалізує новий екземпляр класу значеннями за замовчуванням.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            splitContainer.MinimumSize = new Size(splitContainer.Panel1MinSize + splitContainer.SplitterWidth +
                                                  splitContainer.Panel2MinSize, 300);
            hasChanges = false;
            currentLevelIndex = 0;
            currentQuestionIndex = -1;

            txtQuestion.TextChanged += MakeChanges;
            pnlAnswers.ControlAdded += MakeChanges;
            pnlAnswers.ControlRemoved += MakeChanges;
        }

        #endregion Конструктори

        #region Властивості

        /// <summary>
        /// Повертає або задає прапорець, що вказує, чи були внесені зміни в області редагування.
        /// </summary>
        private bool HasChanges
        {
            get { return hasChanges; }
            set { hasChanges = btnOK.Enabled = btnCancel.Enabled = value; }
        }

        #endregion Властивості

        #region Методи

        /// <summary>
        /// Відображає вікно перевірки паролю, після чого вибирає перший рівень гри як початковий для редагування.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            AskPasswordForm askPasswordWindow = new AskPasswordForm();

            askPasswordWindow.Owner = this;
            if (askPasswordWindow.ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
                return;
            }

            cmbLevel.SelectedIndex = 0;
        }

        /// <summary>
        /// Налаштовує роботу над тестами вказаного рівня.
        /// </summary>
        /// <param name="levelIndex">Номер рівня.</param>
        private void SelectLevel(int levelIndex)
        {
            try
            {
                TestManager.Load(levelIndex);
            }
            catch (TestPathNotFoundException ex)
            {
                MessageBox.Show(
                    String.Format("{0}\n{1}", ex.Message, Properties.Settings.Default.ApplicationExitMessage),
                    Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            catch (TestFileNotFoundException ex)
            {
                if (!CreateNewFile(levelIndex))
                {
                    Application.Exit();
                    return;
                }
                MessageBox.Show(
                    String.Format("{0}\n{1}", ex.Message, Properties.Settings.Default.FileNotFoundMessage),
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (TestFileIsCorruptedException ex)
            {
                if (MessageBox.Show(String.Format("{0}\n{1}\n\n{2}\n{3}",
                                                  ex.Message, Properties.Settings.Default.FileIsCorruptedMessage,
                                                  Properties.Settings.Default.AdditionalInfoMessage,
                                                  ex.PrimaryMessage), Text,
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    if (!CreateNewFile(levelIndex))
                    {
                        Application.Exit();
                        return;
                    }
                    SelectLevel(levelIndex);
                    return;
                }
                Application.Exit();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    String.Format("{0}\n{1}", ex.Message, Properties.Settings.Default.ApplicationExitMessage), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            lstQuestions.Items.Clear();
            for (int i = 0; i < TestManager.Questions.Count; i++)
                lstQuestions.Items.Add(TestManager.Questions[i].Text);
            lstQuestions.Items.Add(Properties.Settings.Default.NewQuestionString);

            lstQuestions.SelectedIndex = 0;
        }

        /// <summary>
        /// Налаштовує роботу над вказаним запитанням.
        /// </summary>
        /// <param name="questionIndex">Номер запитання.</param>
        private void SelectQuestion(int questionIndex)
        {
            ClearQuestionInfo();
            
            if (lstQuestions.SelectedIndex == lstQuestions.Items.Count - 1)
            {
                btnRemoveQuestion.Enabled = false;
                HasChanges = false;
                return;
            }

            txtQuestion.Text = TestManager.Questions[questionIndex].Text;
            foreach (Answer answer in TestManager.Questions[questionIndex].Answers)
                AddAnswer(new AnswerControl(answer.Text, answer.IsRight));
            btnRemoveQuestion.Enabled = true;
            HasChanges = false;
        }

        /// <summary>
        /// Створює масив об'єктів типу <see cref="Answer"/> з колекції відповідей об'єкту <see cref="pnlAnswers"/>.
        /// </summary>
        /// <returns>Масивоб'єктів типу <see cref="Answer"/>. </returns>
        private Answer[] GetAnswerArray()
        {
            Answer[] answers = new Answer[pnlAnswers.Controls.Count];
            for (int i = 0; i < pnlAnswers.Controls.Count; i++)
                answers[i] = ((AnswerControl) pnlAnswers.Controls[i]).ToAnswer();
            return answers;
        }

        /// <summary>
        /// Додає запитання до колекції запитань класу <see cref="TestManager"/>, використовуючи дані з форми.
        /// </summary>
        private void AddQuestion()
        {
            Question question = new Question();
            question.Update(txtQuestion.Text, GetAnswerArray());
            TestManager.Questions.Add(question);
        }

        /// <summary>
        /// Оновлює поточне запитання в колекції запитань класу <see cref="TestManager"/>, використовуючи дані з форми.
        /// </summary>
        private void UpdateQuestion()
        {
            TestManager.Questions[lstQuestions.SelectedIndex].Update(txtQuestion.Text, GetAnswerArray());
        }

        /// <summary>
        /// Видаляє поточне запитання з колекції класу <see cref="TestManager"/>.
        /// </summary>
        private void RemoveQuestion()
        {
            TestManager.Questions.RemoveAt(lstQuestions.SelectedIndex);
            lstQuestions.Items.RemoveAt(lstQuestions.SelectedIndex);
            lstQuestions.SelectedIndex = 0;
        }

        /// <summary>
        /// Додає на форму елемент керування, що уявляє собою відповідь.
        /// </summary>
        /// <param name="answerControl">Елемент керування відповіді, який додається.</param>
        private void AddAnswer(AnswerControl answerControl)
        {
            answerControl.Dock = DockStyle.Top;
            answerControl.ButtonRemoveClick += answerControl_ButtonRemoveClick;
            answerControl.AnswerCheckedChanged += MakeChanges;
            answerControl.AnswerTextChanged += MakeChanges;
            pnlAnswers.Controls.Add(answerControl);
        }

        /// <summary>
        /// Видаляє з форми елемент керування, що уявляє собою відповідь.
        /// </summary>
        /// <param name="answerControl">Елемент керування відповіді, що видаляється.</param>
        private void RemoveAnswer(AnswerControl answerControl)
        {
            pnlAnswers.Controls.Remove(answerControl);
        }

        /// <summary>
        /// Очищує область редагування форми.
        /// </summary>
        private void ClearQuestionInfo()
        {
            txtQuestion.Clear();
            pnlAnswers.Controls.Clear();
        }

        /// <summary>
        /// Створює новий порожній файл із запитаннями.
        /// </summary>
        /// <param name="levelIndex">Рівень гри, порожній файл для якого треба створити.</param>
        /// <returns>true - якщо файл було успішно створено, false - якщо ні.</returns>
        private bool CreateNewFile(int levelIndex)
        {
            try
            {
                TestManager.CreateEmptyXml(levelIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    String.Format("{0}\n{1}", ex.Message, Properties.Settings.Default.ApplicationExitMessage), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Зберігає колекцію запитань до файлу відповідного рівня.
        /// </summary>
        private void Save()
        {
            if (currentLevelIndex == 0) return;

            try
            {
                TestManager.Save(currentLevelIndex);
            }
            catch (TestPathNotFoundException ex)
            {
                MessageBox.Show(
                    String.Format("{0}\n{1}", ex.Message, Properties.Settings.Default.ApplicationExitMessage),
                    Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    String.Format("{0}\n{1}", ex.Message, Properties.Settings.Default.ApplicationExitMessage), Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        /// <summary>
        /// Перевіряє, чи можна засвідчити внесені зміни.
        /// </summary>
        /// <returns>true, якщо внесені зміни можна засвідчити, false - якщо ні.</returns>
        private bool CheckInfo()
        {
            bool doesRightAnswerExist = false;

            if (String.IsNullOrWhiteSpace(txtQuestion.Text))
            {
                MessageBox.Show(Properties.Settings.Default.QuestionTextIsEmptyMessage, Text, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            if (pnlAnswers.Controls.Count == 0)
            {
                MessageBox.Show(Properties.Settings.Default.QuestionHasNoAnswerMessage, Text, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            foreach (AnswerControl answerControl in pnlAnswers.Controls)
            {
                if (String.IsNullOrWhiteSpace(answerControl.Text))
                {
                    answerControl.SetError(errorProvider, Properties.Settings.Default.AnswerTextIsEmptyMessage);
                    answerControl.AnswerTextChanged += ClearError;
                    return false;
                }
                if (answerControl.IsRightAnswer)
                    doesRightAnswerExist = true;
            }

            if (!doesRightAnswerExist)
            {
                MessageBox.Show(Properties.Settings.Default.QuestionHasNoRightAnswer, Text, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Засвідчує усі внесені зміни.
        /// </summary>
        /// <returns>true, якщо внесені зміни були засвідчені, false - якщо ні.</returns>
        private bool AcceptChanges()
        {
            if (!CheckInfo())
                return false;

            HasChanges = false;

            if (lstQuestions.SelectedIndex == lstQuestions.Items.Count - 1)
            {
                AddQuestion();
                lstQuestions.Items.Insert(lstQuestions.SelectedIndex,
                                          TestManager.Questions[lstQuestions.SelectedIndex].Text);
                lstQuestions.SelectedIndex = currentQuestionIndex;
            }
            else
                UpdateQuestion();
            
            return true;
        }

        /// <summary>
        /// Видаляє повідомлення про порожній текст повідомлення для елементу керування, що уявляє собою відповідь.
        /// </summary>
        private void ClearError(object sender, EventArgs e)
        {
            AnswerControl answerControl = ((TextBox)sender).Parent as AnswerControl;
            if (answerControl != null)
            {
                answerControl.SetError(errorProvider, "");
                answerControl.AnswerTextChanged -= ClearError;
            }
        }

        /// <summary>
        /// Встановлює значення прапорця <see cref="HasChanges"/> у true.
        /// </summary>
        private void MakeChanges(object sender, EventArgs e)
        {
            HasChanges = true;
        }

        /// <summary>
        /// Викликає метод <see cref="AddAnswer(AnswerControl)"/>.
        /// </summary>
        private void btnAddAnswer_Click(object sender, EventArgs e)
        {
            AddAnswer(new AnswerControl());
        }

        /// <summary>
        /// Викликає метод <see cref="RemoveAnswer(AnswerControl)"/>.
        /// </summary>
        private void answerControl_ButtonRemoveClick(object sender, EventArgs e)
        {
            AnswerControl answerControl = ((Button)sender).Parent as AnswerControl;
            RemoveAnswer(answerControl);
        }

        /// <summary>
        /// Виконує перевірку на відсутність незбережених змін і налаштовує роботу над запитаннями для певного рівня.
        /// </summary>
        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HasChanges)
            {
                if (cmbLevel.SelectedIndex == currentLevelIndex - 1)
                    return;

                DialogResult dialogResult = MessageBox.Show(Properties.Settings.Default.HasChangesMessage, Text,
                                                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                switch (dialogResult)
                {
                    case DialogResult.Cancel:
                        cmbLevel.SelectedIndex = currentLevelIndex - 1;
                        return;

                    case DialogResult.Yes:
                        bool accepted = AcceptChanges();
                        if (!accepted)
                        {
                            cmbLevel.SelectedIndex = currentLevelIndex - 1;
                            return;
                        }
                        break;

                    case DialogResult.No:
                        hasChanges = false;
                        break;
                }
            }

            Save();
            currentLevelIndex = cmbLevel.SelectedIndex + 1;
            SelectLevel(currentLevelIndex);
        }

        /// <summary>
        /// Викликає метод <see cref="SelectQuestion(int)"/>.
        /// </summary>
        private void lstQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstQuestions.SelectedIndex == -1)
            {
                currentQuestionIndex = -1;
                return;
            }

            if (HasChanges)
            {
                if (lstQuestions.SelectedIndex == currentQuestionIndex)
                    return;

                DialogResult dialogResult = MessageBox.Show(Properties.Settings.Default.HasChangesMessage, Text,
                                                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                switch (dialogResult)
                {
                    case DialogResult.Cancel:
                        lstQuestions.SelectedIndex = currentQuestionIndex;
                        return;

                    case DialogResult.Yes:
                        bool accepted = AcceptChanges();
                        if (!accepted)
                        {
                            lstQuestions.SelectedIndex = currentQuestionIndex;
                            return;
                        }
                        break;
                }
            }

            currentQuestionIndex = lstQuestions.SelectedIndex;
            SelectQuestion(lstQuestions.SelectedIndex);
        }

        /// <summary>
        /// Видаляє запитання, якщо користувач згоден.
        /// </summary>
        private void btnRemoveQuestion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Properties.Settings.Default.RemoveQuestionMessage, Text, MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                HasChanges = false;
                RemoveQuestion();
            }
        }

        /// <summary>
        /// Скасовує усі внесені незафіксовані зміни.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            SelectQuestion(lstQuestions.SelectedIndex);
        }

        /// <summary>
        /// Викликає метод <see cref="AcceptChanges()"/>.
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            AcceptChanges();
        }

        /// <summary>
        /// Перевіряє наявність незасвідчених змін перед виходом з програми.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!HasChanges || e.CloseReason == CloseReason.ApplicationExitCall)
                return;

            DialogResult dialogResult = MessageBox.Show(Properties.Settings.Default.HasChangesMessage, Text,
                                                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            switch (dialogResult)
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;

                case DialogResult.Yes:
                    bool accepted = AcceptChanges();
                    if (!accepted)
                        e.Cancel = true;
                    break;
            }
        }

        /// <summary>
        /// Зберігає запитання після закриття вікна.
        /// </summary>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
                Save();
        }

        #endregion Методи
    }
}