namespace ChocolateCHIP
{
    partial class Form1
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
            this.frameBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.debugCheckBox = new System.Windows.Forms.CheckBox();
            this.debugTextBox = new System.Windows.Forms.TextBox();
            this.printDebugCheckBox = new System.Windows.Forms.CheckBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.stepButton = new System.Windows.Forms.Button();
            this.memoryTextBox = new System.Windows.Forms.TextBox();
            this.viewMemoryButton = new System.Windows.Forms.Button();
            this.clockSpdUpDown = new System.Windows.Forms.NumericUpDown();
            this.SetClockSpeedButton = new System.Windows.Forms.Button();
            this.openROMDialog = new System.Windows.Forms.OpenFileDialog();
            this.openROMButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.colorComboBox = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.setThemeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.frameBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clockSpdUpDown)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // frameBox
            // 
            this.frameBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.frameBox.Location = new System.Drawing.Point(15, 9);
            this.frameBox.Name = "frameBox";
            this.frameBox.Size = new System.Drawing.Size(640, 320);
            this.frameBox.TabIndex = 0;
            this.frameBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 326);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Debug Options";
            // 
            // debugCheckBox
            // 
            this.debugCheckBox.AutoSize = true;
            this.debugCheckBox.Checked = true;
            this.debugCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.debugCheckBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.debugCheckBox.Location = new System.Drawing.Point(3, 363);
            this.debugCheckBox.Name = "debugCheckBox";
            this.debugCheckBox.Size = new System.Drawing.Size(129, 15);
            this.debugCheckBox.TabIndex = 2;
            this.debugCheckBox.Text = "Pause execution";
            this.debugCheckBox.UseVisualStyleBackColor = true;
            // 
            // debugTextBox
            // 
            this.debugTextBox.BackColor = System.Drawing.SystemColors.WindowText;
            this.debugTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.debugTextBox.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.debugTextBox.ForeColor = System.Drawing.Color.Lime;
            this.debugTextBox.Location = new System.Drawing.Point(3, 3);
            this.debugTextBox.Multiline = true;
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.Size = new System.Drawing.Size(210, 320);
            this.debugTextBox.TabIndex = 3;
            // 
            // printDebugCheckBox
            // 
            this.printDebugCheckBox.AutoSize = true;
            this.printDebugCheckBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printDebugCheckBox.Location = new System.Drawing.Point(3, 342);
            this.printDebugCheckBox.Name = "printDebugCheckBox";
            this.printDebugCheckBox.Size = new System.Drawing.Size(136, 15);
            this.printDebugCheckBox.TabIndex = 4;
            this.printDebugCheckBox.Text = "Print debug info";
            this.printDebugCheckBox.UseVisualStyleBackColor = true;
            // 
            // resetButton
            // 
            this.resetButton.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetButton.Location = new System.Drawing.Point(3, 384);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(210, 23);
            this.resetButton.TabIndex = 5;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // stepButton
            // 
            this.stepButton.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepButton.Location = new System.Drawing.Point(3, 413);
            this.stepButton.Name = "stepButton";
            this.stepButton.Size = new System.Drawing.Size(210, 23);
            this.stepButton.TabIndex = 6;
            this.stepButton.Text = "Step";
            this.stepButton.UseVisualStyleBackColor = true;
            this.stepButton.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // memoryTextBox
            // 
            this.memoryTextBox.Location = new System.Drawing.Point(3, 471);
            this.memoryTextBox.Name = "memoryTextBox";
            this.memoryTextBox.Size = new System.Drawing.Size(210, 20);
            this.memoryTextBox.TabIndex = 7;
            // 
            // viewMemoryButton
            // 
            this.viewMemoryButton.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewMemoryButton.Location = new System.Drawing.Point(3, 442);
            this.viewMemoryButton.Name = "viewMemoryButton";
            this.viewMemoryButton.Size = new System.Drawing.Size(210, 23);
            this.viewMemoryButton.TabIndex = 8;
            this.viewMemoryButton.Text = "View byte at address";
            this.viewMemoryButton.UseVisualStyleBackColor = true;
            this.viewMemoryButton.Click += new System.EventHandler(this.ViewMemoryButton_Click);
            // 
            // clockSpdUpDown
            // 
            this.clockSpdUpDown.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clockSpdUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.clockSpdUpDown.Location = new System.Drawing.Point(279, 3);
            this.clockSpdUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.clockSpdUpDown.Name = "clockSpdUpDown";
            this.clockSpdUpDown.Size = new System.Drawing.Size(120, 19);
            this.clockSpdUpDown.TabIndex = 9;
            // 
            // SetClockSpeedButton
            // 
            this.SetClockSpeedButton.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetClockSpeedButton.Location = new System.Drawing.Point(116, 3);
            this.SetClockSpeedButton.Name = "SetClockSpeedButton";
            this.SetClockSpeedButton.Size = new System.Drawing.Size(157, 20);
            this.SetClockSpeedButton.TabIndex = 10;
            this.SetClockSpeedButton.Text = "Set clock speed (Hz)";
            this.SetClockSpeedButton.UseVisualStyleBackColor = true;
            this.SetClockSpeedButton.Click += new System.EventHandler(this.SetClockSpeedButton_Click);
            // 
            // openROMDialog
            // 
            this.openROMDialog.FileName = "openFileDialog1";
            // 
            // openROMButton
            // 
            this.openROMButton.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openROMButton.Location = new System.Drawing.Point(3, 3);
            this.openROMButton.Name = "openROMButton";
            this.openROMButton.Size = new System.Drawing.Size(107, 20);
            this.openROMButton.TabIndex = 11;
            this.openROMButton.Text = "Open ROM";
            this.openROMButton.UseVisualStyleBackColor = true;
            this.openROMButton.Click += new System.EventHandler(this.openROMButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.openROMButton);
            this.flowLayoutPanel1.Controls.Add(this.SetClockSpeedButton);
            this.flowLayoutPanel1.Controls.Add(this.clockSpdUpDown);
            this.flowLayoutPanel1.Controls.Add(this.setThemeButton);
            this.flowLayoutPanel1.Controls.Add(this.colorComboBox);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(15, 332);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(640, 31);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // colorComboBox
            // 
            this.colorComboBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorComboBox.FormattingEnabled = true;
            this.colorComboBox.Location = new System.Drawing.Point(503, 3);
            this.colorComboBox.Name = "colorComboBox";
            this.colorComboBox.Size = new System.Drawing.Size(121, 19);
            this.colorComboBox.TabIndex = 12;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.debugTextBox);
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.printDebugCheckBox);
            this.flowLayoutPanel2.Controls.Add(this.debugCheckBox);
            this.flowLayoutPanel2.Controls.Add(this.resetButton);
            this.flowLayoutPanel2.Controls.Add(this.stepButton);
            this.flowLayoutPanel2.Controls.Add(this.viewMemoryButton);
            this.flowLayoutPanel2.Controls.Add(this.memoryTextBox);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(658, 6);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(214, 498);
            this.flowLayoutPanel2.TabIndex = 13;
            // 
            // setThemeButton
            // 
            this.setThemeButton.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setThemeButton.Location = new System.Drawing.Point(405, 3);
            this.setThemeButton.Name = "setThemeButton";
            this.setThemeButton.Size = new System.Drawing.Size(92, 20);
            this.setThemeButton.TabIndex = 13;
            this.setThemeButton.Text = "Set theme";
            this.setThemeButton.UseVisualStyleBackColor = true;
            this.setThemeButton.Click += new System.EventHandler(this.setThemeButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 511);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.frameBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "ChocolateCHIP";
            ((System.ComponentModel.ISupportInitialize)(this.frameBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clockSpdUpDown)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox frameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox debugCheckBox;
        private System.Windows.Forms.TextBox debugTextBox;
        private System.Windows.Forms.CheckBox printDebugCheckBox;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button stepButton;
        private System.Windows.Forms.TextBox memoryTextBox;
        private System.Windows.Forms.Button viewMemoryButton;
        private System.Windows.Forms.NumericUpDown clockSpdUpDown;
        private System.Windows.Forms.Button SetClockSpeedButton;
        private System.Windows.Forms.OpenFileDialog openROMDialog;
        private System.Windows.Forms.Button openROMButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.ComboBox colorComboBox;
        private System.Windows.Forms.Button setThemeButton;
    }
}

