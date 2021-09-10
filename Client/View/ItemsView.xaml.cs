using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.View
{
    /// <summary>
    /// Interaction logic for ItemsView.xaml
    /// </summary>
    public partial class ItemsView : UserControl
    {
        public ItemsView()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
