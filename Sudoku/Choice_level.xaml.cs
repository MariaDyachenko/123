using System.Windows;

namespace kurs
{
    public partial class Choice_level : Window
    {
        public Choice_level()
        {
            InitializeComponent();
        }
        public int difficult;

        private void Easy_Click(object sender, RoutedEventArgs e)
        {
            difficult = 75;
            DialogResult = true;
            this.Close();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Medium_Click(object sender, RoutedEventArgs e)
        {
            difficult = 65;
            DialogResult = true;
            this.Close();
        }

        private void Challenging_Click(object sender, RoutedEventArgs e)
        {
            difficult = 55;
            DialogResult = true;
            this.Close();
        }

        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            difficult = 45;
            DialogResult = true;
            this.Close();
        }

        private void Finendish_Click(object sender, RoutedEventArgs e)
        {
            difficult = 35;
            DialogResult = true;
            this.Close();
        }
    }
}
