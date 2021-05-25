
namespace PasswordManagerUI
{
    partial class MainView
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
            this.passwordLoad = new System.Windows.Forms.Button();
            this.passwordSave = new System.Windows.Forms.Button();
            this.passwordAdd = new System.Windows.Forms.Button();
            this.passwordEdit = new System.Windows.Forms.Button();
            this.passwordListView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // passwordLoad
            // 
            this.passwordLoad.Location = new System.Drawing.Point(12, 12);
            this.passwordLoad.Name = "passwordLoad";
            this.passwordLoad.Size = new System.Drawing.Size(75, 23);
            this.passwordLoad.TabIndex = 1;
            this.passwordLoad.Text = "Load from file";
            this.passwordLoad.UseVisualStyleBackColor = true;
            // 
            // passwordSave
            // 
            this.passwordSave.Location = new System.Drawing.Point(93, 12);
            this.passwordSave.Name = "passwordSave";
            this.passwordSave.Size = new System.Drawing.Size(75, 23);
            this.passwordSave.TabIndex = 2;
            this.passwordSave.Text = "Save to file";
            this.passwordSave.UseVisualStyleBackColor = true;
            // 
            // passwordAdd
            // 
            this.passwordAdd.Location = new System.Drawing.Point(174, 12);
            this.passwordAdd.Name = "passwordAdd";
            this.passwordAdd.Size = new System.Drawing.Size(75, 23);
            this.passwordAdd.TabIndex = 3;
            this.passwordAdd.Text = "Add";
            this.passwordAdd.UseVisualStyleBackColor = true;
            this.passwordAdd.Click += new System.EventHandler(this.passwordAdd_Click);
            // 
            // passwordEdit
            // 
            this.passwordEdit.Location = new System.Drawing.Point(255, 12);
            this.passwordEdit.Name = "passwordEdit";
            this.passwordEdit.Size = new System.Drawing.Size(75, 23);
            this.passwordEdit.TabIndex = 4;
            this.passwordEdit.Text = "Edit";
            this.passwordEdit.UseVisualStyleBackColor = true;
            // 
            // passwordListView
            // 
            this.passwordListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordListView.HideSelection = false;
            this.passwordListView.Location = new System.Drawing.Point(12, 41);
            this.passwordListView.Name = "passwordListView";
            this.passwordListView.Size = new System.Drawing.Size(639, 383);
            this.passwordListView.TabIndex = 5;
            this.passwordListView.UseCompatibleStateImageBehavior = false;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 436);
            this.Controls.Add(this.passwordListView);
            this.Controls.Add(this.passwordEdit);
            this.Controls.Add(this.passwordAdd);
            this.Controls.Add(this.passwordSave);
            this.Controls.Add(this.passwordLoad);
            this.Name = "MainView";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button passwordLoad;
        private System.Windows.Forms.Button passwordSave;
        private System.Windows.Forms.Button passwordAdd;
        private System.Windows.Forms.Button passwordEdit;
        private System.Windows.Forms.ListView passwordListView;
    }
}

