using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;

namespace WindowsFormsApp1
{

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create an instance of your main form and run it
            Form1 mainForm = new Form1();
            Application.Run(mainForm);
        }
    }
    public partial class Form1 : Form
    {
        private const int BoardWidth = 14;
        private const int BoardHeight = 23;
        private const int CellSize = 20;
        private const int nextblocklimit = 4;
        private int[,] tempGameBoard = new int[BoardWidth, BoardHeight];
        private int[,] permanentGameBoard = new int[BoardWidth, BoardHeight];
        private int[,] nextblockscreen = new int[nextblocklimit, nextblocklimit];
        int Score = 0;
        int Highscore = 0;

        int FinalScore = 0;
        int FinalHighScore = 0;

        int Level = 0;
        int onelevelup = 0;
        int completedlines = 0;

        int nextblock = 0;
        int blocktype = 0;

        Label scoreboard = new Label();
        Label highscore = new Label();
        Label linecount = new Label();
        Label levelcount = new Label();
        bool gameover = false;

        Label finalhighscorep = new Label();
        Label finalscorep = new Label();

        Button startbutton = new Button();
        Button quitbutton = new Button();
        Button restartbutton = new Button();

        //Level Buttons
        Button level0button = new Button();
        Button level1button = new Button();
        Button level2button = new Button();
        Button level3button = new Button();
        Button level4button = new Button();
        Button level5button = new Button();
        Button level6button = new Button();
        Button level7button = new Button();
        Button level8button = new Button();
        Button level9button = new Button();

        PictureBox startscreenimage = new PictureBox();

        // BLOCKS
        // TBLOCK ORIENTATIONS
        private int[,] Tblock = new int[,]
        {
            { 0, 1, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        };
        private int[,] Tblock90 = new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 1 },
            { 0, 1, 0 }
        };
        private int[,] Tblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 0, 1, 0 }
        };
        private int[,] Tblock270 = new int[,]
        {
            { 0, 1, 0 },
            { 1, 1, 0 },
            { 0, 1, 0 }
        };
        // LBLOCK ORIENTATIONS
        private int[,] Lblock = new int[,]
        {
            { 0, 0, 1 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        };
        private int[,] Lblock90 = new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 1 }
        };
        private int[,] Lblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 1, 0, 0 }
        };
        private int[,] Lblock270 = new int[,]
        {
            { 1, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 0 }
        };
        // JBLOCK ORIENTATIONS
        private int[,] Jblock = new int[,]
        {
            { 1, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        };
        private int[,] Jblock90 = new int[,]
        {
            { 0, 1, 1 },
            { 0, 1, 0 },
            { 0, 1, 0 }
        };
        private int[,] Jblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 1 }
        };
        private int[,] Jblock270 = new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 1, 1, 0 }
        };
        // OBLOCK ORIENTATIONS
        private int[,] Oblock = new int[,]
        {
            { 1, 1 },
            { 1, 1 }
        };
        // IBLOCK ORIENTATIONS
        private int[,] Iblock = new int[,]
        {
            { 0, 0, 0, 0 },
            { 1, 1, 1, 1 },
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 }
        };
        private int[,] Iblock90 = new int[,]
        {
            { 0, 0, 1, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 1, 0 }
        };
        private int[,] Iblock180 = new int[,]
        {
            { 0, 0, 0, 0 },
            { 0, 0, 0, 0 },
            { 1, 1, 1, 1 },
            { 0, 0, 0, 0 }
        };
        private int[,] Iblock270 = new int[,]
        {
            { 0, 1, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 1, 0, 0 }
        };
        // SBLOCK ORIENTATIONS
        private int[,] Sblock = new int[,]
        {
            { 0, 1, 1 },
            { 1, 1, 0 },
            { 0, 0, 0 }
        };
        private int[,] Sblock90 = new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 1 },
            { 0, 0, 1 }
        };
        private int[,] Sblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 0, 1, 1 },
            { 1, 1, 0 }
        };
        private int[,] Sblock270 = new int[,]
        {
            { 1, 0, 0 },
            { 1, 1, 0 },
            { 0, 1, 0 }
        };
        // ZBLOCK ORIENTATIONS
        private int[,] Zblock = new int[,]
        {
            { 1, 1, 0 },
            { 0, 1, 1 },
            { 0, 0, 0 }
        };
        private int[,] Zblock90 = new int[,]
        {
            { 0, 0, 1 },
            { 0, 1, 1 },
            { 0, 1, 0 }
        };
        private int[,] Zblock180 = new int[,]
        {
            { 0, 0, 0 },
            { 1, 1, 0 },
            { 0, 1, 1 }
        };
        private int[,] Zblock270 = new int[,]
        {
            { 0, 1, 0 },
            { 1, 1, 0 },
            { 1, 0, 0 }
        };

        public Form1()
        {
            //start

            start();

        }
        private void start()
        {
            InitializeComponent();

            InitializeStartScreen();
            InitializeGameBoard();
            this.KeyDown += Form1_KeyDown;
        }
        private void levelselect()
        {
            int levelbuttonwidth = 100;
            int levelbuttonheight = 75;
            //Level0 Select
            this.Controls.Add(level0button);
            level0button.Text = "0";
            level0button.Location = new Point(10, 100);
            level0button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level0button.Click += level0button_Click;
            //Level1 Select
            this.Controls.Add(level1button);
            level1button.Text = "1";
            level1button.Location = new Point(110, 100);
            level1button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level1button.Click += level1button_Click;
            //Level2 Select
            this.Controls.Add(level2button);
            level2button.Text = "2";
            level2button.Location = new Point(210, 100);
            level2button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level2button.Click += level2button_Click;
            //Level3 Select
            this.Controls.Add(level3button);
            level3button.Text = "3";
            level3button.Location = new Point(310, 100);
            level3button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level3button.Click += level3button_Click;
            //Level4 Select
            this.Controls.Add(level4button);
            level4button.Text = "4";
            level4button.Location = new Point(410, 100);
            level4button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level4button.Click += level4button_Click;
            //Level5 Select
            this.Controls.Add(level5button);
            level5button.Text = "5";
            level5button.Location = new Point(10, 200);
            level5button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level5button.Click += level5button_Click;
            //Level6 Select
            this.Controls.Add(level6button);
            level6button.Text = "6";
            level6button.Location = new Point(110, 200);
            level6button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level6button.Click += level6button_Click;
            //Level7 Select
            this.Controls.Add(level7button);
            level7button.Text = "7";
            level7button.Location = new Point(210, 200);
            level7button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level7button.Click += level7button_Click;
            //Level8 Select
            this.Controls.Add(level8button);
            level8button.Text = "8";
            level8button.Location = new Point(310, 200);
            level8button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level8button.Click += level8button_Click;
            //Level9 Select
            this.Controls.Add(level9button);
            level9button.Text = "9";
            level9button.Location = new Point(410, 200);
            level9button.Size = new Size(levelbuttonwidth, levelbuttonheight);
            level9button.Click += level9button_Click;
        }
        private void gameplay()
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    permanentGameBoard[x, y] = 0;
                    tempGameBoard[x, y] = 0;
                }
            }

            // Initiate & Update

            gameover = false;
            InitializeScore();
            Initializehighscore();
            Updatehighscore();
            InitializeLinecount();
            UpdateLinecount();
            InitializeLevel();
            UpdateLevel();
            UpdateScore();
            initializenextblock();
            updatenextblock();
            drawnextblock();
            updatenextblock();

            timer1.Start();
            // beginning of game
            blockpicker();

            UpdateGameBoard();
            drawblock(startx, starty);
            UpdateGameBoard();

        }
        private void InitializeStartScreen()
        {
            //START BUTTON
            this.Controls.Add(startbutton);
            startbutton.Text = "Start";
            startbutton.Location = new Point(100, 200);
            startbutton.Size = new Size(150, 75);

            startbutton.Click += startbutton_Click;

            //QUIT BUTTON
            this.Controls.Add(quitbutton);
            quitbutton.Text = "quit";
            quitbutton.Location = new Point(300, 200);
            quitbutton.Size = new Size(150, 75);

            quitbutton.Click += quitbutton_Click;

            //Background Picture
            this.Controls.Add(startscreenimage);
            startscreenimage.Location = new Point(0, 0);
            startscreenimage.Size = new Size(500, 500);
            Controls.Add(startscreenimage);
            startscreenimage.SizeMode = PictureBoxSizeMode.StretchImage;

            startscreenimage.Image = Properties.Resources.Homescreen;

        }
        void startbutton_Click(object sender, EventArgs e)
        {
            this.Controls.Remove(startbutton);
            this.Controls.Remove(quitbutton);
            this.Controls.Remove(restartbutton);
            this.Controls.Remove(startscreenimage);


            levelselect();
        }
        void quitbutton_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
        }
        void level0button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 0;
            gameplay();
        }
        void level1button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 1;
            gameplay();
        }
        void level2button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 2;
            gameplay();
        }
        void level3button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 3;
            gameplay();
        }
        void level4button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 4;
            gameplay();
        }
        void level5button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 5;
            gameplay();
        }
        void level6button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 6;
            gameplay();
        }
        void level7button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 7;
            gameplay();
        }
        void level8button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 8;
            gameplay();
        }
        void level9button_Click(object sender, EventArgs e)
        {
            startscriptremove();
            Level = 9;
            gameplay();
        }
        private void startscriptremove()
        {
            this.Controls.Remove(startbutton);
            this.Controls.Remove(quitbutton);
            this.Controls.Remove(restartbutton);
            this.Controls.Remove(startscreenimage);

            this.Controls.Remove(level0button);
            this.Controls.Remove(level1button);
            this.Controls.Remove(level2button);
            this.Controls.Remove(level3button);
            this.Controls.Remove(level4button);
            this.Controls.Remove(level5button);
            this.Controls.Remove(level6button);
            this.Controls.Remove(level7button);
            this.Controls.Remove(level8button);
            this.Controls.Remove(level9button);

            ShowGameBoard();
        }


        private void ShowGameBoard()
        {
            // Set the visibility of each PictureBox in the game board to true
            foreach (Control control in this.Controls)
            {
                if (control is PictureBox)
                {
                    control.Visible = true;
                }
            }

        }
        private void HideGameBoard()
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    permanentGameBoard[x, y] = 0;
                    tempGameBoard[x, y] = 0;
                }
            }

            foreach (Control control in this.Controls)
            {
                if (control is PictureBox)
                {
                    control.Visible = false;
                }
            }
        }
        private void ShowGameOverScreen()
        {
            //Gameover Score
            Finalscorev();

            //Gameover Highscore
            Finalhighscorev();
            // Generate Quit Button
            this.Controls.Add(quitbutton);

            RestartButton();
        }
        private void RestartButton()
        {
            this.Controls.Add(restartbutton);
            restartbutton.Text = "Restart";
            restartbutton.Location = new Point(100, 200);
            restartbutton.Size = new Size(150, 75);

            restartbutton.Click += restartbutton_Click;
        }
        void restartbutton_Click(object sender, EventArgs e)
        {
            Score = 0;
            Highscore = 0;
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    permanentGameBoard[x, y] = 0;
                    tempGameBoard[x, y] = 0;
                }
            }
            this.Controls.Remove(quitbutton);
            this.Controls.Remove(restartbutton);
            this.Controls.Remove(finalhighscorep);
            this.Controls.Remove(finalscorep);
            Level = 0;
            completedlines = 0;
            levelselect();
        }
        private void Finalhighscorev()
        {
            //Gameover Highscore

            finalhighscorep.Visible = true;
            finalhighscorep.Location = new Point(150, 120);
            finalhighscorep.AutoSize = true;
            finalhighscorep.Font = new Font("Calibri", 25);
            finalhighscorep.ForeColor = Color.Black;
            finalhighscorep.Padding = new Padding(6);
            this.Controls.Add(finalhighscorep);
            string Endhighscore = FinalHighScore.ToString();
            finalhighscorep.Text = String.Format($"Highscore: {Endhighscore}");
            finalhighscorep.Refresh();

        }
        private void Finalscorev()
        {
            //Gameover Score

            finalscorep.Visible = true;
            finalscorep.Location = new Point(150, 60);
            finalscorep.AutoSize = true;
            finalscorep.Font = new Font("Calibri", 25);
            finalscorep.ForeColor = Color.Black;
            finalscorep.Padding = new Padding(6);
            this.Controls.Add(finalscorep);
            string Endscore = FinalScore.ToString();
            finalscorep.Text = String.Format($"Score: {Endscore}");
            finalscorep.Refresh();

        }
        private void InitializeGameOver()
        {
            HideGameBoard();
            timer1.Stop();
            this.Controls.Remove(scoreboard);
            this.Controls.Remove(highscore);
            this.Controls.Remove(linecount);
            this.Controls.Remove(levelcount);
            if (Highscore >= FinalHighScore)
            {
                FinalHighScore = Highscore;
            }

            FinalScore = Score;
            ShowGameOverScreen();

        }
        private void InitializeGameBoard()
        {
            for (int y = 0; y < BoardHeight; y++)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    PictureBox pictureBox1 = new PictureBox();
                    pictureBox1.Width = CellSize;
                    pictureBox1.Height = CellSize;
                    pictureBox1.Left = 80 + x * CellSize;
                    pictureBox1.Top = y * CellSize;
                    pictureBox1.BorderStyle = BorderStyle.FixedSingle;
                    System.Drawing.Color color = GetCombinedColor(x, y);
                    pictureBox1.BackColor = color;
                    this.Controls.Add(pictureBox1);
                    this.DoubleBuffered = true;
                    pictureBox1.Visible = false;
                }
            }
        }
        private void InitializeScore()
        {

            scoreboard.Location = new Point(370, 120);
            scoreboard.AutoSize = true;
            scoreboard.Font = new Font("Calibri", 18);
            scoreboard.ForeColor = Color.Black;
            scoreboard.Padding = new Padding(6);
            this.Controls.Add(scoreboard);
            scoreboard.Refresh();

        }
        private void UpdateScore()
        {
            string finalscore = Score.ToString();
            scoreboard.Text = String.Format($"Score {finalscore}");
            if (Score > Highscore)
            {
                Updatehighscore();
            }
        }
        private void Initializehighscore()
        {
            highscore.Location = new Point(370, 50);
            highscore.AutoSize = true;
            highscore.Font = new Font("Calibri", 18);
            highscore.ForeColor = Color.Black;
            highscore.Padding = new Padding(6);
            this.Controls.Add(highscore);
            highscore.Refresh();
        }
        private void Updatehighscore()
        {
            Highscore = Score;
            string finalhighscore = Highscore.ToString();
            highscore.Text = String.Format($"Highscore: {finalhighscore}");
        }
        private void InitializeLevel()
        {

            levelcount.Location = new Point(370, 180);
            levelcount.AutoSize = true;
            levelcount.Font = new Font("Calibri", 18);
            levelcount.ForeColor = Color.Black;
            levelcount.Padding = new Padding(6);
            this.Controls.Add(levelcount);
            levelcount.Refresh();
            levelcount.Visible = true;

        }
        private void UpdateLevel()
        {
            string Levelachieved = Level.ToString();
            levelcount.Text = String.Format($"Level {Levelachieved}");
        }
        private void InitializeLinecount()
        {
            //completedlines
            linecount.Location = new Point(370, 0);
            linecount.AutoSize = true;
            linecount.Font = new Font("Calibri", 18);
            linecount.ForeColor = Color.Black;
            linecount.Padding = new Padding(6);
            this.Controls.Add(linecount);
            linecount.Refresh();
        }
        private void UpdateLevelcount()
        {
            if (onelevelup >= 10)
            {
                Level += 1;
                onelevelup = 0;
            }

            UpdateTimerInterval();
            UpdateLevel();
        }
        private void UpdateTimerInterval()
        {
            if (timer1.Interval > 0)
            {
                //Choose Speed
                timer1.Interval = 1000 - (Level * 100);
            }
        }
        private void UpdateLinecount()
        {
            string Linecounter = completedlines.ToString();
            linecount.Text = String.Format($"Lines: {Linecounter}");
        }
        private System.Drawing.Color GetCombinedColor(int x, int y)
        {
            if (tempGameBoard[x, y] == 1 || permanentGameBoard[x, y] == 1)
                return System.Drawing.Color.Black;
            if (tempGameBoard[x, y] == 2 || permanentGameBoard[x, y] == 2)
                return System.Drawing.Color.Yellow;
            if (tempGameBoard[x, y] == 3 || permanentGameBoard[x, y] == 3)
                return System.Drawing.Color.Blue;
            //Edge Pieces
            if (tempGameBoard[x, y] == 8 || permanentGameBoard[x, y] == 8)
                return System.Drawing.Color.Red;
            else
                return System.Drawing.Color.White;
        }
        private System.Drawing.Color GetCombinedColorNext(int x, int y)
        {
            // Retrieve color based on nextblockscreen value at given position
            if (nextblockscreen[x, y] == 1)
                return System.Drawing.Color.Black;
            if (nextblockscreen[x, y] == 2)
                return System.Drawing.Color.Yellow;
            if (nextblockscreen[x, y] == 3)
                return System.Drawing.Color.Blue;
            if (nextblockscreen[x, y] == 8)
                return System.Drawing.Color.Red;
            else
                return System.Drawing.Color.White;
        }
        private void UpdateGameBoard()
        {
            for (int y = 0; y < BoardHeight; y++)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    Control control = this.Controls[y * BoardWidth + x];
                    if (control is PictureBox pictureBox1)
                    {
                        System.Drawing.Color color = GetCombinedColor(x, y);
                        pictureBox1.BackColor = color;
                    }
                }
            }
            UpdateLevelcount();
        }
        // Example usage
        private void Form1_Load(object sender, EventArgs e)
        {

        }


        int startx = 5;
        int starty = 0;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int blockSizeX = 0;
            int blockSizeY = 0;
            // Calculate the maximum x and y values for the current block
            int maxX = BoardWidth - blockSizeX;
            int maxY = BoardHeight - blockSizeY;
            int minX = BoardWidth - blockSizeX;
            if (gameover == false)
            {
                if (chosenblock == Tblock)
                {
                    switch (rotation)
                    {
                        case 2:
                            //90
                            blockSizeX = Tblock90.GetLength(1);
                            blockSizeY = Tblock90.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 1 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            //180
                            blockSizeX = Tblock180.GetLength(1);
                            blockSizeY = Tblock180.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            //270
                            blockSizeX = Tblock270.GetLength(1);
                            blockSizeY = Tblock270.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 4 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        default:
                            //0
                            blockSizeX = Tblock.GetLength(1);
                            blockSizeY = Tblock.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                else if (chosenblock == Lblock)
                {
                    switch (rotation)
                    {
                        case 2:
                            //90
                            blockSizeX = Lblock90.GetLength(1);
                            blockSizeY = Lblock90.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 1 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            //180
                            blockSizeX = Lblock180.GetLength(1);
                            blockSizeY = Lblock180.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            //270
                            blockSizeX = Lblock270.GetLength(1);
                            blockSizeY = Lblock270.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 4 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        default:
                            //0
                            blockSizeX = Lblock.GetLength(1);
                            blockSizeY = Lblock.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                else if (chosenblock == Jblock)
                {
                    switch (rotation)
                    {
                        case 2:
                            //90
                            blockSizeX = Jblock90.GetLength(1);
                            blockSizeY = Jblock90.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 1 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            //180
                            blockSizeX = Jblock180.GetLength(1);
                            blockSizeY = Jblock180.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            //270
                            blockSizeX = Jblock270.GetLength(1);
                            blockSizeY = Jblock270.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 4 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        default:
                            //0
                            blockSizeX = Jblock.GetLength(1);
                            blockSizeY = Jblock.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                else if (chosenblock == Iblock)
                {
                    switch (rotation)
                    {
                        case 2:
                            //90
                            blockSizeX = Iblock90.GetLength(1);
                            blockSizeY = Iblock90.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 0 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            //180
                            blockSizeX = Iblock180.GetLength(1);
                            blockSizeY = Iblock180.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 6 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            //270
                            blockSizeX = Iblock270.GetLength(1);
                            blockSizeY = Iblock270.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 4 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 1 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        default:
                            //0
                            blockSizeX = Iblock.GetLength(1);
                            blockSizeY = Iblock.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 6 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                else if (chosenblock == Oblock)
                {
                    blockSizeX = Oblock.GetLength(1);
                    blockSizeY = Oblock.GetLength(0);

                    switch (e.KeyCode)
                    {
                        case Keys.Right:
                            if (startx < maxX - 4 && AllowMoveRight == true)
                            {
                                clearblock();
                                startx += 1;
                                drawblock(startx, starty);
                                placeblockcheck();
                                UpdateGameBoard();
                            }
                            break;
                        case Keys.Left:
                            if (startx > 2 && AllowMoveLeft == true)
                            {
                                clearblock();
                                startx -= 1;
                                drawblock(startx, starty);
                                placeblockcheck();
                                UpdateGameBoard();
                            }
                            break;
                        case Keys.Down:
                            if (starty < BoardHeight - 3)
                            {
                                placeblockcheck();
                                clearblock();
                                starty += 1;
                                drawblock(startx, starty);
                                placeblockcheck();
                                UpdateGameBoard();
                            }
                            break;
                    }
                }
                if (chosenblock == Sblock)
                {
                    switch (rotation)
                    {
                        case 2:
                            //90
                            blockSizeX = Sblock90.GetLength(1);
                            blockSizeY = Sblock90.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 1 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            //180
                            blockSizeX = Sblock180.GetLength(1);
                            blockSizeY = Sblock180.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            //270
                            blockSizeX = Sblock270.GetLength(1);
                            blockSizeY = Sblock270.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 4 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        default:
                            //0
                            blockSizeX = Sblock.GetLength(1);
                            blockSizeY = Sblock.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                if (chosenblock == Zblock)
                {
                    switch (rotation)
                    {
                        case 2:
                            //90
                            blockSizeX = Zblock90.GetLength(1);
                            blockSizeY = Zblock90.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 1 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            //180
                            blockSizeX = Zblock180.GetLength(1);
                            blockSizeY = Zblock180.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            //270
                            blockSizeX = Zblock270.GetLength(1);
                            blockSizeY = Zblock270.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 4 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                        default:
                            //0
                            blockSizeX = Zblock.GetLength(1);
                            blockSizeY = Zblock.GetLength(0);

                            switch (e.KeyCode)
                            {
                                case Keys.Right:
                                    if (startx < maxX - 5 && AllowMoveRight == true)
                                    {
                                        clearblock();
                                        startx += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Left:
                                    if (startx > 2 && AllowMoveLeft == true)
                                    {
                                        clearblock();
                                        startx -= 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                                case Keys.Down:
                                    if (starty < BoardHeight - 3)
                                    {
                                        placeblockcheck();
                                        clearblock();
                                        starty += 1;
                                        drawblock(startx, starty);
                                        placeblockcheck();
                                        UpdateGameBoard();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                switch (e.KeyCode)
                {
                    case Keys.Y:
                        if (starty < maxY)
                        {
                            clearblock();
                            rotation += 1;
                            Rotate();
                            clearblock();
                            drawblock(startx, starty);
                            UpdateGameBoard();
                            clearblock();
                        }
                        break;
                    case Keys.X:
                        if (starty < maxY)
                        {
                            clearblock();
                            rotation -= 1;
                            Rotate();
                            clearblock();
                            drawblock(startx, starty);
                            UpdateGameBoard();
                            clearblock();
                        }
                        break;
                }
            }
            CheckCollision();
        }
        bool AllowMoveLeft = true;
        bool AllowMoveRight = true;
        private void CheckCollision()
        {
            if (chosenblock == Oblock)
            {
                for (int y = starty; y <= starty + 1; y++)
                {
                    for (int xleft = startx - 1; xleft <= startx - 1; xleft++)
                    {
                        for (int xright = startx + 2; xright <= startx + 2; xright++)
                        {
                            if (permanentGameBoard[xright, y] == 1 && permanentGameBoard[xright, y] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            if (permanentGameBoard[xright, y] != 1 && permanentGameBoard[xright, y] == 0)
                            {
                                AllowMoveRight = true;
                            }
                            if (permanentGameBoard[xleft, y] == 1 && permanentGameBoard[xleft, y] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            if (permanentGameBoard[xleft, y] != 1 && permanentGameBoard[xleft, y] == 0)
                            {
                                AllowMoveLeft = true;
                            }
                        }
                    }
                }
            }
            else if (chosenblock == Iblock)
            {
                switch (rotation)
                {
                    case 2:
                        {//90
                            for (int y = starty; y <= starty + 3; y++)
                            {
                                for (int xleft = startx + 1; xleft <= startx + 1; xleft++)
                                {
                                    for (int xright = startx + 3; xright <= startx + 3; xright++)
                                    {
                                        if (permanentGameBoard[xright, y] == 1 && permanentGameBoard[xright, y] != 0)//BLock Right movement if collision
                                        {
                                            AllowMoveRight = false;
                                        }
                                        if (permanentGameBoard[xright, y] != 1 && permanentGameBoard[xright, y] == 0)
                                        {
                                            AllowMoveRight = true;
                                        }
                                        if (permanentGameBoard[xleft, y] == 1 && permanentGameBoard[xleft, y] != 0)//BLock Left movement if collision
                                        {
                                            AllowMoveLeft = false;
                                        }
                                        if (permanentGameBoard[xleft, y] != 1 && permanentGameBoard[xleft, y] == 0)
                                        {
                                            AllowMoveLeft = true;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 3:
                        {//180
                            for (int y = starty + 2; y <= starty + 2; y++)
                            {
                                for (int xleft = startx - 1; xleft <= startx - 1; xleft++)
                                {
                                    for (int xright = startx + 4; xright <= startx + 4; xright++)
                                    {
                                        if (permanentGameBoard[xright, y] == 1 && permanentGameBoard[xright, y] != 0)//BLock Right movement if collision
                                        {
                                            AllowMoveRight = false;
                                        }
                                        if (permanentGameBoard[xright, y] != 1 && permanentGameBoard[xright, y] == 0)
                                        {
                                            AllowMoveRight = true;
                                        }
                                        if (permanentGameBoard[xleft, y] == 1 && permanentGameBoard[xleft, y] != 0)//BLock Left movement if collision
                                        {
                                            AllowMoveLeft = false;
                                        }
                                        if (permanentGameBoard[xleft, y] != 1 && permanentGameBoard[xleft, y] == 0)
                                        {
                                            AllowMoveLeft = true;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 4:
                        {//270
                            for (int y = starty; y <= starty + 3; y++)
                            {
                                for (int xleft = startx; xleft <= startx; xleft++)
                                {
                                    for (int xright = startx + 2; xright <= startx + 2; xright++)
                                    {
                                        if (permanentGameBoard[xright, y] == 1 && permanentGameBoard[xright, y] != 0)//BLock Right movement if collision
                                        {
                                            AllowMoveRight = false;
                                        }
                                        if (permanentGameBoard[xright, y] != 1 && permanentGameBoard[xright, y] == 0)
                                        {
                                            AllowMoveRight = true;
                                        }
                                        if (permanentGameBoard[xleft, y] == 1 && permanentGameBoard[xleft, y] != 0)//BLock Left movement if collision
                                        {
                                            AllowMoveLeft = false;
                                        }
                                        if (permanentGameBoard[xleft, y] != 1 && permanentGameBoard[xleft, y] == 0)
                                        {
                                            AllowMoveLeft = true;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        {// 0
                            for (int y = starty + 1; y <= starty + 1; y++)
                            {
                                for (int xleft = startx - 1; xleft <= startx - 1; xleft++)
                                {
                                    for (int xright = startx + 4; xright <= startx + 4; xright++)
                                    {
                                        if (permanentGameBoard[xright, y] == 1 && permanentGameBoard[xright, y] != 0)//BLock Right movement if collision
                                        {
                                            AllowMoveRight = false;
                                        }
                                        if (permanentGameBoard[xright, y] != 1 && permanentGameBoard[xright, y] == 0)
                                        {
                                            AllowMoveRight = true;
                                        }
                                        if (permanentGameBoard[xleft, y] == 1 && permanentGameBoard[xleft, y] != 0)//BLock Left movement if collision
                                        {
                                            AllowMoveLeft = false;
                                        }
                                        if (permanentGameBoard[xleft, y] != 1 && permanentGameBoard[xleft, y] == 0)
                                        {
                                            AllowMoveLeft = true;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            else if (chosenblock == Tblock)
            {
                switch (rotation)
                {
                    case 2:
                        {//90
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 3:
                        {//180
                            if (permanentGameBoard[startx + 3, starty + 1] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty + 2] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 4:
                        {//270
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    default:
                        {// 0
                            if (permanentGameBoard[startx + 3, starty + 1] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                }
            }
            else if (chosenblock == Lblock)
            {
                switch (rotation)
                {
                    case 2:
                        {//90
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 3:
                        {//180
                            if (permanentGameBoard[startx + 3, starty + 1] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 1, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }

                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx - 1, starty + 1] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 4:
                        {//270
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx - 1, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    default:
                        {// 0
                            if (permanentGameBoard[startx + 3, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx + 1, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                }
            }
            else if (chosenblock == Jblock)
            {
                switch (rotation)
                {
                    case 2:
                        {//90
                            if (permanentGameBoard[startx + 3, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 3:
                        {//180
                            if (permanentGameBoard[startx + 3, starty + 1] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx - 1, starty + 1] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx + 1, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 4:
                        {//270
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    default:
                        {// 0
                            if (permanentGameBoard[startx + 1, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx - 1, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                }
            }
            else if (chosenblock == Sblock)
            {
                switch (rotation)
                {
                    case 2:
                        {//90
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx + 1, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 3:
                        {//180
                            if (permanentGameBoard[startx + 3, starty + 1] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty + 1] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 4:
                        {//270
                            if (permanentGameBoard[startx + 1, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx - 1, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    default:
                        {// 0
                            if (permanentGameBoard[startx + 3, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                }
            }
            else if (chosenblock == Zblock)
            {
                switch (rotation)
                {
                    case 2:
                        {//90
                            if (permanentGameBoard[startx + 3, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx + 1, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 3:
                        {//180
                            if (permanentGameBoard[startx + 2, starty + 1] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx - 1, starty + 1] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    case 4:
                        {//270
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 2, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 1, starty + 2] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx - 1, starty + 2] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                    default:
                        {// 0
                            if (permanentGameBoard[startx + 2, starty] != 0)//BLock Right movement if collision
                            {
                                AllowMoveRight = false;
                            }
                            else if (permanentGameBoard[startx + 3, starty + 1] != 0)
                            {
                                AllowMoveRight = false;
                            }
                            else
                            {
                                AllowMoveRight = true;
                            }

                            if (permanentGameBoard[startx - 1, starty] != 0)//BLock Left movement if collision
                            {
                                AllowMoveLeft = false;
                            }
                            else if (permanentGameBoard[startx, starty + 1] != 0)
                            {
                                AllowMoveLeft = false;
                            }
                            else
                            {
                                AllowMoveLeft = true;
                            }
                            break;
                        }
                }
            }
        }

        int rotation = 5;
        private void Rotate()
        {
            if (rotation > 5)
            {
                rotation = 2;
            }
            else if (rotation < 2)
            {
                rotation = 5;
            }
            switch (rotation)
            {
                case 2:
                    // 90
                    Console.WriteLine("90");
                    break;
                case 3:
                    // 180
                    Console.WriteLine("180");
                    break;
                case 4:
                    // 270
                    Console.WriteLine("270");
                    break;
                default:
                    // 0
                    Console.WriteLine("0");
                    break;
            }
        }

        object chosenblock = null;

        public void blockpicker()
        {
            clearnextblock();
            Random rnd = new Random();
            if (blocktype == 0 && nextblock == 0)
            {
                nextblock = rnd.Next(1, 8);
                blocktype = rnd.Next(1, 8);
                Console.WriteLine("set first Value to Blockpicker");
            }
            else
            {
                blocktype = nextblock;
                nextblock = rnd.Next(1, 8);
                Console.WriteLine("Convert and set new Value to Blockpicker");
            }

            updatenextblock();
            // Block type Lock:
            //int  blocktype = 7;

            switch (blocktype)
            {
                case 1:
                    chosenblock = Tblock;
                    Console.WriteLine("Chosenblock = Tblock");
                    break;
                case 2:
                    chosenblock = Lblock;
                    Console.WriteLine("Chosenblock = Lblock");
                    break;
                case 3:
                    chosenblock = Jblock;
                    Console.WriteLine("Chosenblock = Jblock");
                    break;
                case 4:
                    chosenblock = Iblock;
                    Console.WriteLine("Chosenblock = Iblock");
                    break;
                case 5:
                    chosenblock = Oblock;
                    Console.WriteLine("Chosenblock = Oblock");
                    break;
                case 6:
                    chosenblock = Sblock;
                    Console.WriteLine("Chosenblock = Sblock");
                    break;
                case 7:
                    chosenblock = Zblock;
                    Console.WriteLine("Chosenblock = Zblock");
                    break;
            }
            Debug.WriteLine($"The BLocks are {blocktype} and {nextblock}");
            drawnextblock();
        }

        private void nextBlockPreview()
        {
            
        }

        private void initializenextblock()
        {
            for (int y = 0; y < nextblocklimit; y++)
            {
                for (int x = 0; x < nextblocklimit; x++)
                {
                    PictureBox nextblockscreen1 = new PictureBox();
                    nextblockscreen1.Width = CellSize;
                    nextblockscreen1.Height = CellSize;
                    nextblockscreen1.Left = 400 + x * CellSize;
                    nextblockscreen1.Top = 300 + y * CellSize;
                    nextblockscreen1.BorderStyle = BorderStyle.FixedSingle;
                    this.Controls.Add(nextblockscreen1);
                    nextblockscreen1.BackColor = GetCombinedColorNext(x, y); // Set background color
                }
            }
        }
        private void updatenextblock()
        {
            for (int y = 0; y < nextblocklimit; y++)
            {
                for (int x = 0; x < nextblocklimit; x++)
                {
                    Control control = this.Controls[y * nextblocklimit + x];
                    if (control is PictureBox nextblockscreen1)
                    {
                        System.Drawing.Color color = GetCombinedColor(x, y);
                        nextblockscreen1.BackColor = color;
                    }
                }
            }
        }
        private void clearnextblock()
        {
            // Clear the next block screen
            for (int y = 0; y < nextblocklimit; y++)
            {
                for (int x = 0; x < nextblocklimit; x++)
                {
                    nextblockscreen[x, y] = 0; // Clear all cells
                }
            }
            Debug.WriteLine($"Cleared Next BLock");
        }
        private void drawnextblock()
        {
            Debug.WriteLine("Nextblock: " + nextblock);
            if(nextblock == 1)
            {
                for (int y = 0; y < Tblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Tblock.GetLength(1); x++)
                    {
                        if (Tblock[x, y] == 1)
                        {
                            
                            int screenX = 1 + x;
                            int screenY = 0 + y;

                            nextblockscreen[screenY, screenX] = 1;
                        }
                    }
                }
            }
            else if(nextblock == 2)
            {
                for (int y = 0; y < Lblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Lblock.GetLength(1); x++)
                    {
                        if (Lblock[y, x] == 1)
                        {
                            int screenX = 0 + x;
                            int screenY = 1 + y;

                            if (screenX >= 0 && screenX < nextblocklimit && screenY >= 0 && screenY < nextblocklimit)
                            {
                                nextblockscreen[screenX, screenY] = 1;
                            }
                        }
                    }
                }
            }
            else if (nextblock == 3)
            {
                for (int y = 0; y < Jblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Jblock.GetLength(1); x++)
                    {
                        if (Jblock[y, x] == 1)
                        {
                            int screenX = 0 + x;
                            int screenY = 1 + y;

                            if (screenX >= 0 && screenX < nextblocklimit && screenY >= 0 && screenY < nextblocklimit)
                            {
                                nextblockscreen[screenX, screenY] = 1;
                            }
                        }
                    }
                }
            }
            else if (nextblock == 4)
            {
                for (int y = 0; y < Iblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Iblock.GetLength(1); x++)
                    {
                        if (Iblock[y, x] == 1)
                        {
                            int screenX = 0 + x;
                            int screenY = 0 + y;

                            if (screenX >= 0 && screenX < nextblocklimit && screenY >= 0 && screenY < nextblocklimit)
                            {
                                nextblockscreen[screenX, screenY] = 1;
                            }
                        }
                    }
                }
            }
            else if (nextblock == 5)
            {
                for (int y = 0; y < Oblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Oblock.GetLength(1); x++)
                    {
                        if (Oblock[y, x] == 1)
                        {
                            int screenX = 1 + x;
                            int screenY = 1 + y;

                            if (screenX >= 0 && screenX < nextblocklimit && screenY >= 0 && screenY < nextblocklimit)
                            {
                                nextblockscreen[screenX, screenY] = 1; 
                            }
                        }
                    }
                }
            }
            else if(nextblock == 6)
            {
                for (int y = 0; y < Sblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Sblock.GetLength(1); x++)
                    {
                        if (Sblock[y, x] == 1)
                        {
                            int screenX = 0 + x;
                            int screenY = 1 + y;

                            if (screenX >= 0 && screenX < nextblocklimit && screenY >= 0 && screenY < nextblocklimit)
                            {
                                nextblockscreen[screenX, screenY] = 1;
                            }
                        }
                    }
                }
            }
            else if(nextblock == 7)
            {
                for (int y = 0; y < Zblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Zblock.GetLength(1); x++)
                    {
                        if (Zblock[y, x] == 1)
                        {
                            int screenX = 0 + x; 
                            int screenY = 1 + y; 

                            if (screenX >= 0 && screenX < nextblocklimit && screenY >= 0 && screenY < nextblocklimit)
                            {
                                nextblockscreen[screenX, screenY] = 1;
                            }
                        }
                    }
                }
            }

            updatenextblock();
        }
        private void gameovercheck()
        {

            for (int x = 0; x < BoardWidth; x++)
            {
                if (permanentGameBoard[x, 3] == 1)
                {
                    gameover = true;
                }
            }
            if(gameover)
            {
                InitializeGameOver();
            }
        }
        private void placeblockcheck()
        {
            gameovercheck();

            if (chosenblock == Tblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        if(starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 3:
                        //180
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 4:
                        //270
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    default:
                        //0
                        if (starty >= BoardHeight - 3)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;

                }
            }
            if (chosenblock == Lblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 3:
                        //180
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 4:
                        //270
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    default:
                        //0
                        if (starty >= BoardHeight - 3)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;

                }
            }
            if (chosenblock == Jblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 3:
                        //180
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 4:
                        //270
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    default:
                        //0
                        if (starty >= BoardHeight - 3)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;

                }
            }
            if (chosenblock == Iblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        if (starty >= BoardHeight - 5)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 3:
                        //180
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 4:
                        //270
                        if (starty >= BoardHeight - 5)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    default:
                        //0
                        if (starty >= BoardHeight - 3)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;

                }
            }
            if(chosenblock == Oblock)
            {
                if (starty >= BoardHeight - 3)
                {
                    placeblock();
                    UpdateScore();
                    return;
                }
            }
            if (chosenblock == Sblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 3:
                        //180
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 4:
                        //270
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    default:
                        //0
                        if (starty >= BoardHeight - 3)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;

                }
            }
            if (chosenblock == Zblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 3:
                        //180
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    case 4:
                        //270
                        if (starty >= BoardHeight - 4)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;
                    default:
                        //0
                        if (starty >= BoardHeight - 3)
                        {
                            placeblock();
                            UpdateScore();
                            return;
                        }
                        break;

                }
            }


            bool isBlocked = false;

            // Check if any part of the block would hit a permanent block or the bottom of the game board
            if (chosenblock == Oblock)
            {
                switch (rotation)
                {
                    default: // -- works
                        for (int y = starty + 1; y <= starty + 2; y++)
                        {
                            for (int x = startx; x <= startx + 1; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Lblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0 || permanentGameBoard[startx + 2, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 3:
                        //180 -- works
                        if (permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 4:
                        //270 -- works
                        if (permanentGameBoard[startx, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    default:
                        //0 -- works
                        if (permanentGameBoard[startx + 2, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                }
            }
            if (chosenblock == Jblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 2, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 3:
                        //180 -- works
                        if (permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 4:
                        //270 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx, starty + 3] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    default:
                        //0 -- works
                        if (permanentGameBoard[startx, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                }
            }
            if (chosenblock == Iblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90 -- Works
                        for (int y = starty; y <= starty + 4; y++)
                        {
                            for (int x = startx + 2; x <= startx + 2; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case 3:
                        //180 -- works
                        for (int y = starty + 1; y <= starty + 3; y++)
                        {
                            for (int x = startx; x <= startx + 3; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case 4:
                        //270 -- works
                        for (int y = starty; y <= starty + 4; y++)
                        {
                            for (int x = startx + 1; x <= startx + 1; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                    default:
                        //0 -- works
                        for (int y = starty + 1; y <= starty + 2; y++)
                        {
                            for (int x = startx; x <= startx + 3; x++)
                            {
                                if (permanentGameBoard[x, y] != 0)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
            else if(chosenblock == Tblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 3:
                        //180 -- works
                        if (permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 4:
                        //270 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    default:
                        //0 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                }
            }
            if (chosenblock == Sblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 3:
                        //180 -- works
                        if (permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx, starty + 3] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 4:
                        //270 -- works
                        if (permanentGameBoard[startx, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    default:
                        //0 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 2, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                }
            }
            if (chosenblock == Zblock)
            {
                switch (rotation)
                {
                    case 2:
                        //90 -- works
                        if (permanentGameBoard[startx + 2, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 3:
                        //180 -- works
                        if (permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 3] != 0 || permanentGameBoard[startx + 2, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    case 4:
                        //270 -- works
                        if (permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx, starty + 2] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx, starty + 3] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                    default:
                        //0 -- works
                        if (permanentGameBoard[startx , starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 1] != 0 || permanentGameBoard[startx + 1, starty + 2] != 0 || permanentGameBoard[startx + 2, starty + 2] != 0)
                        {
                            isBlocked = true;
                            break;
                        }
                        break;
                }
            }
            if (isBlocked)
            {
                placeblock();
                
            }
            UpdateScore();


        }


        private void drawblock(int startX, int startY)
        {
            if (chosenblock == Tblock)
            {
                switch (rotation)
                {
                    case 2:
                        // 90
                        for (int y = 0; y < Tblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock90.GetLength(1); x++)
                            {
                                if (Tblock90[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        // 180
                        for (int y = 0; y < Tblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock180.GetLength(1); x++)
                            {
                                if (Tblock180[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        // 270
                        for (int y = 0; y < Tblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock270.GetLength(1); x++)
                            {
                                if (Tblock270[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        // 5
                        for (int y = 0; y < Tblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock.GetLength(1); x++)
                            {
                                if (Tblock[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Lblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Lblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock90.GetLength(1); x++)
                            {
                                if (Lblock90[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Lblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock180.GetLength(1); x++)
                            {
                                if (Lblock180[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Lblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock270.GetLength(1); x++)
                            {
                                if (Lblock270[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Lblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock.GetLength(1); x++)
                            {
                                if (Lblock[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Jblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Jblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock90.GetLength(1); x++)
                            {
                                if (Jblock90[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Jblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock180.GetLength(1); x++)
                            {
                                if (Jblock180[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Jblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock270.GetLength(1); x++)
                            {
                                if (Jblock270[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Jblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock.GetLength(1); x++)
                            {
                                if (Jblock[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Iblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Iblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock90.GetLength(1); x++)
                            {
                                if (Iblock90[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Iblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock180.GetLength(1); x++)
                            {
                                if (Iblock180[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Iblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock270.GetLength(1); x++)
                            {
                                if (Iblock270[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Iblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock.GetLength(1); x++)
                            {
                                if (Iblock[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Oblock)
            {
                for (int y = 0; y < Oblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Oblock.GetLength(1); x++)
                    {
                        if (Oblock[y, x] == 1)
                        {
                            int fieldX = startX + x;
                            int fieldY = startY + y;
                            if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                            {
                                tempGameBoard[fieldX, fieldY] = 1;
                            }
                        }
                    }
                }
            }
            if (chosenblock == Sblock)
            {
                switch (rotation)
                {
                    case 2:
                        // 90
                        for (int y = 0; y < Sblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock90.GetLength(1); x++)
                            {
                                if (Sblock90[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        // 180
                        for (int y = 0; y < Sblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock180.GetLength(1); x++)
                            {
                                if (Sblock180[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        // 270
                        for (int y = 0; y < Sblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock270.GetLength(1); x++)
                            {
                                if (Sblock270[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        // 0
                        for (int y = 0; y < Sblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock.GetLength(1); x++)
                            {
                                if (Sblock[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Zblock)
            {
                switch (rotation)
                {
                    case 2:
                        // 90
                        for (int y = 0; y < Zblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock90.GetLength(1); x++)
                            {
                                if (Zblock90[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        // 180
                        for (int y = 0; y < Zblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock180.GetLength(1); x++)
                            {
                                if (Zblock180[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        // 270
                        for (int y = 0; y < Zblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock270.GetLength(1); x++)
                            {
                                if (Zblock270[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        // 5
                        for (int y = 0; y < Zblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock.GetLength(1); x++)
                            {
                                if (Zblock[y, x] == 1)
                                {
                                    int fieldX = startX + x;
                                    int fieldY = startY + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 1;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void clearblock()
        {
            if (chosenblock == Tblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Tblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock90.GetLength(1); x++)
                            {
                                if (Tblock90[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Tblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock180.GetLength(1); x++)
                            {
                                if (Tblock180[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Tblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock270.GetLength(1); x++)
                            {
                                if (Tblock270[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Tblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Tblock.GetLength(1); x++)
                            {
                                if (Tblock[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Lblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Lblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock90.GetLength(1); x++)
                            {
                                if (Lblock90[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Lblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock180.GetLength(1); x++)
                            {
                                if (Lblock180[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Lblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock270.GetLength(1); x++)
                            {
                                if (Lblock270[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Lblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Lblock.GetLength(1); x++)
                            {
                                if (Lblock[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Jblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Jblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock90.GetLength(1); x++)
                            {
                                if (Jblock90[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Jblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock180.GetLength(1); x++)
                            {
                                if (Jblock180[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Jblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock270.GetLength(1); x++)
                            {
                                if (Jblock270[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Jblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Jblock.GetLength(1); x++)
                            {
                                if (Jblock[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Iblock)
            {
                switch (rotation)
                {
                    case 2:
                        for (int y = 0; y < Iblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock90.GetLength(1); x++)
                            {
                                if (Iblock90[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int y = 0; y < Iblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock180.GetLength(1); x++)
                            {
                                if (Iblock180[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < Iblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock270.GetLength(1); x++)
                            {
                                if (Iblock270[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        for (int y = 0; y < Iblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Iblock.GetLength(1); x++)
                            {
                                if (Iblock[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Oblock)
            {
                for (int y = 0; y < Oblock.GetLength(0); y++)
                {
                    for (int x = 0; x < Oblock.GetLength(1); x++)
                    {
                        if (Oblock[y, x] == 1)
                        {
                            int fieldX = startx + x;
                            int fieldY = starty + y;
                            if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                            {
                                tempGameBoard[fieldX, fieldY] = 0;
                            }
                        }
                    }
                }
            }
            if (chosenblock == Sblock)
            {
                switch (rotation)
                {
                    case 2:
                        // 90
                        for (int y = 0; y < Sblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock90.GetLength(1); x++)
                            {
                                if (Sblock90[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        // 180
                        for (int y = 0; y < Sblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock180.GetLength(1); x++)
                            {
                                if (Sblock180[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        // 270
                        for (int y = 0; y < Sblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock270.GetLength(1); x++)
                            {
                                if (Sblock270[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        // 0
                        for (int y = 0; y < Sblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Sblock.GetLength(1); x++)
                            {
                                if (Sblock[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            if (chosenblock == Zblock)
            {
                switch (rotation)
                {
                    case 2:
                        // 90
                        for (int y = 0; y < Zblock90.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock90.GetLength(1); x++)
                            {
                                if (Zblock90[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 3:
                        // 180
                        for (int y = 0; y < Zblock180.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock180.GetLength(1); x++)
                            {
                                if (Zblock180[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    case 4:
                        // 270
                        for (int y = 0; y < Zblock270.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock270.GetLength(1); x++)
                            {
                                if (Zblock270[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        // 5
                        for (int y = 0; y < Zblock.GetLength(0); y++)
                        {
                            for (int x = 0; x < Zblock.GetLength(1); x++)
                            {
                                if (Zblock[y, x] == 1)
                                {
                                    int fieldX = startx + x;
                                    int fieldY = starty + y;
                                    if (fieldX >= 0 && fieldX < BoardWidth && fieldY >= 0 && fieldY < BoardHeight)
                                    {
                                        tempGameBoard[fieldX, fieldY] = 0;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void placeblock()
        {
            if (starty >= 2 )
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    for (int x = 0; x < BoardWidth; x++)
                    {
                        if (tempGameBoard[x, y] == 1)
                        {
                            permanentGameBoard[x, y] = 1;
                        }
                    }
                }
                Array.Clear(tempGameBoard, 0, tempGameBoard.Length);

                CheckCompletedRows();

                AllowMoveLeft = true;
                AllowMoveRight = true;
                rotation = 5;
                blockpicker();
                startx = 5;
                starty = 0;
                drawblock(startx, starty);
                UpdateGameBoard();
            }
        }

        private void CheckCompletedRows()
        {
            for (int y = BoardHeight - 1; y >= 0; y--)
            {
                bool rowCompleted = true;
                for (int x = 2; x < BoardWidth - 2; x++)
                {
                    if (permanentGameBoard[x, y] == 0)
                    {
                        rowCompleted = false;
                        break;
                    }
                }
                if (rowCompleted)
                {
                    RemoveCompletedRow(y);
                    y++;
                }
            }
            
            UpdateLevel();
            
        }
        private void RemoveCompletedRow(int rowIndex)
        {
            for (int y = rowIndex; y > 0; y--)
            {
                for (int x = 0; x < BoardWidth; x++)
                {
                    permanentGameBoard[x, y] = permanentGameBoard[x, y - 1];
                }
            }
            Score += 40 + ((Level + 1) * 40);
            completedlines += 1;
            UpdateLinecount();
            UpdateLevel();
            onelevelup += 1;
            UpdateLevel();
        }
    }
}
