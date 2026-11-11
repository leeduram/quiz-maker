namespace team_project
{
    partial class ShortAnswerControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        private void InitializeComponent()
        {
            this.label_Problem = new System.Windows.Forms.Label();
            this.textBox_Answer = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            this.label_Problem.AutoSize = true;
            this.label_Problem.Font = new System.Drawing.Font("굴림", 20F);
            this.label_Problem.Location = new System.Drawing.Point(46, 37);
            this.label_Problem.Name = "label_Problem";
            this.label_Problem.Size = new System.Drawing.Size(100, 27);
            this.label_Problem.TabIndex = 0;
            this.label_Problem.Text = "n. 문제";
            this.textBox_Answer.Font = new System.Drawing.Font("굴림", 20F);
            this.textBox_Answer.Location = new System.Drawing.Point(51, 203);
            this.textBox_Answer.Name = "textBox_Answer";
            this.textBox_Answer.Size = new System.Drawing.Size(505, 38);
            this.textBox_Answer.TabIndex = 1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_Answer);
            this.Controls.Add(this.label_Problem);
            this.Name = "ShortAnswerControl";
            this.Size = new System.Drawing.Size(1018, 547);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Problem;
        private System.Windows.Forms.TextBox textBox_Answer;
    }
}
