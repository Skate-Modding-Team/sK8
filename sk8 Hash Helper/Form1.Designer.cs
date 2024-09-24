namespace sk8_Hash_Helper
{
    partial class MainWindow
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
            InputBox = new TextBox();
            OutputBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            HashSelector = new ComboBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // InputBox
            // 
            InputBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            InputBox.Location = new Point(12, 27);
            InputBox.Name = "InputBox";
            InputBox.Size = new Size(485, 23);
            InputBox.TabIndex = 0;
            InputBox.TextChanged += InputBox_TextChanged;
            // 
            // OutputBox
            // 
            OutputBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            OutputBox.Location = new Point(12, 71);
            OutputBox.Name = "OutputBox";
            OutputBox.ReadOnly = true;
            OutputBox.Size = new Size(485, 23);
            OutputBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 2;
            label1.Text = "Input:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 53);
            label2.Name = "label2";
            label2.Size = new Size(48, 15);
            label2.TabIndex = 3;
            label2.Text = "Output:";
            // 
            // HashSelector
            // 
            HashSelector.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            HashSelector.FormattingEnabled = true;
            HashSelector.Items.AddRange(new object[] { "Renderware 64 bit FNV", "Renderware 32 bit FNV", "Attribulator Bob Jenkin's lookup8 64 bit", "DJB2 64 bit", "DJB2 32 bit", "Renderware 32 bit FNV (Buffer)", "Renderware 64 bit FNV (Buffer)", "Fast String", "Inverse Fast String" });
            HashSelector.Location = new Point(12, 115);
            HashSelector.Name = "HashSelector";
            HashSelector.Size = new Size(485, 23);
            HashSelector.TabIndex = 4;
            HashSelector.Text = "Select a Hash to Use";
            HashSelector.SelectedIndexChanged += HashSelector_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 97);
            label3.Name = "label3";
            label3.Size = new Size(111, 15);
            label3.TabIndex = 5;
            label3.Text = "Hashing Algorithm:";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(509, 148);
            Controls.Add(label3);
            Controls.Add(HashSelector);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(OutputBox);
            Controls.Add(InputBox);
            Name = "MainWindow";
            Text = "Skate String Hash Helper";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox InputBox;
        private TextBox OutputBox;
        private Label label1;
        private Label label2;
        private ComboBox HashSelector;
        private Label label3;
    }
}
