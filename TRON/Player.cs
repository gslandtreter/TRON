using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;


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

        public Mesh mesh;
        public uint textureID;

        public double inputTimeBuffer;

        public Player()
        {
            speed = 10.0f;
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

        private void getNewDirection(OpenTK.Input.KeyboardDevice keyboard, double elapsedTime)
        {
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
            }
        }

        public void updatePlayerPos(OpenTK.Input.KeyboardDevice keyboard, double elapsedTime)
        {

            getNewDirection(keyboard, elapsedTime);

            switch (direction)
            {
                case PlayerDirection.UP:
                    position.X += speed * elapsedTime;
                    break;
                case PlayerDirection.LEFT:
                    position.Z -= speed * elapsedTime;
                    break;
                case PlayerDirection.DOWN:
                    position.X -= speed * elapsedTime;
                    break;
                case PlayerDirection.RIGHT:
                    position.Z += speed * elapsedTime;
                    break;

            }
        }
    }
}
