using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace PuzzleGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //ObservableCollection<ColumnDefinition> coldef = new ObservableCollection<ColumnDefinition>();
        //ObservableCollection<RowDefinition> rowdef = new ObservableCollection<RowDefinition>();
        ImageSource imageSource;

        protected void CreateColandRowinPole(int Cols,int Rows)
        {   
            for (int i = 0; i < Cols; i++)
            {
                Pole.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }

            for (int i = 0; i < Rows; i++)
            {
                Pole.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(1,GridUnitType.Star) 
                });
            }
            int count = 0;
            for (int col = 0; col < Cols; col++)
            {
                for(int row = 0; row < Rows; row++)
                {
                    Canvas canvas = new Canvas()
                    {
                        Width = 100,
                        Height = 100,
                        Background = new SolidColorBrush(Colors.WhiteSmoke),
                        Margin = new Thickness(0)
                    };
                    if (count % 2 != 0) canvas.Background = new LinearGradientBrush(Colors.Brown, Colors.WhiteSmoke, new Point(0, 0), new Point(1, 0.7));
                    count++;
                    canvas.SetValue(Grid.RowProperty, row);
                    canvas.SetValue(Grid.ColumnProperty, col);
                    Pole.Children.Add(canvas);
                }
            }
        }

        public void LoadPiece(string uriImage)
        {
            Podbor.Children.Clear();
            imageSource = new BitmapImage(new Uri(uriImage));
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Rectangle rect = new Rectangle
                    {
                        Width = 100,
                        Height = 100,
                        Fill = new ImageBrush()
                        {
                            ImageSource = imageSource,
                            Viewbox = new Rect(0, 0, 1, 1)
                        },
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 0,
                        Margin = new Thickness(5)
                    };
                    Podbor.Children.Add(rect);
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            

            CreateColandRowinPole(3, 3);

            LoadPiece("C:/image.jpg");



            //Path path = new Path();
            //RectangleGeometry rg = new RectangleGeometry();
            //ImageSource imageSou = new BitmapImage(new Uri(""));
            //Rectangle rec = new Rectangle()
            //{
            //    Width = 100,
            //    Height = 100,
            //    Fill = new ImageBrush()
            //    {
            //        ImageSource = imageSou
            //    }
            //};



        }

        private void Pole_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(Mouse.GetPosition((IInputElement)sender).ToString());
        }

        private void btnCheckImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog()
            {
                Filter = "Image files (*.png;*.jpeg)|*.png;*.jpg|All files (*.*)|*.*",
                Multiselect = false,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Title = "Check puzzle image"
            };

            if (openDialog.ShowDialog() == true)
            {
                LoadPiece(openDialog.FileName);
            }
        }
    }
}
