﻿using System.Windows.Forms;
using System;

namespace BlockBreaker
{
    class MenuEvents
    {
        private Menu menuForm;
        public MenuEvents(Menu menu)
        {
            menuForm = menu;
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
                menuForm._menuPanel.Visible = true;
                menuForm.Start.Visible = true;
                menuForm._help.Visible = true;
                menuForm._instructions.Visible = false;
                menuForm._highScoresPanel.Visible = false;
                menuForm._logo.Visible = true;
                menuForm._showHighScore = false;
            }
        }

        /// <summary>
        /// Funzione che permette di mostrare Gli highScores migliori
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowHighScore(object sender, EventArgs e)
        {
            menuForm._menuPanel.Visible = false;
            menuForm._logo.Visible = false;
            menuForm._highScoresPanel.Visible = true;
            menuForm.Focus();
            menuForm.KeyPress += this.Help_KeyPress;
            menuForm._showHighScore = true;
        }

        /// <summary>
        /// Evento che permette di nascondere le istruzioni nascondendo il resto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowInstructions(object sender, EventArgs e)
        {
            menuForm._menuPanel.Visible = false;
            menuForm._logo.Visible = false;
            menuForm._instructions.Visible = true;
            menuForm.Focus();
            menuForm.KeyPress += this.Help_KeyPress;
        }
    }
}
