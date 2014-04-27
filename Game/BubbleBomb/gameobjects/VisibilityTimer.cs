using Microsoft.Xna.Framework;

/// <summary>
/// Клас, що представляє собою об'єкт, який перебуває на екрані лише деякий час.
/// </summary>
class VisibilityTimer : GameObject
{
    #region Поля класу

    /// <summary>
    /// Об'єкт.
    /// </summary>
    protected GameObject target;

    /// <summary>
    /// Показує, скільки залишилося часу до програшу.
    /// </summary>
    protected float timeleft;

    /// <summary>
    /// Загальна тривалість показу на екрані.
    /// </summary>
    protected float totaltime;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void Update(GameTime gameTime)
    {
        timeleft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timeleft <= 0)
            target.Visible = false;
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public VisibilityTimer(GameObject target, int layer=0, string id = "")
        : base(layer, id)
    {
        totaltime = 3;
        timeleft = 3;
        this.target = target;
    }

    #endregion Конструктори

    #region Методи

    /// <summary>
    /// Виводить об'єкт на екран
    /// </summary>
    public void StartVisible()
    {
        timeleft = totaltime;
        target.Visible = true;
    }

    #endregion Методи
}