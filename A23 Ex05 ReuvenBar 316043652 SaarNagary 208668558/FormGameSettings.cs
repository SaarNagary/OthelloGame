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
    public partial class FormGameSettings : Form
    {
        public FormGameSettings()
        {
            InitializeComponent();
        }

        public event Action<Boolean, int> ButtonsPlayVsClicked;

        private void m_FormGameSettings_Load(object sender, EventArgs e)
        {

        }

        private Byte m_BoardSize = 6;
        private void m_ButtonBoardSizeIncrease_Click(object sender, EventArgs e)
        {
            m_BoardSize += 2;
            if (m_BoardSize == 12)
            {
                m_ButtonBoardSizeIncrease.Text = String.Format(
                "Board Size: {0}x{0} (click to decrease)", m_BoardSize);
            }
            else if (m_BoardSize > 12)
            {
                m_BoardSize = 6;
                m_ButtonBoardSizeIncrease.Text = String.Format(
                "Board Size: {0}x{0} (click to increase)", m_BoardSize);
            }
            else
            {
                m_ButtonBoardSizeIncrease.Text = String.Format(
                "Board Size: {0}x{0} (click to increase)", m_BoardSize);
            }
        }

        private void m_ButtonsPlayVs_Click(object sender, EventArgs e)
        {

            ButtonsPlayVsClicked?.Invoke(sender == m_ButtonPlayVsComputer, m_BoardSize);
            this.Close();
        }
    }
}
