using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTests
{
    /// <summary>
    /// Перерахування типів запитань.
    /// </summary>
    public enum QuestionType
    {
        /// <summary>
        /// Перемикач (одна правильна відповідь).
        /// </summary>
        RadioButton,

        /// <summary>
        /// Прапорець (декілька правильних відповідей).
        /// </summary>
        CheckBox
    }
}
