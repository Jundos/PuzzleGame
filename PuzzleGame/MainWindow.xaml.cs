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
        #region attribute
        BitmapSource imageSource;
        List<Piece> pieces = new List<Piece>();
        int columns = 1;
        int rows = 1;
        #endregion

        #region constructor
        public MainWindow()
        {
            InitializeComponent();

            CreateColandRowinPole(5, 3);
        }
        #endregion constructor

        #region methods
        public void CreateColandRowinPole(int Cols, int Rows)
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
                    Height = new GridLength(1, GridUnitType.Star)
                });
            }
            int count = 0;
            for (int col = 0; col < Cols; col++)
            {
                for (int row = 0; row < Rows; row++)
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

        public void CreatePieces(string uriImage)
        {
            Podbor.Children.Clear();
            pieces.Clear();
            imageSource = new BitmapImage(new Uri(uriImage));
            columns = (int)Math.Ceiling(imageSource.PixelWidth / 100.0);
            rows = (int)Math.Ceiling(imageSource.PixelHeight / 100.0);
            
            int index = 0;
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    var piece = new Piece(imageSource,x,y);
                    piece.Margin = new Thickness(5);
                    
                    //piece.MouseLeftButtonUp += new MouseButtonEventHandler(piece_MouseLeftButtonUp); обработчик событий
                    pieces.Add(piece);
                    index++;
                }
            }
            
            RandomPiece(Podbor);
        }

        private void RandomPiece(WrapPanel podbor)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < pieces.Count; i++)
            {

                int index = rnd.Next(0, pieces.Count);
                Piece tmp = pieces[i];
                pieces[i] = pieces[index];
                pieces[index] = tmp;
            }
            foreach (var p in pieces)
            {
                podbor.Children.Add(p);
            }

        }
        #endregion methods

        #region events
        //HLAM
        private void Pole_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(Mouse.GetPosition((IInputElement)sender).ToString());
        } 

        private void btnCheckImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog()
            {
                Filter = "All Image Files ( JPEG,GIF,BMP,PNG)|*.jpg;*.jpeg;*.gif;*.bmp;*.png|JPEG Files ( *.jpg;*.jpeg )|*.jpg;*.jpeg|GIF Files ( *.gif )|*.gif|BMP Files ( *.bmp )|*.bmp|PNG Files ( *.png )|*.png",
                Multiselect = false,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Title = "Check puzzle image"
            };

            if (openDialog.ShowDialog() == true)
            {
                CreatePieces(openDialog.FileName);
            }
        }
        #endregion events
    }
}
