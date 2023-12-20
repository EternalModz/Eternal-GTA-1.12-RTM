using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PS3ManagerAPI
{
    public partial class AttachDialog : XtraForm
	{
        public AttachDialog()
		{
            Opacity = 0.82;
			InitializeComponent();
		}

        public AttachDialog(PS3ManagerAPI.PS3MAPI MyPS3MAPI)
            : this()
        {
            comboBox1.Properties.Items.Clear();
            foreach (uint pid in MyPS3MAPI.Process.GetPidProcesses())
            {
                if (pid != 0) comboBox1.Properties.Items.Add(MyPS3MAPI.Process.GetName(pid));
                else break;
            }
            comboBox1.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
