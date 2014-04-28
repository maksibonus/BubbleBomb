using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

/// <summary>
/// Клас, що відповідає за роботу з ресурсами.
/// </summary>
public class AssetManager
{
    #region Поля класу

    /// <summary>
    /// Компонент, який завантажує об'єкти з бінарних файлів.
    /// </summary>
    protected ContentManager contentManager;

    #endregion Поля класу

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public AssetManager(ContentManager Content)
    {
        this.contentManager = Content;
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає компонент, який завантажує об'єкти з бінарних файлів.
    /// </summary>
    public ContentManager Content
    {
        get { return contentManager; }
    }

    #endregion Властивості

    #region Методи

    /// <summary>
    /// Повертає зображення.
    /// </summary>
    /// <param name="assetName">Ім'я файлу.</param>
    /// <returns>Зображення, в формі двумірного масиву пікселів.</returns>
    public Texture2D GetSprite(string assetName)
    {
        if (assetName == "")
            return null;
        return contentManager.Load<Texture2D>(assetName);
    }

    /// <summary>
    /// Відтворює звук.
    /// </summary>
    /// <param name="assetName">Ім'я файлу.</param>
    public void PlaySound(string assetName)
    {
        SoundEffect snd = contentManager.Load<SoundEffect>(assetName);
        snd.Play();
    }

    /// <summary>
    /// Відтворює музичну доріжку.
    /// </summary>
    /// <param name="assetName">Ім'я файлу.</param>
    /// <param name="repeat">Вказує, чи повторювати відтворення.</param>
    public void PlayMusic(string assetName, bool repeat = true)
    {
        MediaPlayer.IsRepeating = repeat;
        MediaPlayer.Play(contentManager.Load<Song>(assetName));
    }

    /// <summary>
    /// Зупиняє музичну доріжку.
    /// </summary>
    public void StopMusic()
    {
        MediaPlayer.Stop();
    }

    #endregion Методи
}