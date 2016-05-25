using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Start : Form
    {
        #region Public Fields
        public Button start = new Button();
        public Bitmap backgroundimage;
        #endregion Public Fields

        #region Public Constructors

        public Start()
        {
            this.InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        public void starter()
        {
            //Direttive che vanno eseguite in ogni caso
            this.start.Size = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            Bitmap start_button_image = new Bitmap(Properties.Resources.BlueRoundedButton, this.start.Size);
            this.start.BackgroundImage = start_button_image;
            this.start.BackgroundImageLayout = ImageLayout.Stretch;
            this.start.BackColor = Color.Black;
            this.start.Top = ClientRectangle.Height / 2 - start.Height / 2;
            this.start.Left = ClientRectangle.Width / 2 - start.Width / 2;
            this.Controls.Add(start);
            backgroundimage = new Bitmap(Properties.Resources.BackGround_Image, this.Size);
            IntPtr handle = backgroundimage.GetHbitmap();
            this.BackgroundImage = backgroundimage;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.starter();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void Start_MouseEnter(object sender, EventArgs e)
        {
            this.start.FlatStyle = FlatStyle.Standard;
        }

        private void Start_MouseLeave(object sender, EventArgs e)
        {
            this.start.FlatStyle = FlatStyle.Flat;
        }

        #endregion Private Methods

        public void on_resize(int l, int h)
        {
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = new Bitmap(Properties.Resources.BackGround_Image, this.ClientSize);
            this.start.Size = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            this.start.Top = ClientRectangle.Height / 2 - start.Height / 2;
            this.start.Left = ClientRectangle.Width / 2 - start.Width / 2;
            Bitmap start_button_image = new Bitmap(Properties.Resources.BlueRoundedButton, this.start.Size);
        }

        public void writer(string testo)
        {
            this.start.Text = testo;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}