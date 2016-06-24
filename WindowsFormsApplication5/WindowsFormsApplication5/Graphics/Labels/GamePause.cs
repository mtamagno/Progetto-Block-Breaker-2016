using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    internal sealed class GamePause : Panel
    {
        #region Private Fields

        private Label _esc;
        private MyFonts _fontParagraph;
        private MyFonts _fontTitle;
        private Label _paragraph;
        private Label _title;

        #endregion Private Fields

        #region Public Constructors
        /// <summary>
        /// Costruttore, prende in ingresso grandezza e posizione e va a costruire il panel riferito allo stato gamePause.
        /// </summary>
        /// <returns></returns>
        /// 
        public GamePause(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            BackColor = Color.Black;
            SetLabels();
            SetFont();
            SetText();
        }

        #endregion Public Constructors

        #region Public Methods
        /// <summary>
        /// Funzione che permettere di settare il font
        /// </summary>
        /// <returns></returns>
        private void SetFont()
        {
            _fontParagraph = new MyFonts(MyFonts.FontType.Paragraph);
            _fontTitle = new MyFonts(MyFonts.FontType.Title);
            _title.Font = new Font(_fontTitle.Type.Families[0], 30, FontStyle.Regular);
            _paragraph.Font = new Font(_fontParagraph.Type.Families[0], 14, FontStyle.Regular);
            _esc.Font = new Font(_fontParagraph.Type.Families[0], 14, FontStyle.Regular);
        }


        /// <summary>
        /// Funzione che crea i label relativi al panel
        /// </summary>
        /// <returns></returns>
        /// 
        private void SetLabels()
        {
            _esc = new Label();
            _title = new Label();
            _paragraph = new Label();
        }

        /// <summary>
        /// Funzione che scrive all interno dei label e ne setta le varie caratteristiche
        /// </summary>
        /// <returns></returns>
        public void SetText()
        {
            _title.UseCompatibleTextRendering = true;
            _title.Top = 40;
            _title.Text = "Game Pause";
            _title.Height = 60;
            _title.Width = Width;
            _title.TextAlign = ContentAlignment.MiddleCenter;
            _title.ForeColor = Color.White;
            _paragraph.Height = 300;
            _paragraph.Top = 100;
            _paragraph.Text = "Press:" +
                              "\n.\n.\n." +
                              "\nSpace To resume the game" +
                              "\n.\n." +
                              "\nEsc To GameOver";
            _paragraph.UseCompatibleTextRendering = true;
            _paragraph.Width = Width;
            _paragraph.TextAlign = ContentAlignment.MiddleCenter;
            _paragraph.ForeColor = Color.White;
            _esc.UseCompatibleTextRendering = true;
            _esc.Height = 40;
            _esc.Top = Height - _esc.Height * 2;
            _esc.Width = 200;
            _esc.TextAlign = ContentAlignment.MiddleCenter;
            _esc.Left = 10;
            _esc.Text = "Space -> Back To Game";
            _esc.ForeColor = Color.White;
            Controls.Add(_esc);
            Controls.Add(_title);
            Controls.Add(_paragraph);
            /*controller.BackgroundImage = Properties.Resources.Instructions;
            controller.BackgroundImageLayout = ImageLayout.Stretch;
            controller.Visible = false;
            return controller;*/
            Visible = false;
        }

        #endregion Public Methods
    }
}
