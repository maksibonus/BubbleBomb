using System;

namespace GameTests
{
    /// <summary>
    /// Виключення, яке виникає, коли файл із запитаннями неможливо коректно прочитати.
    /// </summary>
    public class TestFileIsCorruptedException : Exception
    {
        /// <summary>
        /// Ініціаліє повідомлення про помилку вказаним ім'ям файлу.
        /// </summary>
        /// <param name="filename">Ім'я файлу із запитаннями.</param>
        public TestFileIsCorruptedException(string filename)
            : base(String.Format(Properties.Settings.Default.FileIsCorruptedExceptionMessage, filename))
        {
            PrimaryMessage = "";
        }

        /// <summary>
        /// Ініціаліє повідомлення про помилку вказаним ім'ям файлу і первинним текстом повідомлення про помилку.
        /// </summary>
        /// <param name="filename">Ім'я файлу із запитаннями.</param>
        /// <param name="primaryMessage">Первинне повідомлення про помилку.</param>
        public TestFileIsCorruptedException(string filename, string primaryMessage)
            : this(filename)
        {
            PrimaryMessage = primaryMessage;
        }

        /// <summary>
        /// Первинне повідомлення про помилку.
        /// </summary>
        public string PrimaryMessage { get; private set; }
    }
}
