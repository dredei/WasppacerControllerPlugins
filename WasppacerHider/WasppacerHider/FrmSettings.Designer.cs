namespace WasppacerHider
{
    partial class FrmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbAlwaysHideWasppacer = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbAlwaysHideWasppacer
            // 
            this.cbAlwaysHideWasppacer.AutoSize = true;
            this.cbAlwaysHideWasppacer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbAlwaysHideWasppacer.Location = new System.Drawing.Point(3, 0);
            this.cbAlwaysHideWasppacer.Name = "cbAlwaysHideWasppacer";
            this.cbAlwaysHideWasppacer.Size = new System.Drawing.Size(191, 17);
            this.cbAlwaysHideWasppacer.TabIndex = 0;
            this.cbAlwaysHideWasppacer.Text = "Постоянно скрывать Wasppacer";
            this.cbAlwaysHideWasppacer.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.Location = new System.Drawing.Point(3, 23);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(191, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Сохранить";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(197, 48);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cbAlwaysHideWasppacer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbAlwaysHideWasppacer;
        private System.Windows.Forms.Button btnOk;
    }
}