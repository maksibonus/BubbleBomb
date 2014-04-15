using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        // Ініціалізує поля структури переданими значеннями.
        public AnswerInfo(int _rightCount, int _wrongCount)
        {
            rightAnswersCount = _rightCount;
            wrongAnswersCount = _wrongCount;
        }

        #endregion Конструктори

        #region Властивості

        /// <summary>
        /// Повертає чи задає кількість правильних відповідей.
        /// </summary>
        public int RightAnswersCount
        {
            internal set
            {
                rightAnswersCount = value;
            }
            get
            {
                return rightAnswersCount;
            }
        }

        /// <summary>
        /// Повертає чи задає кількість неправильних відповідей.
        /// </summary>
        public int WrongAnswersCount
        {
            internal set
            {
                wrongAnswersCount = value;
            }
            get
            {
                return wrongAnswersCount;
            }
        }

        /// <summary>
        /// Повертає загальну кількість відповідей.
        /// </summary>
        public int AnswersCount
        {
            get
            {
                return rightAnswersCount + wrongAnswersCount;
            }
        }

        #endregion Властивості
    }
}
