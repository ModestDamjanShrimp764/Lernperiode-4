using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SNAKE
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        int MaxWidth, MaxHeight;
        int score, Highscore;
        Random rand = new Random();
        bool goLeft, goRight, goDown, goUp;
        Timer gameTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            new Settings();
            Settings.directions = "right"; // Startrichtung der Schlange

            gameTimer.Interval = 100;
            gameTimer.Tick += GameTimerEvent;
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.Left && Settings.directions != "right")
            {
                goLeft = true;
                goRight = goUp = goDown = false;
            }
            else if (e.KeyCode == Keys.Right && Settings.directions != "left")
            {
                goRight = true;
                goLeft = goUp = goDown = false;
            }
            else if (e.KeyCode == Keys.Up && Settings.directions != "down")
            {
                goUp = true;
                goLeft = goRight = goDown = false;
            }
            else if (e.KeyCode == Keys.Down && Settings.directions != "up")
            {
                goDown = true;
                goLeft = goRight = goUp = false;
            }
        }


        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void RestartGame()
        {
            MaxWidth = pictureBox1.Width / Settings.Width - 1;
            MaxHeight = pictureBox1.Height / Settings.Height - 1;

            Snake.Clear();
            START.Enabled = false;
            SNAP.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;

            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);

            for (int i = 1; i < 5; i++)
            {
                Snake.Add(new Circle { X = head.X - i, Y = head.Y });
            }

            food = new Circle { X = rand.Next(2, MaxWidth - 1), Y = rand.Next(2, MaxHeight - 1) };

            gameTimer.Start();
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    // Bewegung basierend auf der Richtung
                    switch (Settings.directions)
                    {
                        case "left": Snake[i].X--; break;
                        case "right": Snake[i].X++; break;
                        case "down": Snake[i].Y++; break;
                        case "up": Snake[i].Y--; break;
                    }

                    // Wrap-Around (Schlange erscheint auf der anderen Seite)
                    if (Snake[i].X < 0) Snake[i].X = MaxWidth;
                    if (Snake[i].X > MaxWidth) Snake[i].X = 0;
                    if (Snake[i].Y < 0) Snake[i].Y = MaxHeight;
                    if (Snake[i].Y > MaxHeight) Snake[i].Y = 0;

                    // **Game Over bei Selbstkollision**
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            GameOver();
                            return;
                        }
                    }
                }
                else
                {
                    // Bewegung der Körperteile
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }


            pictureBox1.Invalidate();
        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {

            Graphics canvas = e.Graphics;

            for (int i = 0; i < Snake.Count; i++)
            {
                Brush snakeColor = (i == 0) ? Brushes.Blue : Brushes.Green; // Kopf = Blau, Körper = Grün

                canvas.FillRectangle(snakeColor, new Rectangle(
                    Snake[i].X * Settings.Width,
                    Snake[i].Y * Settings.Height,
                    Settings.Width, Settings.Height
                ));
            }

            // Essen als Kreis
            canvas.FillEllipse(Brushes.Red, new Rectangle(
                food.X * Settings.Width,
                food.Y * Settings.Height,
                Settings.Width, Settings.Height
            ));
        }


        private void EatFood()
        {
            score++;
            txtScore.Text = "Score: " + score;

            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);

            food = new Circle { X = rand.Next(2, MaxWidth - 1), Y = rand.Next(2, MaxHeight - 1) };
        }

        private void GameOver()
        {


            gameTimer.Stop();
            START.Enabled = true;
            SNAP.Enabled = true;

            MessageBox.Show("Game Over! Dein Score: " + score, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (score > Properties.Settings.Default.HighScore)
            {
                Properties.Settings.Default.HighScore = score;
                Properties.Settings.Default.Save();
            }

            HighScore.Text = "High Score: " + Properties.Settings.Default.HighScore;
        }
    }
}
