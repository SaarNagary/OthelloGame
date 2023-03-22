using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    public class OthelloBrain
    {
        private SquareInMatrix[,] m_Map;
        private bool m_RedTurnToPlayNow = true;
        private bool m_RedHaveMoreLegalMoves = true;
        private bool m_YelloHaveMoreLegalMoves = true;
        private short m_RedSqeresCount = 0;
        private short m_YelloSqeresCount = 0;
        private static short m_RedSqeresWinPointsCount = 0;
        private static short m_YelloSqeresWinPointsCount = 0;
        public event Action GameEnd;

        public short RedSqeresCount
        {
            get
            {
                return m_RedSqeresCount;
            }
        }

        public short YelloSqeresCount
        {
            get
            {
                return m_YelloSqeresCount;
            }
        }

        public short RedSqeresWinPointsCount
        {
            get
            {
                return m_RedSqeresWinPointsCount;
            }
        }

        public short YelloSqeresWinPointsCount
        {
            get
            {
                return m_YelloSqeresWinPointsCount;
            }
        }

        public bool RedTurnToPlayNow
        {
            set
            {
                m_RedTurnToPlayNow = value;   
            }

            get
            {
                return m_RedTurnToPlayNow;
            }
        }

        public bool RedHaveMoreLegalMoves
        {
            set
            {
                m_RedHaveMoreLegalMoves = value;
            }

            get
            {
                return m_RedHaveMoreLegalMoves;
            }
        }

        public bool YelloHaveMoreLegalMoves
        {
            set
            {
                m_YelloHaveMoreLegalMoves = value;
            }

            get
            {
                return m_YelloHaveMoreLegalMoves;
            }
        }

        public OthelloBrain(SquareInMatrix[,] i_Map)
        {
            m_Map = i_Map;
        }

        private void checkWhoWon()
        {
            foreach (SquareInMatrix square in m_Map)
            {
                if (square.Position.Equals(square.FirstColor))
                {
                    m_RedSqeresCount++;
                }
                else if (square.Position.Equals(square.SecondColor))
                {
                    m_YelloSqeresCount++;
                }
            }

            if (m_RedSqeresCount > m_YelloSqeresCount)
            {
                m_RedSqeresWinPointsCount++;
            }
            else if (m_YelloSqeresCount > m_RedSqeresCount)
            {
                m_YelloSqeresWinPointsCount++;
            }
        }

        private void endGame()
        {
            checkWhoWon();
            GameEnd?.Invoke();
        }
        public List<(int, int)> GetGreenSqueresIndexIfHave()
        {
            List<(int, int)> o_IndexList = new List<(int Row, int Col)>(6);
            if (!checkIfHaveAvailableMovesForSpecipicColor(m_Map, m_RedTurnToPlayNow, o_IndexList))
            {
                o_IndexList.Insert(0, (-1, -1));
                if (RedTurnToPlayNow)
                {
                    if (!YelloHaveMoreLegalMoves)
                    {
                        endGame();
                    }
                    else
                    {
                        RedHaveMoreLegalMoves = false;
                    }
                }
                else
                {
                    if (!RedHaveMoreLegalMoves)
                    {
                        endGame();
                    }
                    else
                    { 
                        YelloHaveMoreLegalMoves = false;
                    }
                }
            }

            return o_IndexList;
        }

        public void ChangeTurn()
        {
            RedTurnToPlayNow = !RedTurnToPlayNow;
        }

        private bool checkIfHaveAvailableMovesForSpecipicColor(SquareInMatrix[,] i_Matrix, bool i_BlackTurnToPlayNow, List<(int, int)> i_Indexs)
        {
            bool o_HaveAvailableMoves = false;
            int maxValue = i_Matrix.GetLength(0);

            for (int i = 0; i < maxValue; i++)
            {
                for (int j = 0; j < maxValue; j++)
                {
                    if (checkIfOkAndChangeIfWant(i_Matrix, whereAreWeOnTheMatrix(i_Matrix, i, j),
                                                i, j, i_BlackTurnToPlayNow, false))
                    {
                        i_Indexs.Add((i, j));
                        o_HaveAvailableMoves = true;
                    }
                }
            }

            return o_HaveAvailableMoves;
        }

        internal bool checkIfOkAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_WhereAmI, int i_Row, int i_Col,
                                                     bool i_BlackTurnToPlayNow, bool i_WantToChange)
        {
            int countIfWeMadeCahnges = 0;
            bool o_weMadeChange = false;

            if (i_WhereAmI == 1) //top left edge
            {
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownAndChangeIfWant
                                        (i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckRightAndChangeIfWant
                                        (i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownRightAndChangeIfWant
                                        (i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
            }
            else if (i_WhereAmI == 2) //top right edge
            {
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
            }
            else if (i_WhereAmI == 3)//left edge down
            {
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckRightAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpRightAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
            }
            else if (i_WhereAmI == 4)//right edge down
            {
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
            }
            else if (i_WhereAmI == 5)//first line
            {
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckRightAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownRightAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);

            }
            else if (i_WhereAmI == 6)//first column
            {
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckRightAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownRightAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpRightAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
            }
            else if (i_WhereAmI == 7)//last line
            {
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckRightAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpRightAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
            }
            else if (i_WhereAmI == 8)//last column
            {
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
            }
            else if (i_WhereAmI == 9)//all the others
            {
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckRightAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownRightAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpRightAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckUpLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
                countIfWeMadeCahnges += i_Matrix[i_Row, i_Col].CheckDownLeftAndChangeIfWant(i_Matrix, i_Row, i_Col, i_BlackTurnToPlayNow, i_WantToChange);
            }

            if (countIfWeMadeCahnges > 0)
            {
                o_weMadeChange = true;
            }

            if (i_WantToChange)
            {
                if(i_BlackTurnToPlayNow)
                {
                    i_Matrix[i_Row, i_Col].Position = i_Matrix[i_Row, i_Col].FirstColor;
                }
                else
                {
                    i_Matrix[i_Row, i_Col].Position = i_Matrix[i_Row, i_Col].SecondColor;
                }
            }

            return o_weMadeChange;
        }

        internal int whereAreWeOnTheMatrix(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col)
        {
            int maxSize = i_Matrix.GetLength(0) - 1;
            int o_Answer;

            if (i_Row == 0 && i_Col == 0)//told us that func it to long.
            {
                o_Answer = 1;
            }
            else if (i_Row == 0 && i_Col == maxSize)
            {
                o_Answer = 2;
            }
            else if (i_Row == maxSize && i_Col == 0)
            {
                o_Answer = 3;
            }
            else if (i_Row == maxSize && i_Col == maxSize)
            {
                o_Answer = 4;
            }
            else if (i_Row == 0)
            {
                o_Answer = 5;
            }
            else if (i_Col == 0)
            {
                o_Answer = 6;
            }
            else if (i_Row == maxSize)
            {
                o_Answer = 7;
            }
            else if (i_Col == maxSize)
            {
                o_Answer = 8;
            }
            else
            {
                o_Answer = 9;
            }

            return o_Answer;
        }
    }
}
