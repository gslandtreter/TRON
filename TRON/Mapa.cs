using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace TRON
{
    class Mapa
    {
        public static int MAP_UNIT_SIZE = 5;

        public char[,] mapObstacles; // mapa carregado do txt tá aqui. 
        public int sizeX;
        public int sizeY;

        public uint texturaChao;
        public uint texturaParede;
        public uint texturaObstaculo;

        public static Rectangle leftWallHitBox, rightWallHitBox, bottomWallHitBox, topWallHitBox;

        public Mapa()
        {
            
        }

        public void RenderFloor(float alpha)
        {
            GL.PushAttrib(AttribMask.AllAttribBits);

            GL.BindTexture(TextureTarget.Texture2D, texturaChao);

            GL.Material(MaterialFace.Front, MaterialParameter.AmbientAndDiffuse, Color4.AliceBlue);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, Color4.White);

            //chão
            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, 1, 0);
            GL.Color4(0.0f, 0.0f, 1.0f, alpha);

            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);

            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, (float)sizeY * MAP_UNIT_SIZE);

            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 0.0f, (float)sizeY * MAP_UNIT_SIZE);

            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 0.0f, 0.0f);
            GL.End();

            GL.PopAttrib();
        }

        public void RenderWalls()
        {
            GL.PushAttrib(AttribMask.AllAttribBits);

            GL.Color3(Color.White);

            GL.BindTexture(TextureTarget.Texture2D, texturaParede);


            //esquerda
            GL.Begin(BeginMode.Quads);
            GL.Normal3(1, 0, 0);
            //GL.Color3(1.0f, 0.0f, 0.0f);

            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);

            GL.TexCoord2(0.0f, sizeY * 1.0f);
            GL.Vertex3(0.0f, 0.0f, (float)sizeY * MAP_UNIT_SIZE);

            GL.TexCoord2(1.0f, sizeY * 1.0f);
            GL.Vertex3(0.0f, 3.0f, (float)sizeY * MAP_UNIT_SIZE);

            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(0.0f, 3.0f, 0.0f);
            GL.End();

            //direita
            GL.Begin(BeginMode.Quads);
            GL.Normal3(-1, 0, 0);
            //GL.Color3(0.0f, 1.0f, 0.0f);

            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 0.0f, 0.0f);

            GL.TexCoord2(0.0f, sizeY * 1.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 0.0f, (float)sizeY * MAP_UNIT_SIZE);

            GL.TexCoord2(1.0f, sizeY * 1.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 3.0f, (float)sizeY * MAP_UNIT_SIZE);

            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 3.0f, 0.0f);
            GL.End();

            //fundo 
            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, 0, 1);
            //GL.Color3(0.0f, 0.0f, 1.0f); 

            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, (float)sizeY * MAP_UNIT_SIZE);

            GL.TexCoord2(0.0f, sizeX * 1.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 0.0f, (float)sizeY * MAP_UNIT_SIZE);

            GL.TexCoord2(1.0f, sizeX * 1.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 3.0f, (float)sizeY * MAP_UNIT_SIZE);

            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(0.0f, 3.0f, (float)sizeY * MAP_UNIT_SIZE);
            GL.End();

            //frente
            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, 0, -1);
            //GL.Color3(0.0f, 0.0f, 1.0f);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);

            GL.TexCoord2(0.0f, sizeX * 1.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 0.0f, 0.0f);

            GL.TexCoord2(1.0f, sizeX * 1.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 3.0f, 0.0f);

            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(0.0f, 3.0f, 0.0f);
            GL.End();

            RenderObstacles();
            GL.PopAttrib();
        }
        //Cada ponto da matriz é um quadrado 2x2;
        public  void Render()
        {
            RenderFloor(0.5f);
            RenderWalls();
         }


        private void RenderObstacles()
        { 
            for(int i =0; i < sizeY; i++)
                for (int j = 0; j < sizeX; j++)
                {
                    if (mapObstacles[i,j] == '1')
                    {
                        GL.PushAttrib(AttribMask.AllAttribBits);

                        GL.BindTexture(TextureTarget.Texture2D, texturaObstaculo);

                        //teto
                        GL.Begin(BeginMode.Quads);
                       // GL.Color3(1.0f, 1.0f, 0.0f); 
                        GL.Normal3(0, 1, 0);

                        GL.TexCoord2(0.0f, 0.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(0.0f, 1.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(1.0f, 1.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(1.0f, 0.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        
                        GL.End();

                        //direita
                        GL.Begin(BeginMode.Quads);
                        //GL.Color3(1.0f, 1.0f, 0.0f); 
                        GL.Normal3(1, 0, 0);
            
                        GL.TexCoord2(0.0f, 0.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(0.0f, 1.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(1.0f, 1.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(1.0f, 0.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        
                        GL.End();
                        
                        //atras
                        GL.Begin(BeginMode.Quads);
                        //GL.Color3(1.0f, 0.0f, 0.0f);
                        GL.Normal3(0, 0, +1);
            
                        GL.TexCoord2(0.0f, 0.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(1.0f, 0.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(1.0f, 1.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(0.0f, 1.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        GL.End();
                        
                        //frente
                        GL.Begin(BeginMode.Quads);
                        //GL.Color3(0.0f, 0.0f, 1.0f);
                        GL.Normal3(0, 0, -1);
            
                        GL.TexCoord2(0.0f, 0.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(1.0f, 0.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(1.0f, 1.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(0.0f, 1.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE);
                        GL.End();
                        
                        //esquerda
                        GL.Begin(BeginMode.Quads);
                        //GL.Color3(0.0f, 1.0f, 0.0f); 
                        GL.Normal3(-1, 0, 0);
            
                        GL.TexCoord2(0.0f, 0.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(1.0f, 0.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(1.0f, 1.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        
                        GL.TexCoord2(0.0f, 1.0f);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE);
                        GL.End();

                        GL.PopAttrib();

                        
                    }
                }
        }

        public void loadMap(string fileName)
        {
            checkSize(fileName);

            mapObstacles = new char[sizeY, sizeX];
            
            FileStream arquivo = File.OpenRead(fileName);
            int i = 0, j = 0;

            if (arquivo == null)
                return;

            while (arquivo.CanRead == true && arquivo.Position < arquivo.Length && i < sizeX)
            {
                char caracter = (char)arquivo.ReadByte();
                if (caracter.Equals('\n') == false && caracter.Equals('\r') == false)
                {
                    mapObstacles[i, j] = caracter;
                    j++;
                    if (j >= sizeX)
                    {
                        j = 0;
                        i++;
                    }
                }
            }
            arquivo.Close();

            CreateHitBoxes();
        }

        private void checkSize(string fileName)
        {
            StreamReader arquivo = new StreamReader(fileName);
            String line;
            while ((line = arquivo.ReadLine()) != null)
            {
                this.sizeY++;
                this.sizeX = line.Length;
            }
            arquivo.Close();
        }

        private void CreateHitBoxes()
        {
            leftWallHitBox = new Rectangle(-5, 0, 0, mapObstacles.GetLength(0) * Mapa.MAP_UNIT_SIZE);
            rightWallHitBox = new Rectangle(mapObstacles.GetLength(1) * Mapa.MAP_UNIT_SIZE, (mapObstacles.GetLength(1) * Mapa.MAP_UNIT_SIZE) + 5, 0, mapObstacles.GetLength(0) * Mapa.MAP_UNIT_SIZE);
            bottomWallHitBox = new Rectangle(0, mapObstacles.GetLength(1) * Mapa.MAP_UNIT_SIZE, -5, 0);
            topWallHitBox = new Rectangle(0, mapObstacles.GetLength(1) * Mapa.MAP_UNIT_SIZE, mapObstacles.GetLength(0) * Mapa.MAP_UNIT_SIZE, (mapObstacles.GetLength(0) * Mapa.MAP_UNIT_SIZE + 5));
        }

    }
}
