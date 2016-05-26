using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication5
{
    public partial class Container2 : Form
    {

        #region Public Fields

        public int altezza_client;
        public int altezza_client_iniziale;
        public int lunghezza_client;
        public int lunghezza_client_iniziale;

        #endregion Public Fields

        MethodInvoker Invoker;
        private Game game;
        private Start menu;
        private GameOver gameover;
        private Panel gamePanels;
        private HighScore highscore;
        public TextBox textBox = new TextBox();
        private bool lose = false;
        private bool start;
        Thread gameAlive;
        private Music music;

        public Container2()
        {
            InitializeComponent();
        }

        private void Container2_Load(object sender, EventArgs e)
        {
            start = false;
            initializeGamePanel();
            initializeMenu();
        }

        private void initializeGamePanel()
        {
            gamePanels = new Panel();
            gamePanels.Width = this.Width;
            gamePanels.Height = this.Height;
            this.Dock = DockStyle.Fill;
            gamePanels.Top = 0;
            gamePanels.Left = 0;
            gamePanels.Dock = DockStyle.Fill;
            gamePanels.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            gamePanels.Show();
            this.Invoke(new MethodInvoker(delegate
            {
                this.Controls.Add(gamePanels);
            }));
        }

        private void initializeGame()
        {
            game = new Game();
            initializeForm(game);
            initializeThread(gameAlive);
        }

        private void initializeMenu()
        {
            menu = new Start();
            initializeForm(menu);
            if(menu.start.Text == null)
            menu.start.Text = "Start";
            menu.start.Click += new EventHandler(this.StartGame);
        }

        private void initializeGameOver()
        {
            gameover = new GameOver();
            initializeForm(gameover);
            gameover.Continue.Text = "Continue";
            gameover.Continue.Click += new EventHandler(this.ContinueToMenu);
        }

        private void initializeForm(Form form)
        {
            form.TopLevel = false;
            form.AutoScaleMode = AutoScaleMode.Inherit;
            form.Width = gamePanels.Width;
            form.Height = gamePanels.Height;
            form.Left = 0;
            form.Top = 0;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            form.AutoScaleMode = AutoScaleMode.Inherit;
            this.Invoke(new MethodInvoker(delegate
            {
                gamePanels.Controls.Add(form);
                form.Show();
                form.Focus();
                form.BringToFront();
            }));            

        }

        private void initializeThread(Thread thread)
        {
            gameAlive = new Thread(gameoverCheck);
            gameAlive.Start();
        }

        public void gameoverCheck()
        {
            while (game.Visible)
            {
                if (game.logic.shouldStop == true)
                {
                    this.highscore = this.game.logic.highscore;
                    game.logic.shouldStop = false;
                    lose = true;
                    DisposeAll();
                    initializeGamePanel();
                    initializeGameOver();
                    return;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(2000);
            }
        }

        public void StartGame(object sender, EventArgs e)
        {
            DisposeAll();
            initializeGamePanel();
            initializeGame();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void ContinueToMenu(object sender, EventArgs e)
        {
            DisposeAll();
            initializeGamePanel();
            initializeMenu();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void DisposeAll()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                this.Controls.Remove(gamePanels);                
                gamePanels.Dispose();
                gamePanels.Controls.Clear();
                gamePanels = null;
                this.Controls.Clear();

                if (game != null)
                {
                    game.Controls.Clear();
                    game.Close();
                    game.Dispose();
                    game = null;
                }

                if (menu != null)
                {
                    menu.Controls.Clear();
                    menu.Close();
                    menu.Dispose();
                    menu = null;
                }


                if (gameover != null)
                {
                    gameover.Controls.Clear();
                    gameover.Close();
                    gameover.Dispose();
                    gameover = null;
                }
                if (gameAlive != null)
                    gameAlive = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }));
        }
    }
}
