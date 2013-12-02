using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRON
{
    class GameLogic
    {

        public static bool IsGameOver(List<Player> gamePlayers)
        {
            if (!gamePlayers.Find(i => i.isHumanPlayer).isAlive)
                return true;

            foreach (Player enemy in gamePlayers.FindAll(i => !i.isHumanPlayer))
            {
                if (enemy.isAlive)
                    return false;
            }

            return true;
        }
    }
}
