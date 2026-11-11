namespace team_project
{
    partial class ProblemForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Create = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.Type = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(0, 54);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1264, 531);
            this.panel1.TabIndex = 0;
            this.Cancel.Font = new System.Drawing.Font("굴림", 14F);
            this.Cancel.Location = new System.Drawing.Point(1063, 4);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(153, 44);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "생성/수정 취소";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 24F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "문제";
            this.textBox1.Font = new System.Drawing.Font("굴림", 24F);
            this.textBox1.Location = new System.Drawing.Point(86, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(968, 44);
            this.textBox1.TabIndex = 0;
            this.Create.Font = new System.Drawing.Font("굴림", 14F);
            this.Create.Location = new System.Drawing.Point(183, 607);
            this.Create.Name = "Create";
            this.Create.Size = new System.Drawing.Size(180, 52);
            this.Create.TabIndex = 3;
            this.Create.Text = "문항 생성";
            this.Create.UseVisualStyleBackColor = true;
            this.Create.Click += new System.EventHandler(this.Create_Click);
            this.Save.Font = new System.Drawing.Font("굴림", 14F);
            this.Save.Location = new System.Drawing.Point(1072, 607);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(180, 52);
            this.Save.TabIndex = 5;
            this.Save.Text = "저장";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            this.Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Type.Font = new System.Drawing.Font("굴림", 20F);
            this.Type.FormattingEnabled = true;
            this.Type.Items.AddRange(new object[] {
            "단답형",
            "객관형",
            "O / X"});
            this.Type.Location = new System.Drawing.Point(18, 624);
            this.Type.Name = "Type";
            this.Type.Size = new System.Drawing.Size(159, 35);
            this.Type.TabIndex = 6;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Type);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.Create);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ProblemForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "문제은행";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Create;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.ComboBox Type;
    }
}