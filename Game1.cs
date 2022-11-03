using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;

namespace soufGame;

public class Game1 : Game {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;


    Player testPlayer;

    public Game1() {
        graphics = new GraphicsDeviceManager(this);
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
        spriteBatch = new SpriteBatch(GraphicsDevice);

        testPlayer = new Player(Content);
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        testPlayer.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Transparent);

        spriteBatch.Begin();

        testPlayer.Draw(spriteBatch);

        spriteBatch.End();

        base.Draw(gameTime);
    }
}