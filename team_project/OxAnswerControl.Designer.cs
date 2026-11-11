namespace team_project
{
    partial class OxAnswerControl
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
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            this.label_Problem.AutoSize = true;
            this.label_Problem.Font = new System.Drawing.Font("굴림", 20F);
            this.label_Problem.Location = new System.Drawing.Point(46, 37);
            this.label_Problem.Name = "label_Problem";
            this.label_Problem.Size = new System.Drawing.Size(100, 27);
            this.label_Problem.TabIndex = 1;
            this.label_Problem.Text = "n. 문제";
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("굴림", 20F);
            this.radioButton1.Location = new System.Drawing.Point(51, 203);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(51, 31);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "O";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("굴림", 20F);
            this.radioButton2.Location = new System.Drawing.Point(239, 203);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 31);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "X";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label_Problem);
            this.Name = "OxAnswerControl";
            this.Size = new System.Drawing.Size(1018, 547);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Problem;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
    }
}
