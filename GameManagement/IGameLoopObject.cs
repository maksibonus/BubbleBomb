﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IGameLoopObject
{
    void HandleInput(InputHelper inputHelper);

    void Update(GameTime gameTime);

    void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    void Reset();

}
