using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace TRON
{
    class CollisionManager
    {

        public static bool CollideWithMap(Player player, char[,] mapObstacles)
        {
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

                        if (player.position.X > obstacle_x1 && player.position.X < obstacle_x2)
                        {
                            if (player.position.Z > obstacle_y1 && player.position.Z < obstacle_y2)
                            {
                                return true;
                            }
                        }
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

            switch (player.direction)
            {
                case PlayerDirection.UP:
                    if (player.position.X + TrailSector.COLLISION_THRESHOLD > obstacle_x1 && player.position.X + TrailSector.COLLISION_THRESHOLD < obstacle_x2)
                    {
                        if (player.position.Z > obstacle_y1 && player.position.Z < obstacle_y2)
                        {
                            return true;
                        }
                    }
                    break;

                case PlayerDirection.DOWN:
                    if (player.position.X - TrailSector.COLLISION_THRESHOLD > obstacle_x1 && player.position.X - TrailSector.COLLISION_THRESHOLD < obstacle_x2)
                    {
                        if (player.position.Z > obstacle_y1 && player.position.Z < obstacle_y2)
                        {
                            return true;
                        }
                    }
                    break;


                case PlayerDirection.RIGHT:
                    if (player.position.X > obstacle_x1 && player.position.X < obstacle_x2)
                    {
                        if (player.position.Z + TrailSector.COLLISION_THRESHOLD > obstacle_y1 && player.position.Z + TrailSector.COLLISION_THRESHOLD < obstacle_y2)
                        {
                            return true;
                        }
                    }
                    break;

                case PlayerDirection.LEFT:
                    if (player.position.X > obstacle_x1 && player.position.X < obstacle_x2)
                    {
                        if (player.position.Z - TrailSector.COLLISION_THRESHOLD > obstacle_y1 && player.position.Z - TrailSector.COLLISION_THRESHOLD < obstacle_y2)
                        {
                            return true;
                        }
                    }
                    break;
            }
            
            return false;
        }
    }
}
