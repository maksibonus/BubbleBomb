using Microsoft.Xna.Framework;

class VisibilityTimer : GameObject  //задает время отображения подсказки
{
    protected GameObject target;
    protected float timeleft;
    //protected float totaltime;

    public VisibilityTimer(GameObject target, int layer=0, string id = "")
        : base(layer, id)
    {
        //totaltime = 3;
        timeleft = 3;
        this.target = target;
    }

    public override void Update(GameTime gameTime)
    {
        timeleft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timeleft <= 0)
            target.Visible = false;
    }

    public void StartVisible()
    {
        //timeleft = totaltime;
        target.Visible = true;
    }
}
