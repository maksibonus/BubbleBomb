using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTests
{
    /// <summary>
    /// Клас, що зберігає в собі запитання та відповіді на нього.
    /// </summary>
    public class Question
    {
        #region Поля класу

        /// <summary>
        /// Текст запитання.
        /// </summary>
        string text;

        /// <summary>
        /// Колекція відповідей на запитання.
        /// </summary>
        AnswerCollection answers;

        #endregion Поля класу

        #region Конструктори

        /// <summary>
        /// Ініціалізує поля класу початковими значеннями за замовчуванням.
        /// </summary>
        public Question()
        {
            text = "";
            answers = new AnswerCollection();
        }

        #endregion Конструктори

        #region Властивості

        /// <summary>
        /// Повертає текст запитання.
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
        /// Повертає тип запитання.
        /// </summary>
        public QuestionType Type
        {
            get
            {
                return answers.RightCount > 1 ? QuestionType.CheckBox : QuestionType.RadioButton;
            }
        }

        /// <summary>
        /// Повертає колекцію відповідей на запитання.
        /// </summary>
        public AnswerCollection Answers
        {
            get
            {
                return answers;
            }
        }

        #endregion Властивості

        #region Методи

        /// <summary>
        /// Повідомляє, чи є вірною відповідь за вказаним номером.
        /// </summary>
        /// <param name="answerIndex">Номер відповіді.</param>
        /// <returns>true, якщо відповідь є вірною, інакше - false.</returns>
        public bool IsRightAnswer(int answerIndex)
        {
            return answers[answerIndex].IsRight;
        }

        /// <summary>
        /// Повідомляє, чи є вірною відповідь за вказаним текстом.
        /// </summary>
        /// <param name="answerText">Текст відповіді.</param>
        /// <returns>true, якщо відповідь є вірною, інакше - false.</returns>
        public bool IsRightAnswer(string answerText)
        {
            return answers[answerText].IsRight;
        }

        /// <summary>
        /// Повідомляє про кількість правильних і неправильних відповідей.
        /// </summary>
        /// <param name="answersIndexes">Масив номерів відповідей.</param>
        /// <returns>Структура, що містить інформацію про кількість правильних та неправильних відповідей.</returns>
        public AnswerInfo AreRightAnswers(int[] answersIndexes)
        {
            AnswerInfo answerInfo = new AnswerInfo();

            answerInfo.RightAnswersCount = 0;
            answerInfo.WrongAnswersCount = 0;

            foreach (int answerIndex in answersIndexes)
            {
                if (this.answers[answerIndex].IsRight)
                    answerInfo.RightAnswersCount++;
                else
                    answerInfo.WrongAnswersCount++;
            }

            return answerInfo;
        }

        /// <summary>
        /// Повідомляє про кількість правильних і неправильних відповідей.
        /// </summary>
        /// <param name="answersTextStrings">Масив текстів відповідей.</param>
        /// <returns>Структура, що містить інформацію про кількість правильних та неправильних відповідей.</returns>
        public AnswerInfo AreRightAnswers(string[] answersTextStrings)
        {
            AnswerInfo answerInfo = new AnswerInfo();

            answerInfo.RightAnswersCount = 0;
            answerInfo.WrongAnswersCount = 0;

            foreach (string answerText in answersTextStrings)
            {
                if (this.answers[answerText].IsRight)
                    answerInfo.RightAnswersCount++;
                else
                    answerInfo.WrongAnswersCount++;
            }

            return answerInfo;
        }

        /// <summary>
        /// Повідомляє про кількість правильних і неправильних відповідей.
        /// </summary>
        /// <param name="answerArray">Масив відповідей.</param>
        /// <returns>Структура, що містить інформацію про кількість правильних та неправильних відповідей.</returns>
        public AnswerInfo AreRightAnswers(Answer[] answerArray)
        {
            AnswerInfo answerInfo = new AnswerInfo();

            answerInfo.RightAnswersCount = 0;
            answerInfo.WrongAnswersCount = 0;

            foreach (Answer answer in answerArray)
            {
                if (answer.IsRight)
                    answerInfo.RightAnswersCount++;
                else
                    answerInfo.WrongAnswersCount++;
            }

            return answerInfo;
        }

        #endregion Методи
    }
}
