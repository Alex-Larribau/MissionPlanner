using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner
{
    public partial class FenetreTerminal : Form
    {
        public FenetreTerminal()
        {
            InitializeComponent();
        }

        public void AppendLine(string line)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => AppendLine(line)));
            }
            else
            {
                textBox1.AppendText(line + Environment.NewLine);
            }
        }

    }
}
