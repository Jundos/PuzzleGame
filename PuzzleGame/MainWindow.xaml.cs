using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PuzzleGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //ObservableCollection<ColumnDefinition> coldef = new ObservableCollection<ColumnDefinition>();
        //ObservableCollection<RowDefinition> rowdef = new ObservableCollection<RowDefinition>();
        protected void CreateColandRowinPole(int Col,int Row)
        {   
            for (int i = 0; i < Col; i++)
            {

                Pole.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }

            for (int i = 0; i < Row; i++)
            {
                Pole.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(120)
                });
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            CreateColandRowinPole(3, 3);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Rectangle rect = new Rectangle
                    {
                        Width = 100,
                        Height = 100,
                        Fill = new SolidColorBrush(Colors.BlueViolet),
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 2,
                        Margin = new Thickness(5)
                    };
                    rect.SetValue(Grid.RowProperty, i);
                    rect.SetValue(Grid.ColumnProperty, j);
                    Pole.Children.Add(rect);
                }
                
            }

        }
    }
}
