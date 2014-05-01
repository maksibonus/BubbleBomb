namespace GameTests
{
    /// <summary>
    /// Клас відповіді на запитання.
    /// </summary>
    public class Answer
    {
        #region Поля класу

        /// <summary>
        /// Текст відповіді.
        /// </summary>
        string text;

        /// <summary>
        /// Прапорець, що вказує, чи є відповідь вірною.
        /// </summary>
        bool isRight;

        #endregion Поля класу

        #region Конструктори

        /// <summary>
        /// Ініціалізує новий екземпляр класу значеннями за замовчуванням.
        /// </summary>
        public Answer()
        {
            text = "";
            isRight = false;
        }

        /// <summary>
        /// Ініціалізує новий екземпляр класу переданими значеннями.
        /// </summary>
        /// <param name="text">Текст відповіді.</param>
        /// <param name="isRight">Прапорець, чи є відповідь вірною.</param>
        public Answer(string text, bool isRight)
        {
            this.text = text ?? "";
            this.isRight = isRight;
        }

        #endregion Конструктори

        #region Властивості

        /// <summary>
        /// Повертає текст відповіді.
        /// </summary>
        public string Text
        {
            get { return text; }
            internal set { text = value; }
        }

        /// <summary>
        /// Вказує, чи є відповідь вірною.
        /// </summary>
        public bool IsRight
        {
            get { return isRight; }
            internal set { isRight = value; }
        }

        #endregion Властивості
    }
}
