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

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> imageSources = new Dictionary<
            GridValue,
            ImageSource
        >
        {
            { GridValue.Empty, Images.Empty },
            { GridValue.Snake, Images.Body },
            { GridValue.Food, Images.Food },
        };

        private readonly int rows = 15;
        private readonly int columns = 15;
        private readonly Image[,] gridImages;
        private Game gameState;
        private bool gameRunnig;


        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetUpGrid();
            gameState = new Game(rows, columns);
        }
        private async Task StartGame()
        {
            Draw();
            GameOverOverlay.Visibility = Visibility.Hidden;
            await GameLoop();
        }
        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (GameOverOverlay.Visibility == Visibility.Visible)
            {
               e.Handled = true;
            }
            if (!gameRunnig)
            {
                gameRunnig = true;
                await StartGame();
                gameRunnig = false;
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Up:
                    gameState.ChangeDirection(Direction.Up);
                    break;
                case Key.Down:
                    gameState.ChangeDirection(Direction.Down);
                    break;
                case Key.Left:
                    gameState.ChangeDirection(Direction.Left);
                    break;
                case Key.Right:
                    gameState.ChangeDirection(Direction.Right);
                    break;
            }
        }
        private async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
                await Task.Delay(100);
                gameState.Move();
                Draw();
            }
        }

        private Image[,] SetUpGrid()
        {
            Image[,] images = new Image[rows, columns];
            GameGrid.Rows = rows;
            GameGrid.Columns = columns;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Image image = new Image { Source = Images.Empty };
                    GameGrid.Children.Add(image);
                    images[r, c] = image;
                }
            }
            return images;
        }

        private void Draw()
        {
            DrawGrid();
            ScoreText.Text = $"Score: {gameState.Score}";
        }

        private void DrawGrid()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    GridValue value = gameState.Grid[r, c];
                    gridImages[r, c].Source = imageSources[value];
                }
            }
        }
    }
}
