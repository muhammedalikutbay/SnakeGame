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
using SnakeGame;

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

        private readonly Dictionary<Direction, int> directionToRotation = new Dictionary<
            Direction,
            int
        >
        {
            { Direction.Up, 0 },
            { Direction.Right, 90 },
            { Direction.Down, 180 },
            { Direction.Left, 270 },
        };

        private readonly int rows = 32;
        private readonly int columns = 32;
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
            await GameStartCounter();
            GameOverOverlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await GameOver();
            gameState = new Game(rows, columns);
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
            GameGrid.Width = GameGrid.Height * (columns / (double)rows);
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5),
                    };
                    GameGrid.Children.Add(image);
                    images[r, c] = image;
                }
            }
            return images;
        }

        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"Score: {gameState.Score}";
        }

        private void DrawSnakeHead()
        {
            Position head = gameState.HeadPosition();
            Image image = gridImages[head.Row, head.Column];
            image.Source = Images.Head;

            int rotation = directionToRotation[gameState.Dir];
            image.RenderTransform = new RotateTransform(rotation);
        }

        private void DrawGrid()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    GridValue value = gameState.Grid[r, c];
                    gridImages[r, c].Source = imageSources[value];
                    gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }

        private async Task DrawDeadSnake()
        {
            List<Position> positions = new List<Position>(gameState.SnakePosition());
            for (int i = 0; i < positions.Count; i++)
            {
                Position position = positions[i];
                ImageSource source = i == 0 ? Images.DeadHead : Images.DeadBody;
                gridImages[position.Row, position.Column].Source = source;
                await Task.Delay(50);
            }
        }

        private async Task GameStartCounter()
        {
            GameOverOverlay.Visibility = Visibility.Visible;
            for (int i = 3; i > 0; i--)
            {
                GameOverText.Text = i.ToString();
                await Task.Delay(500);
            }
            GameOverOverlay.Visibility = Visibility.Hidden;
        }

        private async Task GameOver()
        {
            await DrawDeadSnake();
            await Task.Delay(1000);
            GameOverOverlay.Visibility = Visibility.Visible;
            GameOverText.Text = $"PRESS ANY KEY TO START";
        }
    }
}
