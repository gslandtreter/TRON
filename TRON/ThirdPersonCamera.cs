using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace TRON
{
    class ThirdPersonCamera
    {

        private Matrix4 getCameraMatrix(Player player)
        {
            Matrix4 lookat = new Matrix4();

            switch (player.direction)
            {
                case PlayerDirection.UP:

                    lookat = Matrix4.LookAt(
                        (float)player.position.X - 15, (float)player.position.Y + 5, (float)player.position.Z,
                        (float)player.position.X, (float)player.position.Y, (float)player.position.Z,
                        0, 1, 0);
                    break;

                case PlayerDirection.LEFT:

                    lookat = Matrix4.LookAt(
                        (float)player.position.X, (float)player.position.Y + 5, (float)player.position.Z + 15,
                        (float)player.position.X, (float)player.position.Y, (float)player.position.Z,
                        0, 1, 0);
                    break;

                case PlayerDirection.DOWN:

                    lookat = Matrix4.LookAt(
                        (float)player.position.X + 15, (float)player.position.Y + 5, (float)player.position.Z,
                        (float)player.position.X, (float)player.position.Y, (float)player.position.Z,
                        0, 1, 0);
                    break;

                case PlayerDirection.RIGHT:

                    lookat = Matrix4.LookAt(
                        (float)player.position.X, (float)player.position.Y + 5, (float)player.position.Z - 15,
                        (float)player.position.X, (float)player.position.Y, (float)player.position.Z,
                        0, 1, 0);
                    break;
            }
            
            return lookat;
        }

        public void doCameraOnPlayer(Player player)
        {
            Matrix4 lookat = getCameraMatrix(player);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
        }
    }
}
