using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GameTests
{
    /// <summary>
    /// Колекція запитань.
    /// </summary>
    public class QuestionCollection : IEnumerable, IEnumerator
    {
        #region Поля класу

        /// <summary>
        /// Список запитань.
        /// </summary>
        List<Question> questions;

        /// <summary>
        /// Список номерів тих запитань, які ще не були поставлені користувачу.
        /// </summary>
        List<int> unusedQuestionNumbers;

        /// <summary>
        /// Генератор псевдовипадкових чисел.
        /// </summary>
        Random random;

        /// <summary>
        /// Індекс, що використовується інтерфейсом IEnumerator.
        /// </summary>
        int index;

        #endregion Поля класу

        #region Реалізація інтерфейсів

        // Реалізуємо інтерфейс IEnumerable.
        public IEnumerator GetEnumerator()
        {
            return this;
        }

        // Реалізуємо інтерфейс IEnumerable.
        public bool MoveNext()
        {
            if (index == questions.Count - 1)
            {
                Reset();
                return false;
            }

            index++;
            return true;
        }

        public void Reset()
        {
            index = -1;
        }

        public object Current
        {
            get
            {
                return questions[index];
            }
        }

        #endregion Реалізація інтерфейсів

        #region Конструктори

        /// <summary>
        /// Ініціалізує поля класу початковими значеннями за замовчуванням.
        /// </summary>
        public QuestionCollection()
        {
            questions = new List<Question>();
            unusedQuestionNumbers = new List<int>();
            random = new Random();
            index = -1;
        }

        #endregion Конструктори

        #region Індексатори

        /// <summary>
        /// Повертає запитання за вказаним номером.
        /// </summary>
        /// <param name="i">Номер запитання.</param>
        /// <returns>Запитання за вказаним номером.</returns>
        public Question this[int i]
        {
            get
            {
                return questions[i];
            }
        }

        #endregion Індексатори

        #region Властивості

        /// <summary>
        /// Повертає кількість запитань у колекції.
        /// </summary>
        public int Count
        {
            get
            {
                return questions.Count;
            }
        }

        #endregion Властивості

        #region Методи

        /// <summary>
        /// Додає запитання до колекції.
        /// </summary>
        /// <param name="question">Запитання, яке необхідно додати до колекції.</param>
        internal void Add(Question question)
        {
            questions.Add(question);
        }

        /// <summary>
        /// Очищує колекцію запитань.
        /// </summary>
        internal void Clear()
        {
            questions.Clear();
            unusedQuestionNumbers.Clear();
            Reset();
        }

        /// <summary>
        /// Повертає випадкове запитання.
        /// </summary>
        /// <returns>Випадкове запитання.</returns>
        public Question GetRandomQuestion()
        {
            if (questions.Count == 0)
                return null;
            
            int randomNumber;
            Question randomQuestion;

            /* Якщо список номерів питань, які не були поставлені користувачу, порожній,
             * усі запитання були поставлені користувачу або жодного запитання не було поставлено.
             * Отже, заповнюємо список номерами усіх запитань, що є в колекції. */
            if (unusedQuestionNumbers.Count == 0)
                ReloadUnusedQuestionNumbers();

            /* Метод Random.Next(int maxValue) повертає число, яке менше за maxValue, попри те, що
             * у документації зазначено "число, яке не перебільшує максимально допустиме значення".
             * Помилка у перекладі з англійської версії документації. */
            randomNumber = random.Next(unusedQuestionNumbers.Count);

            // Отримуємо випадкове запитання.
            randomQuestion = questions[unusedQuestionNumbers[randomNumber]];

            // Видаляємо номер запитання зі списку номерів питань, які не були поставлені користувачу.
            unusedQuestionNumbers.RemoveAt(randomNumber);

            return randomQuestion;
        }

        /// <summary>
        /// Заповнює список номерів питань, що не були поставлені користувачу,
        /// номерами усіх питань колекції.
        /// </summary>
        private void ReloadUnusedQuestionNumbers()
        {
            unusedQuestionNumbers.Clear();
            for (int i = 0; i < questions.Count; i++)
                unusedQuestionNumbers.Add(i);
        }

        #endregion Методи
    }
}
