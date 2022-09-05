namespace Lock_Picking
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.pictureGameField = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureGameField)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureGameField
            // 
            this.pictureGameField.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureGameField.BackColor = System.Drawing.Color.Brown;
            this.pictureGameField.BackgroundImage = global::Lock_Picking.Resource.Lock;
            this.pictureGameField.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureGameField.Location = new System.Drawing.Point(0, 0);
            this.pictureGameField.Name = "pictureGameField";
            this.pictureGameField.Size = new System.Drawing.Size(726, 726);
            this.pictureGameField.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureGameField.TabIndex = 0;
            this.pictureGameField.TabStop = false;
            this.pictureGameField.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureGameField_Paint);
            this.pictureGameField.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureGameField_MouseMove);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 679);
            this.Controls.Add(this.pictureGameField);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "Lock Picking";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureGameField)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureGameField;
        private System.Windows.Forms.Timer timer;
    }
}