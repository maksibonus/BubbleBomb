using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GameTests
{
    /// <summary>
    /// Клас, що забезпечує роботу із Xml-файлами тестів.
    /// </summary>
    public static class TestManager
    {
        #region Поля класу

        /// <summary>
        /// Шлях до файлу із запитаннями.
        /// </summary>
        static string filepath;

        /// <summary>
        /// Колекція запитань.
        /// </summary>
        static QuestionCollection questions;

        #endregion Поля класу

        #region Конструктори

        /// <summary>
        /// Ініціалізує поля класу початковими значеннями.
        /// </summary>
        static TestManager()
        {
            questions = new QuestionCollection();
            filepath = "gametest.xml";
        }

        #endregion Конструктори

        #region Методи

        /// <summary>
        /// Зчитує дані з Xml-файлу та налаштовує клас для подальшої роботи з ним.
        /// </summary>
        public static void Initialize()
        {
            XmlReader xml = XmlReader.Create(filepath);
            Question question = new Question();
            Answer answer = new Answer();

            if (questions.Count > 0)
                questions.Clear();

            while (xml.Read())
            {
                switch (xml.NodeType)
                {
                    case XmlNodeType.Element:
                        if (xml.Name == "question")
                        {
                            xml.MoveToAttribute("text");
                            question.Text = xml.Value;
                        }
                        if (xml.Name == "answer")
                        {
                            if (xml.HasAttributes)
                            {
                                xml.MoveToAttribute("right");
                                answer.IsRight = xml.Value == "true" ? true : false;
                            }
                        }
                        break;

                    case XmlNodeType.Text:
                        answer.Text = xml.Value;
                        break;

                    case XmlNodeType.EndElement:
                        if (xml.Name == "question")
                        {
                            questions.Add(question);
                            question = new Question();
                        }
                        if (xml.Name == "answer")
                        {
                            question.Answers.Add(answer);
                            answer = new Answer();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Зчитує дані з Xml-файлу та налаштовує клас для подальшої роботи з ним.
        /// </summary>
        /// <param name="_filepath">Розташування файлу з тестовими запитаннями.</param>
        public static void Initialize(string _filepath)
        {
            filepath = _filepath;
            Initialize();
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
