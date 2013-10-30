using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;


namespace TRON
{
    class DebugCamera
    {
        private Matrix4 cameraMatrix;

        Vector3 camCoord = new Vector3();
        Vector2 camAngle = new Vector2();

        public Matrix4 getMatrix()
        {
            return cameraMatrix;
        }

        public void setMatrix(Matrix4 matrix)
        {
            cameraMatrix = matrix;
        }

        public Matrix4 generateMatrix(OpenTK.Vector3 cameraPos, OpenTK.Vector3 lookAt)
        {
            cameraMatrix = Matrix4.LookAt(cameraPos, lookAt, OpenTK.Vector3.UnitY);
            return cameraMatrix;
        }

        public void updateCamera(OpenTK.Input.KeyboardDevice keyboard, OpenTK.Input.MouseDevice mouse)
        {

            const float speed = 1.0f;
            const float mouseSpeed = 0.5f;

            camAngle.Y += mouse.XDelta * mouseSpeed;
            camAngle.X += mouse.YDelta * mouseSpeed;

            Vector2 RadAng = new Vector2();

            RadAng.X = (camAngle.X / 180 * Math.PI);
            RadAng.Y = (camAngle.Y / 180 * Math.PI);


            if (keyboard[OpenTK.Input.Key.W])
            {
                camCoord.X -= (float)(Math.Sin(RadAng.Y) * speed);
                camCoord.Z += (float)(Math.Cos(RadAng.Y) * speed);
                camCoord.Y += (float)(Math.Sin(RadAng.X) * speed);
            }
            if (keyboard[OpenTK.Input.Key.S])
            {
                camCoord.X += (float)(Math.Sin(RadAng.Y) * speed);
                camCoord.Z -= (float)(Math.Cos(RadAng.Y) * speed);
                camCoord.Y -= (float)(Math.Sin(RadAng.X) * speed);
            }
            if (keyboard[OpenTK.Input.Key.D])
            {
                camCoord.X -= (float)(Math.Cos(RadAng.Y) * speed);
                camCoord.Z -= (float)(Math.Sin(RadAng.Y) * speed);
            }
            if (keyboard[OpenTK.Input.Key.A])
            {
                camCoord.X += (float)(Math.Cos(RadAng.Y) * speed);
                camCoord.Z += (float)(Math.Sin(RadAng.Y) * speed);
            }

            if (camAngle.X > 89)
            {
                camAngle.X = 89;
            }
            if (camAngle.X < -89)
            {
                camAngle.X = -89;
            }

            if (camAngle.Y > 360)
            {
                camAngle.Y -= 360;
            }
            else if (camAngle.Y < 0)
            {
                camAngle.Y += 360;
            }
        }

        public void doCamera()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Rotate(camAngle.X, 1, 0, 0);
            GL.Rotate(camAngle.Y, 0, 1, 0);
            GL.Translate(camCoord.X, camCoord.Y, camCoord.Z);
        }

        public 
        DebugCamera()
        {

        }



    }
}
