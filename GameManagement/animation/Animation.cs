using System;
using Microsoft.Xna.Framework;

/// <summary>
/// Клас, що відповідає за анімацію об'єктів гри.
/// </summary>
public class Animation : SpriteSheet
{
    #region Поля класу

    /// <summary>
    /// Тривалість відображення одного кадру.
    /// </summary>
    protected float frameTime;

    /// <summary>
    /// Прапорець, що вказує, чи повторюється анімація.
    /// </summary>
    protected bool isLooping;

    /// <summary>
    /// Відображує скільки пройшло часу від початку відображення.
    /// </summary>
    protected float time;

    #endregion Поля класу

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public Animation(string assetname, bool isLooping, float frametime = 0.1f) : base(assetname)
    {
        this.frameTime = frametime;
        this.isLooping = isLooping;
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає тривалість відображення одного кадру.
    /// </summary>
    public float FrameTime
    {
        get { return frameTime; }
    }

    /// <summary>
    /// Вказує, чи повторюється анімація.
    /// </summary>
    public bool IsLooping
    {
        get { return isLooping; }
    }

    /// <summary>
    /// Повертає кількість спрайтів.
    /// </summary>
    public int CountFrames
    {
        get { return this.NumberSheetElements; }
    }

    /// <summary>
    /// Вказує, чи закінчилася анімація.
    /// </summary>
    public bool AnimationEnded
    {
        get { return !this.isLooping && sheetIndex >= NumberSheetElements - 1; }
    }

    #endregion Властивості

    #region Методи

    /// <summary>
    /// Оновлює стан об'єкту.
    /// </summary>
    /// <param name="gameTime">Час, який минув від попереднього до поточного стану гри.</param>
    public void Update(GameTime gameTime)
    {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        while (time > frameTime)
        {
            time -= frameTime;
            if (isLooping)
                sheetIndex = (sheetIndex + 1) % this.NumberSheetElements;
            else
                sheetIndex = Math.Min(sheetIndex + 1, this.NumberSheetElements - 1);
        }
    }

    /// <summary>
    /// Переходить до початкового стану відображення анімації.
    /// </summary>
    public void Play()
    {
        this.sheetIndex = 0;
        this.time = 0.0f;
    }

    #endregion Методи
}

