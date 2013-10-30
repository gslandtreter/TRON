
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Tron;


namespace TRON
{
    /// <summary>
    /// Demonstrates immediate mode rendering.
    /// </summary>
    public class TRONWindow : GameWindow
    {
        Mesh cycle;
        Mapa myMap;
        Camera camera;
        Player player1;
        Player player2;

        public TRONWindow()
            : base(800, 600, new GraphicsMode(16, 16), "TRON")
        {
           
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            myMap = new Mapa();
            camera = new Camera();
            player1 = new Player();
            player2 = new Player();

            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
           
            cycle = ObjLoader.LoadFile("TronBike.obj");

            if (cycle != null)
            {
                player1.textureID = cycle.LoadTexture("Textures//bike blue.png"); //TODO: Wrap to texture loader
                player2.textureID = cycle.LoadTexture("Textures//bike red.png"); //TODO: Wrap to texture loader
                
                cycle.LoadBuffers();

                player1.mesh = cycle;
                player2.mesh = cycle;
            }

            player1.position = new Vector3(10, 0, 10);
            player2.position = new Vector3(15, 0, 10);

            myMap.loadMap("map.txt");
        }

        /// <summary>
        /// Called when the user resizes the window.
        /// </summary>
        /// <param name="e">Contains the new width/height of the window.</param>
        /// <remarks>
        /// You want the OpenGL viewport to match the window. This is the place to do it!
        /// </remarks>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            double aspect_ratio = Width / (double)Height;

            OpenTK.Matrix4 perspective = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 1000);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
        }


        /// <summary>
        /// Prepares the next frame for rendering.
        /// </summary>
        /// <remarks>
        /// Place your control logic here. This is the place to respond to user input,
        /// update object positions etc.
        /// </remarks>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (Keyboard[OpenTK.Input.Key.Escape])
            {
                this.Exit();
                return;
            }

            player1.updatePlayerPos(Keyboard);
            player2.updatePlayerPos(Keyboard);

            camera.updateCamera(Keyboard, Mouse);

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 lookat = Matrix4.LookAt(10, 10, 10, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            camera.doCamera();
            myMap.Render();

            player1.drawPlayer();
            player2.drawPlayer();

            this.SwapBuffers();
        }

    }
}
