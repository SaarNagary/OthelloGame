
namespace Othello
{
    partial class FormGameSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_ButtonBoardSizeIncrease = new System.Windows.Forms.Button();
            this.m_ButtonPlayVsComputer = new System.Windows.Forms.Button();
            this.m_ButtonPlayVsFriend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_ButtonBoardSizeIncrease
            // 
            this.m_ButtonBoardSizeIncrease.Location = new System.Drawing.Point(35, 12);
            this.m_ButtonBoardSizeIncrease.Name = "m_ButtonBoardSizeIncrease";
            this.m_ButtonBoardSizeIncrease.Size = new System.Drawing.Size(224, 33);
            this.m_ButtonBoardSizeIncrease.TabIndex = 0;
            this.m_ButtonBoardSizeIncrease.Text = "Board Size: 6x6 (click to increase)";
            this.m_ButtonBoardSizeIncrease.UseVisualStyleBackColor = true;
            this.m_ButtonBoardSizeIncrease.Click += new System.EventHandler(this.m_ButtonBoardSizeIncrease_Click);
            // 
            // m_ButtonPlayVsComputer
            // 
            this.m_ButtonPlayVsComputer.Location = new System.Drawing.Point(35, 51);
            this.m_ButtonPlayVsComputer.Name = "m_ButtonPlayVsComputer";
            this.m_ButtonPlayVsComputer.Size = new System.Drawing.Size(109, 41);
            this.m_ButtonPlayVsComputer.TabIndex = 1;
            this.m_ButtonPlayVsComputer.Text = "Play against the computer";
            this.m_ButtonPlayVsComputer.UseVisualStyleBackColor = true;
            this.m_ButtonPlayVsComputer.Click += new System.EventHandler(this.m_ButtonsPlayVs_Click);
            // 
            // m_ButtonPlayVsFriend
            // 
            this.m_ButtonPlayVsFriend.Location = new System.Drawing.Point(150, 51);
            this.m_ButtonPlayVsFriend.Name = "m_ButtonPlayVsFriend";
            this.m_ButtonPlayVsFriend.Size = new System.Drawing.Size(109, 41);
            this.m_ButtonPlayVsFriend.TabIndex = 2;
            this.m_ButtonPlayVsFriend.Text = "Play against your friend";
            this.m_ButtonPlayVsFriend.UseVisualStyleBackColor = true;
            this.m_ButtonPlayVsFriend.Click += new System.EventHandler(this.m_ButtonsPlayVs_Click);
            // 
            // FormGameSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 114);
            this.Controls.Add(this.m_ButtonPlayVsFriend);
            this.Controls.Add(this.m_ButtonPlayVsComputer);
            this.Controls.Add(this.m_ButtonBoardSizeIncrease);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormGameSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Othello - Game Settings";
            this.Load += new System.EventHandler(this.m_FormGameSettings_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_ButtonBoardSizeIncrease;
        private System.Windows.Forms.Button m_ButtonPlayVsComputer;
        private System.Windows.Forms.Button m_ButtonPlayVsFriend;
    }
}

