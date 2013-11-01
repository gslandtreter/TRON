using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace TRON
{
    class TopCamera
    {
        private Matrix4 getCameraMatrix(float sizeX, float sizeZ)
        {
            float centerX, centerZ, height;

            centerX = sizeX / 2;
            centerZ = sizeZ / 2;

            height = (float)Math.Sqrt(Math.Pow((double)sizeX, 2) + Math.Pow((double)sizeZ, 2));

            Matrix4 lookat = Matrix4.LookAt(
                centerX - 1, height, centerZ,
                centerX, 0, centerZ ,
                0, 1, 0);

            return lookat;
        }

        public void doCamera(float sizeX, float sizeZ)
        {
            Matrix4 lookat = getCameraMatrix(sizeX, sizeZ);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
        }
    }
}
