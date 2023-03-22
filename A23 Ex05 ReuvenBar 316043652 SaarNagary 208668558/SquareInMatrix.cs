using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Othello
{
    internal class SquareInMatrix : PictureBox
    {
        private String m_Position = "*****";
        private readonly List<String> m_LastMoves = new List<String>();
        private int m_SquareWasChange = 0;

        public SquareInMatrix()
        {
            for (int i = 0; i < 7; i++)
            {
                m_LastMoves.Add(String.Empty);
            }
        }

        public String Position
        {
            get
            {
                return m_Position;
            }

            set 
            {
                if(value == "*****" || value == "White" || value == "Black")
                {
                    m_LastMoves[m_SquareWasChange] = m_Position;
                    SquareWasChange++;
                    m_Position = value;
                }
            } 
        }

        public int SquareWasChange
        {
            get 
            {

                return m_SquareWasChange;
            }

            set 
            {
                if(value == m_SquareWasChange + 1 || value == m_SquareWasChange - 1)
                {
                    m_SquareWasChange = value;
                }
                else 
                {
                    return;
                }

                if(m_SquareWasChange >= m_LastMoves.Count)
                {
                    m_SquareWasChange %= m_LastMoves.Count;
                }
                else if(m_SquareWasChange < 0)
                {
                    m_SquareWasChange = m_LastMoves.Count - 1;
                }
            }
        }

        public String LastMove
        {
            get
            {
                String o_Answer = null;
                
                if (m_SquareWasChange == 0 && m_LastMoves[m_LastMoves.Count - 1].Equals(""))
                {
                    o_Answer = "*****"; 
                }
                else if(m_SquareWasChange == 0)
                {
                    o_Answer = m_LastMoves[m_LastMoves.Count - 1];
                }
                else
                {
                    o_Answer = m_LastMoves[m_SquareWasChange - 1];
                }

                return o_Answer;
            }
        }
            
        internal short CheckRightAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , string i_ColorTurnToPlay, bool i_WantToChange)
        {
            int saveCol = i_Col;
            int maxSize = i_Matrix.GetLength(0) - 1;
            short o_ifWeMadeChanges = 0; 
            bool wasOppositeTool = false;

            if(i_ColorTurnToPlay == "Black" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Col < maxSize && i_Matrix[i_Row, i_Col + 1].Position.Equals("White"))
                {
                    i_Col++;
                    wasOppositeTool = true;
                }

                if(i_Col == maxSize || i_Matrix[i_Row, i_Col + 1].Position.Equals("*****") 
                                    || i_Matrix[i_Row, saveCol +1].Position.Equals("Black"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row, i_Col + 1].Position.Equals("Black"))
                {
                    if(i_WantToChange)
                    {
                        while(saveCol < i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = "Black";
                            i_Col--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(i_ColorTurnToPlay == "White" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Col < maxSize && i_Matrix[i_Row, i_Col + 1].Position.Equals("Black"))
                {
                    i_Col++;
                    wasOppositeTool = true;
                }

                if(i_Col == maxSize || i_Matrix[i_Row, i_Col + 1].Position.Equals("*****")
                                    || i_Matrix[i_Row, saveCol + 1].Position.Equals("White"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row, i_Col + 1].Position.Equals("White"))
                {
                    if(i_WantToChange)
                    {
                        while(saveCol < i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = "White";
                            i_Col--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }

            return o_ifWeMadeChanges;
        }

        internal short CheckLeftAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , string i_ColorTurnToPlay, bool i_WantToChange)
        {
            int saveCol = i_Col;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_ColorTurnToPlay == "Black" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Col > 0 && i_Matrix[i_Row, i_Col - 1].Position.Equals("White"))
                {
                    i_Col--;
                    wasOppositeTool = true;
                }

                if(i_Col == 0 || i_Matrix[i_Row, i_Col - 1].Position.Equals("*****")
                              || i_Matrix[i_Row, saveCol - 1].Position.Equals("Black"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row, i_Col - 1].Position.Equals("Black"))
                {
                    if(i_WantToChange)
                    {
                        while(saveCol > i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = "Black";
                            i_Col++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(i_ColorTurnToPlay == "White" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Col > 0 && i_Matrix[i_Row, i_Col - 1].Position.Equals("Black"))
                {
                    i_Col--;
                    wasOppositeTool = true;
                }

                if(i_Col == 0 || i_Matrix[i_Row, i_Col - 1].Position.Equals("*****")
                              || i_Matrix[i_Row, saveCol - 1].Position.Equals("White"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row, i_Col - 1].Position.Equals("White"))
                {
                    if(i_WantToChange)
                    {
                        while(saveCol > i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = "White";
                            i_Col++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }

            return o_ifWeMadeChanges;
        }

        internal short CheckUpAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , string i_ColorTurnToPlay, bool i_WantToChange)
        {
            int saveRow = i_Row;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_ColorTurnToPlay == "Black" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Row > 0 && i_Matrix[i_Row - 1, i_Col].Position.Equals("White"))
                {
                    i_Row--;
                    wasOppositeTool = true;
                }

                if(i_Row == 0 || i_Matrix[i_Row - 1, i_Col].Position.Equals("*****")
                              || i_Matrix[saveRow - 1, i_Col].Position.Equals("Black"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row - 1, i_Col].Position.Equals("Black"))
                {
                    if(i_WantToChange)
                    {
                        while(saveRow > i_Row)
                        {
                            i_Matrix[i_Row, i_Col].Position = "Black";
                            i_Row++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(i_ColorTurnToPlay == "White" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Row > 0 && i_Matrix[i_Row - 1, i_Col].Position.Equals("Black"))
                {
                    i_Row--;
                    wasOppositeTool = true;
                }

                if(i_Row == 0 || i_Matrix[i_Row - 1, i_Col].Position.Equals("*****")
                              || i_Matrix[saveRow - 1, i_Col].Position.Equals("White"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row - 1, i_Col].Position.Equals("White"))
                {
                    if(i_WantToChange)
                    {
                        while(saveRow > i_Row)
                        {
                            i_Matrix[i_Row, i_Col].Position = "White";
                            i_Row++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }

            return o_ifWeMadeChanges;
        }

        internal short CheckDownAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , string i_ColorTurnToPlay, bool i_WantToChange)
        {
            int saveRow = i_Row;
            int maxSize = i_Matrix.GetLength(0) - 1;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_ColorTurnToPlay == "Black" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Row < maxSize && i_Matrix[i_Row + 1, i_Col].Position.Equals("White"))
                {
                    i_Row++;
                    wasOppositeTool = true;
                }

                if(i_Row == maxSize || i_Matrix[i_Row + 1, i_Col].Position.Equals("*****")
                                    || i_Matrix[saveRow + 1, i_Col].Position.Equals("Black"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row + 1, i_Col].Position.Equals("Black"))
                {
                    if(i_WantToChange)
                    {
                        while(saveRow < i_Row)
                        {
                            i_Matrix[i_Row, i_Col].Position = "Black";
                            i_Row--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(i_ColorTurnToPlay == "White" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Row < maxSize && i_Matrix[i_Row + 1, i_Col].Position.Equals("Black"))
                {
                    i_Row++;
                    wasOppositeTool = true;
                }

                if(i_Row == maxSize || i_Matrix[i_Row + 1, i_Col].Position.Equals("*****")
                                    || i_Matrix[saveRow + 1, i_Col].Position.Equals("White"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row + 1, i_Col].Position.Equals("White"))
                {
                    if(i_WantToChange)
                    {
                        while(saveRow < i_Row)
                        {
                            i_Matrix[i_Row, i_Col].Position = "White";
                            i_Row--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }

            return o_ifWeMadeChanges;
        }

        internal short CheckDownLeftAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , string i_ColorTurnToPlay, bool i_WantToChange)
        {
            int saveCol = i_Col;
            int saveRow = i_Row;
            int maxSize = i_Matrix.GetLength(0) - 1;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_ColorTurnToPlay == "Black" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Row < maxSize && i_Col > 0 && i_Matrix[i_Row + 1, i_Col - 1].Position.Equals("White"))
                {
                    i_Row++;
                    i_Col--;
                    wasOppositeTool = true;
                }

                if(i_Row == maxSize || i_Col == 0 || i_Matrix[i_Row + 1, i_Col - 1].Position.Equals("*****")
                                    || i_Matrix[saveRow + 1, saveCol - 1].Position.Equals("Black"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row + 1, i_Col - 1].Position.Equals("Black"))
                {
                    if(i_WantToChange)
                    {
                        while(saveRow < i_Row && saveCol > i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = "Black";
                            i_Row--;
                            i_Col++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(i_ColorTurnToPlay == "White" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Row < maxSize && i_Col > 0 && i_Matrix[i_Row + 1, i_Col - 1].Position.Equals("Black"))
                {
                    i_Row++;
                    i_Col--;
                    wasOppositeTool = true;
                }

                if(i_Row == maxSize || i_Col == 0 || i_Matrix[i_Row + 1, i_Col - 1].Position.Equals("*****")
                                    || i_Matrix[saveRow + 1, saveCol - 1].Position.Equals("White"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row + 1, i_Col - 1].Position.Equals("White"))
                {
                    if(i_WantToChange)
                    {
                        while(saveRow < i_Row && saveCol > i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = "White";
                            i_Row--;
                            i_Col++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }

            return o_ifWeMadeChanges;
        }

        internal short CheckDownRightAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , string i_ColorTurnToPlay, bool i_WantToChange)
        {
            int saveCol = i_Col;
            int saveRow = i_Row;
            int maxSize = i_Matrix.GetLength(0) - 1;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_ColorTurnToPlay == "Black" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Row < maxSize && i_Col < maxSize && i_Matrix[i_Row + 1, i_Col + 1].Position.Equals("White"))
                {
                    i_Row++;
                    i_Col++;
                    wasOppositeTool = true;
                }

                if(i_Row == maxSize || i_Col == maxSize || i_Matrix[i_Row + 1, i_Col + 1].Position.Equals("*****")
                                    || i_Matrix[saveRow + 1, saveCol + 1].Position.Equals("Black"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row + 1, i_Col + 1].Position.Equals("Black"))
                {
                    if(i_WantToChange)
                    {
                        while(saveRow < i_Row && saveCol < i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = "Black";
                            i_Row--;
                            i_Col--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(i_ColorTurnToPlay == "White" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Row < maxSize && i_Col < maxSize && i_Matrix[i_Row + 1, i_Col + 1].Position.Equals("Black"))
                {
                    i_Row++;
                    i_Col++;
                    wasOppositeTool = true;
                }

                if(i_Row == maxSize || i_Col == maxSize || i_Matrix[i_Row + 1, i_Col + 1].Position.Equals("*****")
                                    || i_Matrix[saveRow + 1, saveCol + 1].Position.Equals("White"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row + 1, i_Col + 1].Position.Equals("White"))
                {
                    if(i_WantToChange)
                    {
                        while(saveRow < i_Row && saveCol < i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = "White";
                            i_Row--;
                            i_Col--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }

            return o_ifWeMadeChanges;
        }

        internal short CheckUpLeftAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , string i_ColorTurnToPlay, bool i_WantToChange)
        {
            int saveCol = i_Col;
            int saveRow = i_Row;
            int maxSize = i_Matrix.GetLength(0) - 1;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_ColorTurnToPlay == "Black" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Row > 0 && i_Col > 0 && i_Matrix[i_Row - 1, i_Col - 1].Position.Equals("White"))
                {
                    i_Row--;
                    i_Col--;
                    wasOppositeTool = true;
                }

                if (i_Row == 0 || i_Col == 0 || i_Matrix[i_Row - 1, i_Col - 1].Position.Equals("*****")
                               || i_Matrix[saveRow - 1, saveCol - 1].Position.Equals("Black"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row - 1, i_Col - 1].Position.Equals("Black"))
                {
                    if(i_WantToChange)
                    {
                        while(saveRow > i_Row && saveCol > i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = "Black";
                            i_Row++;
                            i_Col++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(i_ColorTurnToPlay == "White" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Row > 0 && i_Col > 0 && i_Matrix[i_Row - 1, i_Col - 1].Position.Equals("Black"))
                {
                    i_Row--;
                    i_Col--;
                    wasOppositeTool = true;
                }

                if(i_Row == 0 || i_Col == 0 || i_Matrix[i_Row - 1, i_Col - 1].Position.Equals("*****")
                              || i_Matrix[saveRow - 1, saveCol - 1].Position.Equals("White"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row - 1, i_Col - 1].Position.Equals("White"))
                {
                    if(i_WantToChange)
                    {
                        while(saveRow > i_Row && saveCol > i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = "White";
                            i_Row++;
                            i_Col++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }

            return o_ifWeMadeChanges;
        }

        internal short CheckUpRightAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , string i_ColorTurnToPlay, bool i_WantToChange)
        {
            int saveCol = i_Col;
            int saveRow = i_Row;
            int maxSize = i_Matrix.GetLength(0) - 1;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_ColorTurnToPlay == "Black" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Row > 0 && i_Col < maxSize && i_Matrix[i_Row - 1, i_Col + 1].Position.Equals("White"))
                {
                    i_Row--;
                    i_Col++;
                    wasOppositeTool = true;
                }

                if(i_Row == 0 || i_Col == maxSize || i_Matrix[i_Row - 1, i_Col + 1].Position.Equals("*****")
                              || i_Matrix[saveRow - 1, saveCol + 1].Position.Equals("Black"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row - 1, i_Col + 1].Position.Equals("Black"))
                {
                    if(i_WantToChange)
                    {
                        while(saveRow > i_Row && saveCol < i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = "Black";
                            i_Row++;
                            i_Col--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(i_ColorTurnToPlay == "White" && i_Matrix[i_Row, i_Col].Position.Equals("*****"))
            {
                while(i_Row > 0 && i_Col < maxSize && i_Matrix[i_Row - 1, i_Col + 1].Position.Equals("Black"))
                {
                    i_Row--;
                    i_Col++;
                    wasOppositeTool = true;
                }

                if(i_Row == 0 || i_Col == maxSize || i_Matrix[i_Row - 1, i_Col + 1].Position.Equals("*****")
                              || i_Matrix[saveRow - 1, saveCol + 1].Position.Equals("White"))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row - 1, i_Col + 1].Position.Equals("White"))
                {
                    if(i_WantToChange)
                    {
                        while(saveRow > i_Row && saveCol < i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = "White";
                            i_Row++;
                            i_Col--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }

            return o_ifWeMadeChanges;
        }
    }
}
