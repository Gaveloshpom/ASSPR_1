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
            tabControl1 = new TabControl();
            Part_A = new TabPage();
            Part_B = new TabPage();
            lstConstraints = new ListBox();
            label10 = new Label();
            txtY = new TextBox();
            label9 = new Label();
            txtX = new TextBox();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            nudVarCount = new NumericUpDown();
            btnExample = new Button();
            rbMax = new RadioButton();
            rbMin = new RadioButton();
            btnSolveLP = new Button();
            txtZ = new TextBox();
            dgvConstraints = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvMatrixA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvVectorB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvResult).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRows).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCols).BeginInit();
            tabControl1.SuspendLayout();
            Part_A.SuspendLayout();
            Part_B.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudVarCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvConstraints).BeginInit();
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
            label3.Location = new Point(90, 8);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 13;
            label3.Text = "Матриця А";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(388, 8);
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
            // tabControl1
            // 
            tabControl1.Controls.Add(Part_A);
            tabControl1.Controls.Add(Part_B);
            tabControl1.Location = new Point(-3, 25);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 425);
            tabControl1.TabIndex = 17;
            // 
            // Part_A
            // 
            Part_A.Controls.Add(btnResize);
            Part_A.Controls.Add(label5);
            Part_A.Controls.Add(label4);
            Part_A.Controls.Add(label3);
            Part_A.Controls.Add(label2);
            Part_A.Controls.Add(label1);
            Part_A.Controls.Add(numCols);
            Part_A.Controls.Add(numRows);
            Part_A.Controls.Add(dgvResult);
            Part_A.Controls.Add(BtnFill);
            Part_A.Controls.Add(btnMethod3);
            Part_A.Controls.Add(btnMethod2);
            Part_A.Controls.Add(btnMethod1);
            Part_A.Controls.Add(btnRank);
            Part_A.Controls.Add(btnInverse);
            Part_A.Controls.Add(dgvVectorB);
            Part_A.Controls.Add(dgvMatrixA);
            Part_A.Location = new Point(4, 24);
            Part_A.Name = "Part_A";
            Part_A.Padding = new Padding(3);
            Part_A.Size = new Size(792, 397);
            Part_A.TabIndex = 0;
            Part_A.Text = "Part_A";
            // 
            // Part_B
            // 
            Part_B.Controls.Add(lstConstraints);
            Part_B.Controls.Add(label10);
            Part_B.Controls.Add(txtY);
            Part_B.Controls.Add(label9);
            Part_B.Controls.Add(txtX);
            Part_B.Controls.Add(label8);
            Part_B.Controls.Add(label7);
            Part_B.Controls.Add(label6);
            Part_B.Controls.Add(nudVarCount);
            Part_B.Controls.Add(btnExample);
            Part_B.Controls.Add(rbMax);
            Part_B.Controls.Add(rbMin);
            Part_B.Controls.Add(btnSolveLP);
            Part_B.Controls.Add(txtZ);
            Part_B.Controls.Add(dgvConstraints);
            Part_B.Location = new Point(4, 24);
            Part_B.Name = "Part_B";
            Part_B.Padding = new Padding(3);
            Part_B.Size = new Size(792, 397);
            Part_B.TabIndex = 1;
            Part_B.Text = "Part_B";
            Part_B.UseVisualStyleBackColor = true;
            // 
            // lstConstraints
            // 
            lstConstraints.FormattingEnabled = true;
            lstConstraints.Location = new Point(49, 280);
            lstConstraints.Name = "lstConstraints";
            lstConstraints.Size = new Size(430, 94);
            lstConstraints.TabIndex = 14;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 10F);
            label10.Location = new Point(514, 275);
            label10.Name = "label10";
            label10.Size = new Size(31, 19);
            label10.TabIndex = 13;
            label10.Text = "Y =";
            // 
            // txtY
            // 
            txtY.Location = new Point(551, 275);
            txtY.Name = "txtY";
            txtY.Size = new Size(194, 23);
            txtY.TabIndex = 12;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10F);
            label9.Location = new Point(514, 246);
            label9.Name = "label9";
            label9.Size = new Size(31, 19);
            label9.TabIndex = 11;
            label9.Text = "X =";
            // 
            // txtX
            // 
            txtX.Location = new Point(551, 246);
            txtX.Name = "txtX";
            txtX.Size = new Size(194, 23);
            txtX.TabIndex = 10;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10F);
            label8.Location = new Point(514, 119);
            label8.Name = "label8";
            label8.Size = new Size(119, 19);
            label8.TabIndex = 9;
            label8.Text = "Кількість змінних:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F);
            label7.Location = new Point(49, 95);
            label7.Name = "label7";
            label7.Size = new Size(88, 19);
            label7.TabIndex = 8;
            label7.Text = "Обмеження:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.Location = new Point(49, 69);
            label6.Name = "label6";
            label6.Size = new Size(31, 19);
            label6.TabIndex = 7;
            label6.Text = "Z =";
            // 
            // nudVarCount
            // 
            nudVarCount.Location = new Point(639, 119);
            nudVarCount.Name = "nudVarCount";
            nudVarCount.Size = new Size(106, 23);
            nudVarCount.TabIndex = 6;
            // 
            // btnExample
            // 
            btnExample.Location = new Point(635, 58);
            btnExample.Name = "btnExample";
            btnExample.Size = new Size(110, 41);
            btnExample.TabIndex = 5;
            btnExample.Text = "Приклад";
            btnExample.UseVisualStyleBackColor = true;
            btnExample.Click += btnExample_Click;
            // 
            // rbMax
            // 
            rbMax.AutoSize = true;
            rbMax.Location = new Point(572, 69);
            rbMax.Name = "rbMax";
            rbMax.Size = new Size(47, 19);
            rbMax.TabIndex = 4;
            rbMax.TabStop = true;
            rbMax.Text = "Max";
            rbMax.UseVisualStyleBackColor = true;
            // 
            // rbMin
            // 
            rbMin.AutoSize = true;
            rbMin.Location = new Point(514, 69);
            rbMin.Name = "rbMin";
            rbMin.Size = new Size(46, 19);
            rbMin.TabIndex = 3;
            rbMin.TabStop = true;
            rbMin.Text = "Min";
            rbMin.UseVisualStyleBackColor = true;
            // 
            // btnSolveLP
            // 
            btnSolveLP.Location = new Point(514, 160);
            btnSolveLP.Name = "btnSolveLP";
            btnSolveLP.Size = new Size(231, 57);
            btnSolveLP.TabIndex = 2;
            btnSolveLP.Text = "Знайти оптимальний розв'язок";
            btnSolveLP.UseVisualStyleBackColor = true;
            btnSolveLP.Click += btnSolveLP_Click;
            // 
            // txtZ
            // 
            txtZ.Location = new Point(79, 66);
            txtZ.Name = "txtZ";
            txtZ.Size = new Size(210, 23);
            txtZ.TabIndex = 1;
            // 
            // dgvConstraints
            // 
            dgvConstraints.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvConstraints.Location = new Point(49, 117);
            dgvConstraints.Name = "dgvConstraints";
            dgvConstraints.RowHeadersWidth = 10;
            dgvConstraints.Size = new Size(430, 157);
            dgvConstraints.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMatrixA).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvVectorB).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvResult).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRows).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCols).EndInit();
            tabControl1.ResumeLayout(false);
            Part_A.ResumeLayout(false);
            Part_A.PerformLayout();
            Part_B.ResumeLayout(false);
            Part_B.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudVarCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvConstraints).EndInit();
            ResumeLayout(false);
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
        private TabControl tabControl1;
        private TabPage Part_A;
        private TabPage Part_B;
        private TextBox txtZ;
        private DataGridView dgvConstraints;
        private RadioButton rbMax;
        private RadioButton rbMin;
        private Button btnSolveLP;
        private Button btnExample;
        private NumericUpDown nudVarCount;
        private Label label7;
        private Label label6;
        private Label label10;
        private TextBox txtY;
        private Label label9;
        private TextBox txtX;
        private Label label8;
        private ListBox lstConstraints;
    }
}
