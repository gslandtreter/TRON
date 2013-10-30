using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace TRON
{
    class Player
    {
        public static float SCALE = 0.25f;

        public Vector3 position;
        public int direction;
        public float speed;

        public Mesh mesh;
        public uint textureID;

        public Player()
        {
            speed = 1.0f;
        }

        public void drawPlayer()
        {
            GL.PushMatrix();

            GL.Translate(position.X, position.Y, position.Z);
            GL.Scale(SCALE, SCALE, SCALE);

            mesh.Render(textureID);

            GL.PopMatrix();
        }

        public void updatePlayerPos(OpenTK.Input.KeyboardDevice keyboard)
        {
            if (keyboard[OpenTK.Input.Key.W])
            {
                direction = 0;
            }
        }
    }
}
