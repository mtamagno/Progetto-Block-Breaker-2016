using BlockBreaker.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    public partial class GameOver : Form
    {
        #region Public Fields

        public MenuButton Continue;
        public Label Nickname;
        public TextBox TextBox;

        #endregion Public Fields

        #region Private Fields

        private Bitmap _backgroundimage;
        private MyFonts _fonts;

        #endregion Private Fields

        #region Public Constructors

        public GameOver()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Funzione che permette di liberare la memoria dall'immagine di background
        /// </summary>
        public void Cleaner()
        {
            BackgroundImage.Dispose();
            _backgroundimage.Dispose();
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        /// <summary>
        ///     Funzione che permette di ridimensionare ad ogni resize, l'immagine di backGround, la textBox ed il pulsante
        /// </summary>
        /// <param name="l"></param>
        /// <param name="h"></param>
        public void on_resize(int l, int h)
        {
            if (ClientSize.Height > 0 || ClientSize.Width > 0)
            {
                Cleaner();
                BackgroundImage = new Bitmap(Resources.GameOver, ClientSize);
                BackgroundImageLayout = ImageLayout.Stretch;

                // Imposta l'immagine, il size, il background e il testo del pulsante Continue
                var s = new Size(ClientSize.Width / 10, ClientSize.Height / 10);
                Continue.Text = "Continue";

                // Imposta la sua posizione e lo aggiungo ai controlli
                Continue.Top = ClientRectangle.Height / 11 * 10 - Continue.Height / 2;
                Continue.Left = ClientRectangle.Width / 2 - Continue.Width / 2;
                Controls.Add(Continue);

                // Imposta la label
                Nickname.Dispose();
                Nickname = new Label();
                var fonts = new MyFonts(MyFonts.FontType.Paragraph);
                Nickname.UseCompatibleTextRendering = true;
                Nickname.Width = 80;
                Nickname.Top = Continue.Top - Continue.Height;
                Nickname.Font = new Font(fonts.Type.Families[0], 12, FontStyle.Regular);
                Nickname.ForeColor = Color.White;
                Nickname.BackColor = Color.Black;
                Nickname.Text = "Nickname: ";
                Nickname.Left = ClientRectangle.Width / 2 - Continue.Width / 2 - Nickname.Width / 2;
                Controls.Add(Nickname);

                // Imposta posizione, placeholder e size della textBox
                TextBox.Dispose();
                TextBox = new TextBox();
                TextBox.Size = Continue.Size;
                TextBox.Top = Continue.Top - Continue.Height;
                TextBox.Left = Continue.Left + TextBox.Width / 2;
                TextBox.Text = "Insert Name...";
                TextBox.Click += TextBox_Click;
                Controls.Add(TextBox);

                // Imposta l'immagine di background
                if (Size.Height > 0)
                    _backgroundimage = new Bitmap(Resources.GameOver, Size);

                // Aspetto il Garbage Collector
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Focus();
        }

        #endregion Public Methods

        #region Private Methods

        private void GameOver_Load(object sender, EventArgs e)
        {
            Starter();
        }

        private void Starter()
        {
            // Imposta l'immagine, il size, il background e il testo del pulsante Continue
            var s = new Size(ClientSize.Width / 10, ClientSize.Height / 10);
            TextBox = new TextBox();
            Continue = new MenuButton(s);
            Continue.Text = "Continue";

            // Imposta la sua posizione e lo aggiungo ai controlli
            Continue.Top = ClientRectangle.Height / 11 * 10 - Continue.Height / 2;
            Continue.Left = ClientRectangle.Width / 2 - Continue.Width / 2;
            Controls.Add(Continue);

            // Imposta posizione, placeholder e size della textBox
            TextBox.Size = Continue.Size;
            TextBox.Top = Continue.Top - Continue.Height;
            TextBox.Left = Continue.Left + TextBox.Width / 2;
            TextBox.Text = "Insert Name...";
            Controls.Add(TextBox);
            TextBox.Click += TextBox_Click;

            // Imposta l'immagine di background
            if (Size.Height > 0)
                _backgroundimage = new Bitmap(Resources.GameOver, Size);

            // IntPtr handle = backgroundimage.GetHbitmap();
            BackgroundImage = _backgroundimage;

            // Imposta la label
            Nickname = new Label();
            Nickname.BackColor = Color.Black;
            _fonts = new MyFonts(MyFonts.FontType.Paragraph);
            Nickname.Width = 80;
            Nickname.Top = Continue.Top - Continue.Height;
            Nickname.Font = new Font(_fonts.Type.Families[0], 12, FontStyle.Regular);
            Nickname.ForeColor = Color.White;
            Nickname.Text = "Nickname: ";
            Nickname.Left = ClientRectangle.Width / 2 - Continue.Width / 2 - Nickname.Width / 2;
            Controls.Add(Nickname);

            // Aspetto il Garbage Collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            TextBox.Clear();
        }

        #endregion Private Methods
    }
}
