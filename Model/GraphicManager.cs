using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace soufGame.Model;
internal class GraphicManager
{
    public Texture2D groundTexture;
    public Texture2D playerTexture;
    public SpriteFont soufFont;

    public GraphicManager() { }

    public void LoadContent(GameContext context)
    {
        groundTexture = context.contentManager.Load<Texture2D>("green-background");
        playerTexture = context.contentManager.Load<Texture2D>("slime_spritesheet");


        soufFont = context.contentManager.Load<SpriteFont>("soufFont");
    }

}