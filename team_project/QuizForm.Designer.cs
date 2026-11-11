namespace team_project
{
    partial class QuizForm
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Submit = new System.Windows.Forms.Button();
            this.Before = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.Next = new System.Windows.Forms.Button();
            this.QuizList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Time = new System.Windows.Forms.TextBox();
            this.Problem = new System.Windows.Forms.TextBox();
            this.Quiz = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            this.Submit.Font = new System.Drawing.Font("굴림", 14F);
            this.Submit.Location = new System.Drawing.Point(12, 617);
            this.Submit.Name = "Submit";
            this.Submit.Size = new System.Drawing.Size(216, 52);
            this.Submit.TabIndex = 0;
            this.Submit.Text = "제출하기";
            this.Submit.UseVisualStyleBackColor = true;
            this.Before.Font = new System.Drawing.Font("굴림", 14F);
            this.Before.Location = new System.Drawing.Point(303, 617);
            this.Before.Name = "Before";
            this.Before.Size = new System.Drawing.Size(216, 52);
            this.Before.TabIndex = 1;
            this.Before.Text = "이전 문제";
            this.Before.UseVisualStyleBackColor = true;
            this.Stop.Font = new System.Drawing.Font("굴림", 14F);
            this.Stop.Location = new System.Drawing.Point(525, 617);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(216, 52);
            this.Stop.TabIndex = 2;
            this.Stop.Text = "중단하기";
            this.Stop.UseVisualStyleBackColor = true;
            this.Next.Font = new System.Drawing.Font("굴림", 14F);
            this.Next.Location = new System.Drawing.Point(747, 617);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(216, 52);
            this.Next.TabIndex = 3;
            this.Next.Text = "다음 문제";
            this.Next.UseVisualStyleBackColor = true;
            this.QuizList.Font = new System.Drawing.Font("굴림", 15F);
            this.QuizList.FormattingEnabled = true;
            this.QuizList.ItemHeight = 20;
            this.QuizList.Location = new System.Drawing.Point(12, 64);
            this.QuizList.Name = "QuizList";
            this.QuizList.Size = new System.Drawing.Size(216, 544);
            this.QuizList.TabIndex = 4;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 20F);
            this.label1.Location = new System.Drawing.Point(1011, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 27);
            this.label1.TabIndex = 5;
            this.label1.Text = "TIMER";
            this.Time.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold);
            this.Time.Location = new System.Drawing.Point(1109, 12);
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.Size = new System.Drawing.Size(143, 38);
            this.Time.TabIndex = 6;
            this.Time.Text = "00:00:00";
            this.Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Problem.Font = new System.Drawing.Font("굴림", 20F);
            this.Problem.Location = new System.Drawing.Point(12, 12);
            this.Problem.Name = "Problem";
            this.Problem.ReadOnly = true;
            this.Problem.Size = new System.Drawing.Size(993, 38);
            this.Problem.TabIndex = 7;
            this.Quiz.BackColor = System.Drawing.Color.Transparent;
            this.Quiz.Location = new System.Drawing.Point(234, 64);
            this.Quiz.Name = "Quiz";
            this.Quiz.Size = new System.Drawing.Size(1018, 547);
            this.Quiz.TabIndex = 8;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::team_project.Properties.Resources.back1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.Quiz);
            this.Controls.Add(this.Problem);
            this.Controls.Add(this.Time);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.QuizList);
            this.Controls.Add(this.Next);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.Before);
            this.Controls.Add(this.Submit);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "QuizForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "문제은행";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button Submit;
        private System.Windows.Forms.Button Before;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.ListBox QuizList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Time;
        private System.Windows.Forms.TextBox Problem;
        private System.Windows.Forms.Panel Quiz;
    }
}