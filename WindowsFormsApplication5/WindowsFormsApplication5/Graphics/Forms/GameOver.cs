using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    public partial class GameOver : Form
    {
        #region Fields

        public MenuButton Continue;
        private Bitmap backgroundimage;
        private Bitmap start_button_image;
        public TextBox textBox;
        public Label Nickname;

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
            if (this.ClientSize.Height > 0 || this.ClientSize.Width > 0)
            {
            this.cleaner();
            this.BackgroundImage = new Bitmap(Properties.Resources.GameOver, this.ClientSize);
            this.BackgroundImageLayout = ImageLayout.Stretch;
            Continue.Dispose();

                // Imposta l'immagine, il size, il background e il testo del pulsante Continue
                Size s = new Size(this.ClientSize.Width / 10, this.ClientSize.Height / 10);
                this.Continue = new MenuButton(s);
                this.Continue.Text = "Continue";

                // Imposta la sua posizione e lo aggiungo ai controlli
                this.Continue.Top = ClientRectangle.Height / 11 * 10 - Continue.Height / 2;
                this.Continue.Left = ClientRectangle.Width / 2 - Continue.Width / 2;
                this.Controls.Add(Continue);

                // Imposta la label
                this.Nickname.Dispose();
                this.Nickname = new Label();
                MyFonts fonts = new MyFonts(MyFonts.FontType.paragraph);
                this.Nickname.UseCompatibleTextRendering = true;
                this.Nickname.Width = 80;
                this.Nickname.Top = Continue.Top - Continue.Height;
                this.Nickname.Font = new Font(fonts.type.Families[0], 12, FontStyle.Regular);
                this.Nickname.ForeColor = Color.White;
                this.Nickname.BackColor = Color.Black;
                this.Nickname.Text = "Nickname: ";
                this.Nickname.Left = ClientRectangle.Width / 2 - Continue.Width / 2 - this.Nickname.Width / 2;
                this.Controls.Add(Nickname);

                // Imposta posizione, placeholder e size della textBox
                this.textBox.Dispose();
                this.textBox = new TextBox();
                this.textBox.Size = Continue.Size;
                this.textBox.Top = Continue.Top - Continue.Height;
                this.textBox.Left = Continue.Left + this.Nickname.Width / 2;
                this.textBox.Text = "Insert Name...";
                this.textBox.Click += TextBox_Click;
                this.Controls.Add(textBox);

                // Imposta l'immagine di background
                if (this.Size.Height > 0)
                    this.backgroundimage = new Bitmap(Properties.Resources.GameOver, this.Size);

                // Aspetto il Garbage Collector
                GC.Collect();
                GC.WaitForPendingFinalizers();
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
            this.Continue.Top = ClientRectangle.Height / 11 * 10 - Continue.Height / 2;
            this.Continue.Left = ClientRectangle.Width / 2 - Continue.Width / 2;
            this.Controls.Add(Continue);

            // Imposta la label
            this.Nickname = new Label();
            MyFonts fonts = new MyFonts(MyFonts.FontType.paragraph);
            this.Nickname.UseCompatibleTextRendering = true;
            this.Nickname.Width = 80;
            this.Nickname.Top = Continue.Top - Continue.Height;
            this.Nickname.Font = new Font(fonts.type.Families[0], 12, FontStyle.Regular);
            this.Nickname.ForeColor = Color.White;
            this.Nickname.BackColor = Color.Black;
            this.Nickname.Text = "Nickname: ";
            this.Nickname.Left = ClientRectangle.Width / 2 - Continue.Width / 2 - this.Nickname.Width/2;
            this.Controls.Add(Nickname);

            // Imposta posizione, placeholder e size della textBox
            this.textBox = new TextBox();
            this.textBox.Size = Continue.Size;
            this.textBox.Top = Continue.Top - Continue.Height;
            this.textBox.Left = Continue.Left + this.Nickname.Width/2;
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