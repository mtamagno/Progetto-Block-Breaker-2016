using System;
using System.Windows.Forms;
namespace BlockBreaker
{
    class ContainerEvents
    {
            private Container ContainerForm;

            public ContainerEvents(Container container)
            {
            ContainerForm = container;
            }

            /// <summary>
            /// Funzione necessaria per cambiare form dopo la fine della partita
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void ContinueToMenu(object sender, EventArgs e)
            {
                if (ContainerForm._gameOver.TextBox.Text != "Insert Name..." && !string.IsNullOrEmpty(ContainerForm._gameOver.TextBox.Text) &&
                    !string.IsNullOrWhiteSpace(ContainerForm._gameOver.TextBox.Text))
                {
                // Salva prima lo score, poi l'_highScore nell'xml
                ContainerForm._highScore.Name = ContainerForm._gameOver.TextBox.Text;
                ContainerForm._highScore.ModifyOrCreateXml(ContainerForm._highScore);

                // Imposta che il giocatore ha gia finito una partita
                ContainerForm._again = true;

                // Pulisce tutto
                ContainerForm.DisposeAll();

                // Inizializza il gamePanel
                ContainerForm.InitializeGamePanel();

                // Inizializza il _menu
                ContainerForm.InitializeMenu();

                    // Svuota il garbage collector per liberare memoria
                    GC.Collect();

                    // Aspetta che il garbage collecor finisca
                    GC.WaitForPendingFinalizers();
                    GC.WaitForFullGCComplete();
                }
                else
                {
                MessageBox.Show("Inserisci un NickName");
                }
            }
        }
   



}
