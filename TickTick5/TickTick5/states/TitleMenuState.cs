using Microsoft.Xna.Framework;

class TitleMenuState : GameObjectList
{

    protected Button playButton, helpButton;

    public TitleMenuState()
    {
        // load the title screen
        SpriteGameObject title_screen = new SpriteGameObject("Backgrounds/spr_title", 0, "background");
        this.Add(title_screen);

        // add a play button
        playButton = new Button("Sprites/spr_button_play", 1);
        playButton.Position = new Vector2((GameEnvironment.Screen.X - playButton.Width) / 2, 540);
        this.Add(playButton);

        // add a help button
        helpButton = new Button("Sprites/spr_button_help", 1);
        helpButton.Position = new Vector2((GameEnvironment.Screen.X - helpButton.Width) / 2, 600);
        this.Add(helpButton);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (playButton.Pressed)
            GameEnvironment.GameStateManager.SwitchTo("levelMenu");
        else if (helpButton.Pressed)
            GameEnvironment.GameStateManager.SwitchTo("helpState");
    }
}
