namespace WindowsFormsApplication5
{
    partial class Game
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Gamepause = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800,500);
            this.Controls.Add(this.Gamepause);
            this.Name = "Game";
            this.Text = "Game";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();
            // 
            // Gamepause
            // 
            this.Gamepause.Size = new System.Drawing.Size(this.Size.Width / 5, this.Size.Height / 5);
            this.Gamepause.AutoSize = true;
            this.Gamepause.BackColor = System.Drawing.Color.Aqua;
            this.Gamepause.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Gamepause.Name = "Gamepause";
            this.Gamepause.TabIndex = 0;
            this.Gamepause.Text = "Game Paused\r\n\r\nPress Space to Resume\r\n\r\nPress ESC to Exit";
            this.Gamepause.Location = new System.Drawing.Point(this.ClientRectangle.Width / 2 - this.Gamepause.Width / 2, this.ClientRectangle.Height / 2 - this.Gamepause.Width / 2);
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label Gamepause;
    }
}

