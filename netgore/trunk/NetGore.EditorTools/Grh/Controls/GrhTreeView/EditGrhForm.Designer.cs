// ReSharper disable RedundantThisQualifier
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantDelegateCreation
// ReSharper disable RedundantCast

namespace NetGore.EditorTools
{
    partial class EditGrhForm
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



        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbCategorization = new System.Windows.Forms.GroupBox();
            this.txtIndex = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCategory = new GrhDataCategoryTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radioStationary = new System.Windows.Forms.RadioButton();
            this.radioAnimated = new System.Windows.Forms.RadioButton();
            this.gbAnimated = new System.Windows.Forms.GroupBox();
            this.txtFrames = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbWallType = new System.Windows.Forms.ComboBox();
            this.txtWallH = new System.Windows.Forms.TextBox();
            this.txtWallW = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtWallY = new System.Windows.Forms.TextBox();
            this.txtWallX = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lstWalls = new System.Windows.Forms.ListBox();
            this.gbStationary = new System.Windows.Forms.GroupBox();
            this.chkAutoSize = new System.Windows.Forms.CheckBox();
            this.txtH = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtW = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTexture = new NetGore.EditorTools.GrhDataTextureTextBox();
            this.gbCategorization.SuspendLayout();
            this.gbAnimated.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbStationary.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCategorization
            // 
            this.gbCategorization.Controls.Add(this.txtIndex);
            this.gbCategorization.Controls.Add(this.label10);
            this.gbCategorization.Controls.Add(this.txtTitle);
            this.gbCategorization.Controls.Add(this.label2);
            this.gbCategorization.Controls.Add(this.txtCategory);
            this.gbCategorization.Controls.Add(this.label1);
            this.gbCategorization.Location = new System.Drawing.Point(12, 12);
            this.gbCategorization.Name = "gbCategorization";
            this.gbCategorization.Size = new System.Drawing.Size(212, 102);
            this.gbCategorization.TabIndex = 4;
            this.gbCategorization.TabStop = false;
            this.gbCategorization.Text = "Categorization";
            // 
            // txtIndex
            // 
            this.txtIndex.Enabled = false;
            this.txtIndex.Location = new System.Drawing.Point(139, 9);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.Size = new System.Drawing.Size(67, 20);
            this.txtIndex.TabIndex = 9;
            this.txtIndex.TextChanged += new System.EventHandler(this.txtIndex_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(97, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Index:";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(6, 74);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(200, 20);
            this.txtTitle.TabIndex = 7;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            this.txtTitle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTitle_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Title:";
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(6, 35);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(200, 20);
            this.txtCategory.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Category:";
            // 
            // radioStationary
            // 
            this.radioStationary.AutoSize = true;
            this.radioStationary.Location = new System.Drawing.Point(46, 120);
            this.radioStationary.Name = "radioStationary";
            this.radioStationary.Size = new System.Drawing.Size(72, 17);
            this.radioStationary.TabIndex = 6;
            this.radioStationary.TabStop = true;
            this.radioStationary.Text = "Stationary";
            this.radioStationary.UseVisualStyleBackColor = true;
            this.radioStationary.CheckedChanged += new System.EventHandler(this.radioStationary_CheckedChanged);
            // 
            // radioAnimated
            // 
            this.radioAnimated.AutoSize = true;
            this.radioAnimated.Location = new System.Drawing.Point(124, 120);
            this.radioAnimated.Name = "radioAnimated";
            this.radioAnimated.Size = new System.Drawing.Size(69, 17);
            this.radioAnimated.TabIndex = 7;
            this.radioAnimated.TabStop = true;
            this.radioAnimated.Text = "Animated";
            this.radioAnimated.UseVisualStyleBackColor = true;
            this.radioAnimated.CheckedChanged += new System.EventHandler(this.radioAnimated_CheckedChanged);
            // 
            // gbAnimated
            // 
            this.gbAnimated.Controls.Add(this.txtFrames);
            this.gbAnimated.Controls.Add(this.label9);
            this.gbAnimated.Controls.Add(this.txtSpeed);
            this.gbAnimated.Controls.Add(this.label8);
            this.gbAnimated.Location = new System.Drawing.Point(12, 143);
            this.gbAnimated.Name = "gbAnimated";
            this.gbAnimated.Size = new System.Drawing.Size(212, 161);
            this.gbAnimated.TabIndex = 8;
            this.gbAnimated.TabStop = false;
            this.gbAnimated.Text = "Animated Grh";
            // 
            // txtFrames
            // 
            this.txtFrames.Location = new System.Drawing.Point(10, 33);
            this.txtFrames.Multiline = true;
            this.txtFrames.Name = "txtFrames";
            this.txtFrames.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFrames.Size = new System.Drawing.Size(196, 95);
            this.txtFrames.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Frames:";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(94, 134);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(112, 20);
            this.txtSpeed.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(47, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Speed:";
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(34, 310);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(67, 23);
            this.btnAccept.TabIndex = 9;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(107, 310);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(67, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbWallType);
            this.groupBox1.Controls.Add(this.txtWallH);
            this.groupBox1.Controls.Add(this.txtWallW);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtWallY);
            this.groupBox1.Controls.Add(this.txtWallX);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.lstWalls);
            this.groupBox1.Location = new System.Drawing.Point(230, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 321);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Binded Walls";
            // 
            // cmbWallType
            // 
            this.cmbWallType.Enabled = false;
            this.cmbWallType.FormattingEnabled = true;
            this.cmbWallType.Location = new System.Drawing.Point(6, 207);
            this.cmbWallType.Name = "cmbWallType";
            this.cmbWallType.Size = new System.Drawing.Size(163, 21);
            this.cmbWallType.TabIndex = 21;
            this.cmbWallType.SelectedIndexChanged += new System.EventHandler(this.cmbWallType_SelectedIndexChanged);
            // 
            // txtWallH
            // 
            this.txtWallH.Enabled = false;
            this.txtWallH.Location = new System.Drawing.Point(114, 260);
            this.txtWallH.Name = "txtWallH";
            this.txtWallH.Size = new System.Drawing.Size(55, 20);
            this.txtWallH.TabIndex = 20;
            this.txtWallH.TextChanged += new System.EventHandler(this.txtWallH_TextChanged);
            // 
            // txtWallW
            // 
            this.txtWallW.Enabled = false;
            this.txtWallW.Location = new System.Drawing.Point(26, 260);
            this.txtWallW.Name = "txtWallW";
            this.txtWallW.Size = new System.Drawing.Size(55, 20);
            this.txtWallW.TabIndex = 19;
            this.txtWallW.TextChanged += new System.EventHandler(this.txtWallW_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(91, 264);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(18, 13);
            this.label18.TabIndex = 18;
            this.label18.Text = "H:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 264);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(21, 13);
            this.label17.TabIndex = 17;
            this.label17.Text = "W:";
            // 
            // txtWallY
            // 
            this.txtWallY.Enabled = false;
            this.txtWallY.Location = new System.Drawing.Point(114, 234);
            this.txtWallY.Name = "txtWallY";
            this.txtWallY.Size = new System.Drawing.Size(55, 20);
            this.txtWallY.TabIndex = 16;
            this.txtWallY.TextChanged += new System.EventHandler(this.txtWallY_TextChanged);
            // 
            // txtWallX
            // 
            this.txtWallX.Enabled = false;
            this.txtWallX.Location = new System.Drawing.Point(26, 234);
            this.txtWallX.Name = "txtWallX";
            this.txtWallX.Size = new System.Drawing.Size(55, 20);
            this.txtWallX.TabIndex = 15;
            this.txtWallX.TextChanged += new System.EventHandler(this.txtWallX_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(91, 238);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 13);
            this.label16.TabIndex = 14;
            this.label16.Text = "Y:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 238);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 13);
            this.label15.TabIndex = 13;
            this.label15.Text = "X:";
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(94, 292);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(6, 292);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lstWalls
            // 
            this.lstWalls.FormattingEnabled = true;
            this.lstWalls.Location = new System.Drawing.Point(6, 19);
            this.lstWalls.Name = "lstWalls";
            this.lstWalls.Size = new System.Drawing.Size(163, 173);
            this.lstWalls.TabIndex = 1;
            this.lstWalls.SelectedIndexChanged += new System.EventHandler(this.lstWalls_SelectedIndexChanged);
            // 
            // gbStationary
            // 
            this.gbStationary.Controls.Add(this.chkAutoSize);
            this.gbStationary.Controls.Add(this.txtH);
            this.gbStationary.Controls.Add(this.label7);
            this.gbStationary.Controls.Add(this.txtW);
            this.gbStationary.Controls.Add(this.label6);
            this.gbStationary.Controls.Add(this.txtY);
            this.gbStationary.Controls.Add(this.label5);
            this.gbStationary.Controls.Add(this.txtX);
            this.gbStationary.Controls.Add(this.label4);
            this.gbStationary.Controls.Add(this.txtTexture);
            this.gbStationary.Controls.Add(this.label3);
            this.gbStationary.Location = new System.Drawing.Point(12, 143);
            this.gbStationary.Name = "gbStationary";
            this.gbStationary.Size = new System.Drawing.Size(212, 161);
            this.gbStationary.TabIndex = 12;
            this.gbStationary.TabStop = false;
            this.gbStationary.Text = "Stationary Grh";
            // 
            // chkAutoSize
            // 
            this.chkAutoSize.AutoSize = true;
            this.chkAutoSize.Location = new System.Drawing.Point(110, 116);
            this.chkAutoSize.Name = "chkAutoSize";
            this.chkAutoSize.Size = new System.Drawing.Size(96, 17);
            this.chkAutoSize.TabIndex = 15;
            this.chkAutoSize.Text = "Automatic Size";
            this.chkAutoSize.UseVisualStyleBackColor = true;
            this.chkAutoSize.CheckedChanged += new System.EventHandler(this.chkAutoSize_CheckedChanged);
            // 
            // txtH
            // 
            this.txtH.Location = new System.Drawing.Point(144, 90);
            this.txtH.Name = "txtH";
            this.txtH.Size = new System.Drawing.Size(62, 20);
            this.txtH.TabIndex = 14;
            this.txtH.TextChanged += new System.EventHandler(this.txtH_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(120, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "H:";
            // 
            // txtW
            // 
            this.txtW.Location = new System.Drawing.Point(47, 90);
            this.txtW.Name = "txtW";
            this.txtW.Size = new System.Drawing.Size(60, 20);
            this.txtW.TabIndex = 12;
            this.txtW.TextChanged += new System.EventHandler(this.txtW_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "W:";
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(144, 64);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(62, 20);
            this.txtY.TabIndex = 10;
            this.txtY.TextChanged += new System.EventHandler(this.txtY_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(121, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Y:";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(47, 64);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(60, 20);
            this.txtX.TabIndex = 8;
            this.txtX.TextChanged += new System.EventHandler(this.txtX_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "X:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Texture:";
            // 
            // txtTexture
            // 
            this.txtTexture.Location = new System.Drawing.Point(6, 38);
            this.txtTexture.Name = "txtTexture";
            this.txtTexture.Size = new System.Drawing.Size(200, 20);
            this.txtTexture.TabIndex = 6;
            // 
            // EditGrhForm
            // 
            this.ClientSize = new System.Drawing.Size(414, 341);
            this.Controls.Add(this.gbStationary);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.radioAnimated);
            this.Controls.Add(this.radioStationary);
            this.Controls.Add(this.gbCategorization);
            this.Controls.Add(this.gbAnimated);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditGrhForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Grh Data Editor";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.EditGrhForm_Load);
            this.gbCategorization.ResumeLayout(false);
            this.gbCategorization.PerformLayout();
            this.gbAnimated.ResumeLayout(false);
            this.gbAnimated.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbStationary.ResumeLayout(false);
            this.gbStationary.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        private System.Windows.Forms.GroupBox gbCategorization;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label2;
        private GrhDataCategoryTextBox txtCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioStationary;
        private System.Windows.Forms.RadioButton radioAnimated;
        private System.Windows.Forms.GroupBox gbAnimated;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFrames;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtIndex;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ComboBox cmbWallType;
        private System.Windows.Forms.TextBox txtWallH;
        private System.Windows.Forms.TextBox txtWallW;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtWallY;
        private System.Windows.Forms.TextBox txtWallX;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ListBox lstWalls;
        private System.Windows.Forms.GroupBox gbStationary;
        private System.Windows.Forms.CheckBox chkAutoSize;
        private System.Windows.Forms.TextBox txtH;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtW;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label4;
        private GrhDataTextureTextBox txtTexture;
        private System.Windows.Forms.Label label3;

    }
}