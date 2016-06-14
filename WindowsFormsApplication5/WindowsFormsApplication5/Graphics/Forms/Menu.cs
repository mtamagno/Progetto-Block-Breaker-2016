using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    public partial class Menu : Form
    {
        #region Fields

        public MenuButton Help;
        public MenuButton Highscores;
        public Instructions Instructions;
        public Panel MenuPanel = new Panel();
        public Size s;
        public MenuButton start;
        private Bitmap backgroundimage;
        private PictureBox Logo;
        private HighScoresPanel highscorepanel;
        private string Testo;

        #endregion Fields

        #region Constructors

        public Menu()
        {
            this.Instructions = new Instructions(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Funzione che permette di pulire dalla memoria le immagini caricate nel menù e che segnala se quando ciò avviene erano aperte o meno le istruzioni,
        /// questo è necessario per capire poi se riaprirle dopo un resize
        /// </summary>
        /// <returns></returns>
        public bool cleaner()
        {
            //Crea la variabile che segnala se erano aperte le istruzioni
            var makeInstructionsVisible = false;

            //Libera la memoria dalle immagini
            this.BackgroundImage.Dispose();
            this.backgroundimage.Dispose();
            if (this.Instructions.Visible)
            {
                this.Instructions.Visible = false;
                makeInstructionsVisible = true;
            }
            this.Instructions.Dispose();
            GC.Collect();
            GC.WaitForFullGCComplete();
            return makeInstructionsVisible;
        }

        /// <summary>
        /// Funzione che si occupa del resize di questo form e dei sui componenti
        /// </summary>
        /// <param name="l">new Width</param>
        /// <param name="h">new height</param>
        public void on_resize(int l, int h)
        {
            if (this.ClientSize.Width > 0 && this.ClientSize.Height > 0)
            {
                // Libera la memoria e capisce se si era sulle istruzioni o meno
                var makeInstructionsVisible = this.cleaner();

                // Ricrea il BackGround
                this.BackgroundImage = new Bitmap(Properties.Resources.BackGround_Image, this.ClientSize);
                this.BackgroundImageLayout = ImageLayout.Stretch;

                //Ricrea il Panel
                Testo = this.start.Text;
                this.CreatePanel();
                this.writer(Testo);
                //Ricrea il logo
                this.CreateLogo(this.ClientSize.Width, this.ClientSize.Height);

                //Ricrea le istruzioni
                this.Instructions = new Instructions(0, 0, this.ClientSize.Width, this.ClientSize.Height);
                this.Controls.Add(Instructions);

                if (makeInstructionsVisible == true)
                {
                    this.MenuPanel.Visible = false;
                    this.Logo.Visible = false;
                    this.Instructions.Visible = true;
                }
                this.Help.KeyPress += Help_KeyPress;
            }
        }

        /// <summary>
        /// Direttive che vanno eseguite in ogni caso
        /// </summary>
        public void starter()
        {
            s = new Size(this.ClientSize.Width / 5, this.ClientSize.Height / 10);

            // Background
            this.backgroundimage = new Bitmap(Properties.Resources.BackGround_Image, this.Size);
            this.BackgroundImage = backgroundimage;

            // Crea e riempie il panel centrale
            this.CreatePanel();

            // Instructions
            this.Instructions = new Instructions(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            this.highscorepanel = new HighScoresPanel(0, 0, this.ClientSize.Width, this.ClientSize.Height);

            //Logo
            this.CreateLogo(this.ClientSize.Width, this.ClientSize.Height);

            // Aspetta il Garbage collector
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //Effettua un on_resize che annulla un particolare bug nelle librerie di Drawings.dll
            //Che si può indurre eliminando questo primo on_resize e cliccando su help, poi effettuando il (primo) resize dalla schermata delle istruzioni
            this.on_resize(this.ClientSize.Width, this.ClientSize.Height);

            // Aggiunge i tasti al panel, poi il panel e le istruzioni ai controlli del form
            this.ControlsAdder();
        }

        /// <summary>
        /// Permette di scrivere il testo scelto dentro al tasto start
        /// </summary>
        /// <param name="testo"></param>
        public void writer(string testo)
        {
            this.start.Text = testo;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Carica il menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerLoad(object sender, EventArgs e)
        {
            this.starter();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        ///  Aggiunge i tasti al panel, poi il panel e le istruzioni ai controlli del form
        /// </summary>
        private void ControlsAdder()
        {
            // Aggiunge i bottoni al panel e poi il panel ai controlli
            this.Controls.Add(MenuPanel);
            this.MenuPanel.Controls.Add(start);
            this.MenuPanel.Controls.Add(Help);
            this.MenuPanel.Controls.Add(Highscores);
            this.Controls.Add(Instructions);
            this.Controls.Add(highscorepanel);
        }
        
        /// <summary>
        /// Crea il logo con le giuste dimensioni
        /// </summary>
        /// <param name="Left"></param>
        /// <param name="Top"></param>
        /// <param name="Width"></param>
        /// <param name="height"></param>
        private void CreateLogo(int Width, int height)
        {
            if (this.Logo != null)
            {
                this.Logo.Dispose();
            }
            this.Logo = new PictureBox();
            this.Logo.Width = this.MenuPanel.Width * 4;
            this.Logo.Height = this.MenuPanel.Top;
            this.Logo.Image = new Bitmap(Properties.Resources.logo1, this.Logo.Size);
            this.Logo.BackColor = Color.Transparent;
            this.Logo.Top = 0;
            this.Logo.Left = Width / 2 - this.Logo.Width / 2;
            this.Controls.Add(Logo);
        }

        /// <summary>
        /// Dimensiona il panel a seconda della grandezza della grandezza del 
        /// client e lo riempie con i pulsanti start e help
        /// </summary>
        private void CreatePanel()
        {
            s = new Size(this.ClientSize.Width / 6, this.ClientSize.Height / 10);
            // Imposta le dimensioni e la posizione del pannello panel che conterrà start e help
            this.MenuPanel.Size = new Size(this.ClientRectangle.Width / 5, this.ClientRectangle.Height / 2);
            this.MenuPanel.Top = ClientRectangle.Height / 2 - this.MenuPanel.Size.Height / 15 * 2;
            this.MenuPanel.Left = ClientRectangle.Width / 2 - this.MenuPanel.Size.Width / 2;
            this.MenuPanel.BackColor = Color.Transparent;
            this.MenuPanel.BorderStyle = BorderStyle.Fixed3D;

            // Imposta le dimensioni e la posizione di start
            if (this.start != null)
                this.start.Dispose();
            this.start = new MenuButton(s);
            this.start.Top = this.MenuPanel.Height / 4 - start.Height / 5 * 2;
            this.start.Left = this.MenuPanel.Width / 2 - start.Width / 2;
            
            // Imposta le dimensioni e la posizione di help
            if (this.Help != null)
                this.Help.Dispose();
            this.Help = new MenuButton(s);
            this.Help.Top = this.MenuPanel.Height / 3 + Help.Height / 5 * 2;
            this.Help.Left = this.MenuPanel.Width / 2 - Help.Width / 2;
            this.Help.Text = "Help";

            // Imposta le dimensioni e la posizione di Highscore
            if (this.Highscores != null)
                this.Highscores.Dispose();
            this.Highscores = new MenuButton(s);
            this.Highscores.Top = this.MenuPanel.Height / 4 + Highscores.Height * 2;
            this.Highscores.Left = this.MenuPanel.Width / 2 - Highscores.Width / 2;
            this.Highscores.Text = "Highscores";

            // Eventhandler
            this.Help.Click += new EventHandler(this.Show_Instructions);
            this.Highscores.Click += new EventHandler(this.Show_highscore);
            this.MenuPanel.Controls.Add(start);
            this.MenuPanel.Controls.Add(Help);
            this.MenuPanel.Controls.Add(Highscores);
        }

        /// <summary>
        /// Evento che permette di nascondere le istruzioni nascondendo il resto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show_Instructions(object sender, EventArgs e)
        {
            this.MenuPanel.Visible = false;
            this.Logo.Visible = false;
            this.Instructions.Visible = true;
            this.Focus();
            this.KeyPress += new KeyPressEventHandler(this.Help_KeyPress);
       }

        /// <summary>
        /// Gestore eventi per la pressione di tasti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Help_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.MenuPanel.Visible = true;
                this.start.Visible = true;
                this.Help.Visible = true;
                this.Instructions.Visible = false;
                this.highscorepanel.Visible = false;
                this.Logo.Visible = true;
            }
        }
        
        /// <summary>
        /// Funzione che permette di mostrare Gli highScores migliori
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show_highscore(object sender, EventArgs e)
        {
            this.MenuPanel.Visible = false;
            this.Logo.Visible = false;
            this.highscorepanel.Visible = true;
            this.Focus();
            this.KeyPress += new KeyPressEventHandler(this.Help_KeyPress);
        }

        #endregion Methods
    }
}