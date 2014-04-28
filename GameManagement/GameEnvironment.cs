using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Основний клас гри, що зберігає у собі все.
/// </summary>
public class GameEnvironment : Game
{
    #region Поля класу

    /// <summary>
    /// Обробляє конфігурацію и управління графічними пристроями.
    /// </summary>
    protected static GraphicsDeviceManager graphics;

    /// <summary>
    /// Включає в себе групу спрайтів для відображення з однаковими параметрами.
    /// </summary>
    public SpriteBatch spriteBatch;

    /// <summary>
    /// Відповідає за обробку клавіш.
    /// </summary>
    protected static InputHelper inputHelper;

    /// <summary>
    /// Масштаб спрайту.
    /// </summary>
    public static Matrix spriteScale;

    /// <summary>
    /// Розмір екрану гри.
    /// </summary>
    protected static Point screen;

    /// <summary>
    /// Відповідає за стани гри.
    /// </summary>
    protected static GameStateManager gameStateManager;

    /// <summary>
    /// Генератор псевдовипадкових чисел.
    /// </summary>
    protected static Random random;

    /// <summary>
    /// відповідає за роботу з ресурсами.
    /// </summary>
    protected static AssetManager assetManager;

    //?
    protected static GameSettingsManager gameSettingsManager;

    #endregion Поля класу

    #region Конструктори

    /// <summary>
    /// Ініціалізує поля класу початковими значеннями за замовчуванням.
    /// </summary>
    public GameEnvironment()
    {
        graphics = new GraphicsDeviceManager(this);
        inputHelper = new InputHelper();
        gameStateManager = new GameStateManager();
        spriteScale = Matrix.CreateScale(1, 1, 1);
        random = new Random();
        assetManager = new AssetManager(Content);
        gameSettingsManager = new GameSettingsManager();
    }

    #endregion Конструктори

    #region Властивості

    /// <summary>
    /// Повертає чи задає розмір екрану гри.
    /// </summary>
    public static Point Screen
    {
        get { return GameEnvironment.screen; }
        set { screen = value; }
    }

    /// <summary>
    /// Повертає генератор псевдовипадкових чисел.
    /// </summary>
    public static Random Random
    {
        get { return random; }
    }

    /// <summary>
    /// Повертає об'єкт класу, що відповідає за роботу з ресурсами.
    /// </summary>
    public static AssetManager AssetManager
    {
        get { return assetManager; }
    }

    public static InputHelper InputHelper
    {
        get { return inputHelper; }
    }

    /// <summary>
    /// Повертає об'єкт класу, що відповідає за стани гри.
    /// </summary>
    public static GameStateManager GameStateManager
    {
        get { return gameStateManager; }
    }

    //?
    public static GameSettingsManager GameSettingsManager
    {
        get { return gameSettingsManager; }
    }

    #endregion Властивості

    #region Методи

    /// <summary>
    /// Відображає гру.
    /// </summary>
    /// <param name="fullscreen">Вказує, чи відображати на весь екран.</param>
    public void SetFullScreen(bool fullscreen = true)
    {
        float scalex = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)screen.X;
        float scaley = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / (float)screen.Y;
        float finalscale = 1f;

        if (!fullscreen)
        {
            if (scalex < 1f || scaley < 1f)
                finalscale = Math.Min(scalex, scaley);
        }
        else
        {
            finalscale = scalex;
            if (Math.Abs(1 - scaley) < Math.Abs(1 - scalex))
                finalscale = scaley;
        }
        graphics.PreferredBackBufferWidth = (int)(finalscale * screen.X);
        graphics.PreferredBackBufferHeight = (int)(finalscale * screen.Y);
        graphics.IsFullScreen = fullscreen;
        graphics.ApplyChanges();
        inputHelper.Scale = new Vector2((float)GraphicsDevice.Viewport.Width / screen.X,
                                        (float)GraphicsDevice.Viewport.Height / screen.Y);
        spriteScale = Matrix.CreateScale(inputHelper.Scale.X, inputHelper.Scale.Y, 1);
        //c.Down = b;
        //a = b * spriteScale;
    }

    /// <summary>
    /// Завантажує контент.
    /// </summary>
    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    /// <summary>
    /// Оброблення натискань клавіш.
    /// </summary>
    protected void HandleInput()
    {
        inputHelper.Update();
        if (inputHelper.KeyPressed(Keys.Escape))
            this.Exit();
        if (inputHelper.KeyPressed(Keys.F5))
            SetFullScreen(!graphics.IsFullScreen);
        gameStateManager.HandleInput(inputHelper);
    }

    /// <summary>
    /// Оновлює гру.
    /// </summary>
    /// <param name="gameTime">Час, який минув від попереднього до поточного стану гри.</param>
    protected override void Update(GameTime gameTime)
    {
        HandleInput();
        gameStateManager.Update(gameTime);
    }

    /// <summary>
    /// Відображає гру.
    /// </summary>
    /// <param name="gameTime">Час, який минув від попереднього до поточного стану гри.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, GameEnvironment.spriteScale);
        gameStateManager.Draw(gameTime, spriteBatch);
        spriteBatch.End();
    }

    #endregion Методи
}