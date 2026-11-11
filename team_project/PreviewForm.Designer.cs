namespace team_project
{
    partial class PreviewForm
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
            this.Cancel = new System.Windows.Forms.Button();
            this.SolveBtn = new System.Windows.Forms.Button();
            this.FixBtn = new System.Windows.Forms.Button();
            this.Problem = new System.Windows.Forms.TextBox();
            this.PreviewPanel = new System.Windows.Forms.Panel();
            this.PreviewLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.Cancel.Font = new System.Drawing.Font("굴림", 14F);
            this.Cancel.Location = new System.Drawing.Point(1099, 12);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(153, 44);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "뒤로가기";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            this.SolveBtn.Font = new System.Drawing.Font("굴림", 14F);
            this.SolveBtn.Location = new System.Drawing.Point(1072, 617);
            this.SolveBtn.Name = "SolveBtn";
            this.SolveBtn.Size = new System.Drawing.Size(180, 52);
            this.SolveBtn.TabIndex = 6;
            this.SolveBtn.Text = "바로 풀기";
            this.SolveBtn.UseVisualStyleBackColor = true;
            this.SolveBtn.Click += new System.EventHandler(this.SolveBtn_Click);
            this.FixBtn.Font = new System.Drawing.Font("굴림", 14F);
            this.FixBtn.Location = new System.Drawing.Point(12, 617);
            this.FixBtn.Name = "FixBtn";
            this.FixBtn.Size = new System.Drawing.Size(180, 52);
            this.FixBtn.TabIndex = 7;
            this.FixBtn.Text = "수정하기";
            this.FixBtn.UseVisualStyleBackColor = true;
            this.FixBtn.Click += new System.EventHandler(this.EditButton_Click);
            this.Problem.Font = new System.Drawing.Font("굴림", 24F);
            this.Problem.Location = new System.Drawing.Point(12, 12);
            this.Problem.Name = "Problem";
            this.Problem.ReadOnly = true;
            this.Problem.Size = new System.Drawing.Size(968, 44);
            this.Problem.TabIndex = 8;
            this.PreviewPanel.BackColor = System.Drawing.Color.Transparent;
            this.PreviewPanel.Location = new System.Drawing.Point(98, 168);
            this.PreviewPanel.Name = "PreviewPanel";
            this.PreviewPanel.Size = new System.Drawing.Size(1062, 391);
            this.PreviewPanel.TabIndex = 9;
            this.PreviewLabel.AutoSize = true;
            this.PreviewLabel.Font = new System.Drawing.Font("굴림", 30F);
            this.PreviewLabel.Location = new System.Drawing.Point(12, 82);
            this.PreviewLabel.Name = "PreviewLabel";
            this.PreviewLabel.Size = new System.Drawing.Size(305, 40);
            this.PreviewLabel.TabIndex = 10;
            this.PreviewLabel.Text = "총 n문항 유형 : ";
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::team_project.Properties.Resources.back1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.PreviewLabel);
            this.Controls.Add(this.PreviewPanel);
            this.Controls.Add(this.Problem);
            this.Controls.Add(this.FixBtn);
            this.Controls.Add(this.SolveBtn);
            this.Controls.Add(this.Cancel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "PreviewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "문제은행";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button SolveBtn;
        private System.Windows.Forms.Button FixBtn;
        private System.Windows.Forms.TextBox Problem;
        private System.Windows.Forms.Panel PreviewPanel;
        private System.Windows.Forms.Label PreviewLabel;
    }
}