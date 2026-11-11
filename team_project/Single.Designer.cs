namespace team_project
{
    partial class Single
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
            this.Del = new System.Windows.Forms.Button();
            this.AnswerTxt = new System.Windows.Forms.TextBox();
            this.Num = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.Del.ForeColor = System.Drawing.Color.Red;
            this.Del.Location = new System.Drawing.Point(987, 3);
            this.Del.Name = "Del";
            this.Del.Size = new System.Drawing.Size(59, 24);
            this.Del.TabIndex = 4;
            this.Del.Text = "취소";
            this.Del.UseVisualStyleBackColor = true;
            this.AnswerTxt.Font = new System.Drawing.Font("굴림", 10F);
            this.AnswerTxt.Location = new System.Drawing.Point(50, 4);
            this.AnswerTxt.Name = "AnswerTxt";
            this.AnswerTxt.Size = new System.Drawing.Size(931, 23);
            this.AnswerTxt.TabIndex = 6;
            this.Num.AutoSize = true;
            this.Num.Font = new System.Drawing.Font("굴림", 10F);
            this.Num.Location = new System.Drawing.Point(24, 8);
            this.Num.Name = "Num";
            this.Num.Size = new System.Drawing.Size(12, 14);
            this.Num.TabIndex = 9;
            this.Num.Text = ".";
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Num);
            this.Controls.Add(this.AnswerTxt);
            this.Controls.Add(this.Del);
            this.Name = "Single";
            this.Size = new System.Drawing.Size(1049, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button Del;
        private System.Windows.Forms.TextBox AnswerTxt;
        public System.Windows.Forms.Label Num;
    }
}
