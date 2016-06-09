using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Start : Form
    {
        #region Fields

        public MenuButton start;
        private Bitmap backgroundimage;
        private PictureBox Logo;
        public MenuButton Help;
        public Panel MenuPanel = new Panel();
        public Instructions Instructions = new Instructions();
        public MenuButton Highscores;
        public Size s;
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
            this.Controls.Remove(start);
            this.start.Dispose();
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
            this.start= new MenuButton(s);
            this.start.Text = "Play";
            this.start.Top = this.MenuPanel.Height / 4 - start.Height / 5 * 2;
            this.start.Left = this.MenuPanel.Width / 2 - start.Width / 2;
            this.MenuPanel.Controls.Add(start);
        }

        /// <summary>
        /// Direttive che vanno eseguite in ogni caso
        /// </summary>
        public void starter()
        {
            s = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            // Background
            this.backgroundimage = new Bitmap(Properties.Resources.BackGround_Image, this.Size);
            this.BackgroundImage = backgroundimage;

            // Crea e riempie il panel centrale
            this.riempiPanel();

            // Instructions
            this.Instructions = this.Instructions.Create_Instructions(0, 0, this.ClientSize.Width, this.ClientSize.Height);

            //Logo
            Logo = new PictureBox();
            Logo.Width = 800;
            Logo.Height = 300;
            Logo.BackColor = Color.Transparent;
            Logo.Image = Properties.Resources.logo1;
            Logo.Top = 0;
            Logo.Left = this.ClientRectangle.Width / 2 - this.Logo.Width / 2;
            this.Controls.Add(Logo);
            
            // Aspetta il Garbage collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Dimensiona il panel a seconda della grandezza della grandezza del 
        /// client e lo riempie con i pulsanti start e help
        /// </summary>
        private void riempiPanel()
        {
            // Imposta le dimensioni e la posizione del pannello panel che conterrà start e help
            this.MenuPanel.Size = new Size(this.ClientRectangle.Width / 5, this.ClientRectangle.Height / 2);
            this.MenuPanel.Top =  ClientRectangle.Height / 2 - this.MenuPanel.Size.Height/15*2;
            this.MenuPanel.Left = ClientRectangle.Width / 2 - this.MenuPanel.Size.Width/2;
            this.MenuPanel.BackColor = Color.Transparent;
            this.MenuPanel.BorderStyle = BorderStyle.Fixed3D;

            // Imposta le dimensioni e la posizione di start
            this.start = new MenuButton(s);
            this.start.Top = this.MenuPanel.Height / 4 - start.Height/5*2;
            this.start.Left = this.MenuPanel.Width / 2 - start.Width / 2;

            // Imposta le dimensioni e la posizione di help
            this.Help = new MenuButton(s);
            this.Help.Top = this.MenuPanel.Height / 3 + Help.Height/5*2;
            this.Help.Left = this.MenuPanel.Width / 2 - Help.Width / 2;
            this.Help.Text = "Help";

            // Imposta le dimensioni e la posizione di Highscore
            this.Highscores = new MenuButton(s);
            this.Highscores.Top = this.MenuPanel.Height / 4  + Highscores.Height*2 ;
            this.Highscores.Left = this.MenuPanel.Width / 2 - Highscores.Width / 2;
            this.Highscores.Text = "Highscores";

            // Aggiunge i bottoni a panel
            this.MenuPanel.Controls.Add(start);
            this.MenuPanel.Controls.Add(Help);
            this.MenuPanel.Controls.Add(Highscores);

            // Aggiunge i panel e istruzioni ai controlli
            this.Controls.Add(Instructions);
            this.Controls.Add(MenuPanel);

            // Eventhandler
            this.Help.Click += new EventHandler(this.Show_Instructions);
            this.Help.KeyPress += Help_KeyPress;
        }

        private void Help_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Escape)
            {
                this.start.Visible = true;
                this.Help.Visible = true;
                this.Instructions.Visible = false;
            }
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

        private void Show_Instructions(object sender, EventArgs e)
        {
            this.MenuPanel.Visible = false;
            this.Logo.Visible = false;
            this.Instructions.Visible = true;
        }
        #endregion Methods
    }
}