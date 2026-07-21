using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using clsBus;

namespace Sliding_Puzzle
{
    
    public partial class frm4SlidingPuzzle : Form
    {
        private clsBus.Game _game = new clsBus.Game();

        private Button[,] _btnGrid;

        int Timer = 0;

        private void UpdateHighScore()
        {
            int savedHighScore = Properties.Settings.Default.HighScore;

            if (savedHighScore == 0)
                lblHighScore.Text = "No High Score Yet";
            else
                lblHighScore.Text = savedHighScore.ToString();
        }
        public frm4SlidingPuzzle()
        {
            InitializeComponent();
        }

       private void UpdateBoard()
        {

            int[,] values = _game.GetValues();

            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    Button btn = _btnGrid[i, j];
                    int numberInBusiness = values[i, j];
                    btn.Tag = numberInBusiness;

                    if (numberInBusiness == 0)
                    {
                        btn.Text = "";
                        btn.Visible = false;
                        btn.Enabled = false;
                    }
                    else
                    {
                        btn.Text = numberInBusiness.ToString();
                        btn.Visible = true;
                        btn.Enabled = true;

                        // Colors
                        if (_game.IsInCorrectPosition(numberInBusiness, i, j))
                        {
                            btn.BackColor = Color.LightGreen; // في مكانه الصح
                            btn.ForeColor = Color.DarkGreen;
                        }
                        else
                        {
                            btn.BackColor = Color.WhiteSmoke;  // لسه مش في مكانه
                            btn.ForeColor = Color.Black;
                        }
                    }
                }
            }
        }


        
        private void frm4SlidingPuzzle_Load(object sender, EventArgs e)
        {
            _btnGrid = new Button[4, 4] {{btn1,btn2,btn3,btn4},
                                         {btn5,btn16,btn6,btn7},
                                         {btn8,btn9,btn10,btn11},
                                         {btn12,btn13,btn14,btn15},
                                        };
            int High = Properties.Settings.Default.HighScore;
            if (High == 0)
            {
                lblHighScore.Text = "No HighScore Yet";
            }
            else
            {
            lblHighScore.Text = High.ToString();
                
            }
            _game.Shuffle();
            UpdateBoard();
        }

        private void DidHeWin() 
        {
            if (_game.IsSolved())
            {
                timer1.Stop();

                int currentScore = 100000 - (_game.Moves * 200) - (Timer * 100);
                if(currentScore  < 0)currentScore = 0;
                int savedHighScore = Properties.Settings.Default.HighScore;
                if (currentScore > savedHighScore)
                {
                    Properties.Settings.Default.HighScore = currentScore;
                    Properties.Settings.Default.Save();
                    lblHighScore.Text = currentScore.ToString();
                    MessageBox.Show(
                        $"New High Score 🎉\n\n" +
                        $"Your New HighScore: {currentScore}\n" +
                        $"Your Time: {lblTime.Text} Seconds\n" +
                        $"Number Of Moves: {lblMove.Text}",
                        "Victory!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else 
                {
                    MessageBox.Show(
                        $"Congrats! You solved the puzzle! 🎉\n\n" +
                        $"Your Score: {currentScore}\n" +
                        $"Your Time: {lblTime.Text} Seconds\n" +
                        $"Number Of Moves: {lblMove.Text}",
                        "Victory!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
        }
        private void ClickButton(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (_game.PlayMove((int)btn.Tag))
            {
                lblMove.Text = _game.Moves.ToString();
                UpdateBoard();
                DidHeWin();
               
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Timer++;
            lblTime.Text= Timer.ToString();

        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            _game.Reset();
            Timer = 0;
            lblMove.Text = _game.Moves.ToString();
            UpdateBoard();
            timer1.Start();

        }

      
    }
}
