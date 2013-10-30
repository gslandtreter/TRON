﻿using System;
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

namespace Tron
{
    

    class Mapa
    {
        public static int MAP_UNIT_SIZE = 5;

        private char[,] mapObstacles; // mapa carregado do txt tá aqui. 
        private int sizeX;
        private int sizeY;
      
        public Mapa()
        {
            
        }

        //Cada ponto da matriz é um quadrado 2x2;
        public  void Render()
        {
            GL.PushAttrib(AttribMask.AllAttribBits);

            //chão
            GL.Begin(BeginMode.Quads);
            GL.Color3(0.5f, 0.0f, 0.5f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, (float)sizeY * MAP_UNIT_SIZE);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 0.0f, (float)sizeY * MAP_UNIT_SIZE);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 0.0f, 0.0f);
            GL.End();

            /*
            //esquerda
            GL.Begin(BeginMode.Quads);
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f ,0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, (float)sizeY * MAP_UNIT_SIZE);
            GL.Vertex3(0.0f, 3.0f, (float)sizeY * MAP_UNIT_SIZE);
            GL.Vertex3(0.0f, 3.0f, 0.0f);
            GL.End();

            //direita
            GL.Begin(BeginMode.Quads);
            GL.Color3(0.0f, 1.0f, 0.0f); 
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX , 0.0f, 0.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 0.0f, (float)sizeY * MAP_UNIT_SIZE);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 3.0f, (float)sizeY * MAP_UNIT_SIZE);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 3.0f, 0.0f);
            GL.End();

            //fundo 
            GL.Begin(BeginMode.Quads);
            GL.Color3(0.0f, 0.0f, 1.0f); 
            GL.Vertex3(0.0f, 0.0f, (float)sizeY * MAP_UNIT_SIZE);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 0.0f, (float)sizeY * MAP_UNIT_SIZE);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 3.0f, (float)sizeY * MAP_UNIT_SIZE);
            GL.Vertex3(0.0f, 3.0f, (float)sizeY * MAP_UNIT_SIZE);
            GL.End();

            //frente
            GL.Begin(BeginMode.Quads);
            GL.Color3(1.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 0.0f, 0.0f);
            GL.Vertex3(MAP_UNIT_SIZE * (float)sizeX, 3.0f, 0.0f);
            GL.Vertex3(0.0f, 3.0f, 0.0f);
            GL.End();

            */

            RenderObstacles();

            GL.PopAttrib();
        }


        private void RenderObstacles()
        { 
            for(int i =0; i < sizeY; i++)
                for (int j = 0; j < sizeX; j++)
                {
                    if (mapObstacles[i,j] == '1')
                    {
                        //teto
                        GL.Begin(BeginMode.Quads);
                        GL.Color3(1.0f, 1.0f, 0.0f); 
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 3.0f,     (float)i * MAP_UNIT_SIZE );
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE );
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE +MAP_UNIT_SIZE);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 3.0f,     (float)i * MAP_UNIT_SIZE +MAP_UNIT_SIZE );
                        GL.End();
                        
                        //direita
                        GL.Begin(BeginMode.Quads);
                        GL.Color3(1.0f, 0.0f, 0.0f); 
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE );
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE );
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        GL.End();
                        
                        //atras
                        GL.Begin(BeginMode.Quads);
                        GL.Color3(1.0f, 1.0f, 1.0f); 
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE );
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE );
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE );
                        GL.End();
                        
                        //frente
                        GL.Begin(BeginMode.Quads);
                        GL.Color3(1.0f, 1.0f, 0.0f); 
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE );
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE + MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE);
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE );
                        GL.End();
                        
                        //esquerda
                        GL.Begin(BeginMode.Quads);
                        GL.Color3(0.0f, 1.0f, 0.0f); 
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE );
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 0.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE );
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE + MAP_UNIT_SIZE );
                        GL.Vertex3(0.0f + j * MAP_UNIT_SIZE, 3.0f, (float)i * MAP_UNIT_SIZE );
                        GL.End();
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

    }
}
