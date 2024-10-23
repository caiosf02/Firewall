using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirewallApp
{
    public partial class Form1 : Form
    {
        // Lista de executáveis
        private readonly List<string> executables = new List<string>()
        {
            "dcconfig.exe",
            "agentupgrader.exe",
            "AgentTrayIcon.exe",
            "dcagentservice.exe",
            "dcinventory.exe",
            "dcpatchscan.exe",
            "dcrdservice.exe",
            "dcstatusutil.exe",
            "qchain.exe",
            "rdsAgentWindow.exe"
        };

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(string[] args = null)
        {
            InitializeComponent();

            
            if (args != null && args.Length > 0)
            {
                string action = args[0].ToLower(); 

                switch (action)
                {
                    case "add":
                        Button1_Click(this, EventArgs.Empty);
                        break;

                    case "delete":
                        Button2_Click(this, EventArgs.Empty); 
                        break;

                    case "list":
                        Button3_Click(this, EventArgs.Empty); 
                        break;

                    default:
                        MessageBox.Show("Ação desconhecida. A interface será exibida.", "Ação Desconhecida");
                        break;
                }
            }
        }

        private void AddRule(string processName)
        {
            
            string commandIN = $"netsh advfirewall firewall add rule name=\"Allow {processName} IN\" dir=in action=allow program=\"{processName}\" enable=yes";
            string commandOUT = $"netsh advfirewall firewall add rule name=\"Allow {processName} OUT\" dir=out action=allow program=\"{processName}\" enable=yes";

            ExecuteCommand(commandIN);
            ExecuteCommand(commandOUT);
        }

        private void DeleteRule(string processName)
        {
            
            string commandIN = $"netsh advfirewall firewall delete rule name=\"Allow {processName} IN\"";
            string commandOUT = $"netsh advfirewall firewall delete rule name=\"Allow {processName} OUT\"";

            ExecuteCommand(commandIN);
            ExecuteCommand(commandOUT);
        }

        private void ExecuteCommand(string command)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/C {command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            foreach (var processName in executables)
            {
                AddRule(processName);
            }

            
            MessageBox.Show("Todas as regras foram criadas.", "Regras Criadas");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            
            foreach (var processName in executables)
            {
                DeleteRule(processName);
            }

            
            MessageBox.Show("Todas as regras foram deletadas.", "Regras Deletadas");
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            StringBuilder resultBuilder = new StringBuilder();

            
            foreach (var processName in executables)
            {
                string command = $"netsh advfirewall firewall show rule name=all | findstr /I \"{processName}\"";
                string result = ExecuteCommandAndReturnOutput(command);

                
                if (!string.IsNullOrWhiteSpace(result.Trim()))
                {
                    resultBuilder.AppendLine($"Regras para {processName}:");
                    resultBuilder.AppendLine(result);
                }
            }

            
            if (resultBuilder.Length == 0)
            {
                MessageBox.Show("Nenhuma regra encontrada.", "Regras Listadas");
            }
            else
            {
                MessageBox.Show(resultBuilder.ToString(), "Regras Listadas");
            }
        }

        private string ExecuteCommandAndReturnOutput(string command)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/C {command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return result;
            }
            catch (Exception ex)
            {
                return $"Erro: {ex.Message}";
            }
        }
    }
}