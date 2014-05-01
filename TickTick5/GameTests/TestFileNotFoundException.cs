using System;

namespace GameTests
{
    /// <summary>
    /// Виключення, яке виникає, коли файл із запитаннями відсутній.
    /// </summary>
    public class TestFileNotFoundException : Exception
    {
        /// <summary>
        /// Ініціаліє повідомлення про помилку вказаним ім'ям файлу.
        /// </summary>
        /// <param name="filename">Ім'я файлу із запитаннями.</param>
        public TestFileNotFoundException(string filename)
            : base(String.Format(Properties.Settings.Default.FileNotFoundExceptionMessage, filename)) { }
    }
}
