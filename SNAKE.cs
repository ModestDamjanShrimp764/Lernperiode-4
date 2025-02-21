using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void KeylsDown(object sender, KeyEventArgs e)
        {

        }

        private void KeylsUp(object sender, KeyEventArgs e)
        {

        }

        private void Startgame(object sender, EventArgs e)
        {

        }

        private void TakeSnapShot(object sender, EventArgs e)
        {

        }

        private void GameTimerEvent(object sender, EventArgs e)
        {

        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {

        }

        private void RestartGame()
        {

        }

        private void EatFood()
        {

        }

        private void GameOver()
        {

        }
    }
}
