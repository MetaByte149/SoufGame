using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Security.Cryptography;
using System;
using System.Diagnostics;
using MonoGame.Extended.Animations;
using System.Runtime.InteropServices;
using MonoGame.Extended;

namespace soufGame.Model;

internal class Player
{
    public enum PlayerActionType
    {
        Idle,
        JumpingL,
        JumpingR
    }

    public static class SpriteSheetInfo
    {
        public static int idleAnimationHeight = 640;
        public static int idleAnimationCount = 9;

        public static int jumpAnimationHeight = 256;
        public static int jumpAnimationCount = 7;
    }

    public static Random rnd = new();
    public static Color[] possibleColors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Black, Color.Aqua, Color.Purple };

    private readonly GameContext context;
    public Texture2D texture;
    public SpriteFont font;

    public Vector2 position;
    public Vector2 velocity;
    public PlayerActionType playerAction;
    public Color color;

    public string playerName;

    public int animationIndex;
    public int floorHeight;

    public Player(string _playerName, GameContext _context)
    {
        playerName = _playerName;
        context = _context;
        texture = context.contentManager.Load<Texture2D>("soufPlayer1");
        font = context.contentManager.Load<SpriteFont>("soufFont");

        floorHeight =
            context.graphics.PreferredBackBufferHeight
            - (int)Math.Floor(Constants.playerHeight * 1.2);
        position = new Vector2(context.graphics.PreferredBackBufferWidth / 2, floorHeight);
        velocity = new Vector2(0, 0);
        playerAction = PlayerActionType.Idle;

        color = possibleColors[rnd.Next(0, possibleColors.Length)];
        animationIndex = 0;
    }

    public Player() { }

    public void Update()
    {
        // Behaviour
        if (playerAction == PlayerActionType.Idle)
        {
            int num = rnd.Next(1, 1001);
            if (num <= 5)
            {
                playerAction = PlayerActionType.JumpingL;
                velocity.X = -20;
                velocity.Y = -10;
            }
            else if (num <= 10)
            {
                playerAction = PlayerActionType.JumpingR;
                velocity.X = 20;
                velocity.Y = -10;
            }
        }

        // Update pos

        position.X += velocity.X;
        position.Y += velocity.Y;

        if (velocity.X < 1 && velocity.X > -1)
            velocity.X = 0;
        else
            velocity.X *= 0.85f;

        if (velocity.Y < 1)
            velocity.Y = 1;
        else
            velocity.Y *= 0.8f;

        if (position.Y > floorHeight)
        {
            position.Y = floorHeight;
            playerAction = PlayerActionType.Idle;
            animationIndex = 0;
        }

        if (position.X < 0)
        {
            position.X = 0;
            velocity.X *= -1;
        }
        else if (position.X > context.graphics.PreferredBackBufferWidth - Constants.playerHeight)
        {
            position.X = context.graphics.PreferredBackBufferWidth - Constants.playerHeight;
            velocity.X *= -1;
        }
    }

    public void Draw()
    {
        Rectangle sourceRectangle;

        switch (playerAction)
        {
            case PlayerActionType.Idle:

                sourceRectangle = new Rectangle(
                    animationIndex++ * 64,
                    SpriteSheetInfo.idleAnimationHeight,
                    Constants.playerHeight,
                    Constants.playerHeight
                );
                if (animationIndex >= SpriteSheetInfo.idleAnimationCount)
                    animationIndex = 0;
                context.spriteBatch.Draw(texture, position, sourceRectangle, color);
                break;

            case PlayerActionType.JumpingL:
            case PlayerActionType.JumpingR:

                sourceRectangle = new Rectangle(
                    animationIndex++ * 64,
                    SpriteSheetInfo.jumpAnimationHeight,
                    Constants.playerHeight,
                    Constants.playerHeight
                );
                if (animationIndex >= SpriteSheetInfo.jumpAnimationCount)
                    animationIndex = 0;
                context.spriteBatch.Draw(texture, position, sourceRectangle, color);
                break;

            default:
                Debug.WriteLine("no playeraction!! Not drawing sprite.");
                break;
        }

        // Finds the center of the string in coordinates inside the text rectangle
        Vector2 textMiddlePoint = font.MeasureString(playerName) / 2;

        context.spriteBatch.DrawString(
            font,
            playerName,
            position,
            Color.White,
            0,
            textMiddlePoint,
            1.0f,
            SpriteEffects.None,
            0.5f
        );

        //sourceRectangle = new Rectangle(animationIndex++ * 64, 704, Constants.playerHeight, Constants.playerHeight);
        //if (animationIndex >= 7) animationIndex = 0;
        //spriteBatch.Draw(texture, position, sourceRectangle, Color.White);



        // Drawing the full thing
        //spriteBatch.Draw(texture, new Vector2(position.X, position.Y), Color.White);
    }

    public override string ToString()
    {
        return $"VEL: {velocity.X} {velocity.Y} \nPOS: {position.X} {position.Y} \nACTION:{playerAction}";
    }
}
