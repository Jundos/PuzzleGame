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
        List<Piece> currentSelection = new List<Piece>();
        int columns = 1;
        int rows = 1;
        #endregion

        #region constructor
        public MainWindow()
        {
            InitializeComponent();

            Pole.MouseEnter += new MouseEventHandler(pole_MouseEnter);
            Pole.MouseMove += new MouseEventHandler(pole_MouseMove);
            Pole.MouseLeftButtonUp += new MouseButtonEventHandler(pole_MouseLeftButtonUp);
        }

        #endregion constructor

        #region methods
        public void CreatePole()
        {
            Pole.Children.Clear();
            Pole.Width = columns * 100;
            Pole.Height = rows * 100;
            Pole.Background = new SolidColorBrush(Colors.WhiteSmoke);
            Pole.Margin = new Thickness(50);
            Pole.Parent.SetValue(Grid.BackgroundProperty, new SolidColorBrush(Colors.DarkGray));
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
                    var piece = new Piece(imageSource, x, y)
                    {
                        Margin = new Thickness(5)
                    };

                    piece.MouseLeftButtonUp += new MouseButtonEventHandler(piece_MouseLeftButtonUp);
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

        private bool CanInsertPiece(int cellX, int cellY) // !! IT`S DONT WORK !!
        {
            bool ret = true;
            foreach (var currentPiece in currentSelection)
            {
                if (currentPiece.Row == cellY && currentPiece.Col == cellX) ret = false;
            }

            return ret;
        }
        #endregion methods

        #region events
        private void piece_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var chosenPiece = (Piece)sender;

            if (chosenPiece.Parent is WrapPanel)
            {
                if (currentSelection.Count() > 0)
                {
                    var p = currentSelection[0];
                    Pole.Children.Remove(p);
                    p.Visibility = Visibility.Visible;
                    Podbor.Children.Add(p);
                    currentSelection.Clear();
                }
                else
                {
                    Podbor.Children.Remove(chosenPiece);
                    Pole.Children.Add(chosenPiece);
                    chosenPiece.Visibility = Visibility.Hidden;
                    currentSelection.Add(chosenPiece);
                }
            }
        }

        private void pole_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void pole_MouseEnter(object sender, MouseEventArgs e)
        {
            if (currentSelection.Count > 0)
            {
                foreach (var currentPiece in currentSelection)
                {
                    currentPiece.Visibility = Visibility.Visible;
                }
            }
        }

        private void pole_MouseMove(object sender, MouseEventArgs e)
        {
            var newX = Mouse.GetPosition((IInputElement)Pole).X;
            var newY = Mouse.GetPosition((IInputElement)Pole).Y;

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                //   SetSelectionRectangle(initialRectangleX, initialRectangleY, newX, newY);
            }
            else
            {
                if (currentSelection.Count > 0)
                {
                    var firstPiece = currentSelection[0];
                    foreach (var currentPiece in currentSelection)
                    {
                        double CellX = currentPiece.Row - firstPiece.Row;
                        double CellY = currentPiece.Col - firstPiece.Col;

                        currentPiece.SetValue(Canvas.LeftProperty, newX - 50 + CellX * 100);
                        currentPiece.SetValue(Canvas.TopProperty, newY - 50 + CellY * 100);
                    }
                }
            }
        }

        private void pole_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var newX = Mouse.GetPosition(Pole).X;
            var newY = Mouse.GetPosition(Pole).Y;

            double cellX = (int)((newX) / 100);
            double cellY = (int)((newY) / 100);

            if (currentSelection.Count > 0 && CanInsertPiece((int)cellX,(int)cellY))
            {
                var firstPiece = currentSelection[0];

                var relativeCellX = currentSelection[0].Col - firstPiece.Col;
                var relativeCellY = currentSelection[0].Row - firstPiece.Row;

                double rotatedCellX = relativeCellX;
                double rotatedCellY = relativeCellY;

                currentSelection[0].Col = cellX + rotatedCellX;
                currentSelection[0].Row = cellY + rotatedCellY;

                currentSelection[0].SetValue(Canvas.LeftProperty, currentSelection[0].Col * 100);
                currentSelection[0].SetValue(Canvas.TopProperty, currentSelection[0].Row * 100);

                currentSelection.Clear();
            }
            else
            {
                //var query = from p in pieces
                //            where
                //            (p.Col == cellX) && (p.Col == cellX) &&
                //            (p.Row == cellY) && (p.Row == cellY)
                //            select p;
                foreach (var p in pieces)
                {
                    if ((p.Col == cellX) && (p.Row == cellY))
                    {
                        p.Visibility = Visibility.Visible;
                        currentSelection.Add(p);
                    }
                }
            }
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
                CreatePole();
            }
        }
        #endregion events
    }
}
