namespace team_project
{
    partial class EndForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.EndBtn = new System.Windows.Forms.Button();
            this.ResultBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 40F);
            this.label1.Location = new System.Drawing.Point(345, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(581, 54);
            this.label1.TabIndex = 0;
            this.label1.Text = "제출이 완료되었습니다";
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 30F);
            this.label2.Location = new System.Drawing.Point(485, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(297, 40);
            this.label2.TabIndex = 1;
            this.label2.Text = "수고하셨습니다";
            this.EndBtn.Font = new System.Drawing.Font("굴림", 20F);
            this.EndBtn.Location = new System.Drawing.Point(421, 547);
            this.EndBtn.Name = "EndBtn";
            this.EndBtn.Size = new System.Drawing.Size(186, 59);
            this.EndBtn.TabIndex = 2;
            this.EndBtn.Text = "종료하기";
            this.EndBtn.UseVisualStyleBackColor = true;
            this.EndBtn.Click += new System.EventHandler(this.EndBtn_Click);
            this.ResultBtn.Font = new System.Drawing.Font("굴림", 20F);
            this.ResultBtn.Location = new System.Drawing.Point(659, 547);
            this.ResultBtn.Name = "ResultBtn";
            this.ResultBtn.Size = new System.Drawing.Size(186, 59);
            this.ResultBtn.TabIndex = 3;
            this.ResultBtn.Text = "채점하기";
            this.ResultBtn.UseVisualStyleBackColor = true;
            this.ResultBtn.Click += new System.EventHandler(this.ResultBtn_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::team_project.Properties.Resources.back1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.ResultBtn);
            this.Controls.Add(this.EndBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "EndForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "문제은행";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button EndBtn;
        private System.Windows.Forms.Button ResultBtn;
    }
}