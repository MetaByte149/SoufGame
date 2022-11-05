using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using soufGame.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace soufGame;

public class Game1 : Game {


    private GameContext context;


    private List<Player> players;


    private Texture2D groundTexture;

    public Game1() {

        var _graphics = new GraphicsDeviceManager(this);
        context = new GameContext() { graphics = _graphics, contentManager = Content, game = this };
        Content.RootDirectory = "Content";

        IntPtr hWnd = Window.Handle;
        System.Windows.Forms.Control ctrl = System.Windows.Forms.Control.FromHandle(hWnd);
        System.Windows.Forms.Form form = ctrl.FindForm();
        form.TransparencyKey = System.Drawing.Color.Black;

        players = new List<Player>();



    }

    protected override void Initialize() {


        context.graphics.IsFullScreen = false;
        context.graphics.PreferredBackBufferWidth = 1920;
        context.graphics.PreferredBackBufferHeight = 1080;
        context.graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent() {
        var _spriteBatch = new SpriteBatch(GraphicsDevice);
        context.spriteBatch = _spriteBatch;
        players.Add(new Player("metabyte149", context, Color.White));
        players.Add(new Player("CullTk", context, Color.Blue));
        players.Add(new Player("Tupie_san", context, Color.Red));
        groundTexture = context.contentManager.Load<Texture2D>("green-background");
    }

    protected override void Update(GameTime gameTime) {

        var keyboardState = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        if (keyboardState.IsKeyDown(Keys.Space)) foreach (Player player in players) player.position = new Vector2(context.graphics.PreferredBackBufferWidth / 2, context.graphics.PreferredBackBufferHeight - (int)Math.Floor(Constants.playerHeight * 1.2));

        foreach (Player player in players) player.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Transparent);

        context.spriteBatch.Begin();
        context.spriteBatch.Draw(
            groundTexture,
            new Rectangle(0, context.graphics.PreferredBackBufferHeight - (int)(Constants.playerHeight * 0.7), context.graphics.PreferredBackBufferWidth, (int)(Constants.playerHeight * 0.7)),
            Color.Green
            );

        foreach (Player player in players) player.Draw();

        context.spriteBatch.End();

        base.Draw(gameTime);
    }
}