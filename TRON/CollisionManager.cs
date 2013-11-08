using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace TRON
{
    class Rectangle
    {
        public float xmin, xmax, ymin, ymax;

        public static float STATIC_BIKE_LENGTH = 1.5f;
        public static float STATIC_BIKE_WIDTH = 0.6f;

        public Rectangle(float x_min, float x_max, float y_min, float y_max)
        {
            xmin = x_min;
            xmax = x_max;

            ymin = y_min;
            ymax = y_max;
        }

        public bool CollideWithRectancle(Rectangle collideWith)
        {
            if (collideWith == null)
                return false;

            return ((collideWith.xmax >= this.xmin && collideWith.xmin <= this.xmax) 
                && 
                (collideWith.ymax >= this.ymin && collideWith.ymin <= this.ymax));
        }

        public void Draw()
        {
            GL.PushAttrib(AttribMask.AllAttribBits);
            GL.PushMatrix();

            GL.Begin(BeginMode.LineLoop);

            GL.Vertex3(new Vector3d(xmin, 1, ymin));
            GL.Vertex3(new Vector3d(xmin, 1, ymax));
            GL.Vertex3(new Vector3d(xmax, 1, ymax));
            GL.Vertex3(new Vector3d(xmax, 1, ymin));

            GL.End();

            GL.PopMatrix();
            GL.PopAttrib();
        }

        public static Rectangle GetPlayerHitBox(Player player)
        {
            float xmin = 0, xmax = 0, ymin = 0, ymax = 0;

            switch (player.direction)
            {
                case PlayerDirection.UP:
                case PlayerDirection.DOWN:

                    xmin = (float)player.position.X - STATIC_BIKE_LENGTH;
                    xmax = (float)player.position.X + STATIC_BIKE_LENGTH;

                    ymin = (float)player.position.Z - STATIC_BIKE_WIDTH;
                    ymax = (float)player.position.Z + STATIC_BIKE_WIDTH;

                    break;

                case PlayerDirection.LEFT:
                case PlayerDirection.RIGHT:

                    xmin = (float)player.position.X - STATIC_BIKE_WIDTH;
                    xmax = (float)player.position.X + STATIC_BIKE_WIDTH;

                    ymin = (float)player.position.Z - STATIC_BIKE_LENGTH;
                    ymax = (float)player.position.Z + STATIC_BIKE_LENGTH;

                    break;
            }

            Rectangle rect = new Rectangle(xmin, xmax, ymin, ymax);
            return rect;
        }

        public static Rectangle GetHitBox(Vector3 position, PlayerDirection direction)
        {
            float xmin = 0, xmax = 0, ymin = 0, ymax = 0;

            switch (direction)
            {
                case PlayerDirection.UP:
                case PlayerDirection.DOWN:

                    xmin = (float)position.X - STATIC_BIKE_LENGTH;
                    xmax = (float)position.X + STATIC_BIKE_LENGTH;

                    ymin = (float)position.Z - STATIC_BIKE_WIDTH;
                    ymax = (float)position.Z + STATIC_BIKE_WIDTH;

                    break;

                case PlayerDirection.LEFT:
                case PlayerDirection.RIGHT:

                    xmin = (float)position.X - STATIC_BIKE_WIDTH;
                    xmax = (float)position.X + STATIC_BIKE_WIDTH;

                    ymin = (float)position.Z - STATIC_BIKE_LENGTH;
                    ymax = (float)position.Z + STATIC_BIKE_LENGTH;

                    break;
            }

            Rectangle rect = new Rectangle(xmin, xmax, ymin, ymax);
            return rect;
        }
        
    }

    class CollisionManager
    {

        public static bool CollideWithMap(Player player, char[,] mapObstacles)
        {
            //External Walls
            if (Mapa.leftWallHitBox.CollideWithRectancle(player.hitBox) ||
                Mapa.rightWallHitBox.CollideWithRectancle(player.hitBox) ||
                Mapa.bottomWallHitBox.CollideWithRectancle(player.hitBox) ||
                Mapa.topWallHitBox.CollideWithRectancle(player.hitBox))
                return true;


            //Obstacles
            for(int i =0; i < mapObstacles.GetLength(0); i++)
            {
                for (int j = 0; j < mapObstacles.GetLength(1); j++)
                {
                    if (mapObstacles[i, j] == '1')
                    {
                        float obstacle_x1, obstacle_x2, obstacle_y1, obstacle_y2;

                        obstacle_x1 = j * Mapa.MAP_UNIT_SIZE;
                        obstacle_x2 = obstacle_x1 + Mapa.MAP_UNIT_SIZE;

                        obstacle_y1 = i * Mapa.MAP_UNIT_SIZE;
                        obstacle_y2 = obstacle_y1 + Mapa.MAP_UNIT_SIZE;

                        Rectangle obstacleHitBox = new Rectangle(obstacle_x1, obstacle_x2, obstacle_y1, obstacle_y2);

                        if (obstacleHitBox.CollideWithRectancle(player.hitBox))
                            return true;
                    }
                }
            }

            return false;
        }

        public static bool CollideWithMap(Vector3 position, PlayerDirection direction, char[,] mapObstacles)
        {

            Rectangle playerHitBox = Rectangle.GetHitBox(position, direction);

            //External Walls
            if (Mapa.leftWallHitBox.CollideWithRectancle(playerHitBox) ||
                Mapa.rightWallHitBox.CollideWithRectancle(playerHitBox) ||
                Mapa.bottomWallHitBox.CollideWithRectancle(playerHitBox) ||
                Mapa.topWallHitBox.CollideWithRectancle(playerHitBox))
                return true;


            //Obstacles
            for (int i = 0; i < mapObstacles.GetLength(0); i++)
            {
                for (int j = 0; j < mapObstacles.GetLength(1); j++)
                {
                    if (mapObstacles[i, j] == '1')
                    {
                        float obstacle_x1, obstacle_x2, obstacle_y1, obstacle_y2;

                        obstacle_x1 = j * Mapa.MAP_UNIT_SIZE;
                        obstacle_x2 = obstacle_x1 + Mapa.MAP_UNIT_SIZE;

                        obstacle_y1 = i * Mapa.MAP_UNIT_SIZE;
                        obstacle_y2 = obstacle_y1 + Mapa.MAP_UNIT_SIZE;

                        Rectangle obstacleHitBox = new Rectangle(obstacle_x1, obstacle_x2, obstacle_y1, obstacle_y2);

                        if (obstacleHitBox.CollideWithRectancle(playerHitBox))
                            return true;
                    }
                }
            }

            return false;
        }

        public static bool CollideWithTrail(Player player, TrailSector trailSector)
        {
            float obstacle_x1 = 0, obstacle_x2 = 0, obstacle_y1 = 0, obstacle_y2 = 0;

            switch (trailSector.direction)
            {
                case PlayerDirection.LEFT:
                    obstacle_x1 = (float)trailSector.beginningPoint.X - TrailSector.TRAIL_DEPTH;
                    obstacle_x2 = (float)trailSector.beginningPoint.X + TrailSector.TRAIL_DEPTH;

                    obstacle_y2 = (float)trailSector.beginningPoint.Z;
                    obstacle_y1 = (float)trailSector.endPoint.Z;
                    break;

                case PlayerDirection.RIGHT:
                    obstacle_x1 = (float)trailSector.beginningPoint.X - TrailSector.TRAIL_DEPTH;
                    obstacle_x2 = (float)trailSector.beginningPoint.X + TrailSector.TRAIL_DEPTH;

                    obstacle_y1 = (float)trailSector.beginningPoint.Z;
                    obstacle_y2 = (float)trailSector.endPoint.Z;
                    break;

                case PlayerDirection.UP:
                    obstacle_x1 = (float)trailSector.beginningPoint.X;
                    obstacle_x2 = (float)trailSector.endPoint.X;

                    obstacle_y1 = (float)trailSector.beginningPoint.Z - TrailSector.TRAIL_DEPTH;
                    obstacle_y2 = (float)trailSector.endPoint.Z + TrailSector.TRAIL_DEPTH;
                    break;

                case PlayerDirection.DOWN:
                    obstacle_x2 = (float)trailSector.beginningPoint.X;
                    obstacle_x1 = (float)trailSector.endPoint.X;

                    obstacle_y1 = (float)trailSector.beginningPoint.Z - TrailSector.TRAIL_DEPTH;
                    obstacle_y2 = (float)trailSector.endPoint.Z + TrailSector.TRAIL_DEPTH;
                    break;
            }

            Rectangle trailRect = new Rectangle(obstacle_x1, obstacle_x2, obstacle_y1, obstacle_y2);
            return trailRect.CollideWithRectancle(player.hitBox);
        }

        public static bool CollideWithTrail(Vector3 position, PlayerDirection direction, TrailSector trailSector)
        {
            float obstacle_x1 = 0, obstacle_x2 = 0, obstacle_y1 = 0, obstacle_y2 = 0;

            switch (trailSector.direction)
            {
                case PlayerDirection.LEFT:
                case PlayerDirection.RIGHT:
                    obstacle_x1 = (float)trailSector.beginningPoint.X - TrailSector.TRAIL_DEPTH;
                    obstacle_x2 = (float)trailSector.beginningPoint.X + TrailSector.TRAIL_DEPTH;

                    obstacle_y1 = (float)trailSector.beginningPoint.Z;
                    obstacle_y2 = (float)trailSector.endPoint.Z;
                    break;

                case PlayerDirection.UP:
                case PlayerDirection.DOWN:
                    obstacle_x1 = (float)trailSector.beginningPoint.X;
                    obstacle_x2 = (float)trailSector.endPoint.X;

                    obstacle_y1 = (float)trailSector.beginningPoint.Z - TrailSector.TRAIL_DEPTH;
                    obstacle_y2 = (float)trailSector.endPoint.Z + TrailSector.TRAIL_DEPTH;
                    break;
            }

            Rectangle playerHitBox = Rectangle.GetHitBox(position, direction);
            Rectangle trailRect = new Rectangle(obstacle_x1, obstacle_x2, obstacle_y1, obstacle_y2);
            return trailRect.CollideWithRectancle(playerHitBox);
        }
    }
}
