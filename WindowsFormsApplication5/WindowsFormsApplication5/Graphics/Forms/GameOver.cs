using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class GameOver : Form
    {
        #region Fields

        public MenuButton Continue;
        private Bitmap backgroundimage;
        private Bitmap start_button_image;
        public TextBox textBox = new TextBox();

        #endregion Fields

        #region Constructors

        public GameOver()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Funzione che permette di liberare la memoria dall'immagine di background
        /// </summary>
        public void cleaner()
        {
            this.BackgroundImage.Dispose();
            this.backgroundimage.Dispose();
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        /// <summary>
        /// Funzione che permette di ridimensionare ad ogni resize, l'immagine di backGround, la textBox ed il pulsante
        /// </summary>
        /// <param name="l"></param>
        /// <param name="h"></param>
        public void on_resize(int l, int h)
        {
            if (this.ClientSize.Height < 0 || this.ClientSize.Width < 0)
            {
            this.cleaner();
            this.BackgroundImage = new Bitmap(Properties.Resources.GameOver, this.ClientSize);
            this.BackgroundImageLayout = ImageLayout.Stretch;
            Continue.Dispose();
            Size s = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            Continue = new MenuButton(s);            
            this.Continue.Top = ClientRectangle.Height / 8 * 6 - Continue.Height / 2;
            this.Continue.Left = ClientRectangle.Width / 2 - Continue.Width / 2;
            this.Continue.Text = "Continue";
                this.textBox.Top = this.Continue.Top - this.Continue.Height;
            this.textBox.Size = this.Continue.Size;
            this.textBox.Left = this.Continue.Left;
            this.start_button_image = new Bitmap(Properties.Resources.BlueRoundedButton, this.Continue.Size);
            this.Controls.Add(Continue);
        }
        }

        private void GameOver_Load(object sender, EventArgs e)
        {
            starter();
        }

        private void starter()
        {
            // Imposta l'immagine, il size, il background e il testo del pulsante Continue
            Size s = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
            this.Continue = new MenuButton(s);
            this.Continue.Text = "Continue";

            // Imposta la sua posizione e lo aggiungo ai controlli
            this.Continue.Top = ClientRectangle.Height / 8 * 6 - Continue.Height / 2;
            this.Continue.Left = ClientRectangle.Width / 2 - Continue.Width / 2;
            this.Controls.Add(Continue);

            // Imposta posizione, placeholder e size della textBox
            this.textBox.Size = Continue.Size;
            this.textBox.Top = Continue.Top + Continue.Height;
            this.textBox.Left = Continue.Left;
            this.textBox.Text = "Insert Name...";
            this.textBox.Click += TextBox_Click;
            this.Controls.Add(textBox);

            // Imposta l'immagine di background
            if(this.Size.Height > 0 )
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