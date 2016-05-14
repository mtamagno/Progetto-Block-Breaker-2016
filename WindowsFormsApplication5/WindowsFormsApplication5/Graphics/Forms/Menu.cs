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
            this.InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Form3_Load(object sender, EventArgs e)
        {
            this.starter();
        }

        private void starter()
        {
            this.start.FlatStyle = FlatStyle.Flat;
            this.start.MouseEnter += Start_MouseEnter;
            this.start.MouseLeave += Start_MouseLeave; 
            Bitmap start_button_image = new Bitmap(Properties.Resources.BlueRoundedButton, this.start.Size);
            this.start.BackgroundImage = start_button_image;
            this.start.BackgroundImageLayout = ImageLayout.Stretch;
            this.start.BackColor = Color.Black;
            this.start.Text = "Start";
            this.start.Top = ClientRectangle.Height / 2 - start.Height / 2;
            this.start.Left = ClientRectangle.Width / 2 - start.Width / 2;
            this.Controls.Add(start);
            this.BackgroundImage = new Bitmap(Properties.Resources.BackGround_Image, this.ClientSize);
        }

        private void Start_MouseLeave(object sender, EventArgs e)
        {
            this.start.FlatStyle = FlatStyle.Flat;
        }

        private void Start_MouseEnter(object sender, EventArgs e)
        {
            this.start.FlatStyle = FlatStyle.Standard;
        }

        #endregion Private Methods
    }
}