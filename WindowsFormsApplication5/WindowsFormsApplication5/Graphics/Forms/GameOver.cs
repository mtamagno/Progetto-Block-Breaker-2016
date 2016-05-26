using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class GameOver : Form
    {

        public Button Continue = new Button();
        private Bitmap backgroundimage;
        private Bitmap start_button_image;

        public GameOver()
        {
            InitializeComponent();
        }

        private void GameOver_Load(object sender, EventArgs e)
        {
            starter();
        }

        public void starter()
        {
            this.Continue.Size = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            this.start_button_image = new Bitmap(Properties.Resources.BlueRoundedButton, this.Continue.Size);
            this.Continue.BackgroundImage = start_button_image;
            this.Continue.BackgroundImageLayout = ImageLayout.Stretch;
            this.Continue.Text = "Continue";
            this.Continue.BackColor = Color.Black;
            this.Continue.Top = ClientRectangle.Height / 8 * 6 - Continue.Height / 2;
            this.Continue.Left = ClientRectangle.Width / 2 - Continue.Width / 2;
            this.Controls.Add(Continue);
            this.backgroundimage = new Bitmap(Properties.Resources.GameOver, this.Size);
            IntPtr handle = backgroundimage.GetHbitmap();
            this.BackgroundImage = backgroundimage;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void on_resize(int l, int h)
        {
            this.cleaner();
            this.BackgroundImage = new Bitmap(Properties.Resources.GameOver, this.ClientSize);
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Continue.Size = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            this.Continue.Top = ClientRectangle.Height / 8 * 6 - Continue.Height / 2;
            this.Continue.Left = ClientRectangle.Width / 2 - Continue.Width / 2;
            this.start_button_image = new Bitmap(Properties.Resources.BlueRoundedButton, this.Continue.Size);
        }

        private void cleaner()
        {
            this.BackgroundImage.Dispose();
            this.backgroundimage.Dispose();
            GC.Collect();
            GC.WaitForFullGCComplete();           
        }
    }
}
