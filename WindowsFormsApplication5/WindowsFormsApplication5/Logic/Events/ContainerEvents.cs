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
                if (ContainerForm.GameOver.TextBox.Text != "Insert Name..." && !string.IsNullOrEmpty(ContainerForm.GameOver.TextBox.Text) &&
                    !string.IsNullOrWhiteSpace(ContainerForm.GameOver.TextBox.Text))
                {
                // Salva prima lo score, poi l'_highScore nell'xml
                ContainerForm.HighScore.Name = ContainerForm.GameOver.TextBox.Text;
                ContainerForm.HighScore.ModifyOrCreateXml(ContainerForm.HighScore);

                // Imposta che il giocatore ha gia finito una partita
                ContainerForm.Again = true;

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
