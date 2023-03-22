using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Othello
{
    public class SquareInMatrix : PictureBox
    {
        private const String m_Empty = "*****";
        private const String m_FirstColor = "Red";
        private const String m_SecondColor = "Yello";
        private String m_Position = m_Empty;
        private readonly List<String> m_LastMoves = new List<String>();
        private readonly List<SquareInMatrix> m_SqueresToChange = new List<SquareInMatrix>();
        private int m_SquareWasChange = 0;
        private int m_Row;
        private int m_Col;
        public event Action<SquareInMatrix> ChangeToRed;
        public event Action<SquareInMatrix> ChangeToYello;
        public event Action EndToFlip;
        
        private System.Windows.Forms.Timer timerFlip;
        private bool m_TimerStartWorking = false;


        public String FirstColor
        {
            get
            {
                return m_FirstColor;
            }
        }

        public String SecondColor
        {
            get
            {
                return m_SecondColor;
            }
        }

        public String SquereIsEmpty
        {
            get
            {
                return m_Empty;
            }
        }

        public int Row
        {
            set
            {
                if(value >= 0)
                {
                    m_Row = value;
                }
            }

            get
            {
                return m_Row;
            }
        }

        public int Col
        {
            set
            {
                if (value >= 0)
                {
                    m_Col = value;
                }
            }

            get
            {
                return m_Col;
            }
        }

        public SquareInMatrix()
        {
            for (int i = 0; i < 7; i++)
            {
                m_LastMoves.Add(String.Empty);
            }

            timerFlip = new Timer();
            timerFlip.Interval = 100;
            timerFlip.Tick += this.timer_tick;
        }

        public String Position
        {
            get
            {
                return m_Position;
            }

            set 
            {
                if(value == SquereIsEmpty || value == FirstColor || value == SecondColor)
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
                    o_Answer = m_Empty; 
                }
                else if(m_SquareWasChange == 0)
                {
                    o_Answer = m_LastMoves[m_LastMoves.Count - 2];
                }
                else
                {
                    o_Answer = m_LastMoves[m_SquareWasChange - 2];
                }

                return o_Answer;
            }
        }

        private void timer_tick(object sender, EventArgs e)
        {
            if (m_SqueresToChange.Count != 0)
            {
                if (m_SqueresToChange[0].Position.Equals(FirstColor))
                {
                    ChangeToRed?.Invoke(m_SqueresToChange[0]);
                }
                else
                {
                    ChangeToYello?.Invoke(m_SqueresToChange[0]);
                }

                m_TimerStartWorking = true;
                m_SqueresToChange.RemoveAt(0);
            }
            else if(m_TimerStartWorking)
            {
                timerFlip.Stop();
                EndToFlip?.Invoke();
            }
        }

        internal short CheckRightAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , bool i_BlackTurnToPlayNow, bool i_WantToChange)
        {
            int saveCol = i_Col;
            int maxSize = i_Matrix.GetLength(0) - 1;
            short o_ifWeMadeChanges = 0; 
            bool wasOppositeTool = false;

            if(i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Col < maxSize && i_Matrix[i_Row, i_Col + 1].Position.Equals(SecondColor))
                {
                    i_Col++;
                    wasOppositeTool = true;
                }

                if(i_Col == maxSize || i_Matrix[i_Row, i_Col + 1].Position.Equals(SquereIsEmpty) 
                                    || i_Matrix[i_Row, saveCol +1].Position.Equals(FirstColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row, i_Col + 1].Position.Equals(FirstColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while (saveCol < i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = FirstColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
                            i_Col--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(!i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Col < maxSize && i_Matrix[i_Row, i_Col + 1].Position.Equals(FirstColor))
                {
                    i_Col++;
                    wasOppositeTool = true;
                }

                if(i_Col == maxSize || i_Matrix[i_Row, i_Col + 1].Position.Equals(SquereIsEmpty)
                                    || i_Matrix[i_Row, saveCol + 1].Position.Equals(SecondColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row, i_Col + 1].Position.Equals(SecondColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveCol < i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = SecondColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
                            i_Col--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }

            return o_ifWeMadeChanges;
        }

        internal short CheckLeftAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , bool i_BlackTurnToPlayNow, bool i_WantToChange)
        {
            int saveCol = i_Col;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Col > 0 && i_Matrix[i_Row, i_Col - 1].Position.Equals(SecondColor))
                {
                    i_Col--;
                    wasOppositeTool = true;
                }

                if(i_Col == 0 || i_Matrix[i_Row, i_Col - 1].Position.Equals(SquereIsEmpty)
                              || i_Matrix[i_Row, saveCol - 1].Position.Equals(FirstColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row, i_Col - 1].Position.Equals(FirstColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveCol > i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = FirstColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
                            i_Col++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(!i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Col > 0 && i_Matrix[i_Row, i_Col - 1].Position.Equals(FirstColor))
                {
                    i_Col--;
                    wasOppositeTool = true;
                }

                if(i_Col == 0 || i_Matrix[i_Row, i_Col - 1].Position.Equals(SquereIsEmpty)
                              || i_Matrix[i_Row, saveCol - 1].Position.Equals(SecondColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row, i_Col - 1].Position.Equals(SecondColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveCol > i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = SecondColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
                            i_Col++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }

            return o_ifWeMadeChanges;
        }

        internal short CheckUpAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , bool i_BlackTurnToPlayNow, bool i_WantToChange)
        {
            int saveRow = i_Row;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Row > 0 && i_Matrix[i_Row - 1, i_Col].Position.Equals(SecondColor))
                {
                    i_Row--;
                    wasOppositeTool = true;
                }

                if(i_Row == 0 || i_Matrix[i_Row - 1, i_Col].Position.Equals(SquereIsEmpty)
                              || i_Matrix[saveRow - 1, i_Col].Position.Equals(FirstColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row - 1, i_Col].Position.Equals(FirstColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveRow > i_Row)
                        {
                            i_Matrix[i_Row, i_Col].Position = FirstColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
                            i_Row++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(!i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Row > 0 && i_Matrix[i_Row - 1, i_Col].Position.Equals(FirstColor))
                {
                    i_Row--;
                    wasOppositeTool = true;
                }

                if(i_Row == 0 || i_Matrix[i_Row - 1, i_Col].Position.Equals(SquereIsEmpty)
                              || i_Matrix[saveRow - 1, i_Col].Position.Equals(SecondColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row - 1, i_Col].Position.Equals(SecondColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveRow > i_Row)
                        {
                            i_Matrix[i_Row, i_Col].Position = SecondColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
                            i_Row++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }

            return o_ifWeMadeChanges;
        }

        internal short CheckDownAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , bool i_BlackTurnToPlayNow, bool i_WantToChange)
        {
            int saveRow = i_Row;
            int maxSize = i_Matrix.GetLength(0) - 1;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Row < maxSize && i_Matrix[i_Row + 1, i_Col].Position.Equals(SecondColor))
                {
                    i_Row++;
                    wasOppositeTool = true;
                }

                if(i_Row == maxSize || i_Matrix[i_Row + 1, i_Col].Position.Equals(SquereIsEmpty)
                                    || i_Matrix[saveRow + 1, i_Col].Position.Equals(FirstColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row + 1, i_Col].Position.Equals(FirstColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveRow < i_Row)
                        {
                            i_Matrix[i_Row, i_Col].Position = FirstColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
                            i_Row--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(!i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Row < maxSize && i_Matrix[i_Row + 1, i_Col].Position.Equals(FirstColor))
                {
                    i_Row++;
                    wasOppositeTool = true;
                }

                if(i_Row == maxSize || i_Matrix[i_Row + 1, i_Col].Position.Equals(SquereIsEmpty)
                                    || i_Matrix[saveRow + 1, i_Col].Position.Equals(SecondColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row + 1, i_Col].Position.Equals(SecondColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveRow < i_Row)
                        {
                            i_Matrix[i_Row, i_Col].Position = SecondColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
                            i_Row--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }

            return o_ifWeMadeChanges;
        }

        internal short CheckDownLeftAndChangeIfWant(SquareInMatrix[,] i_Matrix, int i_Row, int i_Col
                                                    , bool i_BlackTurnToPlayNow, bool i_WantToChange)
        {
            int saveCol = i_Col;
            int saveRow = i_Row;
            int maxSize = i_Matrix.GetLength(0) - 1;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Row < maxSize && i_Col > 0 && i_Matrix[i_Row + 1, i_Col - 1].Position.Equals(SecondColor))
                {
                    i_Row++;
                    i_Col--;
                    wasOppositeTool = true;
                }

                if(i_Row == maxSize || i_Col == 0 || i_Matrix[i_Row + 1, i_Col - 1].Position.Equals(SquereIsEmpty)
                                    || i_Matrix[saveRow + 1, saveCol - 1].Position.Equals(FirstColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row + 1, i_Col - 1].Position.Equals(FirstColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveRow < i_Row && saveCol > i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = FirstColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
                            i_Row--;
                            i_Col++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(!i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Row < maxSize && i_Col > 0 && i_Matrix[i_Row + 1, i_Col - 1].Position.Equals(FirstColor))
                {
                    i_Row++;
                    i_Col--;
                    wasOppositeTool = true;
                }

                if(i_Row == maxSize || i_Col == 0 || i_Matrix[i_Row + 1, i_Col - 1].Position.Equals(SquereIsEmpty)
                                    || i_Matrix[saveRow + 1, saveCol - 1].Position.Equals(SecondColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row + 1, i_Col - 1].Position.Equals(SecondColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveRow < i_Row && saveCol > i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = SecondColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
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
                                                    , bool i_BlackTurnToPlayNow, bool i_WantToChange)
        {
            int saveCol = i_Col;
            int saveRow = i_Row;
            int maxSize = i_Matrix.GetLength(0) - 1;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Row < maxSize && i_Col < maxSize && i_Matrix[i_Row + 1, i_Col + 1].Position.Equals(SecondColor))
                {
                    i_Row++;
                    i_Col++;
                    wasOppositeTool = true;
                }

                if(i_Row == maxSize || i_Col == maxSize || i_Matrix[i_Row + 1, i_Col + 1].Position.Equals(SquereIsEmpty)
                                    || i_Matrix[saveRow + 1, saveCol + 1].Position.Equals(FirstColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row + 1, i_Col + 1].Position.Equals(FirstColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveRow < i_Row && saveCol < i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = FirstColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
                            i_Row--;
                            i_Col--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(!i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Row < maxSize && i_Col < maxSize && i_Matrix[i_Row + 1, i_Col + 1].Position.Equals(FirstColor))
                {
                    i_Row++;
                    i_Col++;
                    wasOppositeTool = true;
                }

                if(i_Row == maxSize || i_Col == maxSize || i_Matrix[i_Row + 1, i_Col + 1].Position.Equals(SquereIsEmpty)
                                    || i_Matrix[saveRow + 1, saveCol + 1].Position.Equals(SecondColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row + 1, i_Col + 1].Position.Equals(SecondColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveRow < i_Row && saveCol < i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = SecondColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
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
                                                    , bool i_BlackTurnToPlayNow, bool i_WantToChange)
        {
            int saveCol = i_Col;
            int saveRow = i_Row;
            int maxSize = i_Matrix.GetLength(0) - 1;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Row > 0 && i_Col > 0 && i_Matrix[i_Row - 1, i_Col - 1].Position.Equals(SecondColor))
                {
                    i_Row--;
                    i_Col--;
                    wasOppositeTool = true;
                }

                if (i_Row == 0 || i_Col == 0 || i_Matrix[i_Row - 1, i_Col - 1].Position.Equals(SquereIsEmpty)
                               || i_Matrix[saveRow - 1, saveCol - 1].Position.Equals(FirstColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row - 1, i_Col - 1].Position.Equals(FirstColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveRow > i_Row && saveCol > i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = FirstColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
                            i_Row++;
                            i_Col++;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(!i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Row > 0 && i_Col > 0 && i_Matrix[i_Row - 1, i_Col - 1].Position.Equals(FirstColor))
                {
                    i_Row--;
                    i_Col--;
                    wasOppositeTool = true;
                }

                if(i_Row == 0 || i_Col == 0 || i_Matrix[i_Row - 1, i_Col - 1].Position.Equals(SquereIsEmpty)
                              || i_Matrix[saveRow - 1, saveCol - 1].Position.Equals(SecondColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row - 1, i_Col - 1].Position.Equals(SecondColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveRow > i_Row && saveCol > i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = SecondColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
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
                                                    , bool i_BlackTurnToPlayNow, bool i_WantToChange)
        {
            int saveCol = i_Col;
            int saveRow = i_Row;
            int maxSize = i_Matrix.GetLength(0) - 1;
            short o_ifWeMadeChanges = 0;
            bool wasOppositeTool = false;

            if(i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Row > 0 && i_Col < maxSize && i_Matrix[i_Row - 1, i_Col + 1].Position.Equals(SecondColor))
                {
                    i_Row--;
                    i_Col++;
                    wasOppositeTool = true;
                }

                if(i_Row == 0 || i_Col == maxSize || i_Matrix[i_Row - 1, i_Col + 1].Position.Equals(SquereIsEmpty)
                              || i_Matrix[saveRow - 1, saveCol + 1].Position.Equals(FirstColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row - 1, i_Col + 1].Position.Equals(FirstColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();
                        while(saveRow > i_Row && saveCol < i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = FirstColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
                            i_Row++;
                            i_Col--;
                        }
                    }

                    o_ifWeMadeChanges = 1;
                }
            }
            else if(!i_BlackTurnToPlayNow && i_Matrix[i_Row, i_Col].Position.Equals(SquereIsEmpty))
            {
                while(i_Row > 0 && i_Col < maxSize && i_Matrix[i_Row - 1, i_Col + 1].Position.Equals(FirstColor))
                {
                    i_Row--;
                    i_Col++;
                    wasOppositeTool = true;
                }

                if(i_Row == 0 || i_Col == maxSize || i_Matrix[i_Row - 1, i_Col + 1].Position.Equals(SquereIsEmpty)
                              || i_Matrix[saveRow - 1, saveCol + 1].Position.Equals(SecondColor))
                {
                    o_ifWeMadeChanges = 0;
                }
                else if(wasOppositeTool && i_Matrix[i_Row - 1, i_Col + 1].Position.Equals(SecondColor))
                {
                    if(i_WantToChange)
                    {
                        timerFlip.Start();  
                        while(saveRow > i_Row && saveCol < i_Col)
                        {
                            i_Matrix[i_Row, i_Col].Position = SecondColor;
                            m_SqueresToChange.Add(i_Matrix[i_Row, i_Col]);
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
