using System.Collections.Generic;
using Microsoft.Xna.Framework;

/// <summary>
/// Клас, що відповідає за відображення анімацій.
/// </summary>
public class AnimatedGameObject : SpriteGameObject
{
    #region Поля класу

    /// <summary>
    /// Зв'язує імена з об'єктами анімацій.
    /// </summary>
    protected Dictionary<string,Animation> animations;

    #endregion Поля класу

    #region Реалізація інтерфейсів

    // Реалізуємо інтерфейс IGameLoopObject.
    public override void Update(GameTime gameTime)
    {
        if (sprite == null)
            return;
        Current.Update(gameTime);
        base.Update(gameTime);
    }

    #endregion Реалізація інтерфейсів

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу переданими значеннями.
    /// </summary>
    public AnimatedGameObject(int layer = 0, string id = "")
        : base("", layer, id)
    {
        animations = new Dictionary<string, Animation>();
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає спрайт як об'єкт класу Animation.
    /// </summary>
    public Animation Current
    {
        get { return sprite as Animation; }
    }

    #endregion Властивості

    #region Методи

    /// <summary>
    /// Завантажує спрайти до ОП і додає в словник анімацій.
    /// </summary>
    /// <param name="assetname">Шлях до ресурсу(зображення).</param>
    /// <param name="id">Програмне ім'я зображення.</param>
    /// <param name="looping">Прапорець, що вказує, чи повторюється анімація.</param>
    /// <param name="frametime">Тривалість відображення одного кадру.</param>
    public void LoadAnimation(string assetname, string id, bool looping, 
                              float frametime = 0.1f)
    {
        Animation anim = new Animation(assetname, looping, frametime);
        animations[id] = anim;        
    }

    /// <summary>
    /// Запускає анімацію.
    /// </summary>
    /// <param name="id">Програмне ім'я зображення.</param>
    public void PlayAnimation(string id)
    {
        if (sprite == animations[id])
            return;
        if (sprite != null)
            animations[id].Mirror = sprite.Mirror;
        animations[id].Play();
        sprite = animations[id];
        origin = new Vector2(sprite.Width / 2, sprite.Height);        
    }

    #endregion Методи
}