﻿using System;
using System.Drawing;
using System.Windows.Forms;
using BlockBreaker.Properties;

namespace BlockBreaker
{
    public sealed class MenuButton : Button
    {
        #region Fields

        private readonly MyFonts _fonts;

        #endregion Fields

        #region Constructors

        public MenuButton(Size s)
        {
            if (s.Height > 0 && s.Width > 0)
            {
                _fonts = new MyFonts(MyFonts.FontType.Paragraph);
                UseCompatibleTextRendering = true;
                Size = s;
                Font = new Font(_fonts.Type.Families[0], 12, FontStyle.Regular);
                var buttonBackground = new Bitmap(Resources.BlueRoundedButton, Size);
                BackgroundImage = buttonBackground;
                BackgroundImageLayout = ImageLayout.Stretch;
                BackColor = Color.Transparent;
                MouseHover += MouseHoverButton;
                MouseLeave += MouseLeaveButton;
                FlatStyle = FlatStyle.Flat;
            }
            else
                MessageBox.Show("invalid button size");
        }


        private void MouseHoverButton(object sender, EventArgs e)
        {
            FlatStyle = FlatStyle.Popup;
        }

        private void MouseLeaveButton(object sender, EventArgs e)
        {
            FlatStyle = FlatStyle.Flat;
        }

        #endregion Constructors
    }
}