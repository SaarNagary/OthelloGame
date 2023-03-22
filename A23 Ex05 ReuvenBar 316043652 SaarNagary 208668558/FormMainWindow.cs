using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Othello
{
    public partial class FormMainWindow : Form
    {
        public FormMainWindow()
        {
            FormGameSettings settingForm = new FormGameSettings();
            settingForm.ButtonsPlayVsClicked += new System.Action<Boolean, int>(this.ButtonsPlayVs_Click);
            settingForm.Show();
        }

        private bool m_WantPlayVsComputer = false;
        private int m_sizeOfMap;
        private int m_width;
        private int m_height;
        private SquareInMatrix[,] m_mapMatrix;
        private void ButtonsPlayVs_Click(Boolean i_WantPlayVsComputer, int i_SizeOfMap)
        {
            m_WantPlayVsComputer = i_WantPlayVsComputer;
            m_sizeOfMap = i_SizeOfMap;

            this.createOthelloMap();
            InitializeComponent();
            this.Show();
        }

        private void createOthelloMap()
        {
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

                    if(firstRow)
                    {
                        if(firstCol)
                        {
                            y = 5;
                            firstCol = false;
                        }

                        x = 12;
                        firstRow = false;
                    }

                    this.Controls.Add(m_mapMatrix[i, j]);
                    //putImageOnSquare(m_mapMatrix[i, j]);
                    m_mapMatrix[i, j].Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                            .Properties.Resources.Grey;
                    m_mapMatrix[i, j].Location = new Point(x, y);
                    //m_mapMatrix[i, j].Enabled = false;
                    m_mapMatrix[i, j].Click += onSquereClick;
                    m_mapMatrix[i, j].SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                    x += 52;
                }   
            }

            m_height = m_mapMatrix[rowAndColSize - 1, rowAndColSize - 1].Bottom + 10;
            this.initStart();
        }

        private void initStart()
        {
            int yelloLeftPoint = m_mapMatrix.GetLength(0) / 2 - 1;

            changePosition(m_mapMatrix[yelloLeftPoint, yelloLeftPoint], "White");
            changePosition(m_mapMatrix[yelloLeftPoint, yelloLeftPoint + 1], "Black");
            changePosition(m_mapMatrix[yelloLeftPoint + 1, yelloLeftPoint], "Black");
            changePosition(m_mapMatrix[yelloLeftPoint + 1, yelloLeftPoint + 1], "White");

            /*foreach (SquareInMatrix square in m_mapMatrix)
            {
                changePosition(square, "Black");
            }*/
        }

        private void changePosition(SquareInMatrix i_SquareToChangeHisColor, String i_ColorToChange)
        {
            i_SquareToChangeHisColor.Position = i_ColorToChange;
            putImageOnSquare(i_SquareToChangeHisColor);
        }
        
        private void fromGreenToGray(SquareInMatrix i_GreenSquareToPutGrayOn)
        {
            i_GreenSquareToPutGrayOn.Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                                        .Properties.Resources.Grey;
            i_GreenSquareToPutGrayOn.Size = new Size(53, 53);//perfect size of gray image.
            i_GreenSquareToPutGrayOn.Location = new Point(i_GreenSquareToPutGrayOn.Location.X - 4,
                                                            i_GreenSquareToPutGrayOn.Location.Y - 5);
            i_GreenSquareToPutGrayOn.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void fromGreenToYello(SquareInMatrix i_GreenSquareToPutYelloOn)
        {
            i_GreenSquareToPutYelloOn.Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                                        .Properties.Resources.CoinYellow;
            i_GreenSquareToPutYelloOn.Size = new Size(43, 43);//perfect size of yellow image.

            i_GreenSquareToPutYelloOn.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void fromGreenToRed(SquareInMatrix i_GreenSquareToPutRedOn)
        {
            i_GreenSquareToPutRedOn.Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                                        .Properties.Resources.CoinYellow;
            i_GreenSquareToPutRedOn.Size = new Size(43, 43);//perfect size of yellow image.

            i_GreenSquareToPutRedOn.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void fromGrayToRed(SquareInMatrix i_GraySquareToPutRedOn)
        {
            i_GraySquareToPutRedOn.Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                                        .Properties.Resources.CoinRed;
            i_GraySquareToPutRedOn.Size = new Size(43, 43);//perfect size of gray image.

            i_GraySquareToPutRedOn.Location = new Point(i_GraySquareToPutRedOn.Location.X + 4,
                                                           i_GraySquareToPutRedOn.Location.Y + 5);

            i_GraySquareToPutRedOn.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void fromGrayToYellow(SquareInMatrix i_GraySquareToPutYellowOn)
        {
            i_GraySquareToPutYellowOn.Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                                        .Properties.Resources.CoinRed;
            i_GraySquareToPutYellowOn.Size = new Size(43, 43);//perfect size of gray image.

            i_GraySquareToPutYellowOn.Location = new Point(i_GraySquareToPutYellowOn.Location.X + 4,
                                                           i_GraySquareToPutYellowOn.Location.Y + 5);

            i_GraySquareToPutYellowOn.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void fromGrayToGreen(SquareInMatrix i_GraySquareToPutGreenOn)
        {
            i_GraySquareToPutGreenOn.Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                                        .Properties.Resources.Green;
            i_GraySquareToPutGreenOn.Size = new Size(47, 47);//perfect size of gray image.

            i_GraySquareToPutGreenOn.Location = new Point(i_GraySquareToPutGreenOn.Location.X + 4,
                                                           i_GraySquareToPutGreenOn.Location.Y + 7);

            i_GraySquareToPutGreenOn.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void putImageOnSquare(SquareInMatrix i_SquareToPutImageOn)
        {
            if (i_SquareToPutImageOn.LastMove.Equals("*****"))
            {
                if (i_SquareToPutImageOn.Enabled)//from green 
                {
                    if (i_SquareToPutImageOn.Position.Equals("*****"))
                    {
                        fromGreenToGray(i_SquareToPutImageOn);
                    }
                }
            } 
        
                    /*else if (i_SquareToPutImageOn.Position.Equals("White"))
                    {
                        fromGreenToYello();
                    }
                    else if (i_SquareToPutImageOn.Position.Equals("Black"))
                    {
                        fromGreenToRed();
                    }
                }
                else//from grey
                {
                    if (i_SquareToPutImageOn.Position.Equals("White"))
                    {
                        fromGreyToYello();
                    }
                    else if (i_SquareToPutImageOn.Position.Equals("Black"))
                    {
                        fromGreyToRed();
                    }
                }
            }
            else if(i_SquareToPutImageOn.LastMove.Equals("White"))//from Yello
            {
                i_SquareToPutImageOn.Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                            .Properties.Resources.CoinYellow;
                if(i_SquareToPutImageOn.Position.Equals("Black"))
                {
                    fromYelloToRed();
                }
                i_SquareToPutImageOn.Size = new Size(43, 43);
                    i_SquareToPutImageOn.Location = new Point(i_SquareToPutImageOn.Location.X + 5,
                                                            i_SquareToPutImageOn.Location.Y + 6);
                
            }
            else if (i_SquareToPutImageOn.LastMove.Equals("Black"))//from Red
            {
                i_SquareToPutImageOn.Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                            .Properties.Resources.CoinRed;
                if (i_SquareToPutImageOn.Position.Equals("Black"))
                {
                    fromRedToYello();
                }
                    i_SquareToPutImageOn.Size = new Size(43, 43);
                    i_SquareToPutImageOn.Location = new Point(i_SquareToPutImageOn.Location.X + 5,
                                                            i_SquareToPutImageOn.Location.Y + 6);
            }*/

            //i_SquareToPutImageOn.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void onSquereClick(object sender, EventArgs e)
        {
            (sender as SquareInMatrix).Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                                        .Properties.Resources.CoinRed;
            (sender as SquareInMatrix).Size = new Size(47, 47);//perfect size of gray image.

            (sender as SquareInMatrix).Location = new Point((sender as SquareInMatrix).Location.X + 4,
                                                           (sender as SquareInMatrix).Location.Y + 7);

            (sender as SquareInMatrix).SizeMode = PictureBoxSizeMode.StretchImage;
            //(sender as SquareInMatrix).Location = new Point((sender as SquareInMatrix).Location.X - 4,
            // (sender as SquareInMatrix).Location.Y - 5);
            /*changePosition((sender as SquareInMatrix), "Black");
            if ((sender as SquareInMatrix).Enabled == true)
            {
                putImageOnSquare((sender as SquareInMatrix));
            }
            else
            { 
                putGreenImage(sender as SquareInMatrix);
            }*/
            //(sender as PictureBox).SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            //MessageBox.Show((sender as SquareInMatrix).Image.ToString(), "dey", MessageBoxButtons.OKCancel);

        }

        private void putGreenImage(SquareInMatrix i_SquareToPutGreenOn)
        {
            i_SquareToPutGreenOn.Image = A23_Ex05_ReuvenBar_316043652_SaarNagary_208668558
                                            .Properties.Resources.Green;
            i_SquareToPutGreenOn.Enabled = true;
            if (i_SquareToPutGreenOn.Position.Equals("*****"))
            {
                i_SquareToPutGreenOn.Size = new Size(49, 49);
                i_SquareToPutGreenOn.Location = new Point(i_SquareToPutGreenOn.Location.X + 2,
                                                          i_SquareToPutGreenOn.Location.Y + 5);
                i_SquareToPutGreenOn.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else if (i_SquareToPutGreenOn.Position.Equals("White"))
            {
                i_SquareToPutGreenOn.Size = new Size(47, 47);
                i_SquareToPutGreenOn.Location = new Point(i_SquareToPutGreenOn.Location.X - 2,
                                                          i_SquareToPutGreenOn.Location.Y + 1);
            }
            else if (i_SquareToPutGreenOn.Position.Equals("Black"))
            {
                i_SquareToPutGreenOn.Size = new Size(47, 47);
                i_SquareToPutGreenOn.Location = new Point(i_SquareToPutGreenOn.Location.X - 2,
                                                          i_SquareToPutGreenOn.Location.Y + 1);
            }
        }


    }
}
