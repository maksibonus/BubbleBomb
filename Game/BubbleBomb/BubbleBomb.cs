using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using RamGecXNAControls;

/// <summary>
/// ���� ������� ���.
/// </summary>
class BubbleBomb : GameEnvironment
{
    #region ���� �����

    /// <summary>
    /// �������� ������������ � ��������� ���������� ����������.
    /// </summary>
    static public BubbleBomb game;

    #endregion ���� �����

    #region ������������

    /// <summary>
    /// �������� ���� ����� ����������� ���������� �� �������������.
    /// </summary>
    public BubbleBomb()
    {
        Content.RootDirectory = "Content";
        this.IsMouseVisible = true;
    }

    #endregion ������������

    #region ������

    /// <summary>
    /// ���� ������ ��������� ���.
    /// </summary>
    protected override void LoadContent()
    {
        base.LoadContent();

        screen = new Point(1440, 825);//�������� �����
        this.SetFullScreen(false);
        gameStateManager.AddGameState("titleMenu", new TitleMenuState());
        gameStateManager.AddGameState("helpState", new HelpState());
        gameStateManager.AddGameState("playingState", new PlayingState(Content));
        gameStateManager.AddGameState("levelMenu", new LevelMenuState());
        gameStateManager.AddGameState("gameOverState", new GameOverState());
        gameStateManager.AddGameState("levelFinishedState", new LevelFinishedState());
        gameStateManager.SwitchTo("titleMenu");

        AssetManager.PlayMusic("Sounds/Legend Of Zelda - Zelda's lullaby (Original)");
    }

    /// <summary>
    /// ����� ����� � ���.
    /// </summary>
    static void Main()
    {
        GameTests.TestManager.Initialize();
        game = new BubbleBomb();
        game.Run();
    }

    #endregion ������
}