using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form3 : Form
    {
        #region Public Fields

        public Button start = new Button();

        #endregion Public Fields

        #region Public Constructors

        public Form3()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Form3_Load(object sender, EventArgs e)
        {
            starter();
        }

        private void starter()
        {
            this.Controls.Add(start);
            start.ForeColor = Color.Black;
            start.BackColor = ProfessionalColors.ButtonCheckedGradientBegin;
            start.UseVisualStyleBackColor = true;
            start.Text = "Start";
            start.Top = ClientRectangle.Height / 2 - start.Height / 2;
            start.Left = ClientRectangle.Width / 2 - start.Width / 2;
        }

        #endregion Private Methods
    }
}