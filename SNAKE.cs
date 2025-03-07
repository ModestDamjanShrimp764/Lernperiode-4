using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SNAKE
{
    public partial class Form1: Form
    {

        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        int MaxWidth;
        int MaxHeight;

        int score;
        int Highscore;

        Random rand = new Random();

        bool goLeft, goRight, goDown, goUp;
        public Form1()
        {
            InitializeComponent();

         
        
            
            
            new Settings();
        }


        
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        
            this.Focus();
        

        }

        private void KeylsDown(object sender, KeyEventArgs e)
        {
       
        
            if (e.KeyCode == Keys.Left && Settings.directions != "right")
            {
                goLeft = true;
                goRight = false;
            }
            else if (e.KeyCode == Keys.Right && Settings.directions != "left")
            {
                goRight = true;
                goLeft = false;
            }
            else if (e.KeyCode == Keys.Up && Settings.directions != "down")
            {
                goUp = true;
                goDown = false;
            }
            else if (e.KeyCode == Keys.Down && Settings.directions != "up")
            {
                goDown = true;
                goUp = false;
            }
        }

        
            


        private void KeylsUp(object sender, KeyEventArgs e)
        {
            
        }

        private void Startgame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void TakeSnapShot(object sender, EventArgs e)
        {

        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            // setting the directions

            if (goLeft)
            {
                Settings.directions = "left";
            }
            if (goRight)
            {
                Settings.directions = "right";
            }
            if (goDown)
            {
                Settings.directions = "down";
            }
            if (goUp)
            {
                Settings.directions = "up";
            }
            // end of directions 
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.directions)
                    {
                        case "left":
                            Snake[i].X--;
                            break;
                        case "right":
                            Snake[i].X++;
                            break;
                        case "down":
                            Snake[i].Y++;
                            break;
                        case "up":
                            Snake[i].Y--;
                            break;

                    }
                    if (Snake[i].X < 0)
                    {
                        Snake[i].X = MaxWidth;
                    }
                    if (Snake[i].X > MaxWidth)
                    {
                        Snake[i].X = 0;
                    }
                    if (Snake[i].Y < 0)
                    {
                        Snake[i].Y = MaxHeight;
                    }
                    if (Snake[i].Y < MaxHeight)
                    {
                        Snake[i].Y = 0;
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

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            Brush SnakeColour;

            for (int i = 0; i < Snake.Count; i++)
            {
                if (i == 0)
                {
                    SnakeColour = Brushes.Black;
                }
                else
                {
                    SnakeColour = Brushes.DarkGreen;
                }

                canvas.FillEllipse(SnakeColour, new Rectangle
                    (
                    Snake[i].X * Settings.Width,
                    Snake[i].Y * Settings.Height,
                    Settings.Width, Settings.Height
                    ));
            }

            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
                    (
                    food.X * Settings.Width,
                    food.Y * Settings.Height,
                    Settings.Width, Settings.Height
                    ));
           
            }

        private void RestartGame()
        {
          
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

            for (int i = 0; i < 10; i++)
            {
                Circle body = new Circle();
                Snake.Add(body);
            }

            food = new Circle { X = rand.Next(2, MaxWidth), Y = rand.Next(2, MaxHeight) };

        
            gametimer.Stop();
            gametimer.Tick -= GameTimerEvent; 
            gametimer.Tick += GameTimerEvent;
            gametimer.Interval = 100;
            gametimer.Start();
        }
 }

        private void EatFood()
        {

        }

        private void GameOver()
        {

        }
    }
}
