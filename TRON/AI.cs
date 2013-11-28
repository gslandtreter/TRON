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
                        {// Confere se tem objeto para um lado
                            player.setDirection(PlayerDirection.LEFT);
                            if (GonnaCollide(player, gamePlayersList))
                            {
                                player.setDirection(PlayerDirection.RIGHT);
                                if (GonnaCollide(player, gamePlayersList) == true)
                                {
                                    player.setDirection(PlayerDirection.RIGHT);
                                }
                            }
                            else
                            {   // confere se tem objeto no outro lado
                                player.setDirection(PlayerDirection.RIGHT);
                                if (GonnaCollide(player, gamePlayersList) == true)
                                {
                                    player.setDirection(PlayerDirection.LEFT);
                                }
                                else
                                { //numero randomico 
                                    Random randNum = new Random();
                                    if (randNum.NextDouble() > 0.5)
                                        player.setDirection(PlayerDirection.RIGHT);
                                    else
                                        player.setDirection(PlayerDirection.LEFT);


                                }
                            }


                            break;
                        }
                    case PlayerDirection.LEFT:
                        {
                            player.setDirection(PlayerDirection.DOWN);
                            if (GonnaCollide(player, gamePlayersList))
                            {
                                player.setDirection(PlayerDirection.UP);
                                if (GonnaCollide(player, gamePlayersList) == true)
                                {
                                    player.setDirection(PlayerDirection.UP);
                                }
                            }
                            else
                            {   // confere se tem objeto no outro lado
                                player.setDirection(PlayerDirection.UP);
                                if (GonnaCollide(player, gamePlayersList) == true)
                                {
                                    player.setDirection(PlayerDirection.DOWN);
                                }
                                else
                                { //numero randomico 
                                    Random randNum = new Random();
                                    if (randNum.NextDouble() > 0.5)
                                        player.setDirection(PlayerDirection.UP);
                                    else
                                        player.setDirection(PlayerDirection.DOWN);


                                }
                            }

                            break;
                        }
                    case PlayerDirection.DOWN:
                        {
                            player.setDirection(PlayerDirection.LEFT);
                            if (GonnaCollide(player, gamePlayersList))
                            {
                                player.setDirection(PlayerDirection.RIGHT);
                                if (GonnaCollide(player, gamePlayersList) == true)
                                {
                                    player.setDirection(PlayerDirection.RIGHT);
                                }
                            }
                            else
                            {   // confere se tem objeto no outro lado
                                player.setDirection(PlayerDirection.RIGHT);
                                if (GonnaCollide(player, gamePlayersList) == true)
                                {
                                    player.setDirection(PlayerDirection.LEFT);
                                }
                                else
                                { //numero randomico 
                                    Random randNum = new Random();
                                    if (randNum.NextDouble() > 0.5)
                                        player.setDirection(PlayerDirection.RIGHT);
                                    else
                                        player.setDirection(PlayerDirection.LEFT);
                                }
                            }
                            break;

                        }
                    case PlayerDirection.RIGHT:
                        {
                            player.setDirection(PlayerDirection.DOWN);
                            if (GonnaCollide(player, gamePlayersList))
                            {
                                player.setDirection(PlayerDirection.UP);
                                if (GonnaCollide(player, gamePlayersList) == true)
                                {
                                    player.setDirection(PlayerDirection.UP);
                                }
                            }
                            else
                            {   // confere se tem objeto no outro lado
                                player.setDirection(PlayerDirection.UP);
                                if (GonnaCollide(player, gamePlayersList) == true)
                                {
                                    player.setDirection(PlayerDirection.DOWN);
                                }
                                else
                                { //numero randomico 
                                    Random randNum = new Random();
                                    if (randNum.NextDouble() > 0.5)
                                        player.setDirection(PlayerDirection.UP);
                                    else
                                        player.setDirection(PlayerDirection.DOWN);
                                }
                            }
                            break;
                        }
                }

            }
             /*   
            else    // tomar decisões em linha reta
            {
                Random randNum = new Random();
                double number = randNum.NextDouble();
                if(number <0.05 || number > 0.95)
                {
                        //acha posição do player a ser seguido
                        foreach (Player others in gamePlayersList)
                        {

                            if (others.isHumanPlayer == true && others.isAlive == true)
                            {

                                if (player.position.Z > others.position.Z)
                                {
                                    if (player.direction == PlayerDirection.UP)
                                    { 
                                        player.direction = PlayerDirection.LEFT;
                                        if (GonnaCollide(player, gamePlayersList) == true)
                                        {
                                            player.direction = PlayerDirection.UP;
                                        }
                                        else
                                        {
                                            player.currentTrail.direction = PlayerDirection.LEFT;
                                            player.directionChanged = true; 
                                        }
                                    }
                                    if (player.direction == PlayerDirection.DOWN)
                                    {
                                        player.direction = PlayerDirection.LEFT;
                                        if (GonnaCollide(player, gamePlayersList) == true)
                                        {
                                            player.direction = PlayerDirection.DOWN;
                                        }    
                                        else
                                        {
                                            player.currentTrail.direction = PlayerDirection.LEFT;
                                            player.directionChanged = true;
                                        }
                                    }
                                    if (player.direction == PlayerDirection.RIGHT)
                                    {
                                        Random rand = new Random();
                                        double num = rand.NextDouble();
                                        if (num > 0.5)
                                        {
                                            player.direction = PlayerDirection.UP;
                                            if (GonnaCollide(player, gamePlayersList) == true)
                                            {
                                                player.direction = PlayerDirection.RIGHT;
                                            }
                                            else
                                            {
                                                player.currentTrail.direction = PlayerDirection.UP;
                                                player.directionChanged = true;
                                            }
                                        }
                                        else
                                        {
                                            player.direction = PlayerDirection.DOWN;
                                            if (GonnaCollide(player, gamePlayersList) == true)
                                            {
                                                player.direction = PlayerDirection.RIGHT;
                                            }
                                            else
                                            {
                                                player.currentTrail.direction = PlayerDirection.DOWN;
                                                player.directionChanged = true;
                                            }
                                        }
                                    }
                                    if (player.direction == PlayerDirection.LEFT)
                                    {
                                       
                                        
                                        
                                    }

                                }
                              
                                

                    
                            }
                
                
                
                        }
                }
                //senão continua reto
            
            
            }
            */
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

                if (player != collisionTestPlayer &&  CollisionManager.CollideWithTrail(furtherPos, player.direction, collisionTestPlayer.currentTrail))
                    return true;
                
                if (player != collisionTestPlayer && hitBox.CollideWithRectancle(collisionTestPlayer.hitBox))
                    return true;

                foreach (TrailSector trailSector in collisionTestPlayer.trailHistory)
                {
                    if (collisionTestPlayer != player || !trailSector.isFirstOnHistory)
                    {
                        if (CollisionManager.CollideWithTrail(furtherPos, player.direction, trailSector))
                            return true;
                    }
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
