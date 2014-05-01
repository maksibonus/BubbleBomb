using System;
using System.Windows.Forms;
using GameTests;

namespace TestEditor
{
    /// <summary>
    /// Елемент керування, що уявляє собою відповідь на запитання.
    /// </summary>
    public partial class AnswerControl : UserControl
    {
        #region Конструктори

        /// <summary>
        /// Ініціалізує новий екземпляр класу значеннями за замовчуванням.
        /// </summary>
        public AnswerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ініціалізує новий екземпляр класу переданими значеннями.
        /// </summary>
        /// <param name="answerText">Текст відповіді.</param>
        /// <param name="isRightAnswer">Прапорець, який вказує, чи є відповідь вірною.</param>
        public AnswerControl(string answerText, bool isRightAnswer) : this()
        {
            Text = answerText;
            IsRightAnswer = isRightAnswer;
        }

        #endregion Конструктори

        #region Властивості

        /// <summary>
        /// Повертає або задає текст відповіді.
        /// </summary>
        public override sealed string Text
        {
            get { return txtAnswer.Text; }
            set { txtAnswer.Text = value; }
        }

        /// <summary>
        /// Повертає або задає прапорець, що визначає, чи є відповідь вірною.
        /// </summary>
        public bool IsRightAnswer
        {
            get { return chkRight.Checked; }
            set { chkRight.Checked = value; }
        }

        #endregion Властивості

        #region Методи

        /// <summary>
        /// Задає повідомлення про помилку для даного елементу.
        /// </summary>
        /// <param name="errorProvider">Об'єкт класу <see cref="ErrorProvider"/>.</param>
        /// <param name="message">Текст повідомлення.</param>
        public void SetError(ErrorProvider errorProvider, string message)
        {
            errorProvider.SetError(lblAnswer, "");

            if (string.IsNullOrEmpty(message))
                return;

            errorProvider.SetIconAlignment(lblAnswer, ErrorIconAlignment.MiddleLeft);
            errorProvider.SetError(lblAnswer, message);
            System.Media.SystemSounds.Beep.Play();
        }

        /// <summary>
        ///  Перетворює поточний об'єкт у об'єкт класу <see cref="Answer"/>.
        /// </summary>
        /// <returns>Новий об'єкт класу <see cref="Answer"/>.</returns>
        public Answer ToAnswer()
        {
            return new Answer(txtAnswer.Text, chkRight.Checked);
        }

        #endregion Методи

        #region Події

        /// <summary>
        /// Виникає, коли користувач натискає кнопку видалення.
        /// </summary>
        public event EventHandler ButtonRemoveClick
        {
            add { btnRemove.Click += value; }
            remove { btnRemove.Click -= value; }
        }

        /// <summary>
        /// Виникає, коли змінюється текст запитання.
        /// </summary>
        public event EventHandler AnswerTextChanged
        {
            add { txtAnswer.TextChanged += value; }
            remove { txtAnswer.TextChanged -= value; }
        }

        /// <summary>
        /// Виникає, коли змінюється стан прапорця <see cref="IsRightAnswer"/>.
        /// </summary>
        public event EventHandler AnswerCheckedChanged
        {
            add { chkRight.CheckedChanged += value; }
            remove { chkRight.CheckedChanged -= value; }
        }

        #endregion Події
    }
}