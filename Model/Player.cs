using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace soufGame.Model;

internal class Player
{
    public enum PlayerActionType
    {
        Idle,
        JumpingL,
        JumpingR
    }

    public static Random rnd = new();
    public static Color[] possibleColors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Black, Color.Aqua, Color.Purple };

    public static int animationFrameTime = 100;

    private readonly GameContext context;

    public Vector2 position;
    public Vector2 velocity;
    public PlayerActionType playerAction;
    public Color color;

    public string playerName;

    public int animationIndex;
    public int animationTimer;
    public int floorHeight;

    public Player(string _playerName, GameContext _context)
    {
        playerName = _playerName;
        context = _context;

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

    public void Update(GameTime gameTime)
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

        if (velocity.Y < 0.8f)
            velocity.Y = 0.8f;
        else
            velocity.Y *= 0.8f;

        if (position.Y > floorHeight)
        {
            position.Y = floorHeight;
            playerAction = PlayerActionType.Idle;
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


        // animation index calculation
        animationTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (animationTimer > animationFrameTime)
        {
            animationTimer = 0;
            if (animationIndex >= 1)
                animationIndex = 0;
            else
                animationIndex++;
        }

    }

    public void Draw()
    {
        Rectangle sourceRectangle;

        switch (playerAction)
        {
            case PlayerActionType.Idle:


                sourceRectangle = new Rectangle(
                    animationIndex * 64,
                    0,
                    Constants.playerWidth,
                    Constants.playerHeight
                );


                context.spriteBatch.Draw(context.graphicManager.playerTexture, position, sourceRectangle, color);
                break;

            case PlayerActionType.JumpingL:
            case PlayerActionType.JumpingR:

                sourceRectangle = new Rectangle(
                    64,
                    0,
                    Constants.playerWidth,
                    Constants.playerHeight
                );

                context.spriteBatch.Draw(context.graphicManager.playerTexture, position, sourceRectangle, color);
                break;
        }



        // Finds the center of the string in coordinates inside the text rectangle
        Vector2 textMiddlePoint = context.graphicManager.soufFont.MeasureString(playerName) / 2;

        context.spriteBatch.DrawString(
            context.graphicManager.soufFont,
            playerName,
            position,
            Color.White,
            0,
            textMiddlePoint,
            1.0f,
            SpriteEffects.None,
            0.5f
        );
    }

    public override string ToString()
    {
        return $"VEL: {velocity.X} {velocity.Y} \nPOS: {position.X} {position.Y} \nACTION:{playerAction}";
    }
}
