using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using Sudoku;

namespace kurs
{

    public partial class Sudoku : Window
    {
        int[,] completeSudoku = new int[9, 9];
        bool IsSq(int i, int j, int b)
        {
            int di = i / 3,
                dj = j / 3;
            for (int _b = di * 3, ddi = _b; ddi < _b + 3; ++ddi)
            {
                for (int __b = dj * 3, ddj = __b; ddj < __b + 3; ++ddj)
                {
                    if (completeSudoku[ddi, ddj] == b)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool IsVer(int i, int b)
        {
            for (int dj = 0; dj < 9; ++dj)
            {
                if (completeSudoku[i, dj] == b)
                {
                    return true;
                }
            }
            return false;
        }

        bool IsHor(int j, int b)
        {
            for (int di = 0; di < 9; ++di)
            {
                if (completeSudoku[di, j] == b)
                {
                    return true;
                }
            }
            return false;
        }
        void InitCompleteSudoku()
        {
            for (int i = 0; i < 9; ++i)
                for (int j = 0; j < 9; ++j)
                    completeSudoku[i, j] = 0;
        }
        void GenerateSudoku(Random rnd)
        {
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    int isReal = 9;
                    for (int h = 1; h < 10; ++h)
                        if (IsSq(i, j, h) || IsHor(j, h) || IsVer(i, h))
                            isReal--;
                    if (isReal == 0)
                    {
                        InitCompleteSudoku();
                        GenerateSudoku(rnd);
                        return;
                    }

                    int b = rnd.Next(1, 10);
                    while (IsSq(i, j, b) || IsHor(j, b) || IsVer(i, b))
                        b = rnd.Next(1, 10);
                    completeSudoku[i, j] = b;
                }
            }
        }

        void Fillrnd(int difficult)
        {
            int x, y;
            Random rnd = new Random();
            CleanGrid();
            for (int i = 0; i < difficult; i++)
            {
                do
                {
                    x = rnd.Next(9);
                    y = rnd.Next(9);
                    TextBox element = (TextBox)myGrid.Children.Cast<UIElement>().
                       FirstOrDefault(e => Grid.GetColumn(e) == x && Grid.GetRow(e) == y);

                    if (element.Text == "")
                    {
                        element.IsEnabled = false;
                        element.Text = completeSudoku[x, y].ToString();
                        break;
                    }
                } while (true);
            }
        }

        public void CleanGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox element = (TextBox)myGrid.Children.Cast<UIElement>()
                        .FirstOrDefault(arg => Grid.GetColumn(arg) == i && Grid.GetRow(arg) == j);
                    element.Text = "";
                    element.Background = Brushes.White;
                    element.IsEnabled = true;
                }
            }
        }

        public Sudoku()
        {
            InitializeComponent();
        }


        private void Check_Click(object sender, RoutedEventArgs e)
        {
            bool result = true;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox element = (TextBox)myGrid.Children.Cast<UIElement>().
                        FirstOrDefault(arg => Grid.GetColumn(arg) == i && Grid.GetRow(arg) == j);
                    if (element.IsEnabled)
                    {
                        if (element.Text.Equals(completeSudoku[i, j].ToString()))
                            element.Background = Brushes.GreenYellow;
                        else
                        {
                            element.Background = Brushes.Red;
                            result = false;
                        }
                    }
                }
            }
            if (result)
            {
                Win win= new Win();

                if (win.ShowDialog() == true)
                {

                }

            }
        }

        private void New_Game_Click(object sender, RoutedEventArgs e)
        {
            Choice_level choiceLevel = new Choice_level();
            if (choiceLevel.ShowDialog() == true)
            {
                Random rnd = new Random();
                InitCompleteSudoku();
                GenerateSudoku(rnd);
                Fillrnd(choiceLevel.difficult);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            TextBox textBox = (TextBox)sender;
            textBox.Background = Brushes.White;
        }

        private void Load_Game_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.Title = "Select a Cursor File";
            if (openFileDialog1.ShowDialog() == true)
            {
                CleanGrid();
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        completeSudoku[i,j] = sr.Read() -48;
                    }
                    sr.ReadLine();
                }
                int temp;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        TextBox element = (TextBox)myGrid.Children.Cast<UIElement>()
                   .FirstOrDefault(arg => Grid.GetColumn(arg) == i && Grid.GetRow(arg) == j);
                        temp = sr.Read() - 48;
                        if (temp != 0)
                            element.Text =temp.ToString();
                    }
                    sr.ReadLine();
                }
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        TextBox element = (TextBox)myGrid.Children.Cast<UIElement>()
                   .FirstOrDefault(arg => Grid.GetColumn(arg) == i && Grid.GetRow(arg) == j);
                        if (sr.Read()-48 == 0)
                        {
                            element.IsEnabled = false;
                        }
                    }
                    sr.ReadLine();
                }

                sr.Close();
            }
        }

        private void Save_Game_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "Sudoku.txt";
            savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (savefile.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(savefile.FileName))
                {
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            sw.Write(completeSudoku[i, j]);
                        }
                        sw.WriteLine();
                    }
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            TextBox element = (TextBox) myGrid.Children.Cast<UIElement>()
                                .FirstOrDefault(arg => Grid.GetColumn(arg) == i && Grid.GetRow(arg) == j);
                            if(element.Text.Equals(""))
                                sw.Write(0);
                            else
                                sw.Write(element.Text);
                        }
                        sw.WriteLine();
                    }
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            TextBox element = (TextBox) myGrid.Children.Cast<UIElement>()
                                .FirstOrDefault(arg => Grid.GetColumn(arg) == i && Grid.GetRow(arg) == j);
                            if (element.IsEnabled == true)
                                sw.Write(1);
                            else
                                sw.Write(0);
                        }
                        sw.WriteLine();
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Exit", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }
    }
}
