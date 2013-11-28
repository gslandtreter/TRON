
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace TRON
{
    /// <summary>
    /// Demonstrates immediate mode rendering.
    /// </summary>
    public class TRONWindow : GameWindow
    {

        private static int WINDOW_WIDTH = 800;
        private static int WINDOW_HEIGHT = 600;

        Mesh cycle;
        Mapa myMap;
        //DebugCamera camera;
        ThirdPersonCamera thirdPersonCamera;
        TopCamera topCamera;
        AI ai;
        Player player1;
        Player player2;
        Player player3;

        List<Player> gamePlayers;
        bool cameraMode = false;


        float[] ambientLight = { 0.2f, 0.2f, 0.8f };
        float[] diffuseLight = { 0.8f, 0.8f, 0.8f };
        float[] specularLight = { 0.5f, 0.5f, 0.8f };
        float[] position = { 15, 15, 15 };

        public TRONWindow()
            : base(WINDOW_WIDTH, WINDOW_HEIGHT, new GraphicsMode(16, 16), "TRON")
        {
           
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            myMap = new Mapa();
            myMap.loadMap("map.txt");

            thirdPersonCamera = new ThirdPersonCamera();
            topCamera = new TopCamera();
            ai = new AI();

            ai.SetMapObstacles(myMap.mapObstacles);
            
            gamePlayers = new List<Player>();
            player1 = new Player(myMap.mapObstacles, Color.BlueViolet);
            player2 = new Player(myMap.mapObstacles, Color.Crimson);
            player3 = new Player(myMap.mapObstacles,Color.Pink);

            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Normalize);

            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.Light(LightName.Light0, LightParameter.Position, position);
            GL.Light(LightName.Light0, LightParameter.Diffuse, diffuseLight);
            GL.Light(LightName.Light0, LightParameter.Ambient, ambientLight);
            GL.Light(LightName.Light0, LightParameter.Specular, specularLight);



            cycle = ObjLoader.LoadFile("TronBike.obj");

            if (cycle != null)
            {
                player1.textureID = cycle.LoadTexture("Textures//bike blue.png"); //TODO: Wrap to texture loader
                player2.textureID = cycle.LoadTexture("Textures//bike red.png"); //TODO: Wrap to texture loader
              player3.textureID = cycle.LoadTexture("Textures//bike rose.png");

                myMap.texturaChao = Texture.LoadTex("Textures//grid.jpg");
                myMap.texturaParede = Texture.LoadTex("Textures//wall2.jpg");
                myMap.texturaObstaculo = Texture.LoadTex("Textures//Obstaculo.jpg");
                cycle.LoadBuffers();

                player1.mesh = cycle;
                player2.mesh = cycle;
                player3.mesh = cycle;
            }

            player1.isHumanPlayer = true;
            player2.speed = 12;
            player3.speed = 12;
            
            gamePlayers.Add(player1);
            gamePlayers.Add(player2);
            gamePlayers.Add(player3);
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

            foreach (Player player in gamePlayers)
            {
                if (!player.isAlive)
                    continue;

                if (player.isHumanPlayer)
                    player.updatePlayerPos(Keyboard, e.Time, cameraMode);
                else
                {
                    ai.Think(player, gamePlayers);
                    player.updatePlayerPos(null, e.Time, cameraMode);
                }
                    

                if (CollisionManager.CollideWithMap(player, myMap.mapObstacles))
                {
                    player.Die();
                }

                foreach (Player collisionTestPlayer in gamePlayers)
                {
                    if (!collisionTestPlayer.isAlive)
                        continue;

                    if (player != collisionTestPlayer && CollisionManager.CollideWithTrail(player, collisionTestPlayer.currentTrail))
                    {
                        player.Die();
                    }

                    if (player != collisionTestPlayer && player.hitBox.CollideWithRectancle(collisionTestPlayer.hitBox))
                    {
                        player.Die();
                        collisionTestPlayer.Die();
                    }

                    foreach (TrailSector trailSector in collisionTestPlayer.trailHistory)
                    {
                        if (collisionTestPlayer == player && trailSector.isFirstOnHistory)
                            continue;

                        if (CollisionManager.CollideWithTrail(player, trailSector))
                        {
                            player.Die();
                        }
                    }
                }

            }

        }

        protected override void OnKeyPress(OpenTK.KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (Keyboard[OpenTK.Input.Key.V])
            {
                cameraMode = !cameraMode;
            }
        }

        protected void DrawGame()
        {
            foreach (Player player in gamePlayers)
            {
                if (!player.isAlive)
                    continue;

                player.drawPlayer();
                player.drawTrail();
            }


            //REFLECTION

            GL.Enable(EnableCap.StencilTest);
            GL.ColorMask(false, false, false, false);
            GL.Disable(EnableCap.DepthTest);
            GL.StencilFunc(StencilFunction.Always, 1, 1);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
            myMap.RenderFloor(1.0f);

            GL.ColorMask(true, true, true, true);
            GL.Enable(EnableCap.DepthTest);
            GL.StencilFunc(StencilFunction.Equal, 1, 1);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);

            GL.PushMatrix();

            GL.Scale(1, -1, 1);
            myMap.RenderWalls();

            foreach (Player player in gamePlayers)
            {
                if (!player.isAlive)
                    continue;

                player.drawPlayer();
                player.drawTrail();
            }

            GL.PopMatrix();
            GL.Disable(EnableCap.StencilTest);
            //ENDOF Reflection

            myMap.Render();

        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);



            if (cameraMode)
            {
                GL.Viewport(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);
                topCamera.doCamera(myMap.sizeX * Mapa.MAP_UNIT_SIZE, myMap.sizeY * Mapa.MAP_UNIT_SIZE);
                DrawGame();
            }

            else
            {
                GL.Viewport(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);
                thirdPersonCamera.doCameraOnPlayer(gamePlayers.Find(i => i.isHumanPlayer));

                DrawGame();

                GL.Clear(ClearBufferMask.DepthBufferBit);

                GL.Viewport(0, WINDOW_HEIGHT - 150, 180, 150);

                topCamera.doCamera(myMap.sizeX * Mapa.MAP_UNIT_SIZE, myMap.sizeY * Mapa.MAP_UNIT_SIZE);

                DrawGame();

                
            }
            this.SwapBuffers();
        }

    }
}
