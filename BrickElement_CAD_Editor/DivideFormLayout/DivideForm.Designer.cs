namespace App.DivideFormLayout
{
    partial class DivideForm
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
            button1 = new Button();
            textBoxValueX = new TextBox();
            textBoxValueY = new TextBox();
            textBoxValueZ = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(107, 159);
            button1.Name = "button1";
            button1.Size = new Size(100, 23);
            button1.TabIndex = 0;
            button1.Text = "Divide";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBoxValueX
            // 
            textBoxValueX.Location = new Point(107, 29);
            textBoxValueX.Name = "textBoxValueX";
            textBoxValueX.Size = new Size(100, 23);
            textBoxValueX.TabIndex = 1;
            // 
            // textBoxValueY
            // 
            textBoxValueY.Location = new Point(107, 73);
            textBoxValueY.Name = "textBoxValueY";
            textBoxValueY.Size = new Size(100, 23);
            textBoxValueY.TabIndex = 2;
            // 
            // textBoxValueZ
            // 
            textBoxValueZ.Location = new Point(107, 111);
            textBoxValueZ.Name = "textBoxValueZ";
            textBoxValueZ.Size = new Size(100, 23);
            textBoxValueZ.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 32);
            label1.Name = "label1";
            label1.Size = new Size(89, 15);
            label1.TabIndex = 4;
            label1.Text = "Divide by X axis";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 73);
            label2.Name = "label2";
            label2.Size = new Size(89, 15);
            label2.TabIndex = 5;
            label2.Text = "Divide by Y axis";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 114);
            label3.Name = "label3";
            label3.Size = new Size(89, 15);
            label3.TabIndex = 6;
            label3.Text = "Divide by Z axis";
            // 
            // DivideForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(273, 207);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxValueZ);
            Controls.Add(textBoxValueY);
            Controls.Add(textBoxValueX);
            Controls.Add(button1);
            Name = "DivideForm";
            Text = "Brick Element Division";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBoxValueX;
        private TextBox textBoxValueY;
        private TextBox textBoxValueZ;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}