using System;
using System.Windows.Forms;

namespace TestEditor
{
    /// <summary>
    /// Вікно запиту пароля у користувача.
    /// </summary>
    public partial class AskPasswordForm : Form
    {
        /// <summary>
        /// Ініціалізує новий екземпляр класу <see cref="AskPasswordForm"/>.
        /// </summary>
        public AskPasswordForm()
        {
            InitializeComponent();
            errorProvider.SetIconPadding(txtPassword, 5);
        }

        /// <summary>
        /// Перевіряє правильність уведеного паролю і відображає повідомлення, якщо уведений пароль є невірним.
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == Properties.Settings.Default.Password)
                DialogResult = DialogResult.OK;
            else
            {
                errorProvider.SetError(txtPassword, "");
                errorProvider.SetError(txtPassword, Properties.Settings.Default.IncorrectPasswordMessage);
                System.Media.SystemSounds.Beep.Play();
            }
        }

        /// <summary>
        /// Задає результат діалогу для форми як <see cref="DialogResult.Cancel"/>.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
