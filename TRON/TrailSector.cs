using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace TRON
{
    class TrailSector
    {

        public static float TRAIL_HEIGHT = 1.5f;
        public static float TRAIL_DEPTH = 0.5f;
        public static float BIKE_LENGTH = 3.5f;
        public static float COLLISION_THRESHOLD = 0.0f;
        public static byte TRAIL_OPACITY = 170;

        public PlayerDirection direction;

        public bool isCurrentTrail;
        public bool isFirstOnHistory = false;
        public Vector3 beginningPoint;
        public Vector3 endPoint;

        public Color color;

        public TrailSector(PlayerDirection trailDirection, Color trailColor)
        {
            color = trailColor;
            direction = trailDirection;
            isCurrentTrail = true;
        }

        public void DrawLength(float lengthToIgnore)
        {
            switch (direction)
            {
                case PlayerDirection.UP:
                    //Inicio
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X, 0, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.Vertex3(beginningPoint.X, 0, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(beginningPoint.X, TRAIL_HEIGHT, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(beginningPoint.X, TRAIL_HEIGHT, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.End();

                    //Fim
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(endPoint.X - lengthToIgnore, 0, endPoint.Z - TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X - lengthToIgnore, 0, endPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X - lengthToIgnore, TRAIL_HEIGHT, endPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X - lengthToIgnore, TRAIL_HEIGHT, endPoint.Z - TRAIL_DEPTH / 2);
                    GL.End();

                    //Teto 
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X, TRAIL_HEIGHT, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.Vertex3(beginningPoint.X, TRAIL_HEIGHT, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X - lengthToIgnore, TRAIL_HEIGHT, endPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X - lengthToIgnore, TRAIL_HEIGHT, endPoint.Z - TRAIL_DEPTH / 2);
                    GL.End();

                    //Paredes laterais
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X, 0, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X - lengthToIgnore, 0, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X - lengthToIgnore, TRAIL_HEIGHT, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.Vertex3(beginningPoint.X, TRAIL_HEIGHT, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.End();

                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X, 0, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X - lengthToIgnore, 0, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X - lengthToIgnore, TRAIL_HEIGHT, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(beginningPoint.X, TRAIL_HEIGHT, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.End();

                    break;

                case PlayerDirection.DOWN:

                    //Inicio
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X, 0, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.Vertex3(beginningPoint.X, 0, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(beginningPoint.X, TRAIL_HEIGHT, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(beginningPoint.X, TRAIL_HEIGHT, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.End();

                    //Fim
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(endPoint.X + lengthToIgnore, 0, endPoint.Z - TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X + lengthToIgnore, 0, endPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X + lengthToIgnore, TRAIL_HEIGHT, endPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X + lengthToIgnore, TRAIL_HEIGHT, endPoint.Z - TRAIL_DEPTH / 2);
                    GL.End();

                    //Teto 
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X, TRAIL_HEIGHT, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.Vertex3(beginningPoint.X, TRAIL_HEIGHT, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X + lengthToIgnore, TRAIL_HEIGHT, endPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X + lengthToIgnore, TRAIL_HEIGHT, endPoint.Z - TRAIL_DEPTH / 2);
                    GL.End();

                    //Paredes laterais
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X, 0, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X + lengthToIgnore, 0, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X + lengthToIgnore, TRAIL_HEIGHT, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.Vertex3(beginningPoint.X, TRAIL_HEIGHT, beginningPoint.Z - TRAIL_DEPTH / 2);
                    GL.End();

                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X, 0, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X + lengthToIgnore, 0, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(endPoint.X + lengthToIgnore, TRAIL_HEIGHT, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.Vertex3(beginningPoint.X, TRAIL_HEIGHT, beginningPoint.Z + TRAIL_DEPTH / 2);
                    GL.End();

                    break;

                case PlayerDirection.RIGHT:

                    //Inicio
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X - TRAIL_DEPTH / 2, 0, beginningPoint.Z);
                    GL.Vertex3(beginningPoint.X + TRAIL_DEPTH / 2, 0, beginningPoint.Z);
                    GL.Vertex3(beginningPoint.X + TRAIL_DEPTH / 2, TRAIL_HEIGHT, beginningPoint.Z);
                    GL.Vertex3(beginningPoint.X - TRAIL_DEPTH / 2, TRAIL_HEIGHT, beginningPoint.Z);
                    GL.End();

                    //Fim
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(endPoint.X - TRAIL_DEPTH / 2, 0, endPoint.Z - lengthToIgnore);
                    GL.Vertex3(endPoint.X + TRAIL_DEPTH / 2, 0, endPoint.Z - lengthToIgnore);
                    GL.Vertex3(endPoint.X + TRAIL_DEPTH / 2, TRAIL_HEIGHT, endPoint.Z - lengthToIgnore);
                    GL.Vertex3(endPoint.X - TRAIL_DEPTH / 2, TRAIL_HEIGHT, endPoint.Z - lengthToIgnore);
                    GL.End();

                    //Teto 
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X - TRAIL_DEPTH / 2, TRAIL_HEIGHT, beginningPoint.Z);
                    GL.Vertex3(beginningPoint.X + TRAIL_DEPTH / 2, TRAIL_HEIGHT, beginningPoint.Z);
                    GL.Vertex3(endPoint.X + TRAIL_DEPTH / 2, TRAIL_HEIGHT, endPoint.Z - lengthToIgnore);
                    GL.Vertex3(endPoint.X - TRAIL_DEPTH / 2, TRAIL_HEIGHT, endPoint.Z - lengthToIgnore);
                    GL.End();

                    //Paredes laterais
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X - TRAIL_DEPTH / 2, 0, beginningPoint.Z);
                    GL.Vertex3(endPoint.X - TRAIL_DEPTH / 2, 0, endPoint.Z - lengthToIgnore);
                    GL.Vertex3(endPoint.X - TRAIL_DEPTH / 2, TRAIL_HEIGHT, endPoint.Z - lengthToIgnore);
                    GL.Vertex3(beginningPoint.X - TRAIL_DEPTH / 2, TRAIL_HEIGHT, beginningPoint.Z);
                    GL.End();

                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X + TRAIL_DEPTH / 2, 0, beginningPoint.Z);
                    GL.Vertex3(endPoint.X + TRAIL_DEPTH / 2, 0, endPoint.Z - lengthToIgnore);
                    GL.Vertex3(endPoint.X + TRAIL_DEPTH / 2, TRAIL_HEIGHT, endPoint.Z - lengthToIgnore);
                    GL.Vertex3(beginningPoint.X + TRAIL_DEPTH / 2, TRAIL_HEIGHT, beginningPoint.Z);
                    GL.End();

                    break;

                case PlayerDirection.LEFT:

                    //Inicio
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X - TRAIL_DEPTH / 2, 0, beginningPoint.Z);
                    GL.Vertex3(beginningPoint.X + TRAIL_DEPTH / 2, 0, beginningPoint.Z);
                    GL.Vertex3(beginningPoint.X + TRAIL_DEPTH / 2, TRAIL_HEIGHT, beginningPoint.Z);
                    GL.Vertex3(beginningPoint.X - TRAIL_DEPTH / 2, TRAIL_HEIGHT, beginningPoint.Z);
                    GL.End();

                    //Fim
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(endPoint.X - TRAIL_DEPTH / 2, 0, endPoint.Z + lengthToIgnore);
                    GL.Vertex3(endPoint.X + TRAIL_DEPTH / 2, 0, endPoint.Z + lengthToIgnore);
                    GL.Vertex3(endPoint.X + TRAIL_DEPTH / 2, TRAIL_HEIGHT, endPoint.Z + lengthToIgnore);
                    GL.Vertex3(endPoint.X - TRAIL_DEPTH / 2, TRAIL_HEIGHT, endPoint.Z + lengthToIgnore);
                    GL.End();

                    //Teto 
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X - TRAIL_DEPTH / 2, TRAIL_HEIGHT, beginningPoint.Z);
                    GL.Vertex3(beginningPoint.X + TRAIL_DEPTH / 2, TRAIL_HEIGHT, beginningPoint.Z);
                    GL.Vertex3(endPoint.X + TRAIL_DEPTH / 2, TRAIL_HEIGHT, endPoint.Z + lengthToIgnore);
                    GL.Vertex3(endPoint.X - TRAIL_DEPTH / 2, TRAIL_HEIGHT, endPoint.Z + lengthToIgnore);
                    GL.End();

                    //Paredes laterais
                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X - TRAIL_DEPTH / 2, 0, beginningPoint.Z);
                    GL.Vertex3(endPoint.X - TRAIL_DEPTH / 2, 0, endPoint.Z + lengthToIgnore);
                    GL.Vertex3(endPoint.X - TRAIL_DEPTH / 2, TRAIL_HEIGHT, endPoint.Z + lengthToIgnore);
                    GL.Vertex3(beginningPoint.X - TRAIL_DEPTH / 2, TRAIL_HEIGHT, beginningPoint.Z);
                    GL.End();

                    GL.Begin(BeginMode.Quads);
                    GL.Vertex3(beginningPoint.X + TRAIL_DEPTH / 2, 0, beginningPoint.Z);
                    GL.Vertex3(endPoint.X + TRAIL_DEPTH / 2, 0, endPoint.Z + lengthToIgnore);
                    GL.Vertex3(endPoint.X + TRAIL_DEPTH / 2, TRAIL_HEIGHT, endPoint.Z + lengthToIgnore);
                    GL.Vertex3(beginningPoint.X + TRAIL_DEPTH / 2, TRAIL_HEIGHT, beginningPoint.Z);
                    GL.End();

                    break;
            }
        }

        public void Draw()
        {
            GL.PushAttrib(AttribMask.AllAttribBits);

            GL.BindTexture(TextureTarget.Texture2D, 0); //TODO: add Texture

            GL.Color4(color.R, color.G, color.B, TRAIL_OPACITY);

            if (isCurrentTrail)
                DrawLength(BIKE_LENGTH);
            else
                DrawLength(0.0f);

            GL.PopAttrib();

        }

    }
}
