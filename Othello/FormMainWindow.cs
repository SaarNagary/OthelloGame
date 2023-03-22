using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Othello
{
    internal partial class FormMainWindow : Form
    {
        public FormMainWindow()
        {
            FormGameSettings settingForm = new FormGameSettings();
            settingForm.ButtonsPlayVsClicked += new System.Action<Boolean, int>(this.ButtonsPlayVs_Click);
            settingForm.FormClosed += settingForm_CloseByX;
            this.TopLevel = false;
            settingForm.Show();
        }

        private OthelloBrain m_Logic;
        private bool m_WantPlayVsComputer = false;
        private bool m_ItsNewGame = false;
        private bool m_HaveLegalMoves = true;
        private int m_sizeOfMap;
        private int m_width;
        private int m_height;
        System.Windows.Forms.Timer timerToPcPlay;
        List<(int, int)> m_IndexOfGreenSqueres = new List<(int Row, int Col)>(6);
        private SquareInMatrix[,] m_mapMatrix;

        private void settingForm_CloseByX(object sender, EventArgs e)
        {
            if (m_sizeOfMap < 6)
            {
                Application.Exit();
            }
        }
        public SquareInMatrix[,] MapMatrix
        {
            get
            {
                return m_mapMatrix;
            }
        }

        private void gameEnd()
        {
            DialogResult result = new DialogResult();
            if (m_WantPlayVsComputer)
            {
                timerToPcPlay.Stop();
            }
            if (m_Logic.RedSqeresCount > m_Logic.YelloSqeresCount)
            {
                result = MessageBox.Show(string.Format("{0} Won!! ({1}/{2}) ({3}/{4}){5}" +
                    "Would you like another round?",MapMatrix[0,0].FirstColor,
                    m_Logic.RedSqeresCount, m_Logic.YelloSqeresCount, 
                    m_Logic.RedSqeresWinPointsCount, m_Logic.YelloSqeresWinPointsCount, 
                    System.Environment.NewLine), "Othello", MessageBoxButtons.YesNo);
            }
            else if (m_Logic.YelloSqeresCount > m_Logic.RedSqeresCount)
            {
                result = MessageBox.Show(string.Format("{0} Won!! ({1}/{2}) ({3}/{4}){5}" +
                    "Would you like another round?", MapMatrix[0, 0].SecondColor,
                    m_Logic.YelloSqeresCount, m_Logic.RedSqeresCount, 
                    m_Logic.YelloSqeresWinPointsCount, m_Logic.RedSqeresWinPointsCount, 
                    System.Environment.NewLine), "Othello", MessageBoxButtons.YesNo);
            }
            else//draw
            {
                result = MessageBox.Show(string.Format("Nobody won, its a draw!! ({0}/{1})" +
                    " ({2}: {3} / {4}: {5}){6}" +
                    "Would you like another round?",
                    m_Logic.YelloSqeresCount, m_Logic.RedSqeresCount, MapMatrix[0, 0].FirstColor,
                    m_Logic.RedSqeresWinPointsCount, MapMatrix[0, 0].SecondColor,
                    m_Logic.YelloSqeresWinPointsCount,System.Environment.NewLine),
                    "Othello", MessageBoxButtons.YesNo);
            }

            if(result == DialogResult.Yes)
            {
                cleanForm();
                createOthelloMap();
                InitializeComponent();
                m_ItsNewGame = true;
            }
            else
            {
                this.Close();
            }
        }

        private void cleanForm()
        {
            foreach (SquareInMatrix Square in m_mapMatrix)
            {
                this.Controls.Remove(Square);
            }
        }

        private void ButtonsPlayVs_Click(Boolean i_WantPlayVsComputer, int i_SizeOfMap)
        {
            m_WantPlayVsComputer = i_WantPlayVsComputer;
            m_sizeOfMap = i_SizeOfMap;


            if (m_WantPlayVsComputer)
            {
                timerToPcPlay = new System.Windows.Forms.Timer();
                timerToPcPlay.Interval = 900;
                timerToPcPlay.Tick += pcTurnToPlay;
            }

            this.createOthelloMap();
            InitializeComponent();
            this.TopLevel = true;
        }

        private void createOthelloMap()
        {
            if(m_mapMatrix != null)
            {
                m_mapMatrix = null;
            }

            m_mapMatrix = new SquareInMatrix[m_sizeOfMap, m_sizeOfMap];
            createMatrix();
        }

        private void createMatrix()
        {
            int rowAndColSize = m_mapMatrix.GetLength(0);
            bool firstRow;
            bool firstCol = true;
            int x = 0;
            int y = 0;

            for (int i = 0; i < rowAndColSize; i++)
            {
                if(!firstCol)
                {
                    m_width = m_mapMatrix[i - 1, rowAndColSize - 1].Right + 12;
                    y = m_mapMatrix[i - 1, 0].Bottom + 1;
                }

                firstRow = true;
                for (int j = 0; j < rowAndColSize; j++)
                {
                    m_mapMatrix[i, j] = new SquareInMatrix { };
                    m_mapMatrix[i, j].Row = i;
                    m_mapMatrix[i, j].Col = j;
                    m_mapMatrix[i, j].EndToFlip += PaintGreenAndMakeEnable;

                    if (firstRow)
                    {
                        if(firstCol)
                        {
                            y = 5;
                            firstCol = false;
                        }

                        x = 12;
                        firstRow = false;
                    }

                    addFirstSquare(m_mapMatrix[i, j], new Point(x, y));
                    x += 52;
                }   
            }

            m_height = m_mapMatrix[rowAndColSize - 1, rowAndColSize - 1].Bottom + 10;
            this.initStart();
        }

        private void addFirstSquare(SquareInMatrix i_Square, Point i_point)
        {
            this.Controls.Add(i_Square);
            i_Square.BackColor = Color.Gray;
            i_Square.Size = new Size(45, 45);
            i_Square.Location = i_point;
            i_Square.Enabled = false;
            i_Square.Click += onSquereClick;
            i_Square.ChangeToRed += fromYelloToRed;
            i_Square.ChangeToYello += fromRedToYello;
            i_Square.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        }

        private void initStart()
        {
            int yelloLeftPoint = m_mapMatrix.GetLength(0) / 2 - 1;

            changePosition(m_mapMatrix[yelloLeftPoint, yelloLeftPoint],
                m_mapMatrix[yelloLeftPoint, yelloLeftPoint].SecondColor);
            changePosition(m_mapMatrix[yelloLeftPoint, yelloLeftPoint + 1],
                m_mapMatrix[yelloLeftPoint, yelloLeftPoint].FirstColor);
            changePosition(m_mapMatrix[yelloLeftPoint + 1, yelloLeftPoint],
                m_mapMatrix[yelloLeftPoint, yelloLeftPoint].FirstColor);
            changePosition(m_mapMatrix[yelloLeftPoint + 1, yelloLeftPoint + 1],
                m_mapMatrix[yelloLeftPoint, yelloLeftPoint].SecondColor);
            if (m_Logic != null)
            {
                m_Logic = null;
            }

            m_Logic = new OthelloBrain(m_mapMatrix);

            m_Logic.GameEnd += this.gameEnd;
            m_IndexOfGreenSqueres = m_Logic.GetGreenSqueresIndexIfHave();
            PaintGreenAndMakeEnable();
        }

        private void changeTurn()
        {
            if(m_Logic.RedTurnToPlayNow)
            {
                this.Text = string.Format("Othello - {0}'s Turn", MapMatrix[0, 0].FirstColor);
            }
            else
            {
                this.Text = string.Format("Othello - {0}'s Turn", MapMatrix[0, 0].SecondColor);
            }
        }

        internal void PaintGreenAndMakeEnable()
        {
            if (!(m_IndexOfGreenSqueres[0].Item1 == -1))
            {
                foreach ((int, int) indexToGreen in m_IndexOfGreenSqueres)
                {
                    changePosition(m_mapMatrix[indexToGreen.Item1, indexToGreen.Item2], "Green");
                }
            }
        }

        private void changePosition(SquareInMatrix i_SquareToChangeHisColor, String i_ColorToChange)
        {
            if (i_ColorToChange.Equals("Green"))
            {
                i_SquareToChangeHisColor.BackColor = Color.Green;
                if (m_Logic.RedTurnToPlayNow || !m_WantPlayVsComputer)
                {
                    i_SquareToChangeHisColor.Enabled = true;
                }
            }
            else if(i_ColorToChange.Equals("Gray"))
            {
                i_SquareToChangeHisColor.BackColor = Color.Gray;
                i_SquareToChangeHisColor.Enabled = false;
            }
            else
            {
                i_SquareToChangeHisColor.Position = i_ColorToChange;
                putImageOnSquare(i_SquareToChangeHisColor);
            }
        }

        private void fromYelloToRed(SquareInMatrix i_YelloSquareToPutRedOn)
        {
            i_YelloSquareToPutRedOn.Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                                        .Properties.Resources.CoinRed;
        }

        private void fromRedToYello(SquareInMatrix i_RedSquareToPutYelloOn)
        {
            i_RedSquareToPutYelloOn.Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                                        .Properties.Resources.CoinYellow;
        }

        private void putImageOnSquare(SquareInMatrix i_SquareToPutImageOn)
        {
            if (i_SquareToPutImageOn.Position.Equals(i_SquareToPutImageOn.FirstColor))
            {
                fromYelloToRed(i_SquareToPutImageOn);
            }
            else if (i_SquareToPutImageOn.Position.Equals(i_SquareToPutImageOn.SecondColor))
            {
                fromRedToYello(i_SquareToPutImageOn);
            }

            i_SquareToPutImageOn.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void onSquereClick(object sender, EventArgs e)
        {
            int row = (sender as SquareInMatrix).Row;
            int col = (sender as SquareInMatrix).Col;
            bool firstColorTurn = m_Logic.RedTurnToPlayNow;

            if (m_HaveLegalMoves)
            {
                changeBackToGray(row, col);
                m_Logic.checkIfOkAndChangeIfWant(m_mapMatrix,
                     m_Logic.whereAreWeOnTheMatrix(m_mapMatrix, row, col),
                     row, col, firstColorTurn, true);
                if (firstColorTurn)
                {
                    changePosition((sender as SquareInMatrix), (sender as SquareInMatrix).FirstColor);
                }
                else
                {
                    changePosition((sender as SquareInMatrix), (sender as SquareInMatrix).SecondColor);
                }
            }

            if (chackIfHaveLegalMovesToNextColor())
            {
                m_HaveLegalMoves = true;
                changeTurn();
                if(firstColorTurn)
                {
                    m_Logic.YelloHaveMoreLegalMoves = true;
                }
                else
                {
                    m_Logic.RedHaveMoreLegalMoves = true;
                }

                if (m_WantPlayVsComputer && !m_Logic.RedTurnToPlayNow)
                {
                    timerToPcPlay.Start();
                }
            }
            else
            {
                if(chackIfHaveLegalMovesToNextColor())
                {
                    if (m_Logic.RedTurnToPlayNow)
                    {
                        MessageBox.Show(String.Format("{0} No ligal moves to play, turn pass", MapMatrix[0, 0].SecondColor)
                            , "No Ligal Moves", MessageBoxButtons.OK);
                        if (m_WantPlayVsComputer)
                        {
                            timerToPcPlay.Stop();
                        }

                        m_Logic.YelloHaveMoreLegalMoves = false;
                    }
                    else
                    {
                        MessageBox.Show(String.Format("{0} No ligal moves to play, turn pass", MapMatrix[0, 0].FirstColor)
                            , "No Ligal Moves", MessageBoxButtons.OK);
                        if (m_WantPlayVsComputer)
                        {
                            timerToPcPlay.Start();
                        }

                        m_Logic.RedHaveMoreLegalMoves = false;
                    }
                }
                else
                {
                    m_Logic.YelloHaveMoreLegalMoves = false;
                    m_Logic.RedHaveMoreLegalMoves = false;
                }
            }

            if (m_ItsNewGame)
            {
                m_ItsNewGame = false;
                m_IndexOfGreenSqueres = m_Logic.GetGreenSqueresIndexIfHave();
                PaintGreenAndMakeEnable();
            }
            else if(m_WantPlayVsComputer && m_HaveLegalMoves && !m_Logic.RedTurnToPlayNow)
            {
                timerToPcPlay.Start();
            }
        }

        private bool chackIfHaveLegalMovesToNextColor()
        {
            m_Logic.RedTurnToPlayNow = !m_Logic.RedTurnToPlayNow;
            m_IndexOfGreenSqueres = m_Logic.GetGreenSqueresIndexIfHave();
            
            return m_IndexOfGreenSqueres[0].Item1 != -1;
        }

        private void pcTurnToPlay(object sender, EventArgs e)
        {
            Random rnd = new Random();
            bool firstColorTurn = m_Logic.RedTurnToPlayNow;
            int pcNumberMoveChooseFromList = rnd.Next(0, m_IndexOfGreenSqueres.Count);
            int row = m_IndexOfGreenSqueres[pcNumberMoveChooseFromList].Item1;
            int col = m_IndexOfGreenSqueres[pcNumberMoveChooseFromList].Item2;


            if (m_HaveLegalMoves)
            {
                changeBackToGray(row, col);
                m_Logic.checkIfOkAndChangeIfWant(m_mapMatrix,
                     m_Logic.whereAreWeOnTheMatrix(m_mapMatrix, row, col),
                     row, col, firstColorTurn, true);
                if (firstColorTurn)
                {
                    changePosition(m_mapMatrix[row, col], m_mapMatrix[row, col].FirstColor);
                }
                else
                {
                    changePosition(m_mapMatrix[row, col], m_mapMatrix[row, col].SecondColor);
                }
            }

            timerToPcPlay.Stop();
            if (chackIfHaveLegalMovesToNextColor())
            {
                m_HaveLegalMoves = true;
                changeTurn();
                m_Logic.RedHaveMoreLegalMoves = true;
            }
            else
            {
                if (chackIfHaveLegalMovesToNextColor())
                {
                    MessageBox.Show(String.Format("{0} No ligal moves to play, turn pass", MapMatrix[0, 0].FirstColor)
                        , "No Ligal Moves", MessageBoxButtons.OK);
                    timerToPcPlay.Start();
                    m_Logic.RedHaveMoreLegalMoves = false;
                }
                else
                {
                    m_Logic.YelloHaveMoreLegalMoves = false;
                    m_Logic.RedHaveMoreLegalMoves = false;
                }
            }

            if (m_ItsNewGame)
            {
                m_ItsNewGame = false;
                m_IndexOfGreenSqueres = m_Logic.GetGreenSqueresIndexIfHave();
                PaintGreenAndMakeEnable();
            }
        }

        private void changeBackToGray(int i_Row, int i_Col)
        {
            m_mapMatrix[i_Row, i_Col].BackColor = Color.Gray;
            foreach ((int, int) indexToGreen in m_IndexOfGreenSqueres)
            {
                changePosition(m_mapMatrix[indexToGreen.Item1, indexToGreen.Item2], "Gray");
            }
        }
    }
}
