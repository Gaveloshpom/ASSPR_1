using static ASSPR_1.Program;

namespace ASSPR_1
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
            dgvMatrixA = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            dgvVectorB = new DataGridView();
            Column1_Result = new DataGridViewTextBoxColumn();
            btnInverse = new Button();
            btnRank = new Button();
            btnMethod1 = new Button();
            btnMethod2 = new Button();
            btnMethod3 = new Button();
            BtnFill = new Button();
            dgvResult = new DataGridView();
            Column1_dgvResult = new DataGridViewTextBoxColumn();
            numRows = new NumericUpDown();
            numCols = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            btnResize = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvMatrixA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvVectorB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvResult).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRows).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCols).BeginInit();
            SuspendLayout();
            // 
            // dgvMatrixA
            // 
            dgvMatrixA.AllowUserToAddRows = false;
            dgvMatrixA.AllowUserToDeleteRows = false;
            dgvMatrixA.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMatrixA.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMatrixA.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3 });
            dgvMatrixA.Location = new Point(12, 30);
            dgvMatrixA.Name = "dgvMatrixA";
            dgvMatrixA.Size = new Size(240, 150);
            dgvMatrixA.TabIndex = 0;
            // 
            // Column1
            // 
            Column1.HeaderText = "Column1";
            Column1.Name = "Column1";
            // 
            // Column2
            // 
            Column2.HeaderText = "Column2";
            Column2.Name = "Column2";
            // 
            // Column3
            // 
            Column3.HeaderText = "Column3";
            Column3.Name = "Column3";
            // 
            // dgvVectorB
            // 
            dgvVectorB.AllowUserToAddRows = false;
            dgvVectorB.AllowUserToDeleteRows = false;
            dgvVectorB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVectorB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVectorB.Columns.AddRange(new DataGridViewColumn[] { Column1_Result });
            dgvVectorB.GridColor = SystemColors.ButtonFace;
            dgvVectorB.Location = new Point(309, 30);
            dgvVectorB.Name = "dgvVectorB";
            dgvVectorB.Size = new Size(240, 150);
            dgvVectorB.TabIndex = 1;
            dgvVectorB.CellContentClick += dataGridView1_CellContentClick_1;
            // 
            // Column1_Result
            // 
            Column1_Result.HeaderText = "Vector";
            Column1_Result.Name = "Column1_Result";
            // 
            // btnInverse
            // 
            btnInverse.Location = new Point(607, 275);
            btnInverse.Name = "btnInverse";
            btnInverse.Size = new Size(190, 23);
            btnInverse.TabIndex = 2;
            btnInverse.Text = "Обернена матриця";
            btnInverse.UseVisualStyleBackColor = true;
            btnInverse.Click += btnInverse_Click_1;
            // 
            // btnRank
            // 
            btnRank.Location = new Point(607, 304);
            btnRank.Name = "btnRank";
            btnRank.Size = new Size(190, 23);
            btnRank.TabIndex = 3;
            btnRank.Text = "Обчислити ранг";
            btnRank.UseVisualStyleBackColor = true;
            btnRank.Click += btnRank_Click;
            // 
            // btnMethod1
            // 
            btnMethod1.Location = new Point(607, 333);
            btnMethod1.Name = "btnMethod1";
            btnMethod1.Size = new Size(190, 23);
            btnMethod1.TabIndex = 4;
            btnMethod1.Text = "СЛАР: Спосіб 1 (A⁻¹ * B)";
            btnMethod1.UseVisualStyleBackColor = true;
            btnMethod1.Click += btnMethod1_Click;
            // 
            // btnMethod2
            // 
            btnMethod2.Location = new Point(607, 362);
            btnMethod2.Name = "btnMethod2";
            btnMethod2.Size = new Size(190, 23);
            btnMethod2.TabIndex = 5;
            btnMethod2.Text = "СЛАР: Спосіб 2 (AX - B = 0)";
            btnMethod2.UseVisualStyleBackColor = true;
            btnMethod2.Click += btnMethod2_Click;
            // 
            // btnMethod3
            // 
            btnMethod3.Location = new Point(607, 391);
            btnMethod3.Name = "btnMethod3";
            btnMethod3.Size = new Size(190, 23);
            btnMethod3.TabIndex = 6;
            btnMethod3.Text = "СЛАР: Спосіб 3 (Метод Гаусса)";
            btnMethod3.UseVisualStyleBackColor = true;
            btnMethod3.Click += btnMethod3_Click;
            // 
            // BtnFill
            // 
            BtnFill.Location = new Point(607, 221);
            BtnFill.Name = "BtnFill";
            BtnFill.Size = new Size(190, 23);
            BtnFill.TabIndex = 7;
            BtnFill.Text = "Заповнити";
            BtnFill.UseVisualStyleBackColor = true;
            BtnFill.Click += BtnFill_Click;
            // 
            // dgvResult
            // 
            dgvResult.AllowUserToAddRows = false;
            dgvResult.AllowUserToDeleteRows = false;
            dgvResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResult.Columns.AddRange(new DataGridViewColumn[] { Column1_dgvResult });
            dgvResult.Location = new Point(174, 235);
            dgvResult.Name = "dgvResult";
            dgvResult.Size = new Size(240, 150);
            dgvResult.TabIndex = 8;
            // 
            // Column1_dgvResult
            // 
            Column1_dgvResult.HeaderText = "Result";
            Column1_dgvResult.Name = "Column1_dgvResult";
            // 
            // numRows
            // 
            numRows.Location = new Point(662, 73);
            numRows.Name = "numRows";
            numRows.Size = new Size(51, 23);
            numRows.TabIndex = 9;
            // 
            // numCols
            // 
            numCols.Location = new Point(719, 73);
            numCols.Name = "numCols";
            numCols.Size = new Size(51, 23);
            numCols.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(662, 55);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 11;
            label1.Text = "Рядки";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(719, 55);
            label2.Name = "label2";
            label2.Size = new Size(50, 15);
            label2.TabIndex = 12;
            label2.Text = "Стовпці";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(99, 9);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 13;
            label3.Text = "Матриця А";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(405, 9);
            label4.Name = "label4";
            label4.Size = new Size(66, 15);
            label4.TabIndex = 14;
            label4.Text = "Матриця B";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(257, 217);
            label5.Name = "label5";
            label5.Size = new Size(60, 15);
            label5.TabIndex = 15;
            label5.Text = "Результат";
            // 
            // btnResize
            // 
            btnResize.Location = new Point(662, 113);
            btnResize.Name = "btnResize";
            btnResize.Size = new Size(108, 23);
            btnResize.TabIndex = 16;
            btnResize.Text = "Застосувати";
            btnResize.UseVisualStyleBackColor = true;
            btnResize.Click += btnResize_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnResize);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numCols);
            Controls.Add(numRows);
            Controls.Add(dgvResult);
            Controls.Add(BtnFill);
            Controls.Add(btnMethod3);
            Controls.Add(btnMethod2);
            Controls.Add(btnMethod1);
            Controls.Add(btnRank);
            Controls.Add(btnInverse);
            Controls.Add(dgvVectorB);
            Controls.Add(dgvMatrixA);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMatrixA).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvVectorB).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvResult).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRows).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCols).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvMatrixA;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridView dgvVectorB;
        private Button btnInverse;
        private Button btnRank;
        private Button btnMethod1;
        private Button btnMethod2;
        private Button btnMethod3;
        private Button BtnFill;
        private DataGridView dgvResult;
        private DataGridViewTextBoxColumn Column1_dgvResult;
        private DataGridViewTextBoxColumn Column1_Result;
        private NumericUpDown numRows;
        private NumericUpDown numCols;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button btnResize;
    }
}
