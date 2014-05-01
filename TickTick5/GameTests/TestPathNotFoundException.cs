using System;

namespace GameTests
{
    /// <summary>
    /// Виключення, яке виникає, коли шлях, де зберігаються файли із запитаннями, не знайдено.
    /// </summary>
    public class TestPathNotFoundException : Exception
    {
        /// <summary>
        /// Ініціаліє повідомлення про помилку вказаним ім'ям шляху.
        /// </summary>
        /// <param name="path">Ім'я шляху.</param>
        public TestPathNotFoundException(string path)
            : base(String.Format(Properties.Settings.Default.PathNotFoundExceptionMessage, path)) { }
    }
}
