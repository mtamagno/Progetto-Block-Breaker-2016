using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Start : Form
    {
        #region Fields

        public Button start = new Button();
        private Bitmap backgroundimage;
        public Button Help = new Button();
        public Instruction instruction;
        private Bitmap start_button_image;

        #endregion Fields

        #region Constructors

        public Start()
        {
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        public void cleaner()
        {
            this.BackgroundImage.Dispose();
            this.backgroundimage.Dispose();
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        public void on_resize(int l, int h)
        {
            this.cleaner();
            this.BackgroundImage = new Bitmap(Properties.Resources.BackGround_Image, this.ClientSize);
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.start.Size = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            this.start.Top = ClientRectangle.Height / 2 - start.Height / 2;
            this.start.Left = ClientRectangle.Width / 2 - start.Width / 2;
            this.start_button_image = new Bitmap(Properties.Resources.BlueRoundedButton, this.start.Size);
        }

        public void starter()
        {
            //Direttive che vanno eseguite in ogni caso
            this.start.Size = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            this.start_button_image = new Bitmap(Properties.Resources.BlueRoundedButton, this.start.Size);
            this.start.BackgroundImage = start_button_image;
            this.start.BackgroundImageLayout = ImageLayout.Stretch;
            this.start.BackColor = Color.Black;
            this.start.Top = ClientRectangle.Height / 2 - start.Height / 2;
            this.start.Left = ClientRectangle.Width / 2 - start.Width / 2;
            this.Controls.Add(start);
            this.backgroundimage = new Bitmap(Properties.Resources.BackGround_Image, this.Size);
            this.Help.Size = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            this.Help.BackgroundImage = start_button_image;
            this.Help.BackgroundImageLayout = ImageLayout.Stretch;
            this.Help.BackColor = Color.Black;
            this.Help.Top = ClientRectangle.Height / 2 + Help.Height / 2;
            this.Help.Left = ClientRectangle.Width / 2 - Help.Width / 2;
            this.Help.Text = "Help";
            this.Controls.Add(Help);
            this.instruction = new Instruction(0, 0, 1000, 500);
            this.instruction.Visible = false;
            this.Controls.Add(instruction);
            this.BackgroundImage = backgroundimage;
            this.Help.Click += new EventHandler(this.Commands);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void writer(string testo)
        {
            this.start.Text = testo;
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

        private void Commands(object sender, EventArgs e)
        {
            this.start.Visible = false;
            this.Help.Visible = false;
            this.instruction.Visible = true;
        }
        #endregion Methods
    }
}