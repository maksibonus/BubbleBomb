using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// ���� ��������� ���������.
/// </summary>
partial class Player : AnimatedGameObject
{
    #region ���� �����

    /// <summary>
    /// ��������� �������.
    /// </summary>
    protected Vector2 startPosition;

    /// <summary>
    /// �������� ����� �� �������.
    /// </summary>
    protected bool isOnTheGround;

    /// <summary>
    /// ��������� ������� �� �����.
    /// </summary>
    protected float previousYPosition;

    /// <summary>
    /// ���������, �� �����, �� ����� ��������.
    /// </summary>
    protected bool isAlive;

    /// <summary>
    /// ���������, �� �����, �� �������� ��������.
    /// </summary>
    protected bool exploded;

    /// <summary>
    /// ���������, �� �����, �� ������� �����.
    /// </summary>
    protected bool finished;

    /// <summary>
    /// ���������, �� �����, �� �� ���������� �������� ����.
    /// </summary>
    protected bool walkingOnIce;
        
    /// <summary>
    /// ���������, �� �����, �� �� ���������� ������� ����.
    /// </summary>
    protected bool walkingOnHot;

    #endregion ���� �����

    #region ��������� ����������

    // �������� ��������� IGameLoopObject.
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        this.Layer = 100;
        if (!finished && isAlive)
        {
            if (isOnTheGround)
                if (velocity.X == 0)
                    this.PlayAnimation("idle");
                else
                    this.PlayAnimation("run");
            else if (velocity.Y < 0)
                this.PlayAnimation("jump");

            TimerGameObject timer = GameWorld.Find("timer") as TimerGameObject;
            if (walkingOnHot)
                timer.Multiplier = 1.3f;
            else if (walkingOnIce)
                timer.Multiplier = 0.7f;
            else
                timer.Multiplier = 1f;
            TileField tiles = GameWorld.Find("tiles") as TileField;
            if (BoundingBox.Top >= tiles.Rows * tiles.CellHeight)
                this.Die(true);
        }
        DoPhysics();
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        float walkingSpeed = 400;
        if (walkingOnIce)
            walkingSpeed *= 1.5f;
        if (!isAlive)
            return;
        if (inputHelper.IsKeyDown(Keys.Left))
            velocity.X = -walkingSpeed;
        else if (inputHelper.IsKeyDown(Keys.Right))
            velocity.X = walkingSpeed;
        else if (!walkingOnIce && isOnTheGround)
            velocity.X = 0.0f;
        if (velocity.X != 0.0f)
            Mirror = velocity.X < 0;
        if ((inputHelper.KeyPressed(Keys.Space) || inputHelper.KeyPressed(Keys.Up)) && isOnTheGround)
            Jump();
    }

    public override void Reset()
    {
        this.position = startPosition;
        this.velocity = Vector2.Zero;
        isOnTheGround = true;
        isAlive = true;
        exploded = false;
        finished = false;
        walkingOnIce = false;
        walkingOnHot = false;
        this.PlayAnimation("idle");
        previousYPosition = BoundingBox.Bottom;
    }

    #endregion ��������� ����������

    #region ������������

    /// <summary>
    /// �������� ���� ����� ���������� ����������.
    /// </summary>
    public Player(Vector2 start) : base(2, "player")
    {
        this.LoadAnimation("Sprites/Player/spr_idle", "idle", true); 
        this.LoadAnimation("Sprites/Player/spr_run@13", "run", true, 0.05f);
        this.LoadAnimation("Sprites/Player/spr_jump@14", "jump", false, 0.05f); 
        this.LoadAnimation("Sprites/Player/spr_celebrate@14", "celebrate", false, 0.05f);
        this.LoadAnimation("Sprites/Player/spr_die@5", "die", false);
        this.LoadAnimation("Sprites/Player/spr_explode@5x5", "explode", false, 0.04f); 

        startPosition = start;
        Reset();
    }

    #endregion ������������

    #region ����������

    /// <summary>
    /// �����, �� �������� ��������.
    /// </summary>
    public bool IsAlive
    {
        get { return isAlive; }
    }

    /// <summary>
    /// �����, �� ������� �����.
    /// </summary>
    public bool Finished
    {
        get { return finished; }
    }

    #endregion ����������

    #region ������

    /// <summary>
    /// ĳ�, ���� ��������� ���.
    /// </summary>
    public void Explode()
    {
        if (!isAlive || finished)
            return;
        isAlive = false;
        exploded = true;
        velocity = Vector2.Zero;
        position.Y += 15;
        this.PlayAnimation("explode");
    }

    /// <summary>
    /// ĳ�, ���� �������� �����.
    /// </summary>
    /// <param name="falling">�����, �� ���� ��������.</param>
    public void Die(bool falling)
    {
        if (!isAlive || finished)
            return;
        isAlive = false;
        velocity.X = 0.0f;
        if (falling)
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_fall");
        else
        {
            velocity.Y = -900;
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_die");
        }
        this.PlayAnimation("die");
    }

    /// <summary>
    /// ĳ�, ���� ������� ������� �����.
    /// </summary>
    public void LevelFinished()
    {
        finished = true;
        velocity.X = 0.0f;
        this.PlayAnimation("celebrate");
        GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_won");
    }

    #endregion ������
}