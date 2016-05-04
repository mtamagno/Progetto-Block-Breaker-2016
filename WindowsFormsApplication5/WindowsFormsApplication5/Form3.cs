using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form3 : Form
    {

        public  Button start = new Button();

        public Form3()
        {
            InitializeComponent();
            return;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.Controls.Add(start);
            
            start.ForeColor = Color.Black;
            start.BackColor = ProfessionalColors.ButtonCheckedGradientBegin;
            start.UseVisualStyleBackColor = true;
            start.Text = "Start";
            start.Top = Form2.ActiveForm.ClientRectangle.Width /2;
            start.Left = Form2.ActiveForm.ClientRectangle.Height/2;
        }

    }
}
