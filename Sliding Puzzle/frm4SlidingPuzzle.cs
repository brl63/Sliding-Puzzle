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

        private Image[] _imagePieces = new Image[16]; 
        private bool _isImageMode = false;           

        private bool _isFlashlightMode = false;

        private clsBus.Game _game = new clsBus.Game();

        private Button[,] _btnGrid;

        int Timer = 0;

        private void frm4SlidingPuzzle_Load(object sender, EventArgs e)
        {
            _btnGrid = new Button[4, 4]
            {
        { btn1,  btn2,  btn3,  btn4  },
        { btn5,  btn16,  btn6,  btn7  },
        { btn8,  btn9, btn10, btn11 },
        { btn12, btn13, btn14, btn15 }
            };

            foreach (Button btn in tblGame.Controls)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackgroundImageLayout = ImageLayout.Stretch;

                btn.Click += ClickButton;

                btn.MouseEnter += Btn_MouseEnter;
                btn.MouseLeave += Btn_MouseLeave;
            }

            _game.Reset();
            UpdateBoard();
        }
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
                        btn.Image = null;
                        btn.Visible = false;
                        btn.Enabled = false;
                    }
                    else
                    {
                        btn.Visible = true;
                        btn.Enabled = true;

                        // 1. أولاً: التشييك على مود الفلاش لايت (أعلى أولوية)
                        if (_isFlashlightMode)
                        {
                            btn.Image = null; // إخفاء الصورة في الضلمة
                            btn.Text = numberInBusiness.ToString();
                            btn.BackColor = Color.Black;
                            btn.ForeColor = Color.Black; // خط أسود عشان يختفي
                        }
                        // 2. ثانياً: التشييك على مود الصور
                        else if (_isImageMode)
                        {
                            btn.Text = "";
                            btn.Image = _imagePieces[numberInBusiness];
                            btn.ImageAlign = ContentAlignment.MiddleCenter;
                        }
                        // 3. ثالثاً: المود العادي (الأرقام والـ Heatmap)
                        else
                        {
                            btn.Image = null;
                            btn.Text = numberInBusiness.ToString();

                            if (_game.IsInCorrectPosition(numberInBusiness, i, j))
                            {
                                btn.BackColor = Color.LightGreen;
                                btn.ForeColor = Color.DarkGreen;
                            }
                            else
                            {
                                btn.BackColor = Color.WhiteSmoke;
                                btn.ForeColor = Color.Black;
                            }
                        }
                    }
                }
            }
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

        private void Btn_MouseEnter(object sender, EventArgs e)
        {
            if (_isFlashlightMode)
            {
                Button btn = (Button)sender;
                if ((int)btn.Tag != 0)
                {
                    btn.ForeColor = Color.White;
                    btn.BackColor = Color.DarkSlateGray; 
                }
            }
        }

        private void Btn_MouseLeave(object sender, EventArgs e)
        {
            if (_isFlashlightMode)
            {
                Button btn = (Button)sender;
                if ((int)btn.Tag != 0)
                {
                    btn.ForeColor = Color.Black; 
                    btn.BackColor = Color.Black;
                }
            }
        }

        private void chkFlashMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFlashMode.Checked) 
            {
               _isFlashlightMode = true;
                UpdateBoard();
            }
            else
            {
                _isFlashlightMode =false;
                UpdateBoard();
            }
        }

        private Image CropImage(Image sourceImage, int row, int col)
        {
            int pieceWidth = sourceImage.Width / 4;
            int pieceHeight = sourceImage.Height / 4;

            Rectangle cropArea = new Rectangle(col * pieceWidth, row * pieceHeight, pieceWidth, pieceHeight);

            Bitmap bmp = new Bitmap(sourceImage);
            Bitmap croppedPiece = bmp.Clone(cropArea, bmp.PixelFormat);

            return croppedPiece;
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Image loadedImg = Image.FromFile(openFileDialog.FileName);

                int totalWidth = btn1.Width * 4;   // _btnGrid[0,0].Width * 4
                int totalHeight = btn1.Height * 4; // _btnGrid[0,0].Height * 4

                Bitmap resizedImage = new Bitmap(loadedImg, new Size(totalWidth, totalHeight));

                int index = 1;
                for (int r = 0; r < 4; r++)
                {
                    for (int c = 0; c < 4; c++)
                    {
                        if (index <= 15)
                        {
                            _imagePieces[index] = CropImage(resizedImage, r, c);
                            index++;
                        }
                    }
                }

                _isImageMode = true;
                _game.Reset();
                UpdateBoard();
            }
        }

        private void btnRestMode_Click(object sender, EventArgs e)
        {
            _isImageMode = false;
            UpdateBoard();

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    }

//