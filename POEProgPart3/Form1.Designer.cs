namespace POEProgPart3
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lblTitle = new Label();
            rbtOutput = new RichTextBox();
            txtInput = new TextBox();
            btnSubmit = new Button();
            btnStart = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Engravers MT", 30F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.Yellow;
            lblTitle.Location = new Point(266, 45);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(951, 58);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "CyberSecurity ChatBot";
            // 
            // rbtOutput
            // 
            rbtOutput.BackColor = Color.Black;
            rbtOutput.ForeColor = Color.White;
            rbtOutput.Location = new Point(250, 190);
            rbtOutput.Name = "rbtOutput";
            rbtOutput.Size = new Size(976, 444);
            rbtOutput.TabIndex = 1;
            rbtOutput.Text = "";
            rbtOutput.TextChanged += rbtOutput_TextChanged;
            // 
            // txtInput
            // 
            txtInput.BackColor = Color.Black;
            txtInput.ForeColor = Color.White;
            txtInput.Location = new Point(250, 687);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(480, 27);
            txtInput.TabIndex = 2;
            txtInput.TextChanged += txtInput_TextChanged;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = Color.Black;
            btnSubmit.Font = new Font("Britannic Bold", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSubmit.ForeColor = Color.Yellow;
            btnSubmit.Location = new Point(994, 679);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(232, 35);
            btnSubmit.TabIndex = 3;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.Black;
            btnStart.Font = new Font("Elephant", 10.1999989F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnStart.ForeColor = Color.Yellow;
            btnStart.Location = new Point(631, 123);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(213, 35);
            btnStart.TabIndex = 4;
            btnStart.Text = "Start Program";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1485, 776);
            Controls.Add(btnStart);
            Controls.Add(btnSubmit);
            Controls.Add(txtInput);
            Controls.Add(rbtOutput);
            Controls.Add(lblTitle);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private RichTextBox rbtOutput;
        private TextBox txtInput;
        private Button btnSubmit;
        private Button btnStart;
    }
}
