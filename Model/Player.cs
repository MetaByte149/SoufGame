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

internal class Player {

    public enum PlayerActionType {
        Idle,
        JumpingL,
        JumpingR
    }

    public static class SpriteSheetInfo {

        public static int idleAnimationHeight = 640;
        public static int idleAnimationCount = 9;

        public static int jumpAnimationHeight = 256;
        public static int jumpAnimationCount = 7;
    }

    public static Random rnd = new Random();

    public Texture2D texture;
    public Vector2 position;
    public Vector2 velocity;
    public PlayerActionType playerAction;

    public int floorHeight;


    public int animationIndex;

    public int width;
    public int height;
    private GameContext context;

    public Player(GameContext context) {
        texture = context.contentManager.Load<Texture2D>("soufPlayer1");

        width = 64;
        height = 64;

        floorHeight = context.graphics.PreferredBackBufferHeight - (int)Math.Floor(height * 1.5);
        position = new Vector2(context.graphics.PreferredBackBufferWidth / 2, floorHeight);
        velocity = new Vector2(0, 0);
        playerAction = PlayerActionType.Idle;


        animationIndex = 0;


    }

    public Player() {
    }

    public void Update() {


        // Behaviour
        if (playerAction == PlayerActionType.Idle) {

            int num = rnd.Next(1, 1001);
            if (num <= 5) {
                playerAction = PlayerActionType.JumpingL;
                velocity.X = -20;
                velocity.Y = -10;

            } else if (num <= 10) {
                playerAction = PlayerActionType.JumpingR;
                velocity.X = 20;
                velocity.Y = -10;

            }
        }


        // Update pos

        position.X += velocity.X;
        position.Y += velocity.Y;


        if (velocity.X < 1 && velocity.X > -1) velocity.X = 0;
        else
            velocity.X *= 0.8f;

        if (velocity.Y < 1) velocity.Y = 1;
        else
            velocity.Y *= 0.8f;


        if (position.Y > floorHeight) {
            position.Y = floorHeight;
            playerAction = PlayerActionType.Idle;
            animationIndex = 0;

        }


    }

    public void Draw(SpriteBatch spriteBatch) {

        Rectangle sourceRectangle;

        switch (playerAction) {
            case PlayerActionType.Idle:

                sourceRectangle = new Rectangle(animationIndex++ * 64, SpriteSheetInfo.idleAnimationHeight, width, height);
                if (animationIndex >= SpriteSheetInfo.idleAnimationCount) animationIndex = 0;
                spriteBatch.Draw(texture, position, sourceRectangle, Color.White);
                break;

            case PlayerActionType.JumpingL:
            case PlayerActionType.JumpingR:

                sourceRectangle = new Rectangle(animationIndex++ * 64, SpriteSheetInfo.jumpAnimationHeight, width, height);
                if (animationIndex >= SpriteSheetInfo.jumpAnimationCount) animationIndex = 0;
                spriteBatch.Draw(texture, position, sourceRectangle, Color.White);
                break;


            default:
                break;

        }

        //sourceRectangle = new Rectangle(animationIndex++ * 64, 704, width, height);
        //if (animationIndex >= 7) animationIndex = 0;
        //spriteBatch.Draw(texture, position, sourceRectangle, Color.White);



        // Drawing the full thing
        //spriteBatch.Draw(texture, new Vector2(position.X, position.Y), Color.White);
    }

    public override string ToString() {
        return $"VEL: {velocity.X} {velocity.Y} \nPOS: {position.X} {position.Y} \nACTION:{playerAction}";
    }


}
