using System.Windows;

namespace PointOfSaleSystem
{
    public partial class ConfirmationDialog : Window
    {
        public bool IsDone { get; private set; }

        public ConfirmationDialog()
        {
            InitializeComponent();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            IsDone = true;
            Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            IsDone = false;
            Close();
        }
    }
}
