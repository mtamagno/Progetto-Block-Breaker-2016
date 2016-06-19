using System;
using System.Drawing;
using System.Windows.Forms;
using BlockBreaker.Properties;

namespace BlockBreaker
{
    public partial class Menu : Form
    {
        #region Constructors

        public Menu()
        {
            Instructions = new Instructions(0, 0, ClientSize.Width, ClientSize.Height);
            InitializeComponent();
            _showhighscore = false;
        }

        #endregion Constructors

        #region Fields

        public MenuButton Help;
        public MenuButton Highscores;
        public Instructions Instructions;
        public Panel MenuPanel = new Panel();
        public Size S;
        public MenuButton Start;
        private Bitmap _backgroundimage;
        private PictureBox _logo;
        private HighScoresPanel _highScoresPanel;
        private string _testo;
        private bool _showhighscore;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Funzione che permette di pulire dalla memoria le immagini caricate nel menù e che segnala se quando ciò avviene
        ///     erano aperte o meno le istruzioni,
        ///     questo è necessario per capire poi se riaprirle dopo un resize
        /// </summary>
        /// <returns></returns>
        public bool Cleaner()
        {
            //Crea la variabile che segnala se erano aperte le istruzioni
            var makeInstructionsVisible = false;

            //Libera la memoria dalle immagini
            BackgroundImage.Dispose();
            _backgroundimage.Dispose();
            if (Instructions.Visible)
            {
                Instructions.Visible = false;
                makeInstructionsVisible = true;
            }
            Instructions.Dispose();
            GC.Collect();
            GC.WaitForFullGCComplete();
            return makeInstructionsVisible;
        }

        /// <summary>
        ///     Funzione che si occupa del resize di questo form e dei sui componenti
        /// </summary>
        /// <param name="l">new Width</param>
        /// <param name="h">new height</param>
        public void on_resize(int l, int h)
        {
            if (ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                // Libera la memoria e capisce se si era sulle istruzioni o meno
                var makeInstructionsVisible = Cleaner();

                // Ricrea il BackGround
                BackgroundImage = new Bitmap(Resources.BackGround_Image, ClientSize);
                BackgroundImageLayout = ImageLayout.Stretch;

                //Ricrea il Panel
                _testo = Start.Text;
                CreatePanel();
                Writer(_testo);
                //Ricrea il logo
                CreateLogo(ClientSize.Width, ClientSize.Height);

                //Ricrea le istruzioni
                Instructions = new Instructions(0, 0, ClientSize.Width, ClientSize.Height);
                _highScoresPanel.Dispose();
                _highScoresPanel = new HighScoresPanel(0, 0, ClientSize.Width, ClientSize.Height);
                Controls.Add(Instructions);
                Controls.Add(_highScoresPanel);
                if (_showhighscore)
                {
                    _highScoresPanel.Visible = true;
                    MenuPanel.Visible = false;
                    _logo.Visible = false;
                }
                if (makeInstructionsVisible)
                {
                    MenuPanel.Visible = false;
                    _logo.Visible = false;
                    Instructions.Visible = true;
                }
                Help.KeyPress += Help_KeyPress;
            }
        }

        /// <summary>
        ///     Direttive che vanno eseguite in ogni caso
        /// </summary>
        private void Starter()
        {
            S = new Size(ClientSize.Width/5, ClientSize.Height/10);

            // Background
            _backgroundimage = new Bitmap(Resources.BackGround_Image, Size);
            BackgroundImage = _backgroundimage;

            // Crea e riempie il panel centrale
            CreatePanel();

            // Instructions
            Instructions = new Instructions(0, 0, ClientSize.Width, ClientSize.Height);
            _highScoresPanel = new HighScoresPanel(0, 0, ClientSize.Width, ClientSize.Height);

            //_logo
            CreateLogo(ClientSize.Width, ClientSize.Height);

            // Aspetta il Garbage collector
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //Effettua un on_resize che annulla un particolare bug nelle librerie di Drawings.dll
            //Che si può indurre eliminando questo primo on_resize e cliccando su help, poi effettuando il (primo) resize dalla schermata delle istruzioni
            on_resize(ClientSize.Width, ClientSize.Height);

            // Aggiunge i tasti al panel, poi il panel e le istruzioni ai controlli del form
            ControlsAdder();
        }

        /// <summary>
        ///     Permette di scrivere il testo scelto dentro al tasto Start
        /// </summary>
        /// <param name="testo"></param>
        public void Writer(string testo)
        {
            Start.Text = testo;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        ///     Carica il menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerLoad(object sender, EventArgs e)
        {
            Starter();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        ///     Aggiunge i tasti al panel, poi il panel e le istruzioni ai controlli del form
        /// </summary>
        private void ControlsAdder()
        {
            // Aggiunge i bottoni al panel e poi il panel ai controlli
            Controls.Add(MenuPanel);
            MenuPanel.Controls.Add(Start);
            MenuPanel.Controls.Add(Help);
            MenuPanel.Controls.Add(Highscores);
            Controls.Add(Instructions);
            Controls.Add(_highScoresPanel);
        }

        /// <summary>
        ///     Crea il logo con le giuste dimensioni
        /// </summary>
        /// <param name="Left"></param>
        /// <param name="Top"></param>
        /// <param name="Width"></param>
        /// <param name="height"></param>
        private void CreateLogo(int Width, int height)
        {
            if (_logo != null)
            {
                _logo.Dispose();
            }
            _logo = new PictureBox();
            _logo.Width = MenuPanel.Width*4;
            _logo.Height = MenuPanel.Top;
            _logo.Image = new Bitmap(Resources.logo1, _logo.Size);
            _logo.BackColor = Color.Transparent;
            _logo.Top = 0;
            _logo.Left = Width/2 - _logo.Width/2;
            Controls.Add(_logo);
        }

        /// <summary>
        ///     Dimensiona il panel a seconda della grandezza della grandezza del
        ///     client e lo riempie con i pulsanti Start e help
        /// </summary>
        private void CreatePanel()
        {
            S = new Size(ClientSize.Width/6, ClientSize.Height/10);
            // Imposta le dimensioni e la posizione del pannello panel che conterrà Start e help
            MenuPanel.Size = new Size(ClientRectangle.Width/5, ClientRectangle.Height/2);
            MenuPanel.Top = ClientRectangle.Height/2 - MenuPanel.Size.Height/15*2;
            MenuPanel.Left = ClientRectangle.Width/2 - MenuPanel.Size.Width/2;
            MenuPanel.BackColor = Color.FromArgb(150, Color.Black);
            MenuPanel.BorderStyle = BorderStyle.Fixed3D;

            // Imposta le dimensioni e la posizione di Start
            if (Start != null)
                Start.Dispose();
            Start = new MenuButton(S);
            Start.Top = MenuPanel.Height/4 - Start.Height/5*2;
            Start.Left = MenuPanel.Width/2 - Start.Width/2;

            // Imposta le dimensioni e la posizione di help
            Help?.Dispose();
            Help = new MenuButton(S);
            Help.Top = MenuPanel.Height/3 + Help.Height/5*2;
            Help.Left = MenuPanel.Width/2 - Help.Width/2;
            Help.Text = "Help";

            // Imposta le dimensioni e la posizione di Highscore
            Highscores?.Dispose();
            Highscores = new MenuButton(S);
            Highscores.Top = MenuPanel.Height/4 + Highscores.Height*2;
            Highscores.Left = MenuPanel.Width/2 - Highscores.Width/2;
            Highscores.Text = "Highscores";

            // Eventhandler
            Help.Click += Show_Instructions;
            Highscores.Click += Show_highscore;
            MenuPanel.Controls.Add(Start);
            MenuPanel.Controls.Add(Help);
            MenuPanel.Controls.Add(Highscores);
        }

        /// <summary>
        ///     Evento che permette di nascondere le istruzioni nascondendo il resto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show_Instructions(object sender, EventArgs e)
        {
            MenuPanel.Visible = false;
            _logo.Visible = false;
            Instructions.Visible = true;
            Focus();
            KeyPress += Help_KeyPress;
        }

        /// <summary>
        ///     Gestore eventi per la pressione di tasti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Help_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Escape)
            {
                MenuPanel.Visible = true;
                Start.Visible = true;
                Help.Visible = true;
                Instructions.Visible = false;
                _highScoresPanel.Visible = false;
                _logo.Visible = true;
                _showhighscore = false;
            }
        }

        /// <summary>
        ///     Funzione che permette di mostrare Gli highScores migliori
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show_highscore(object sender, EventArgs e)
        {
            MenuPanel.Visible = false;
            _logo.Visible = false;
            _highScoresPanel.Visible = true;
            Focus();
            KeyPress += Help_KeyPress;
            _showhighscore = true;
        }

        #endregion Methods
    }
}