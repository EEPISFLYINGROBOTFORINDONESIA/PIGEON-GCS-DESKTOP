namespace GUI_KRTI_BismillahNyoba
{
    partial class UserControl_Waypoint
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.SaveWP = new System.Windows.Forms.Button();
            this.LoadWP = new System.Windows.Forms.Button();
            this.SendWP = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel15 = new System.Windows.Forms.Panel();
            this.txtLongitude = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.txtLatitude = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.Remove = new System.Windows.Forms.Button();
            this.Line = new System.Windows.Forms.Button();
            this.panel15.SuspendLayout();
            this.panel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.SystemColors.Control;
            this.panel13.Location = new System.Drawing.Point(0, 221);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(24, 438);
            this.panel13.TabIndex = 15;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.SystemColors.Control;
            this.panel12.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel12.Location = new System.Drawing.Point(0, 580);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(1223, 10);
            this.panel12.TabIndex = 14;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.SystemColors.Control;
            this.panel11.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel11.Location = new System.Drawing.Point(1223, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(10, 590);
            this.panel11.TabIndex = 13;
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.SystemColors.Control;
            this.panel14.Location = new System.Drawing.Point(0, 0);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(24, 169);
            this.panel14.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.DarkCyan;
            this.label10.Location = new System.Drawing.Point(239, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 16);
            this.label10.TabIndex = 66;
            this.label10.Text = "EFRISA";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Century Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DarkCyan;
            this.label9.Location = new System.Drawing.Point(41, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(206, 38);
            this.label9.TabIndex = 65;
            this.label9.Text = "FLIGHT PLAN";
            // 
            // gMapControl1
            // 
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapControl1.LevelsKeepInMemmory = 5;
            this.gMapControl1.Location = new System.Drawing.Point(51, 83);
            this.gMapControl1.MarkersEnabled = true;
            this.gMapControl1.MaxZoom = 2;
            this.gMapControl1.MinZoom = 2;
            this.gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl1.Name = "gMapControl1";
            this.gMapControl1.NegativeMode = false;
            this.gMapControl1.PolygonsEnabled = true;
            this.gMapControl1.RetryLoadTile = 0;
            this.gMapControl1.RoutesEnabled = true;
            this.gMapControl1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapControl1.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapControl1.ShowTileGridLines = false;
            this.gMapControl1.Size = new System.Drawing.Size(864, 454);
            this.gMapControl1.TabIndex = 67;
            this.gMapControl1.Zoom = 0D;
            this.gMapControl1.Load += new System.EventHandler(this.gMapControl1_Load);
            this.gMapControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gMapControl1_MouseClick);
            // 
            // SaveWP
            // 
            this.SaveWP.Location = new System.Drawing.Point(932, 83);
            this.SaveWP.Name = "SaveWP";
            this.SaveWP.Size = new System.Drawing.Size(75, 23);
            this.SaveWP.TabIndex = 68;
            this.SaveWP.Text = "Save WP";
            this.SaveWP.UseVisualStyleBackColor = true;
            this.SaveWP.Click += new System.EventHandler(this.SaveWP_Click);
            // 
            // LoadWP
            // 
            this.LoadWP.Location = new System.Drawing.Point(1013, 83);
            this.LoadWP.Name = "LoadWP";
            this.LoadWP.Size = new System.Drawing.Size(75, 23);
            this.LoadWP.TabIndex = 69;
            this.LoadWP.Text = "Load WP";
            this.LoadWP.UseVisualStyleBackColor = true;
            // 
            // SendWP
            // 
            this.SendWP.Location = new System.Drawing.Point(1094, 83);
            this.SendWP.Name = "SendWP";
            this.SendWP.Size = new System.Drawing.Size(75, 23);
            this.SendWP.TabIndex = 70;
            this.SendWP.Text = "Send WP";
            this.SendWP.UseVisualStyleBackColor = true;
            this.SendWP.Click += new System.EventHandler(this.SendWP_Click);
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(932, 113);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(237, 221);
            this.listView1.TabIndex = 71;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel15.Controls.Add(this.txtLongitude);
            this.panel15.Controls.Add(this.label13);
            this.panel15.Location = new System.Drawing.Point(932, 409);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(237, 33);
            this.panel15.TabIndex = 75;
            // 
            // txtLongitude
            // 
            this.txtLongitude.Location = new System.Drawing.Point(91, 6);
            this.txtLongitude.Name = "txtLongitude";
            this.txtLongitude.Size = new System.Drawing.Size(143, 20);
            this.txtLongitude.TabIndex = 11;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label13.Location = new System.Drawing.Point(8, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 14);
            this.label13.TabIndex = 10;
            this.label13.Text = "LONGITUDE";
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel10.Controls.Add(this.txtLatitude);
            this.panel10.Controls.Add(this.label12);
            this.panel10.Location = new System.Drawing.Point(932, 370);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(237, 33);
            this.panel10.TabIndex = 74;
            // 
            // txtLatitude
            // 
            this.txtLatitude.Location = new System.Drawing.Point(91, 6);
            this.txtLatitude.Name = "txtLatitude";
            this.txtLatitude.Size = new System.Drawing.Size(143, 20);
            this.txtLatitude.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label12.Location = new System.Drawing.Point(8, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 14);
            this.label12.TabIndex = 10;
            this.label12.Text = "LATITUDE";
            // 
            // Remove
            // 
            this.Remove.Location = new System.Drawing.Point(932, 340);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(117, 23);
            this.Remove.TabIndex = 76;
            this.Remove.Text = "Remove List";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // Line
            // 
            this.Line.Location = new System.Drawing.Point(1055, 340);
            this.Line.Name = "Line";
            this.Line.Size = new System.Drawing.Size(114, 23);
            this.Line.TabIndex = 77;
            this.Line.Text = "Line";
            this.Line.UseVisualStyleBackColor = true;
            this.Line.Click += new System.EventHandler(this.Line_Click);
            // 
            // UserControl_Waypoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.Line);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.panel15);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.SendWP);
            this.Controls.Add(this.LoadWP);
            this.Controls.Add(this.SaveWP);
            this.Controls.Add(this.gMapControl1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel14);
            this.Controls.Add(this.panel13);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.panel11);
            this.Name = "UserControl_Waypoint";
            this.Size = new System.Drawing.Size(1233, 590);
            this.Load += new System.EventHandler(this.UserControl_Waypoint_Load);
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private System.Windows.Forms.Button SaveWP;
        private System.Windows.Forms.Button LoadWP;
        private System.Windows.Forms.Button SendWP;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.TextBox txtLongitude;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.TextBox txtLatitude;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Button Line;
    }
}
