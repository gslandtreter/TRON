using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRON
{
    class AI
    {
        float POSITION_LOOKAHEAD = 0.2f;

        static public Player crashTestDummy = null;

        char[,] mapObstacles;

        public AI()
        {

        }

        public void SetMapObstacles(char[,] obstacles)
        {
            this.mapObstacles = obstacles;
        }

        public void Think(Player player, List<Player> gamePlayersList)
        {
            if (GonnaCollide(player, gamePlayersList))
            {
                switch (player.direction)
                {
                    case PlayerDirection.UP:
                        player.setDirection(PlayerDirection.LEFT);
                        break;

                    case PlayerDirection.LEFT:
                        player.setDirection(PlayerDirection.DOWN);
                        break;

                    case PlayerDirection.DOWN:
                        player.setDirection(PlayerDirection.RIGHT);
                        break;

                    case PlayerDirection.RIGHT:
                        player.setDirection(PlayerDirection.UP);
                        break;
                }
                
            }
        }


        public bool GonnaCollide(Player player, List<Player> gamePlayersList)
        {
            Vector3 furtherPos = GetFurtherPosition(player);

            Rectangle hitBox = Rectangle.GetHitBox(furtherPos, player.direction);

            if(CollisionManager.CollideWithMap(furtherPos, player.direction, mapObstacles))
            {
                return true;
            }

            foreach (Player collisionTestPlayer in gamePlayersList)
            {
                if (!collisionTestPlayer.isAlive)
                    continue;

                if (player != collisionTestPlayer && CollisionManager.CollideWithTrail(furtherPos, player.direction, collisionTestPlayer.currentTrail))
                    return true;

                if (player != collisionTestPlayer && hitBox.CollideWithRectancle(collisionTestPlayer.hitBox))
                    return true;

                foreach (TrailSector trailSector in collisionTestPlayer.trailHistory)
                {
                    if (collisionTestPlayer == player && trailSector.isFirstOnHistory)
                        continue;

                    if (CollisionManager.CollideWithTrail(furtherPos, player.direction, trailSector))
                        return true;
                }
            }
            return false;
        }

        public Vector3 GetFurtherPosition(Player player)
        {
            Vector3 newPos = player.position;

            switch (player.direction)
            {
                case PlayerDirection.UP:
                    newPos.X += POSITION_LOOKAHEAD;
                    break;

                case PlayerDirection.DOWN:
                    newPos.X -= POSITION_LOOKAHEAD;
                    break;

                case PlayerDirection.RIGHT:
                    newPos.Z += POSITION_LOOKAHEAD;
                    break;

                case PlayerDirection.LEFT:
                    newPos.Z -= POSITION_LOOKAHEAD;
                    break;

            }

            return newPos;
        }
    }
}
