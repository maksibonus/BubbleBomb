using System;
using Microsoft.Xna.Framework;

/// <summary>
/// Клас, що представляє собою таймер.
/// </summary>
class TimerGameObject : TextGameObject
{
    #region Поля класу

    /// <summary>
    /// Структура, що показує, скільки залишилося часу до програшу.
    /// </summary>
    protected TimeSpan timeLeft;

    /// <summary>
    /// Прапорець, що вказує, чи запущений таймер.
    /// </summary>
    protected bool running;

    /// <summary>
    /// Множник часу.
    /// </summary>
    protected double multiplier;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void Update(GameTime gameTime)
    {
        if (!running)
            return;
        double totalSeconds = gameTime.ElapsedGameTime.TotalSeconds * 0.6 * multiplier;
        if (WaterDrop.myControl != null && WaterDrop.myControl.Visible)
            totalSeconds = gameTime.ElapsedGameTime.TotalSeconds * 1.1 * multiplier;
        timeLeft -= TimeSpan.FromSeconds(totalSeconds);
        if (timeLeft.Ticks < 0)
            return;
        DateTime timeleft = new DateTime(timeLeft.Ticks);
        this.Text = timeleft.ToString("mm:ss");
        this.color = Color.Yellow;
        if (timeLeft.TotalSeconds <= 10 && (int)timeLeft.TotalSeconds % 2 == 0)
            this.color = Color.Red;
    }

    public override void Reset()
    {
        base.Reset();
        this.timeLeft = TimeSpan.FromMinutes(2);
        this.running = true;
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу початковими значеннями за замовчуванням.
    /// </summary>
    public TimerGameObject(int layer = 0, string id = "")
        : base("Fonts/Hud", layer, id)
    {
        this.multiplier = 1;
        this.running = true;
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Вказує, чи запущений таймер.
    /// </summary>
    public bool Running
    {
        get { return running; }
        set { running = value; }
    }

    /// <summary>
    /// Повертає чи задає множник часу.
    /// </summary>
    public double Multiplier
    {
        get {return multiplier; }
        set { multiplier = value; }
    }

    /// <summary>
    /// Вказує, чи закінчився час.
    /// </summary>
    public bool GameOver
    {
        get { return (timeLeft.Ticks <= 0); }
    }

    #endregion Властивості
}