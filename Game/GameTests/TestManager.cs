using System;
using System.Xml;
using System.IO;

namespace GameTests
{
    /// <summary>
    /// Клас, що забезпечує роботу з Xml-файлами тестів.
    /// </summary>
    public static class TestManager
    {
        #region Поля класу

        /// <summary>
        /// Колекція запитань.
        /// </summary>
        private static readonly QuestionCollection questions;

        #endregion Поля класу

        #region Конструктори

        /// <summary>
        /// Ініціалізує колекцію запитань.
        /// </summary>
        static TestManager()
        {
            questions = new QuestionCollection();
        }

        #endregion Конструктори

        #region Властивості

        /// <summary>
        /// Повертає колекцію запитань.
        /// </summary>
        public static QuestionCollection Questions
        {
            get { return questions; }
        }

        #endregion Властивості

        #region Методи

        /// <summary>
        /// Повертає ім'я файлу для певного рівня гри.
        /// </summary>
        /// <param name="level">Рівень гри.</param>
        /// <returns>Ім'я файлу переданого рівня гри.</returns>
        private static string GetFileName(int level)
        {
            return Properties.Settings.Default.TestFilePath + '\\' + Properties.Settings.Default.FilePrefix + level + ".xml";
        }

        /// <summary>
        /// Шифрує вхідний потік і записує зашифровану інформацію в файл.
        /// </summary>
        /// <param name="filename">Ім'я файлу, в який треба записати зашифровану інформацію із вхідного потоку.</param>
        /// <param name="inputStream">Вхідний потік.</param>
        private static void Encrypt(string filename, Stream inputStream)
        {
            FileStream outputStream = File.Open(filename, FileMode.Create);

            inputStream.Position = 0;

            for (long l = 0, length = inputStream.Length; l < length; l++)
                outputStream.WriteByte((byte) (inputStream.ReadByte() ^ Properties.Settings.Default.EncryptionKey));
            
            inputStream.Close();
            outputStream.Close();
        }

        /// <summary>
        /// Розшифровує файл у вихідний потік типу <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="filename">Ім'я файлу, інформацію з якого треба розшифрувати.</param>
        /// <returns>Потік типу <see cref="MemoryStream"/>.</returns>
        private static MemoryStream Decrypt(string filename)
        {
            MemoryStream outputStream = new MemoryStream();
            FileStream inputStream = File.Open(filename, FileMode.Open);

            for (long l = 0, length = inputStream.Length; l < length; l++)
                outputStream.WriteByte((byte)(inputStream.ReadByte() ^ Properties.Settings.Default.EncryptionKey));

            inputStream.Close();

            outputStream.Position = 0;
            return outputStream;
        }

        /// <summary>
        /// Створює порожній Xml-файл з кореневим елементом для певного рівня гри.
        /// </summary>
        /// <param name="level">Рівень гри, порожній файл для якого треба створити.</param>
        /// <exception cref="TestPathNotFoundException">Виключення, яке виникає, коли шлях,
        /// де зберігаються файли із запитаннями, не знайдено.</exception>
        public static void CreateEmptyXml(int level)
        {
            string filename = GetFileName(level);

            if (!Directory.Exists(Properties.Settings.Default.TestFilePath))
                throw new TestPathNotFoundException(Properties.Settings.Default.TestFilePath);

            CreateEmptyXml(filename);
        }

        /// <summary>
        /// Створює порожній Xml-файл з кореневим елементом.
        /// </summary>
        /// <param name="filename">Ім'я файлу, який необхідно створити.</param>
        private static void CreateEmptyXml(string filename)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlWriter xml = XmlWriter.Create(memoryStream);

            xml.WriteStartDocument();
            xml.WriteStartElement(Properties.Settings.Default.RootElement);
            xml.WriteEndElement();
            xml.WriteEndDocument();

            xml.Close();
            Encrypt(filename, memoryStream);
        }

        /// <summary>
        /// Зчитує запитання з файлу для певного рівня.
        /// </summary>
        /// <param name="level">Рівень гри, запитання для його треба зчитати.</param>
        /// <exception cref="TestPathNotFoundException">Виключення, яке виникає, коли шлях,
        /// де зберігаються файли із запитаннями, не знайдено.</exception>
        /// <exception cref="TestFileNotFoundException">Виключення, яке виникає, коли файл із запитаннями відсутній.</exception>
        /// <exception cref="TestFileIsCorruptedException">Виключення, яке виникає, коли файл із запитаннями неможливо
        /// коректно прочитати.</exception>
        public static void Load(int level)
        {
            string filename = GetFileName(level);

            if (!Directory.Exists(Properties.Settings.Default.TestFilePath))
                throw new TestPathNotFoundException(Properties.Settings.Default.TestFilePath);

            if (!File.Exists(filename))
                throw new TestFileNotFoundException(filename);

            Load(filename);
        }

        /// <summary>
        /// Зчитує дані з Xml-файлу.
        /// </summary>
        /// <param name="filename">Ім'я файлу з тестовими запитаннями.</param>
        /// <exception cref="TestFileIsCorruptedException">Виключення, яке виникає, коли файл із запитаннями неможливо
        /// коректно прочитати.</exception>
        private static void Load(string filename)
        {
            if (questions.Count > 0)
                questions.Clear();

            XmlReader xml = XmlReader.Create(Decrypt(filename));
            Question question = new Question();
            Answer answer = new Answer();

            try
            {
                while (xml.Read())
                {
                    switch (xml.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (xml.Name == Properties.Settings.Default.QuestionElement)
                            {
                                xml.MoveToAttribute(Properties.Settings.Default.QuestionTextAttribute);
                                question.Text = xml.Value;
                            }
                            if (xml.Name == Properties.Settings.Default.AnswerElement)
                            {
                                if (xml.HasAttributes)
                                {
                                    xml.MoveToAttribute(Properties.Settings.Default.AnswerRightAttribute);
                                    answer.IsRight = xml.Value == Properties.Settings.Default.AnswerRightValue;
                                }
                            }
                            break;

                        case XmlNodeType.Text:
                            answer.Text = xml.Value;
                            break;

                        case XmlNodeType.EndElement:
                            if (xml.Name == Properties.Settings.Default.QuestionElement)
                            {
                                questions.Add(question);
                                question = new Question();
                            }
                            if (xml.Name == Properties.Settings.Default.AnswerElement)
                            {
                                question.Answers.Add(answer);
                                answer = new Answer();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TestFileIsCorruptedException(filename, ex.Message);
            }
            finally
            {
                xml.Close();
            }
        }

        /// <summary>
        /// Зберігає запитання в файл для певного рівня.
        /// </summary>
        /// <param name="level">Рівень гри, запитання для його треба зберегти.</param>
        /// <exception cref="TestPathNotFoundException">Виключення, яке виникає, коли шлях,
        /// де зберігаються файли із запитаннями, не знайдено.</exception>
        public static void Save(int level)
        {
            string filename = GetFileName(level);

            if (!Directory.Exists(Properties.Settings.Default.TestFilePath))
                throw new TestPathNotFoundException(Properties.Settings.Default.TestFilePath);

            Save(filename);
        }

        /// <summary>
        /// Зберігає колекцію запитань як Xml-файл.
        /// </summary>
        /// <param name="filename">Ім'я файлу для збереження.</param>
        private static void Save(string filename)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlWriter xml = XmlWriter.Create(memoryStream);

            xml.WriteStartDocument();
            xml.WriteStartElement(Properties.Settings.Default.RootElement);

            foreach (Question question in questions)
            {
                xml.WriteStartElement(Properties.Settings.Default.QuestionElement);
                xml.WriteAttributeString(Properties.Settings.Default.QuestionTextAttribute, question.Text);
                foreach (Answer answer in question.Answers)
                {
                    xml.WriteStartElement(Properties.Settings.Default.AnswerElement);
                    if (answer.IsRight)
                        xml.WriteAttributeString(Properties.Settings.Default.AnswerRightAttribute,
                                                 Properties.Settings.Default.AnswerRightValue);
                    xml.WriteValue(answer.Text);
                    xml.WriteEndElement();
                }
                xml.WriteEndElement();
            }

            xml.WriteEndElement();
            xml.WriteEndDocument();

            xml.Close();
            Encrypt(filename, memoryStream);
        }

        /// <summary>
        /// Повертає випадкове запитання.
        /// </summary>
        /// <returns>Випадкове запитання.</returns>
        public static Question GetRandomQuestion()
        {
            return questions.GetRandomQuestion();
        }

        #endregion Методи
    }
}
