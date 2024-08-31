using System;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Media;



namespace _2048_Game
{
    public partial class Main : Form
    {
        private int[,] grid = new int[4, 4];
        private Guna2HtmlLabel[,] labels = new Guna2HtmlLabel[4, 4];
        private SoundPlayer moveSoundPlayer;
        public Main()
        {
            InitializeComponent();
            
            InitializeGridLabels();
            this.KeyDown += new KeyEventHandler(OnKeyDownHandler);
            this.KeyPreview = true;
            
            moveSoundPlayer = new SoundPlayer("MoveSound.wav");

        }
        
        private string highScoreFilePath = "highscore.txt";
        

        private void InitializeGridLabels()
        {
            labels[0, 0] = Num1;
            labels[0, 1] = Num2;
            labels[0, 2] = Num3;
            labels[0, 3] = Num4;
            labels[1, 0] = Num5;
            labels[1, 1] = Num6;
            labels[1, 2] = Num7;
            labels[1, 3] = Num8;
            labels[2, 0] = Num9;
            labels[2, 1] = Num10;
            labels[2, 2] = Num11;
            labels[2, 3] = Num12;
            labels[3, 0] = Num13;
            labels[3, 1] = Num14;
            labels[3, 2] = Num15;
            labels[3, 3] = Num16;
        }

        private void UpdateUI()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (grid[i, j] == 0)
                        labels[i, j].Text = "";
                    else
                        labels[i, j].Text = grid[i, j].ToString();
                }
            }
            UpdatePanelColors(); 
            UpdateFontSize();
        }
        private void UpdatePanelColors()
        {
           
            var colors = new Dictionary<int, Color>
    {
        { 2, Color.FromArgb(238, 228, 218) },
        { 4, Color.FromArgb(238, 225, 201) },
        { 8, Color.FromArgb(243, 178, 122) },
        { 16, Color.FromArgb(246, 150, 100) },
        { 32, Color.FromArgb(247, 124, 95) },
        { 64, Color.FromArgb(247, 95, 59) },
        { 128, Color.FromArgb(237, 208, 115) },
        { 256, Color.FromArgb(237, 204, 98) },
        { 512, Color.FromArgb(109, 109, 242) },
        { 1024, Color.FromArgb(40, 204, 62) },
        { 2048, Color.FromArgb(44, 240, 247) }
    };

            
            for (int i = 1; i <= 16; i++)
            {
                Guna2HtmlLabel label = this.Controls.Find("Num" + i, true).FirstOrDefault() as Guna2HtmlLabel;
                Guna2Panel panel = this.Controls.Find("lbNum" + i, true).FirstOrDefault() as Guna2Panel;

                if (label != null && panel != null)
                {
                    int value;
                    if (int.TryParse(label.Text, out value))
                    {
                        if (colors.ContainsKey(value))
                        {
                            panel.FillColor = colors[value];
                            
                            double luminance = 0.299 * panel.FillColor.R + 0.587 * panel.FillColor.G + 0.114 * panel.FillColor.B;
                            if (luminance < 128)
                            {
                                label.ForeColor = Color.White;
                            }
                            else
                            {
                                label.ForeColor = Color.Black;
                            }
                        }
                        else
                        {
                            panel.FillColor = Color.Black;
                            label.ForeColor = Color.White;
                        }
                    }
                    else
                    {
                        panel.FillColor = Color.FromArgb(200, 189, 176); 
                        label.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void UpdateFontSize()
        {
            foreach (var label in labels)
            {
                int value;
                if (int.TryParse(label.Text, out value))
                {
                    if (value >= 128 && value <= 1024)
                    {
                        label.Font = new Font(label.Font.FontFamily, 20);
                    }
                    else if (value > 1024)
                    {
                        label.Font = new Font(label.Font.FontFamily, 15);
                    }
                    else
                    {
                        label.Font = new Font(label.Font.FontFamily, 25); 
                    }
                }
                else
                {
                    label.Font = new Font(label.Font.FontFamily, 25);
                }
            }
        }

        public void startGame()
        {
            LoadingHighScore();
            ResetSocer();
            ResetGrid();
            AddRandomNumber();
            AddRandomNumber();
            UpdateUI();
        }
        private void LoadingHighScore()
        {
            try
            {
                if (File.Exists(highScoreFilePath))
                {
                    string highScoreText = File.ReadAllText(highScoreFilePath);
                    int highScore;
                    if (int.TryParse(highScoreText, out highScore))
                    {
                        lbhighScore.Text = highScore.ToString();
                    }
                    else
                    {
                        lbhighScore.Text = "0";
                    }
                }
                else
                {
                    lbhighScore.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load high score: " + ex.Message);
                lbhighScore.Text = "0";
            }
        }
        private void ResetGrid()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    grid[i, j] = 0;
                }
            }
        }
        private void ResetSocer()
        {
            score = 0;
            lbScore.Text = score.ToString();
        }

        private void AddRandomNumber()
        {
            Random random = new Random();
            int x, y;
            do
            {
                x = random.Next(0, 4);
                y = random.Next(0, 4);
            } while (grid[x, y] != 0);

            grid[x, y] = random.Next(1, 3) * 2;
        }
        private void ResetButtonColors()
        {
            BtnW.FillColor = Color.White;
            BtnS.FillColor = Color.White;
            BtnA.FillColor = Color.White;
            BtnD.FillColor = Color.White;
        }

        public void UpdateBtn(object sender, KeyEventArgs e)
        {
           
            ResetButtonColors();

            
            switch (e.KeyCode)
            {
                case Keys.W:
                    BtnW.FillColor = Color.FromArgb(247, 146, 31);
                    break;
                case Keys.S:
                    BtnS.FillColor = Color.FromArgb(247, 146, 31);
                    break;
                case Keys.A:
                    BtnA.FillColor = Color.FromArgb(247, 146, 31);
                    break;
                case Keys.D:
                    BtnD.FillColor = Color.FromArgb(247, 146, 31);
                    break;
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            bool moved = false;
            UpdateBtn(sender, e);

            switch (e.KeyCode)
            {
                case Keys.W:
                    moved = MoveUp();
                    break;
                case Keys.S:
                    moved = MoveDown();
                    break;
                case Keys.A:
                    moved = MoveLeft();
                    break;
                case Keys.D:
                    moved = MoveRight();
                    break;
            }

            if (moved)
            {
                try
                {
                    moveSoundPlayer.Play();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to play sound: " + ex.Message);
                }
                AddRandomNumber();
                UpdateUI();
                if (IsGameOver())
                {
                    MessageBox.Show("Game Over!");
                }
            }
        }
        private bool MoveUp()
        {
         
            bool moved = false;
            for (int j = 0; j < 4; j++)
            {
                int[] column = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    column[i] = grid[i, j];
                }
                moved |= MoveAndMerge(column);
                for (int i = 0; i < 4; i++)
                {
                    grid[i, j] = column[i];
                }
            }
            return moved;
        }
        private bool MoveDown()
        {
            bool moved = false;
            for (int j = 0; j < 4; j++)
            {
                int[] column = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    column[i] = grid[i, j];
                }
                Array.Reverse(column);
                moved |= MoveAndMerge(column);
                Array.Reverse(column);
                for (int i = 0; i < 4; i++)
                {
                    grid[i, j] = column[i];
                }
            }
            return moved;
        }
        private bool MoveLeft()
        {
            bool moved = false;
            for (int i = 0; i < 4; i++)
            {
                int[] row = new int[4];
                for (int j = 0; j < 4; j++)
                {
                    row[j] = grid[i, j];
                }
                moved |= MoveAndMerge(row);
                for (int j = 0; j < 4; j++)
                {
                    grid[i, j] = row[j];
                }
            }
            return moved;
        }
        private bool MoveRight()
        {
            bool moved = false;
            for (int i = 0; i < 4; i++)
            {
                int[] row = new int[4];
                for (int j = 0; j < 4; j++)
                {
                    row[j] = grid[i, j];
                }
                Array.Reverse(row);
                moved |= MoveAndMerge(row);
                Array.Reverse(row);
                for (int j = 0; j < 4; j++)
                {
                    grid[i, j] = row[j];
                }
            }
            return moved;
        }

        private int score = 0;
        private int highScore = 0; 
        private void InitializeGame()
        {
            highScore = LoadHighScore();
            lbhighScore.Text = highScore.ToString();
            score = 0; 
            lbScore.Text = score.ToString();

           
        }

        private bool MoveAndMerge(int[] line)
        {
            int[] newLine = new int[4];
            int position = 0;
            bool moved = false;

            for (int i = 0; i < 4; i++)
            {
                if (line[i] != 0)
                {
                    if (position > 0 && newLine[position - 1] == line[i])
                    {
                        newLine[position - 1] *= 2;
                        score += newLine[position - 1]; 
                        moved = true;
                    }
                    else
                    {
                        newLine[position] = line[i];
                        if (position != i)
                        {
                            moved = true;
                        }
                        position++;
                    }
                }
            }

            Array.Copy(newLine, line, 4);

            
            lbScore.Text = score.ToString();

            
            UpdateHighScore();

            return moved;
        }

        private void UpdateHighScore()
        {
            if (score > highScore)
            {
                highScore = score;
                lbhighScore.Text = highScore.ToString();
                SaveHighScore(highScore);
            }
        }

        private void SaveHighScore(int highScore)
        {
            try
            {
                File.WriteAllText(highScoreFilePath, highScore.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save high score: " + ex.Message);
            }
        }

        private int LoadHighScore()
        {
            try
            {
                if (File.Exists(highScoreFilePath))
                {
                    string highScoreText = File.ReadAllText(highScoreFilePath);
                    if (int.TryParse(highScoreText, out int highScore))
                    {
                        return highScore;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load high score: " + ex.Message);
            }

            return 0;
        }

        private bool IsGameOver()
        {
           
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (grid[i, j] == 0)
                        return false;
                }
                
            }

           
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i < 3 && grid[i, j] == grid[i + 1, j])
                        return false;
                    if (j < 3 && grid[i, j] == grid[i, j + 1])
                        return false;
                }
            }

            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            startGame();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            startGame();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
           
            Info infoForm = new Info();

           
            infoForm.StartPosition = FormStartPosition.Manual;
            infoForm.Location = new Point(this.Location.X + (this.Width - infoForm.Width) / 2,
                                          this.Location.Y + (this.Height - infoForm.Height) / 2);

           
            this.Enabled = false;

           
            infoForm.Show();

            
            infoForm.FormClosed += (s, args) => this.Enabled = true;
        }



    }
}
