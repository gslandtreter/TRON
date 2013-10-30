using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Windows.Forms;

namespace TRON
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            
            using (TRONWindow janela = new TRONWindow())
            {
                janela.Run(60);
            }

        }
    }
}
