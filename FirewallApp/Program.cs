using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirewallApp
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Verificar se existem argumentos
            if (args.Length > 0)
            {
                // Passar os argumentos diretamente para o Form1
                Application.Run(new Form1(args));
            }
            else
            {
                // Se não houver argumentos, rodar a interface gráfica normalmente
                Application.Run(new Form1());
            }
        }
    }
}