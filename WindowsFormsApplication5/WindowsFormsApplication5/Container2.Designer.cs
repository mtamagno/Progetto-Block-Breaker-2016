﻿namespace WindowsFormsApplication5
{
    partial class Container2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Container2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 461);
            this.Name = "Container2";
            this.Text = "Container2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Container2_FormClosing);
            this.Load += new System.EventHandler(this.Container2_Load);
            this.ResizeBegin += new System.EventHandler(this.Container2_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.OnResizeEnd);
            this.ResumeLayout(false);

        }

        #endregion
    }
}