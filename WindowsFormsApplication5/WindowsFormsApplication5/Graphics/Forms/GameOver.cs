using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class GameOver : Form
    {
        #region Fields

        public Button Continue = new Button();
        public HighScore highScore = new HighScore();
        private Bitmap backgroundimage;
        private Bitmap start_button_image;
        private TextBox textBox = new TextBox();

        #endregion Fields

        #region Constructors

        // Costruttore che inizializza lo starter
        public GameOver()
        {
            InitializeComponent();
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
            this.BackgroundImage = new Bitmap(Properties.Resources.GameOver, this.ClientSize);
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Continue.Size = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            this.Continue.Top = ClientRectangle.Height / 8 * 6 - Continue.Height / 2;
            this.Continue.Left = ClientRectangle.Width / 2 - Continue.Width / 2;
            this.textBox.Top = this.Continue.Top + this.Continue.Height;
            this.textBox.Size = this.Continue.Size;
            this.textBox.Left = this.Continue.Left;
            this.start_button_image = new Bitmap(Properties.Resources.BlueRoundedButton, this.Continue.Size);
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            highScore.Name = textBox.Text;
        }

        private void GameOver_Load(object sender, EventArgs e)
        {
            starter();
        }

        private void starter()
        {
            // Imposta l'immagine, il size, il background e il testo del pulsante Continue
            this.start_button_image = new Bitmap(Properties.Resources.BlueRoundedButton, this.Continue.Size);
            this.Continue.Size = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            this.Continue.BackgroundImage = start_button_image;
            this.Continue.BackgroundImageLayout = ImageLayout.Stretch;
            this.Continue.Text = "Continue";
            this.Continue.BackColor = Color.Black;

            // Imposta la sua posizione e lo aggiungo ai controlli
            this.Continue.Top = ClientRectangle.Height / 8 * 6 - Continue.Height / 2;
            this.Continue.Left = ClientRectangle.Width / 2 - Continue.Width / 2;
            this.Controls.Add(Continue);

            // Imposta posizione, placeholder e size della texBox
            this.textBox.Size = Continue.Size;
            this.textBox.Top = Continue.Top + Continue.Height;
            this.textBox.Left = Continue.Left;
            this.textBox.Text = "Insert Name...";
            this.textBox.Click += TextBox_Click;
            this.Continue.Click += Continue_Click;
            this.Controls.Add(textBox);

            // Imposta l'immagine di background
            this.backgroundimage = new Bitmap(Properties.Resources.GameOver, this.Size);

            // IntPtr handle = backgroundimage.GetHbitmap();
            this.BackgroundImage = backgroundimage;

            // Aspetto il Garbage Collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            textBox.Clear();
        }

        #endregion Methods
    }
}