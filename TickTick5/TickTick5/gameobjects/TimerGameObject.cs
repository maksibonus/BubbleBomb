using System;
using Microsoft.Xna.Framework;

class TimerGameObject : TextGameObject
{
    protected TimeSpan timeLeft;
    protected bool running;
    protected double multiplier;

    public TimerGameObject(int layer = 0, string id = "")
        : base("Fonts/Hud", layer, id)
    {
        this.multiplier = 1;
        this.running = true;
    }

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

    public bool Running
    {
        get { return running; }
        set { running = value; }
    }

    public double Multiplier
    {
        get {return multiplier; }
        set { multiplier = value; }
    }

    public bool GameOver
    {
        get { return (timeLeft.Ticks <= 0); }
    }
}