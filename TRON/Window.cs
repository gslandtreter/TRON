
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
        //DebugCamera camera;
        ThirdPersonCamera thirdPersonCamera;
        TopCamera topCamera;
        Player player1;
        Player player2;

        bool cameraMode = false;

        public TRONWindow()
            : base(800, 600, new GraphicsMode(16, 16), "TRON")
        {
           
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            myMap = new Mapa();
            thirdPersonCamera = new ThirdPersonCamera();
            topCamera = new TopCamera();
            player1 = new Player(new Vector3(10, 0, 10), Color.BlueViolet);
            player2 = new Player(new Vector3(15, 0, 10), Color.Crimson);

            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);


            cycle = ObjLoader.LoadFile("TronBike.obj");

            if (cycle != null)
            {
                player1.textureID = cycle.LoadTexture("Textures//bike blue.png"); //TODO: Wrap to texture loader
                player2.textureID = cycle.LoadTexture("Textures//bike red.png"); //TODO: Wrap to texture loader

                myMap.texturaChao = Texture.LoadTex("Textures//grid.jpg");

                cycle.LoadBuffers();

                player1.mesh = cycle;
                player2.mesh = cycle;
            }

            player2.speed = 12;

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

            player1.updatePlayerPos(Keyboard, e.Time);
            player2.updatePlayerPos(Keyboard, e.Time);

            //camera.updateCamera(Keyboard, Mouse);

        }

        protected override void OnKeyPress(OpenTK.KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (Keyboard[OpenTK.Input.Key.V])
            {
                cameraMode = !cameraMode;
            }
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if(cameraMode)
                topCamera.doCamera(myMap.sizeX * Mapa.MAP_UNIT_SIZE, myMap.sizeY * Mapa.MAP_UNIT_SIZE);
            else
                thirdPersonCamera.doCameraOnPlayer(player1);
            

            myMap.Render();

            player1.drawPlayer();
            player2.drawPlayer();

            player1.drawTrail();
            player2.drawTrail();

            this.SwapBuffers();
        }

    }
}
