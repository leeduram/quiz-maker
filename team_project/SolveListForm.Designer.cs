namespace team_project
{
    partial class SolveListForm
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
            this.Cancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SearchText = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 33F);
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 44);
            this.label1.TabIndex = 1;
            this.label1.Text = "문제 선택";
            this.Cancel.Font = new System.Drawing.Font("굴림", 14F);
            this.Cancel.Location = new System.Drawing.Point(1099, 12);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(153, 44);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "뒤로가기";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 20F);
            this.label2.Location = new System.Drawing.Point(607, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 27);
            this.label2.TabIndex = 4;
            this.label2.Text = "검색:";
            this.SearchText.Font = new System.Drawing.Font("굴림", 20F);
            this.SearchText.Location = new System.Drawing.Point(679, 15);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(414, 38);
            this.SearchText.TabIndex = 5;
            this.SearchText.TextChanged += new System.EventHandler(this.SearchText_TextChanged);
            this.listView1.BackColor = System.Drawing.SystemColors.Window;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listView1.Font = new System.Drawing.Font("굴림", 20F);
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 62);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1264, 619);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.columnHeader1.Text = "문제 제목";
            this.columnHeader1.Width = 550;
            this.columnHeader2.Text = "문제 개수";
            this.columnHeader2.Width = 140;
            this.columnHeader3.Text = "유형";
            this.columnHeader3.Width = 550;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.SearchText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SolveListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "문제은행";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SearchText;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}