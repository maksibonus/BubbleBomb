using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RamGecXNAControls;
using RamGecXNAControls.ExtendedControls;


class WaterDrop : SpriteGameObject
{
    protected float bounce;
    TextGameObject textAboveWater;

    public WaterDrop(TextGameObject textAboveWater, int layer = 0, string id = "")
        : base("Sprites/spr_water", layer, id) 
    {
        this.textAboveWater = textAboveWater;
        
    }

    public override void Update(GameTime gameTime)
    {
        double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + Position.X;
        bounce = (float)Math.Sin(t) * 0.2f;
        position.Y += bounce;
        Player player = GameWorld.Find("player") as Player;
        if (this.visible && this.CollidesWith(player))
        {
            GameEnvironment.GameStateManager.SwitchTo("questionState");
            textAboveWater.Visible = false;
            this.visible = false;
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_watercollected");
        }    
    }
}
