namespace GameTests
{
    /// <summary>
    /// Структура, що містить інформацію про кількість правильних та неправильних відповідей.
    /// </summary>
    public struct AnswerInfo
    {
        #region Поля структури

        /// <summary>
        /// Кількість неправильних відповідей.
        /// </summary>
        int rightAnswersCount;

        /// <summary>
        /// Кількість правильних відповідей.
        /// </summary>
        int wrongAnswersCount;

        #endregion Поля структури

        #region Конструктори

        /// <summary>
        /// Ініціалізує поля структури переданими значеннями.
        /// </summary>
        /// <param name="rightCount">Кількість правильних відповідей.</param>
        /// <param name="wrongCount">Кількість неправильних відповідей.</param>
        internal AnswerInfo(int rightCount, int wrongCount)
        {
            rightAnswersCount = rightCount;
            wrongAnswersCount = wrongCount;
        }

        #endregion Конструктори

        #region Властивості

        /// <summary>
        /// Повертає чи задає кількість правильних відповідей.
        /// </summary>
        public int RightAnswersCount
        {
            get { return rightAnswersCount; }
            internal set { rightAnswersCount = value; }
        }

        /// <summary>
        /// Повертає чи задає кількість неправильних відповідей.
        /// </summary>
        public int WrongAnswersCount
        {
            get { return wrongAnswersCount; }
            internal set { wrongAnswersCount = value; }
        }

        /// <summary>
        /// Повертає загальну кількість відповідей.
        /// </summary>
        public int AnswersCount
        {
            get { return rightAnswersCount + wrongAnswersCount; }
        }

        #endregion Властивості
    }
}
