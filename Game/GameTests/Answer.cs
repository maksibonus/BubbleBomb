using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// Ініціалізує поля класу початковими значеннями за замовчуванням.
        /// </summary>
        public Answer()
        {
            text = "";
            isRight = false;
        }

        #endregion Конструктори

        #region Властивості

        /// <summary>
        /// Повертає текст відповіді.
        /// </summary>
        public string Text
        {
            internal set
            {
                text = value;
            }
            get
            {
                return text;
            }
        }

        /// <summary>
        /// Вказує, чи є відповідь вірною.
        /// </summary>
        public bool IsRight
        {
            internal set
            {
                isRight = value;
            }
            get
            {
                return isRight;
            }
        }

        #endregion Властивості
    }
}
