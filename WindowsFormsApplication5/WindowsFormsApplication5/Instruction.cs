using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public class Instructions : Panel
    {
        public Instructions CreateInstructions(int Left, int Top, int Width, int Height)
        {
            var controller = this;
            controller.Left = Left;
            controller.Top = Top;
            controller.Width = Width;
            controller.Height = Height;
            controller.BackgroundImage = Properties.Resources.Instructions;
            controller.BackgroundImageLayout = ImageLayout.Stretch;
            controller.Visible = false;
            return controller;
        }
    }
}
