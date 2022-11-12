using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using soufGame.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace soufGame;


enum GameState
{
    inputName,
    game
}
public class Game1 : Game
{

    private GameState gameState;

    private GameContext context;
    private ServerConnection serverConnection;


    private List<Player> players;

    private string textInput;


    public Game1()
    {

        context = new GameContext()
        {
            graphics = new GraphicsDeviceManager(this),
            contentManager = Content,
            game = this,
            graphicManager = new()
        };

        Content.RootDirectory = "Content";

        IntPtr hWnd = Window.Handle;
        System.Windows.Forms.Control ctrl = System.Windows.Forms.Control.FromHandle(hWnd);
        System.Windows.Forms.Form form = ctrl.FindForm();
        form.TransparencyKey = System.Drawing.Color.Black;

        players = new List<Player>();

    }

    protected override void Initialize()
    {


        context.graphics.IsFullScreen = false;
        context.graphics.PreferredBackBufferWidth = 860; // 1920
        context.graphics.PreferredBackBufferHeight = 540; // 1080
        context.graphics.ApplyChanges();

        base.Initialize();

        Window.TextInput += TextInputHandler;

        gameState = GameState.inputName;

        textInput = string.Empty;
    }

    protected override void LoadContent()
    {
        var _spriteBatch = new SpriteBatch(GraphicsDevice);
        context.spriteBatch = _spriteBatch;

        context.graphicManager.LoadContent(context);

    }

    protected override void OnExiting(object sender, EventArgs args)
    {
        serverConnection.Close();
        base.OnExiting(sender, args);
    }

    protected override void Update(GameTime gameTime)
    {

        var keyboardState = Keyboard.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        switch (gameState)
        {
            case GameState.inputName:
                if (keyboardState.IsKeyDown(Keys.Enter))
                {

                    serverConnection = new(textInput);

                    if (textInput == "test")
                    {
                        serverConnection.StartDummy();

                    }
                    else
                    {

                        serverConnection.Start();
                    }

                    gameState = GameState.game;
                }
                break;

            case GameState.game:

                if (keyboardState.IsKeyDown(Keys.Space)) foreach (Player player in players) player.position = new Vector2(context.graphics.PreferredBackBufferWidth / 2, context.graphics.PreferredBackBufferHeight - (int)Math.Floor(Constants.playerHeight * 1.2));

                // Check if new chatUsers are different from the current ones
                var oldPlayers = players.Select((el) => el.playerName).ToArray();
                var newPlayers = serverConnection.latestChatUsers.Select((el) => el.username).ToArray();
                var foundNewName = false;

                if (oldPlayers.Count() == 0 && newPlayers.Count() > 0)
                {
                    foundNewName = true;
                }
                else
                {
                    foreach (string oldPlayerName in oldPlayers)
                        if (!newPlayers.Contains(oldPlayerName))
                        {
                            foundNewName = true;
                            Console.WriteLine("NEW PLAYERS");
                        }
                }

                if (foundNewName)
                    players = newPlayers.Select((name) => new Player(name, context)).ToList();


                foreach (Player player in players) player.Update(gameTime);

                break;
        }





        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Transparent);




        context.spriteBatch.Begin();

        switch (gameState)
        {
            case GameState.inputName:

                context.spriteBatch.Draw(
                        context.graphicManager.groundTexture,
                        new Rectangle(0, context.graphics.PreferredBackBufferHeight - (int)(Constants.playerHeight * 2.5), context.graphics.PreferredBackBufferWidth, (int)(Constants.playerHeight * 2.5)),
                        Color.Green
                        );

                context.spriteBatch.DrawString(context.graphicManager.soufFont, textInput, new(20, 20), Color.Red);



                break;

            case GameState.game:

                context.spriteBatch.Draw(
                    context.graphicManager.groundTexture,
                    new Rectangle(0, context.graphics.PreferredBackBufferHeight - (int)(Constants.playerHeight * 0.7), context.graphics.PreferredBackBufferWidth, (int)(Constants.playerHeight * 0.7)),
                    Color.Green
                    );

                foreach (Player player in players) player.Draw();

                break;
        }

        context.spriteBatch.End();
        base.Draw(gameTime);
    }

    private void TextInputHandler(object sender, TextInputEventArgs args)
    {
        var pressedKey = args.Key;
        var character = args.Character.ToString();

        if (Regex.IsMatch(character.ToString(), @"^[a-zA-Z0-9_]+$"))
        {
            Debug.WriteLine($"ACCEPTED: {character}");
            textInput += character;
        }
        else if (pressedKey == Keys.Back)
        {
            textInput = string.Empty;
        }


        // Console.WriteLine(pressedKey);
        Debug.WriteLine(character);


    }
}