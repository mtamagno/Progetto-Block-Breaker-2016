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
        public MenuButton Start;
        private Bitmap _backgroundimage;
        private PictureBox _logo;
        private HighScoresPanel _highScoresPanel;
        private string _testo;
        private bool _showhighscore;

        #endregion Fields

        #region Constructors

        public Menu()
        {
            this.Instructions = new Instructions(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            this.InitializeComponent();
            _showhighscore = false;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Funzione che permette di pulire dalla memoria le immagini caricate nel menù e che segnala se quando ciò avviene erano aperte o meno le istruzioni,
        /// questo è necessario per capire poi se riaprirle dopo un resize
        /// </summary>
        /// <returns></returns>
        public bool Cleaner()
        {
            //Crea la variabile che segnala se erano aperte le istruzioni
            var makeInstructionsVisible = false;

            //Libera la memoria dalle immagini
            this.BackgroundImage.Dispose();
            this._backgroundimage.Dispose();
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
                var makeInstructionsVisible = this.Cleaner();

                // Ricrea il BackGround
                this.BackgroundImage = new Bitmap(Properties.Resources.BackGround_Image, this.ClientSize);
                this.BackgroundImageLayout = ImageLayout.Stretch;

                //Ricrea il Panel
                _testo = this.Start.Text;
                this.CreatePanel();
                this.Writer(_testo);
                //Ricrea il logo
                this.CreateLogo(this.ClientSize.Width, this.ClientSize.Height);

                //Ricrea le istruzioni
                this.Instructions = new Instructions(0, 0, this.ClientSize.Width, this.ClientSize.Height);
                this._highScoresPanel.Dispose();
                this._highScoresPanel = new HighScoresPanel(0, 0, this.ClientSize.Width, this.ClientSize.Height);
                this.Controls.Add(Instructions);
                this.Controls.Add(_highScoresPanel);
                if (_showhighscore == true)
                {
                    this._highScoresPanel.Visible = true;
                    this.MenuPanel.Visible = false;
                    this._logo.Visible = false;
                }
                if (makeInstructionsVisible == true)
                {
                    this.MenuPanel.Visible = false;
                    this._logo.Visible = false;
                    this.Instructions.Visible = true;
                }
                this.Help.KeyPress += Help_KeyPress;
            }
        }

        /// <summary>
        /// Direttive che vanno eseguite in ogni caso
        /// </summary>
        private void Starter()
        {
            s = new Size(this.ClientSize.Width / 5, this.ClientSize.Height / 10);

            // Background
            this._backgroundimage = new Bitmap(Properties.Resources.BackGround_Image, this.Size);
            this.BackgroundImage = _backgroundimage;

            // Crea e riempie il panel centrale
            this.CreatePanel();

            // Instructions
            this.Instructions = new Instructions(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            this._highScoresPanel = new HighScoresPanel(0, 0, this.ClientSize.Width, this.ClientSize.Height);

            //_logo
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
        /// Permette di scrivere il testo scelto dentro al tasto Start
        /// </summary>
        /// <param name="testo"></param>
        public void Writer(string testo)
        {
            this.Start.Text = testo;
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
            this.Starter();
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
            this.MenuPanel.Controls.Add(Start);
            this.MenuPanel.Controls.Add(Help);
            this.MenuPanel.Controls.Add(Highscores);
            this.Controls.Add(Instructions);
            this.Controls.Add(_highScoresPanel);
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
            if (this._logo != null)
            {
                this._logo.Dispose();
            }
            this._logo = new PictureBox();
            this._logo.Width = this.MenuPanel.Width * 4;
            this._logo.Height = this.MenuPanel.Top;
            this._logo.Image = new Bitmap(Properties.Resources.logo1, this._logo.Size);
            this._logo.BackColor = Color.Transparent;
            this._logo.Top = 0;
            this._logo.Left = Width / 2 - this._logo.Width / 2;
            this.Controls.Add(_logo);
        }

        /// <summary>
        /// Dimensiona il panel a seconda della grandezza della grandezza del 
        /// client e lo riempie con i pulsanti Start e help
        /// </summary>
        private void CreatePanel()
        {
            s = new Size(this.ClientSize.Width / 6, this.ClientSize.Height / 10);
            // Imposta le dimensioni e la posizione del pannello panel che conterrà Start e help
            this.MenuPanel.Size = new Size(this.ClientRectangle.Width / 5, this.ClientRectangle.Height / 2);
            this.MenuPanel.Top = ClientRectangle.Height / 2 - this.MenuPanel.Size.Height / 15 * 2;
            this.MenuPanel.Left = ClientRectangle.Width / 2 - this.MenuPanel.Size.Width / 2;
            this.MenuPanel.BackColor = Color.FromArgb(150, Color.Black);
            this.MenuPanel.BorderStyle = BorderStyle.Fixed3D;

            // Imposta le dimensioni e la posizione di Start
            if (this.Start != null)
                this.Start.Dispose();
            this.Start = new MenuButton(s);
            this.Start.Top = this.MenuPanel.Height / 4 - Start.Height / 5 * 2;
            this.Start.Left = this.MenuPanel.Width / 2 - Start.Width / 2;
            
            // Imposta le dimensioni e la posizione di help
            this.Help?.Dispose();
            this.Help = new MenuButton(s);
            this.Help.Top = this.MenuPanel.Height / 3 + Help.Height / 5 * 2;
            this.Help.Left = this.MenuPanel.Width / 2 - Help.Width / 2;
            this.Help.Text = "Help";

            // Imposta le dimensioni e la posizione di Highscore
            this.Highscores?.Dispose();
            this.Highscores = new MenuButton(s);
            this.Highscores.Top = this.MenuPanel.Height / 4 + Highscores.Height * 2;
            this.Highscores.Left = this.MenuPanel.Width / 2 - Highscores.Width / 2;
            this.Highscores.Text = "Highscores";

            // Eventhandler
            this.Help.Click += new EventHandler(this.Show_Instructions);
            this.Highscores.Click += new EventHandler(this.Show_highscore);
            this.MenuPanel.Controls.Add(Start);
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
            this._logo.Visible = false;
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
                this.Start.Visible = true;
                this.Help.Visible = true;
                this.Instructions.Visible = false;
                this._highScoresPanel.Visible = false;
                this._logo.Visible = true;
                _showhighscore = false;
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
            this._logo.Visible = false;
            this._highScoresPanel.Visible = true;
            this.Focus();
            this.KeyPress += new KeyPressEventHandler(this.Help_KeyPress);
            _showhighscore = true;
        }

        #endregion Methods
    }
}