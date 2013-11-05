using System;
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

        public Player(Vector3 beginningPos, Color playerColor)
        {
            color = playerColor;
            speed = 10.0f;
            direction = PlayerDirection.UP;
            isHumanPlayer = false;
            isAlive = true;

            trailHistory = new List<TrailSector>();
            currentTrail = new TrailSector(direction, color);

            directionChanged = false;

            position = beginningPos;

            currentTrail.beginningPoint = beginningPos;

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

        public void getNewDirection(OpenTK.Input.KeyboardDevice keyboard, double elapsedTime)
        {
            //TODO: O movimento deve ser diferente na camera de cima. Ver definicao do trabalho.
            inputTimeBuffer += elapsedTime;

            if (keyboard[OpenTK.Input.Key.Left])
            {
                if (inputTimeBuffer < INPUT_DELAY)
                    return;
                else inputTimeBuffer = 0;

                switch (direction)
                {
                    case PlayerDirection.UP:
                        direction = PlayerDirection.LEFT;
                        break;
                    case PlayerDirection.LEFT:
                        direction = PlayerDirection.DOWN;
                        break;
                    case PlayerDirection.DOWN:
                        direction = PlayerDirection.RIGHT;
                        break;
                    case PlayerDirection.RIGHT:
                        direction = PlayerDirection.UP;
                        break;
                }

                directionChanged = true;
            }
            else if (keyboard[OpenTK.Input.Key.Right])
            {
                if (inputTimeBuffer < INPUT_DELAY)
                    return;
                else inputTimeBuffer = 0;

                switch (direction)
                {
                    case PlayerDirection.UP:
                        direction = PlayerDirection.RIGHT;
                        break;
                    case PlayerDirection.LEFT:
                        direction = PlayerDirection.UP;
                        break;
                    case PlayerDirection.DOWN:
                        direction = PlayerDirection.LEFT;
                        break;
                    case PlayerDirection.RIGHT:
                        direction = PlayerDirection.DOWN;
                        break;
                }

                directionChanged = true;
            }
        }

        public void setPosition(Vector3 newPos)
        {
            position = newPos;
            currentTrail.endPoint = newPos;
        }

        public void Die()
        {
            //TODO: die properly
            this.trailHistory = new List<TrailSector>();
            this.setPosition(new Vector3(10, 0, 10));

            this.isAlive = false;
        }

        public void updatePlayerPos(OpenTK.Input.KeyboardDevice keyboard, double elapsedTime)
        {

            getNewDirection(keyboard, elapsedTime);

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
