using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using soufGame.Model;
using System;
using System.Diagnostics;

namespace soufGame;

public class Game1 : Game {


    private GameContext context;


    private Player testPlayer;


    private Texture2D groundTexture;

    public Game1() {

        var _graphics = new GraphicsDeviceManager(this);
        context = new GameContext() { graphics = _graphics, contentManager = Content, game = this };
        Content.RootDirectory = "Content";
        IntPtr hWnd = Window.Handle;
        System.Windows.Forms.Control ctrl = System.Windows.Forms.Control.FromHandle(hWnd);
        System.Windows.Forms.Form form = ctrl.FindForm();
        form.TransparencyKey = System.Drawing.Color.Black;
    }

    protected override void Initialize() {




        base.Initialize();
    }

    protected override void LoadContent() {
        var _spriteBatch = new SpriteBatch(GraphicsDevice);
        context.spriteBatch = _spriteBatch;
        testPlayer = new Player("metabyte149", context);
        groundTexture = context.contentManager.Load<Texture2D>("green-background");
    }

    protected override void Update(GameTime gameTime) {

        var keyboardState = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        if (keyboardState.IsKeyDown(Keys.Space)) testPlayer.position = new Vector2(context.graphics.PreferredBackBufferWidth / 2, testPlayer.floorHeight);
        testPlayer.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Transparent);

        context.spriteBatch.Begin();
        context.spriteBatch.Draw(
            groundTexture,
            new Rectangle(0, context.graphics.PreferredBackBufferHeight - (int)(testPlayer.height * 0.7), context.graphics.PreferredBackBufferWidth, (int)(testPlayer.height * 0.7)),
            Color.Red
            );
        testPlayer.Draw(context.spriteBatch);

        context.spriteBatch.End();

        base.Draw(gameTime);
    }
}