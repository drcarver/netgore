﻿using NetGore.EditorTools;
using NetGore.EditorTools.NPCChat;

namespace DemoGame.NPCChatEditor
{
    partial class frmMain
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
            this.gbSelectedNode = new System.Windows.Forms.GroupBox();
            this.gbConditionals = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbEvaluateType = new System.Windows.Forms.ComboBox();
            this.btnAddConditional = new System.Windows.Forms.Button();
            this.btnDeleteConditional = new System.Windows.Forms.Button();
            this.lstConditionals = new NetGore.EditorTools.NPCChat.NPCChatConditionalsListBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tcChatDialogItem = new System.Windows.Forms.TabControl();
            this.tpDialog = new System.Windows.Forms.TabPage();
            this.chkIsBranch = new System.Windows.Forms.CheckBox();
            this.btnDeleteDialog = new System.Windows.Forms.Button();
            this.btnAddResponse = new System.Windows.Forms.Button();
            this.txtDialogPage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDialogText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tpResponse = new System.Windows.Forms.TabPage();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.btnAddAction = new System.Windows.Forms.Button();
            this.cmbAddAction = new System.Windows.Forms.ComboBox();
            this.lstActions = new System.Windows.Forms.ListBox();
            this.btnAddRedirect = new System.Windows.Forms.Button();
            this.btnAddDialog = new System.Windows.Forms.Button();
            this.txtResponseValue = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDeleteResponse = new System.Windows.Forms.Button();
            this.txtResponseIndex = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tpRedirect = new System.Windows.Forms.TabPage();
            this.btnDeleteRedirect = new System.Windows.Forms.Button();
            this.txtRedirectIndex = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDialogTitle = new System.Windows.Forms.TextBox();
            this.cmbSelectedDialog = new NetGore.EditorTools.NPCChat.NPCChatDialogComboBox();
            this.npcChatDialogView = new NetGore.EditorTools.NPCChat.NPCChatDialogView();
            this.gbSelectedNode.SuspendLayout();
            this.gbConditionals.SuspendLayout();
            this.tcChatDialogItem.SuspendLayout();
            this.tpDialog.SuspendLayout();
            this.tpResponse.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.tpRedirect.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSelectedNode
            // 
            this.gbSelectedNode.Controls.Add(this.gbConditionals);
            this.gbSelectedNode.Controls.Add(this.txtTitle);
            this.gbSelectedNode.Controls.Add(this.label1);
            this.gbSelectedNode.Controls.Add(this.tcChatDialogItem);
            this.gbSelectedNode.Location = new System.Drawing.Point(12, 373);
            this.gbSelectedNode.Name = "gbSelectedNode";
            this.gbSelectedNode.Size = new System.Drawing.Size(815, 246);
            this.gbSelectedNode.TabIndex = 2;
            this.gbSelectedNode.TabStop = false;
            this.gbSelectedNode.Text = "Selected Node";
            // 
            // gbConditionals
            // 
            this.gbConditionals.Controls.Add(this.label8);
            this.gbConditionals.Controls.Add(this.cmbEvaluateType);
            this.gbConditionals.Controls.Add(this.btnAddConditional);
            this.gbConditionals.Controls.Add(this.btnDeleteConditional);
            this.gbConditionals.Controls.Add(this.lstConditionals);
            this.gbConditionals.Location = new System.Drawing.Point(445, 12);
            this.gbConditionals.Name = "gbConditionals";
            this.gbConditionals.Size = new System.Drawing.Size(364, 228);
            this.gbConditionals.TabIndex = 15;
            this.gbConditionals.TabStop = false;
            this.gbConditionals.Text = "Conditionals";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 202);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Evaluation Type:";
            // 
            // cmbEvaluateType
            // 
            this.cmbEvaluateType.FormattingEnabled = true;
            this.cmbEvaluateType.Location = new System.Drawing.Point(99, 199);
            this.cmbEvaluateType.Name = "cmbEvaluateType";
            this.cmbEvaluateType.Size = new System.Drawing.Size(72, 21);
            this.cmbEvaluateType.TabIndex = 22;
            // 
            // btnAddConditional
            // 
            this.btnAddConditional.Location = new System.Drawing.Point(248, 195);
            this.btnAddConditional.Name = "btnAddConditional";
            this.btnAddConditional.Size = new System.Drawing.Size(52, 27);
            this.btnAddConditional.TabIndex = 20;
            this.btnAddConditional.Text = "Add";
            this.btnAddConditional.UseVisualStyleBackColor = true;
            this.btnAddConditional.Click += new System.EventHandler(this.btnAddConditional_Click);
            // 
            // btnDeleteConditional
            // 
            this.btnDeleteConditional.Location = new System.Drawing.Point(306, 195);
            this.btnDeleteConditional.Name = "btnDeleteConditional";
            this.btnDeleteConditional.Size = new System.Drawing.Size(52, 27);
            this.btnDeleteConditional.TabIndex = 19;
            this.btnDeleteConditional.Text = "Delete";
            this.btnDeleteConditional.UseVisualStyleBackColor = true;
            this.btnDeleteConditional.Click += new System.EventHandler(this.btnDeleteConditional_Click);
            // 
            // lstConditionals
            // 
            this.lstConditionals.ConditionalCollection = null;
            this.lstConditionals.EvaluationTypeComboBox = this.cmbEvaluateType;
            this.lstConditionals.FormattingEnabled = true;
            this.lstConditionals.Location = new System.Drawing.Point(6, 19);
            this.lstConditionals.Name = "lstConditionals";
            this.lstConditionals.Size = new System.Drawing.Size(351, 173);
            this.lstConditionals.TabIndex = 8;
            this.lstConditionals.DoubleClick += new System.EventHandler(this.lstConditionals_DoubleClick);
            // 
            // txtTitle
            // 
            this.txtTitle.Enabled = false;
            this.txtTitle.Location = new System.Drawing.Point(43, 19);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(396, 20);
            this.txtTitle.TabIndex = 4;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Title:";
            // 
            // tcChatDialogItem
            // 
            this.tcChatDialogItem.Controls.Add(this.tpDialog);
            this.tcChatDialogItem.Controls.Add(this.tpResponse);
            this.tcChatDialogItem.Controls.Add(this.tpRedirect);
            this.tcChatDialogItem.Location = new System.Drawing.Point(6, 45);
            this.tcChatDialogItem.Name = "tcChatDialogItem";
            this.tcChatDialogItem.SelectedIndex = 0;
            this.tcChatDialogItem.Size = new System.Drawing.Size(433, 195);
            this.tcChatDialogItem.TabIndex = 2;
            // 
            // tpDialog
            // 
            this.tpDialog.Controls.Add(this.chkIsBranch);
            this.tpDialog.Controls.Add(this.btnDeleteDialog);
            this.tpDialog.Controls.Add(this.btnAddResponse);
            this.tpDialog.Controls.Add(this.txtDialogPage);
            this.tpDialog.Controls.Add(this.label5);
            this.tpDialog.Controls.Add(this.txtDialogText);
            this.tpDialog.Controls.Add(this.label3);
            this.tpDialog.Location = new System.Drawing.Point(4, 22);
            this.tpDialog.Name = "tpDialog";
            this.tpDialog.Padding = new System.Windows.Forms.Padding(3);
            this.tpDialog.Size = new System.Drawing.Size(425, 169);
            this.tpDialog.TabIndex = 0;
            this.tpDialog.Text = "Dialog";
            this.tpDialog.UseVisualStyleBackColor = true;
            // 
            // chkIsBranch
            // 
            this.chkIsBranch.AutoSize = true;
            this.chkIsBranch.Location = new System.Drawing.Point(98, 139);
            this.chkIsBranch.Name = "chkIsBranch";
            this.chkIsBranch.Size = new System.Drawing.Size(71, 17);
            this.chkIsBranch.TabIndex = 9;
            this.chkIsBranch.Text = "Is Branch";
            this.chkIsBranch.UseVisualStyleBackColor = true;
            this.chkIsBranch.CheckedChanged += new System.EventHandler(this.chkIsBranch_CheckedChanged);
            // 
            // btnDeleteDialog
            // 
            this.btnDeleteDialog.Location = new System.Drawing.Point(367, 136);
            this.btnDeleteDialog.Name = "btnDeleteDialog";
            this.btnDeleteDialog.Size = new System.Drawing.Size(52, 27);
            this.btnDeleteDialog.TabIndex = 8;
            this.btnDeleteDialog.Text = "Delete";
            this.btnDeleteDialog.UseVisualStyleBackColor = true;
            this.btnDeleteDialog.Click += new System.EventHandler(this.btnDeleteDialog_Click);
            // 
            // btnAddResponse
            // 
            this.btnAddResponse.Location = new System.Drawing.Point(272, 136);
            this.btnAddResponse.Name = "btnAddResponse";
            this.btnAddResponse.Size = new System.Drawing.Size(89, 27);
            this.btnAddResponse.TabIndex = 6;
            this.btnAddResponse.Text = "Add Response";
            this.btnAddResponse.UseVisualStyleBackColor = true;
            this.btnAddResponse.Click += new System.EventHandler(this.btnAddResponse_Click);
            // 
            // txtDialogPage
            // 
            this.txtDialogPage.Enabled = false;
            this.txtDialogPage.Location = new System.Drawing.Point(47, 136);
            this.txtDialogPage.Name = "txtDialogPage";
            this.txtDialogPage.Size = new System.Drawing.Size(45, 20);
            this.txtDialogPage.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Page:";
            // 
            // txtDialogText
            // 
            this.txtDialogText.Location = new System.Drawing.Point(6, 19);
            this.txtDialogText.Multiline = true;
            this.txtDialogText.Name = "txtDialogText";
            this.txtDialogText.Size = new System.Drawing.Size(413, 111);
            this.txtDialogText.TabIndex = 1;
            this.txtDialogText.TextChanged += new System.EventHandler(this.txtDialogText_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Text:";
            // 
            // tpResponse
            // 
            this.tpResponse.Controls.Add(this.gbActions);
            this.tpResponse.Controls.Add(this.btnAddRedirect);
            this.tpResponse.Controls.Add(this.btnAddDialog);
            this.tpResponse.Controls.Add(this.txtResponseValue);
            this.tpResponse.Controls.Add(this.label7);
            this.tpResponse.Controls.Add(this.btnDeleteResponse);
            this.tpResponse.Controls.Add(this.txtResponseIndex);
            this.tpResponse.Controls.Add(this.label2);
            this.tpResponse.Location = new System.Drawing.Point(4, 22);
            this.tpResponse.Name = "tpResponse";
            this.tpResponse.Padding = new System.Windows.Forms.Padding(3);
            this.tpResponse.Size = new System.Drawing.Size(425, 169);
            this.tpResponse.TabIndex = 1;
            this.tpResponse.Text = "Response";
            this.tpResponse.UseVisualStyleBackColor = true;
            // 
            // gbActions
            // 
            this.gbActions.Controls.Add(this.btnAddAction);
            this.gbActions.Controls.Add(this.cmbAddAction);
            this.gbActions.Controls.Add(this.lstActions);
            this.gbActions.Location = new System.Drawing.Point(6, 6);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(198, 157);
            this.gbActions.TabIndex = 12;
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Actions";
            // 
            // btnAddAction
            // 
            this.btnAddAction.Location = new System.Drawing.Point(153, 130);
            this.btnAddAction.Name = "btnAddAction";
            this.btnAddAction.Size = new System.Drawing.Size(39, 21);
            this.btnAddAction.TabIndex = 8;
            this.btnAddAction.Text = "Add";
            this.btnAddAction.UseVisualStyleBackColor = true;
            this.btnAddAction.Click += new System.EventHandler(this.btnAddAction_Click);
            // 
            // cmbAddAction
            // 
            this.cmbAddAction.FormattingEnabled = true;
            this.cmbAddAction.Location = new System.Drawing.Point(6, 130);
            this.cmbAddAction.Name = "cmbAddAction";
            this.cmbAddAction.Size = new System.Drawing.Size(141, 21);
            this.cmbAddAction.TabIndex = 1;
            // 
            // lstActions
            // 
            this.lstActions.FormattingEnabled = true;
            this.lstActions.Location = new System.Drawing.Point(6, 16);
            this.lstActions.Name = "lstActions";
            this.lstActions.Size = new System.Drawing.Size(186, 108);
            this.lstActions.TabIndex = 0;
            this.lstActions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstActions_KeyDown);
            // 
            // btnAddRedirect
            // 
            this.btnAddRedirect.Location = new System.Drawing.Point(210, 136);
            this.btnAddRedirect.Name = "btnAddRedirect";
            this.btnAddRedirect.Size = new System.Drawing.Size(77, 27);
            this.btnAddRedirect.TabIndex = 11;
            this.btnAddRedirect.Text = "Add Redirect";
            this.btnAddRedirect.UseVisualStyleBackColor = true;
            this.btnAddRedirect.Click += new System.EventHandler(this.btnAddRedirect_Click);
            // 
            // btnAddDialog
            // 
            this.btnAddDialog.Location = new System.Drawing.Point(293, 136);
            this.btnAddDialog.Name = "btnAddDialog";
            this.btnAddDialog.Size = new System.Drawing.Size(68, 27);
            this.btnAddDialog.TabIndex = 10;
            this.btnAddDialog.Text = "Add Dialog";
            this.btnAddDialog.UseVisualStyleBackColor = true;
            this.btnAddDialog.Click += new System.EventHandler(this.btnAddDialog_Click);
            // 
            // txtResponseValue
            // 
            this.txtResponseValue.Enabled = false;
            this.txtResponseValue.Location = new System.Drawing.Point(253, 6);
            this.txtResponseValue.Name = "txtResponseValue";
            this.txtResponseValue.Size = new System.Drawing.Size(48, 20);
            this.txtResponseValue.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(210, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Value:";
            // 
            // btnDeleteResponse
            // 
            this.btnDeleteResponse.Location = new System.Drawing.Point(367, 136);
            this.btnDeleteResponse.Name = "btnDeleteResponse";
            this.btnDeleteResponse.Size = new System.Drawing.Size(52, 27);
            this.btnDeleteResponse.TabIndex = 7;
            this.btnDeleteResponse.Text = "Delete";
            this.btnDeleteResponse.UseVisualStyleBackColor = true;
            this.btnDeleteResponse.Click += new System.EventHandler(this.btnDeleteResponse_Click);
            // 
            // txtResponseIndex
            // 
            this.txtResponseIndex.Enabled = false;
            this.txtResponseIndex.Location = new System.Drawing.Point(280, 32);
            this.txtResponseIndex.Name = "txtResponseIndex";
            this.txtResponseIndex.Size = new System.Drawing.Size(48, 20);
            this.txtResponseIndex.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Page Index:";
            // 
            // tpRedirect
            // 
            this.tpRedirect.Controls.Add(this.btnDeleteRedirect);
            this.tpRedirect.Controls.Add(this.txtRedirectIndex);
            this.tpRedirect.Controls.Add(this.label4);
            this.tpRedirect.Location = new System.Drawing.Point(4, 22);
            this.tpRedirect.Name = "tpRedirect";
            this.tpRedirect.Size = new System.Drawing.Size(425, 169);
            this.tpRedirect.TabIndex = 2;
            this.tpRedirect.Text = "Redirect";
            this.tpRedirect.UseVisualStyleBackColor = true;
            // 
            // btnDeleteRedirect
            // 
            this.btnDeleteRedirect.Location = new System.Drawing.Point(367, 139);
            this.btnDeleteRedirect.Name = "btnDeleteRedirect";
            this.btnDeleteRedirect.Size = new System.Drawing.Size(52, 27);
            this.btnDeleteRedirect.TabIndex = 10;
            this.btnDeleteRedirect.Text = "Delete";
            this.btnDeleteRedirect.UseVisualStyleBackColor = true;
            this.btnDeleteRedirect.Click += new System.EventHandler(this.btnDeleteRedirect_Click);
            // 
            // txtRedirectIndex
            // 
            this.txtRedirectIndex.Location = new System.Drawing.Point(45, 6);
            this.txtRedirectIndex.Name = "txtRedirectIndex";
            this.txtRedirectIndex.Size = new System.Drawing.Size(48, 20);
            this.txtRedirectIndex.TabIndex = 6;
            this.txtRedirectIndex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRedirectIndex_KeyDown);
            this.txtRedirectIndex.Leave += new System.EventHandler(this.txtRedirectIndex_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Index:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(817, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(19, 89);
            this.button1.TabIndex = 8;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(473, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Title:";
            // 
            // txtDialogTitle
            // 
            this.txtDialogTitle.Location = new System.Drawing.Point(509, 8);
            this.txtDialogTitle.Name = "txtDialogTitle";
            this.txtDialogTitle.Size = new System.Drawing.Size(302, 20);
            this.txtDialogTitle.TabIndex = 11;
            this.txtDialogTitle.TextChanged += new System.EventHandler(this.txtDialogTitle_TextChanged);
            // 
            // cmbSelectedDialog
            // 
            this.cmbSelectedDialog.FormattingEnabled = true;
            this.cmbSelectedDialog.Location = new System.Drawing.Point(12, 8);
            this.cmbSelectedDialog.Name = "cmbSelectedDialog";
            this.cmbSelectedDialog.Size = new System.Drawing.Size(455, 21);
            this.cmbSelectedDialog.TabIndex = 9;
            this.cmbSelectedDialog.OnChangeDialog += new NetGore.EditorTools.NPCChat.NPCChatDialogComboBoxChangeDialogHandler(this.cmbSelectedDialog_OnChangeDialog);
            // 
            // npcChatDialogView
            // 
            this.npcChatDialogView.Location = new System.Drawing.Point(12, 35);
            this.npcChatDialogView.Name = "npcChatDialogView";
            this.npcChatDialogView.NodeForeColorBranch = System.Drawing.Color.DarkRed;
            this.npcChatDialogView.NodeForeColorGoTo = System.Drawing.Color.Blue;
            this.npcChatDialogView.NodeForeColorNormal = System.Drawing.Color.Black;
            this.npcChatDialogView.NodeForeColorResponse = System.Drawing.Color.Green;
            this.npcChatDialogView.NPCChatDialog = null;
            this.npcChatDialogView.Size = new System.Drawing.Size(815, 332);
            this.npcChatDialogView.TabIndex = 0;
            this.npcChatDialogView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.npcChatDialogView_NodeMouseClick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 628);
            this.Controls.Add(this.txtDialogTitle);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbSelectedDialog);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gbSelectedNode);
            this.Controls.Add(this.npcChatDialogView);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NPC Chat Editor";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.gbSelectedNode.ResumeLayout(false);
            this.gbSelectedNode.PerformLayout();
            this.gbConditionals.ResumeLayout(false);
            this.gbConditionals.PerformLayout();
            this.tcChatDialogItem.ResumeLayout(false);
            this.tpDialog.ResumeLayout(false);
            this.tpDialog.PerformLayout();
            this.tpResponse.ResumeLayout(false);
            this.tpResponse.PerformLayout();
            this.gbActions.ResumeLayout(false);
            this.tpRedirect.ResumeLayout(false);
            this.tpRedirect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NPCChatDialogView npcChatDialogView;
        private System.Windows.Forms.GroupBox gbSelectedNode;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tcChatDialogItem;
        private System.Windows.Forms.TabPage tpDialog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tpResponse;
        private System.Windows.Forms.TabPage tpRedirect;
        private System.Windows.Forms.TextBox txtDialogText;
        private System.Windows.Forms.TextBox txtResponseIndex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRedirectIndex;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDialogPage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddResponse;
        private System.Windows.Forms.Button btnDeleteResponse;
        private System.Windows.Forms.Button button1;
        private NPCChatDialogComboBox cmbSelectedDialog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDialogTitle;
        private System.Windows.Forms.TextBox txtResponseValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnDeleteDialog;
        private System.Windows.Forms.Button btnDeleteRedirect;
        private System.Windows.Forms.Button btnAddDialog;
        private System.Windows.Forms.Button btnAddRedirect;
        private System.Windows.Forms.GroupBox gbConditionals;
        private System.Windows.Forms.Button btnAddConditional;
        private System.Windows.Forms.Button btnDeleteConditional;
        private NPCChatConditionalsListBox lstConditionals;
        private System.Windows.Forms.ComboBox cmbEvaluateType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkIsBranch;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.ComboBox cmbAddAction;
        private System.Windows.Forms.ListBox lstActions;
        private System.Windows.Forms.Button btnAddAction;

    }
}

