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
        private PictureBox logo;
        public Button Help = new Button();
        public Panel MenuPanel = new Panel();
        public Instruction instruction;
        public Button Highscores = new Button();
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

        /// <summary>
        /// Direttive che vanno eseguite in ogni caso
        /// </summary>
        public void starter()
        {
            // Background
            this.backgroundimage = new Bitmap(Properties.Resources.BackGround_Image, this.Size);
            this.BackgroundImage = backgroundimage;

            // Crea e riempie il panel centrale
            this.riempiPanel();

            // Instructions
            this.instruction = new Instruction(0, 0, 1000, 500);
            this.instruction.Visible = false;

            //Logo
            logo = new PictureBox();
            logo.Width = 800;
            logo.Height = 300;
            logo.BackColor = Color.Transparent;
            logo.Image = Properties.Resources.logo1;
            logo.Top = 0;
            logo.Left = this.ClientRectangle.Width / 2 - this.logo.Width / 2;
            this.Controls.Add(logo);
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
            this.start.Size = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            this.start_button_image = new Bitmap(Properties.Resources.BlueRoundedButton, this.start.Size);
            this.start.BackgroundImage = start_button_image;
            this.start.BackgroundImageLayout = ImageLayout.Stretch;
            this.start.BackColor = Color.Black;
            this.start.Top = this.MenuPanel.Height / 4 - start.Height/5*2;
            this.start.Left = this.MenuPanel.Width / 2 - start.Width / 2;

            // Imposta le dimensioni e la posizione di help
            this.Help.Size = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            this.Help.BackgroundImage = start_button_image;
            this.Help.BackgroundImageLayout = ImageLayout.Stretch;
            this.Help.BackColor = Color.Black;
            this.Help.Top = this.MenuPanel.Height / 3 + Help.Height/5*2;
            this.Help.Left = this.MenuPanel.Width / 2 - Help.Width / 2;
            this.Help.Text = "Help";

            // Imposta le dimensioni e la posizione di Highscore
            this.Highscores.Size = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            this.start_button_image = new Bitmap(Properties.Resources.BlueRoundedButton, this.start.Size);
            this.Highscores.BackgroundImage = start_button_image;
            this.Highscores.BackgroundImageLayout = ImageLayout.Stretch;
            this.Highscores.BackColor = Color.Black;
            this.Highscores.Top = this.MenuPanel.Height / 4  + Highscores.Height*2 ;
            this.Highscores.Left = this.MenuPanel.Width / 2 - Highscores.Width / 2;
            this.Highscores.Text = "Highscores";

            // Aggiunge i bottoni a panel
            this.MenuPanel.Controls.Add(start);
            this.MenuPanel.Controls.Add(Help);
            this.MenuPanel.Controls.Add(Highscores);

            // Aggiunge i panel e istruzioni ai controlli
            this.Controls.Add(instruction);
            this.Controls.Add(MenuPanel);

            // Eventhandler
            this.Help.Click += new EventHandler(this.Commands);
            this.Help.KeyPress += Help_KeyPress;
        }

        private void Help_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Escape)
            {
                this.start.Visible = true;
                this.Help.Visible = true;
                this.instruction.Visible = false;
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

        private void Commands(object sender, EventArgs e)
        {
            this.start.Visible = false;
            this.Help.Visible = false;
            this.instruction.Visible = true;
        }
        #endregion Methods
    }
}