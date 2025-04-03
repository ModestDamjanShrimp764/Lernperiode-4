using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SNAKE
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private List<Circle> obstacles = new List<Circle>();
        private Circle food = new Circle();
        bool isGoldenFood = false;
        int MaxWidth, MaxHeight;
        int score, Highscore;
        bool isInvincible = false;
        Timer powerUpTimer = new Timer();
        Random rand = new Random();

        Label gameOverLabel = new Label();
        Button restartButton = new Button();

        public Form1()
        {
            InitializeComponent();
            new Settings();
            Settings.directions = "right";

            this.Load += Form1_Load;
            gametimer.Tick += GameTimerEvent;
            this.KeyDown += new KeyEventHandler(KeyIsDown);
            this.KeyPreview = true;
            pictureBox1.Paint += new PaintEventHandler(UpdatePictureBoxGraphics);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MaxWidth = pictureBox1.Width / Settings.Width - 1;
            MaxHeight = pictureBox1.Height / Settings.Height - 1;
            GenerateObstacles();
            SetupEndScreen();
        }

        private void SetupEndScreen()
        {
            gameOverLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            gameOverLabel.Size = new Size(300, 100);
            gameOverLabel.TextAlign = ContentAlignment.MiddleCenter;
            gameOverLabel.BackColor = Color.White;
            gameOverLabel.Visible = false;
            gameOverLabel.BringToFront();

            restartButton.Text = "Nochmal spielen";
            restartButton.Size = new Size(150, 40);
            restartButton.BackColor = Color.LightGreen;
            restartButton.Visible = false;
            restartButton.Click += (s, e) =>
            {
                gameOverLabel.Visible = false;
                restartButton.Visible = false;
                RestartGame();
            };

            this.Controls.Add(gameOverLabel);
            this.Controls.Add(restartButton);
        }

        private void ShowEndScreen()
        {
            gameOverLabel.Text = $"Game Over!\nScore: {score}\nHighscore: {Highscore}";
            int x = pictureBox1.Left + pictureBox1.Width / 2 - gameOverLabel.Width / 2;
            int y = pictureBox1.Top + pictureBox1.Height / 2 - gameOverLabel.Height / 2;
            gameOverLabel.Location = new Point(x, y);
            restartButton.Location = new Point(x + 70, y + 100);

            gameOverLabel.Visible = true;
            gameOverLabel.BringToFront();
            restartButton.Visible = true;
            restartButton.BringToFront();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Left || e.KeyCode == Keys.A) && Settings.directions != "right")
                Settings.directions = "left";
            else if ((e.KeyCode == Keys.Right || e.KeyCode == Keys.D) && Settings.directions != "left")
                Settings.directions = "right";
            else if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.W) && Settings.directions != "down")
                Settings.directions = "up";
            else if ((e.KeyCode == Keys.Down || e.KeyCode == Keys.S) && Settings.directions != "up")
                Settings.directions = "down";
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void RestartGame()
        {
            gametimer.Interval = 100;

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

            SpawnFood();
            gametimer.Start();
            isInvincible = false;
        }

        private void SpawnFood()
        {
            isGoldenFood = rand.Next(0, 5) == 0;
            food = new Circle { X = rand.Next(2, MaxWidth - 1), Y = rand.Next(2, MaxHeight - 1) };
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.directions)
                    {
                        case "left": Snake[i].X--; break;
                        case "right": Snake[i].X++; break;
                        case "down": Snake[i].Y++; break;
                        case "up": Snake[i].Y--; break;
                    }

                    if (Snake[i].X < 0) Snake[i].X = MaxWidth;
                    else if (Snake[i].X > MaxWidth) Snake[i].X = 0;
                    if (Snake[i].Y < 0) Snake[i].Y = MaxHeight;
                    else if (Snake[i].Y > MaxHeight) Snake[i].Y = 0;

                    if (Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        if (isGoldenFood) EatGoldFood();
                        else EatNormalFood();
                        SpawnFood();
                    }

                    foreach (var obs in obstacles)
                    {
                        if (Snake[i].X == obs.X && Snake[i].Y == obs.Y)
                        {
                            if (!isInvincible)
                            {
                                gametimer.Stop();
                                if (score > Highscore) Highscore = score;
                                ShowEndScreen();
                                return;
                            }
                        }
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            if (!isInvincible)
                            {
                                gametimer.Stop();
                                if (score > Highscore) Highscore = score;
                                ShowEndScreen();
                                return;
                            }
                        }
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
            pictureBox1.Invalidate();
        }

        private void EatNormalFood()
        {
            score++;
            txtScore.Text = "Score: " + score;
            Snake.Add(new Circle { X = Snake[Snake.Count - 1].X, Y = Snake[Snake.Count - 1].Y });
            if (score % 5 == 0 && gametimer.Interval > 40)
            {
                gametimer.Interval -= 10;
            }
        }

        private void EatGoldFood()
        {
            score += 5;
            txtScore.Text = "Score: " + score;
            for (int i = 0; i < 3; i++)
            {
                Snake.Add(new Circle { X = Snake[Snake.Count - 1].X, Y = Snake[Snake.Count - 1].Y });
            }
            isInvincible = true;
            powerUpTimer.Interval = 500;
            powerUpTimer.Tick += DeactivatePowerUp;
            powerUpTimer.Start();
        }

        private void DeactivatePowerUp(object sender, EventArgs e)
        {
            isInvincible = false;
            powerUpTimer.Stop();
            powerUpTimer.Tick -= DeactivatePowerUp;
        }

        private void GenerateObstacles()
        {
            for (int i = 0; i < 5; i++)
            {
                obstacles.Add(new Circle { X = rand.Next(5, MaxWidth - 5), Y = rand.Next(5, MaxHeight - 5) });
            }
        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            foreach (var obs in obstacles)
            {
                canvas.FillRectangle(Brushes.Brown, new Rectangle(obs.X * Settings.Width, obs.Y * Settings.Height, Settings.Width, Settings.Height));
            }
            for (int i = 0; i < Snake.Count; i++)
            {
                Brush color = (i == 0) ? Brushes.Red : Brushes.Blue;
                canvas.FillRectangle(color, new Rectangle(Snake[i].X * Settings.Width, Snake[i].Y * Settings.Height, Settings.Width, Settings.Height));
            }
            Brush foodColor = isGoldenFood ? Brushes.Gold : Brushes.Red;
            canvas.FillEllipse(foodColor, new Rectangle(food.X * Settings.Width, food.Y * Settings.Height, Settings.Width, Settings.Height));
        }
    }
}

