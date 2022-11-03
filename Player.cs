﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Security.Cryptography;
using System;
using System.Diagnostics;
using MonoGame.Extended.Animations;
using System.Runtime.InteropServices;


namespace soufGame;

internal class Player {
    private Texture2D texture;
    private Vector2 position;

    private int idleTimer;
    private int walkTimer;
    private int direction;
    private bool walking;

    private int animationIndex;
    private int animationSpeed;




    public Player(ContentManager content) {
        texture = content.Load<Texture2D>("soufPlayer2");
        position = new Vector2(0, 0);

        idleTimer = 100;
        walkTimer = 0;
        direction = 1;
        walking = true;

        animationIndex = 0;
        animationSpeed = 5;


    }

    public void Update() {


        if (walking) {
            position.X += (direction == 1) ? 1 : -1;
            walkTimer--;

            if (walkTimer <= 0) {
                walking = !walking;
                idleTimer = 100;
            }
        } else {
            idleTimer--;
            if (idleTimer <= 0) {
                walking = !walking;
                walkTimer = 100;
            }
        }


    }

    public void Draw(SpriteBatch spriteBatch) {



        var sourceRectangle = new Rectangle(animationIndex++ * 64, 704, 64, 64);
        if (animationIndex >= 7) animationIndex = 0;
        // Only draw the area contained within the sourceRectangle.
        spriteBatch.Draw(texture, position, sourceRectangle, Color.White);


        // Drawing the full thing
        //spriteBatch.Draw(texture, new Vector2(position.X, position.Y), Color.White);
    }


}
