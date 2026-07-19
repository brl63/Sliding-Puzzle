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
        int Moves = 0;
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
            _game.Shuffle();
            UpdateBoard();
        }

        private void DidHeWin() 
        {
            if (_game.IsSolved())
            {
                timer1.Stop();
                MessageBox.Show($"Congrat u won", $"Congrats u solved the puzzle\n Number of Moves : {lblMove.Text} \n Your Time : {lblTime.Text} Seconds", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            timer1.Start();

        }
    }
}
