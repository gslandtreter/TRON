﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace TRON
{
    public enum PlayerDirection
    {
        UP, LEFT, DOWN, RIGHT
    }

    class Player
    {
        public static float SCALE = 0.25f;
        public static double INPUT_DELAY = 0.2;

        public Vector3 position;
        public PlayerDirection direction;
        public float speed;
        public bool isHumanPlayer;
        public bool isAlive;

        public Mesh mesh;
        public uint textureID;

        public double inputTimeBuffer;

        public bool directionChanged;

        public TrailSector currentTrail;
        public List<TrailSector> trailHistory;

        public Color color;

        public Rectangle hitBox;


        public Player(char[,] mapObstacles, Color playerColor)
        {
            color = playerColor;
            speed = 12.0f;
            direction = PlayerDirection.UP;
            isHumanPlayer = false;
            isAlive = true;         
          
            trailHistory = new List<TrailSector>();
            
            currentTrail = new TrailSector(direction, color);
            SetBeginningPos(mapObstacles);
            //SetDirection(mapObstacles);
            
            directionChanged = false;

        }
        
        private void SetBeginningPos(char[,] mapObstacles)
        {
            for (int i = 0; i < mapObstacles.GetLength(0); i++)
            {
                for (int j = 0; j < mapObstacles.GetLength(1); j++)
                {
                    if (mapObstacles[i, j] == '2')
                    {
                        
                        this.position = new Vector3(j * Mapa.MAP_UNIT_SIZE, 0, i * Mapa.MAP_UNIT_SIZE);
                        currentTrail.beginningPoint = new Vector3(j * Mapa.MAP_UNIT_SIZE, 0, i * Mapa.MAP_UNIT_SIZE);

                        mapObstacles[i, j] = '0';
                        return;
                    }
                }
            }
        }




        public void drawPlayer()
        {
            GL.PushMatrix();

            GL.Translate(position.X, position.Y, position.Z);
            GL.Scale(SCALE, SCALE, SCALE);

            rotateByDirection();

            mesh.Render(textureID);

            GL.PopMatrix();
           
        }

        public void drawTrail()
        {
            currentTrail.Draw();

            foreach (TrailSector sector in trailHistory)
            {
                sector.Draw();
            }
        }

        private void rotateByDirection()
        {
            switch (direction)
            {
                case PlayerDirection.UP:
                    GL.Rotate(90, 0, 1, 0);
                    break;
                case PlayerDirection.LEFT:
                    GL.Rotate(180, 0, 1, 0);
                    break;
                case PlayerDirection.DOWN:
                    GL.Rotate(270, 0, 1, 0);
                    break;
                case PlayerDirection.RIGHT:
                    GL.Rotate(0, 0, 1, 0);
                    break;

            }
        }

        public void setDirection(PlayerDirection newDir)
        {
            direction = newDir;
            directionChanged = true;
        }


        public void getNewDirection(OpenTK.Input.KeyboardDevice keyboard, double elapsedTime, bool cameraMode)
        {
            //TODO: O movimento deve ser diferente na camera de cima. Ver definicao do trabalho.
            inputTimeBuffer += elapsedTime;

            if (!cameraMode) //3rd person
            {
                if (keyboard[OpenTK.Input.Key.Left])
                {
                    if (inputTimeBuffer < INPUT_DELAY)
                        return;
                    else inputTimeBuffer = 0;

                    switch (direction)
                    {
                        case PlayerDirection.UP:
                            setDirection(PlayerDirection.LEFT);
                            break;
                        case PlayerDirection.LEFT:
                            setDirection(PlayerDirection.DOWN);
                            break;
                        case PlayerDirection.DOWN:
                            setDirection(PlayerDirection.RIGHT);
                            break;
                        case PlayerDirection.RIGHT:
                            setDirection(PlayerDirection.UP);
                            break;
                    }
                }
                else if (keyboard[OpenTK.Input.Key.Right])
                {
                    if (inputTimeBuffer < INPUT_DELAY)
                        return;
                    else inputTimeBuffer = 0;

                    switch (direction)
                    {
                        case PlayerDirection.UP:
                            setDirection(PlayerDirection.RIGHT);
                            break;
                        case PlayerDirection.LEFT:
                            setDirection(PlayerDirection.UP);
                            break;
                        case PlayerDirection.DOWN:
                            setDirection(PlayerDirection.LEFT);
                            break;
                        case PlayerDirection.RIGHT:
                            setDirection(PlayerDirection.DOWN);
                            break;
                    }
                }
            }

            else //Top View
            {
                if (keyboard[OpenTK.Input.Key.Left])
                {
                    if (inputTimeBuffer < INPUT_DELAY)
                        return;
                    else inputTimeBuffer = 0;

                    if(direction != PlayerDirection.RIGHT && direction != PlayerDirection.LEFT)
                        setDirection(PlayerDirection.LEFT);
                }
                else if (keyboard[OpenTK.Input.Key.Right])
                {
                    if (inputTimeBuffer < INPUT_DELAY)
                        return;
                    else inputTimeBuffer = 0;

                    if (direction != PlayerDirection.RIGHT && direction != PlayerDirection.LEFT)
                        setDirection(PlayerDirection.RIGHT);
                }
                else if (keyboard[OpenTK.Input.Key.Up])
                {
                    if (inputTimeBuffer < INPUT_DELAY)
                        return;
                    else inputTimeBuffer = 0;

                    if (direction != PlayerDirection.UP && direction != PlayerDirection.DOWN)
                        setDirection(PlayerDirection.UP);
                }
                else if (keyboard[OpenTK.Input.Key.Down])
                {
                    if (inputTimeBuffer < INPUT_DELAY)
                        return;
                    else inputTimeBuffer = 0;

                    if (direction != PlayerDirection.UP && direction != PlayerDirection.DOWN)
                        setDirection(PlayerDirection.DOWN);
                }
            }
        }

        public void setPosition(Vector3 newPos)
        {
            position = newPos;
            currentTrail.endPoint = newPos;

            hitBox = Rectangle.GetPlayerHitBox(this);
        }

        public void Die()
        {
            //TODO: die properly
            this.trailHistory = new List<TrailSector>();
            this.setPosition(new Vector3(10, 0, 10));

            this.isAlive = false;
        }

        public void updatePlayerPos(OpenTK.Input.KeyboardDevice keyboard, double elapsedTime, bool cameraMode)
        {
            if(keyboard != null)
                getNewDirection(keyboard, elapsedTime, cameraMode);

            if (directionChanged)
            {
                createNewTrail();
                directionChanged = false;
            }

            switch (direction)
            {
                case PlayerDirection.UP:
                    position.X += speed * elapsedTime;
                    setPosition(position);
                    break;
                case PlayerDirection.LEFT:
                    position.Z -= speed * elapsedTime;
                    setPosition(position);
                    break;
                case PlayerDirection.DOWN:
                    position.X -= speed * elapsedTime;
                    setPosition(position);
                    break;
                case PlayerDirection.RIGHT:
                    position.Z += speed * elapsedTime;
                    setPosition(position);
                    break;

            }
        }

        public void createNewTrail()
        {
            currentTrail.isCurrentTrail = false;
            currentTrail.isFirstOnHistory = true;

            TrailSector firstOnHistory = trailHistory.Find(i => i.isFirstOnHistory);

            if (firstOnHistory != null)
                firstOnHistory.isFirstOnHistory = false;

            trailHistory.Add(currentTrail);

            currentTrail = new TrailSector(direction, color);
            currentTrail.beginningPoint = currentTrail.endPoint = position;
        }
    }
}
