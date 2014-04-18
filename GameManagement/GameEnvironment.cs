using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class GameEnvironment : Game
{
    protected GraphicsDeviceManager graphics;
    protected SpriteBatch spriteBatch;
    protected InputHelper inputHelper;
    protected Matrix spriteScale;
    
    protected static Point screen;
    protected static GameStateManager gameStateManager;
    protected static Random random;
    protected static AssetManager assetManager;
    protected static GameSettingsManager gameSettingsManager;

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

    public static Point Screen
    {
        get { return GameEnvironment.screen; }
        set { screen = value; }
    }

    public static Random Random
    {
        get { return random; }
    }

    public static AssetManager AssetManager
    {
        get { return assetManager; }
    }

    public static GameStateManager GameStateManager
    {
        get { return gameStateManager; }
    }

    public static GameSettingsManager GameSettingsManager
    {
        get { return gameSettingsManager; }
    }

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
        //подсчет размера экрана игры
        graphics.PreferredBackBufferWidth = (int)(finalscale * screen.X);
        graphics.PreferredBackBufferHeight = (int)(finalscale * screen.Y);
        graphics.IsFullScreen = fullscreen;
        graphics.ApplyChanges();
        inputHelper.Scale = new Vector2((float)GraphicsDevice.Viewport.Width / screen.X,
                                        (float)GraphicsDevice.Viewport.Height / screen.Y);//?
        spriteScale = Matrix.CreateScale(inputHelper.Scale.X, inputHelper.Scale.Y, 1);//масштабирование спрайта
    }

    protected override void LoadContent()
    {
        DrawingHelper.Initialize(this.GraphicsDevice);//класс рендеринга прямоугольного спрайта 
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected void HandleInput()
    {
        inputHelper.Update();
        if (inputHelper.KeyPressed(Keys.Escape))
            this.Exit();
        if (inputHelper.KeyPressed(Keys.F5))
            SetFullScreen(!graphics.IsFullScreen);
        gameStateManager.HandleInput(inputHelper);
    }

    protected override void Update(GameTime gameTime)
    {
        HandleInput();
        gameStateManager.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, spriteScale);
        //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, spriteScale); //Deferred -> рисует пока End() не применим и рисует много SpriteBatch
        gameStateManager.Draw(gameTime, spriteBatch);
        spriteBatch.End();
    }
}