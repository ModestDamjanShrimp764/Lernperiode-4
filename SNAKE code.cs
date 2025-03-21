using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SNAKE
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private List<Circle> obstacles = new List<Circle>(); // Hindernisse
        private Circle food = new Circle();
        private Circle specialFood = new Circle(); // Spezialfrucht
        int MaxWidth, MaxHeight;
        int score, Highscore;
        Random rand = new Random();
        Timer gameTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            new Settings();
            Settings.directions = "right";

            gameTimer.Interval = 100;
            gameTimer.Tick += GameTimerEvent;
            this.KeyDown += new KeyEventHandler(KeyIsDown); // Sicherstellen, dass KeyDown registriert ist
            GenerateObstacles(); // Hindernisse erstellen
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {

        
            Console.WriteLine("Taste gedrückt: " + e.KeyCode); // Debug-Ausgabe

            if ((e.KeyCode == Keys.Left || e.KeyCode == Keys.A) && Settings.directions != "right")
            {
                Console.WriteLine("Links gedrückt!");
                Settings.directions = "left";
            }
            else if ((e.KeyCode == Keys.Right || e.KeyCode == Keys.D) && Settings.directions != "left")
            {
                Console.WriteLine("Rechts gedrückt!");
                Settings.directions = "right";
            }
            else if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.W) && Settings.directions != "down")
            {
                Console.WriteLine("Hoch gedrückt!");
                Settings.directions = "up";
            }
            else if ((e.KeyCode == Keys.Down || e.KeyCode == Keys.S) && Settings.directions != "up")
            {
                Console.WriteLine("Runter gedrückt!");
                Settings.directions = "down";
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
            specialFood = new Circle { X = rand.Next(2, MaxWidth - 1), Y = rand.Next(2, MaxHeight - 1) };
            Console.WriteLine("Timer gestartet: " + gameTimer.Enabled);

            gameTimer.Start();
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
   
        
            Console.WriteLine($"Vorher: Kopf X={Snake[0].X}, Y={Snake[0].Y} | Richtung: {Settings.directions}");

            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0) // Kopf bewegt sich
                {
                    switch (Settings.directions)
                    {
                        case "left": Snake[i].X--; break;
                        case "right": Snake[i].X++; break;
                        case "down": Snake[i].Y++; break;
                        case "up": Snake[i].Y--; break;
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }

            Console.WriteLine($"Nachher: Kopf X={Snake[0].X}, Y={Snake[0].Y}");

            pictureBox1.Invalidate();
        }





        private void GenerateObstacles()
        {
            for (int i = 0; i < 5; i++)
            {
                obstacles.Add(new Circle { X = rand.Next(5, MaxWidth - 5), Y = rand.Next(5, MaxHeight - 5) });
            }
        }

        private void EatSpecialFood()
        {
            score += 5;
            txtScore.Text = "Score: " + score;

            for (int i = 0; i < 3; i++)
            {
                Snake.Add(new Circle { X = Snake[Snake.Count - 1].X, Y = Snake[Snake.Count - 1].Y });
            }

            specialFood = new Circle { X = rand.Next(2, MaxWidth), Y = rand.Next(2, MaxHeight) };
        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
       
        
            Console.WriteLine("UpdatePictureBoxGraphics wird ausgeführt!"); // Debug-Ausgabe
            Graphics canvas = e.Graphics;

            foreach (var obs in obstacles)
            {
                canvas.FillRectangle(Brushes.Brown, new Rectangle(
                    obs.X * Settings.Width,
                    obs.Y * Settings.Height,
                    Settings.Width, Settings.Height
                ));
            }

            foreach (var part in Snake)
            {
                canvas.FillRectangle(Brushes.Green, new Rectangle(
                    part.X * Settings.Width,
                    part.Y * Settings.Height,
                    Settings.Width, Settings.Height
                ));
            }

            canvas.FillEllipse(Brushes.Red, new Rectangle(
                food.X * Settings.Width,
                food.Y * Settings.Height,
                Settings.Width, Settings.Height
            ));
        }

    }
}

