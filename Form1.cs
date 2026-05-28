using Microsoft.VisualBasic.Logging;
using System.Text;
using static ASSPR_1.Program;

namespace ASSPR_1
{
    public partial class Form1 : Form
    {
        private double[] globalP;
        private double[] globalQ;
        private double[,] globalMatrix;

        public Form1()
        {
            Console.OutputEncoding = Encoding.UTF8;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvSimulation.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void ShowMatrixInGrid(double[,] matrix, DataGridView grid)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);

            grid.RowCount = n;
            grid.ColumnCount = m;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    // Округлимо до 3 знаків для краси
                    grid.Rows[i].Cells[j].Value = System.Math.Round(matrix[i, j], 3);
                }
            }
        }

        // Метод для зчитування даних з DataGridView у масив double[,]
        private double[,] GetMatrixFromGrid(DataGridView grid)
        {
            int n = grid.RowCount;
            int m = grid.ColumnCount;
            double[,] matrix = new double[n, m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    matrix[i, j] = Convert.ToDouble(grid.Rows[i].Cells[j].Value);
                }
            }
            return matrix;
        }

        private double[] GetVectorFromGrid(DataGridView grid)
        {
            int n = grid.RowCount;
            double[] vector = new double[n];

            for (int i = 0; i < n; i++)
            {
                // Зчитуємо значення з єдиної колонки (індекс 0)
                vector[i] = Convert.ToDouble(grid.Rows[i].Cells[0].Value);
            }
            return vector;
        }

        private void FillVariant15()
        {
            dgvMatrixA.RowCount = 3;
            dgvMatrixA.ColumnCount = 3;

            dgvVectorB.RowCount = 3;
            dgvVectorB.ColumnCount = 1;

            // Матриця A 
            double[,] a = { { 3, 5, 1 }, { -2, 2, -3 }, { 1, 3, -2 } };
            // Вектор B
            double[] b = { 1, 7, 4 };

            for (int i = 0; i < 3; i++)
            {
                dgvVectorB.Rows[i].Cells[0].Value = b[i];
                for (int j = 0; j < 3; j++)
                    dgvMatrixA.Rows[i].Cells[j].Value = a[i, j];
            }
        }

        private void BtnFill_Click(object sender, EventArgs e)
        {
            FillVariant15();
        }

        private void btnMethod2_Click(object sender, EventArgs e)
        {
            try
            {
                double[,] A = GetMatrixFromGrid(dgvMatrixA);
                double[] B = GetVectorFromGrid(dgvVectorB);

                double[] results = MathHelper.SolveMethod2(A, B, out string log);

                dgvResult.RowCount = results.Length;
                dgvResult.ColumnCount = 1;
                for (int i = 0; i < results.Length; i++)
                    dgvResult.Rows[i].Cells[0].Value = Math.Round(results[i], 3);

                SaveLogToFile(log);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnInverse_Click(object sender, EventArgs e)
        {
            try
            {
                double[,] A = GetMatrixFromGrid(dgvMatrixA);

                double[,] invA = MathHelper.Inverse(A, out string log);

                ShowMatrixInGrid(invA, dgvResult);
                SaveLogToFile(log);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }

        private void btnRank_Click(object sender, EventArgs e)
        {
            try
            {
                double[,] A = GetMatrixFromGrid(dgvMatrixA);

                int rank = MathHelper.Rank(A, out string log);

                MessageBox.Show($"Ранг матриці А дорівнює: {rank}", "Результат");
                SaveLogToFile(log);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }

        private void btnMethod1_Click(object sender, EventArgs e)
        {
            try
            {
                double[,] A = GetMatrixFromGrid(dgvMatrixA);
                double[] B = GetVectorFromGrid(dgvVectorB);

                double[] X = Program.MathHelper.SolveMethod1(A, B, out string log);

                double[,] resultMatrix = new double[X.Length, 1];
                for (int i = 0; i < X.Length; i++) resultMatrix[i, 0] = X[i];

                ShowMatrixInGrid(resultMatrix, dgvResult);

                SaveLogToFile(log);
            }
            catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
        }

        private void btnMethod3_Click(object sender, EventArgs e)
        {
            try
            {
                double[,] A = GetMatrixFromGrid(dgvMatrixA);
                double[] B = GetVectorFromGrid(dgvVectorB);

                double[] X = MathHelper.SolveMethod3(A, B, out string log);

                double[,] resultMatrix = new double[X.Length, 1];
                for (int i = 0; i < X.Length; i++) resultMatrix[i, 0] = X[i];

                ShowMatrixInGrid(resultMatrix, dgvResult);
                SaveLogToFile(log);
            }
            catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            int rows = (int)numRows.Value;
            int cols = (int)numCols.Value;

            dgvMatrixA.RowCount = rows;
            dgvMatrixA.ColumnCount = cols;

            dgvVectorB.RowCount = rows;
            dgvVectorB.ColumnCount = 1;

            dgvResult.RowCount = rows;
            dgvResult.ColumnCount = 1;

            for (int j = 0; j < cols; j++)
            {
                dgvMatrixA.Columns[j].HeaderText = $"x{j + 1}";
                dgvMatrixA.Columns[j].Width = 50;
            }
        }

        //Part_B
        // Обробник для кнопки "Приклад"
        private void btnExample_Click(object sender, EventArgs e)
        {
            //nudVarCount.Value = 4;
            //txtZ.Text = "x1+2x2-x3-x4";
            //rbMin.Checked = false;

            //dgvConstraints_B.Rows.Clear();
            //dgvConstraints_B.Rows.Add("x1+x2-x3-2x4<=6");
            //dgvConstraints_B.Rows.Add("x1+x2+x3-x4>=5");
            //dgvConstraints_B.Rows.Add("2x1-x2+3x3+4x4<=10");

            //txtX.Text = "";
            //txtY.Text = "";

            //nudVarCount.Value = 4;
            //txtZ.Text = "-2x1+3x2-3x4";
            //rbMin.Checked = true;

            //dgvConstraints_B.Rows.Clear();
            //dgvConstraints_B.Rows.Add("x1+x2-x3-2x4<=6");
            //dgvConstraints_B.Rows.Add("x1+x2+x3-x4>=5");
            //dgvConstraints_B.Rows.Add("2x1-x2+3x3+4x4<=10");

            //txtX.Text = "";
            //txtY.Text = "";

            //nudVarCount.Value = 4;
            //txtZ.Text = "x1+x3+x6";
            //rbMin.Checked = false;

            //dgvConstraints_B.Rows.Clear();
            //dgvConstraints_B.Rows.Add("x1+x2+x3+x4+x5+3x6<=4");
            //dgvConstraints_B.Rows.Add("x1-4x2+x4+10x5-x6<=5");
            //dgvConstraints_B.Rows.Add("x1-3x2+7x3+x4+15x5-x6<=2");

            //txtX.Text = "";
            //txtY.Text = "";

            //Part_2
            nudVarCount.Value = 2;
            txtZ.Text = "-1x1+5x2";
            rbMin.Checked = true;

            dgvConstraints_B.Rows.Clear();
            dgvConstraints_B.Rows.Add("x1+x2-x3-3>=0");
            dgvConstraints_B.Rows.Add("-1x1+2x2-1x4-1>=0");

            txtX.Text = "";
            txtY.Text = "";
        }

        // Обробник для кнопки "Знайти оптимальний розв'язок"
        private void btnSolveLP_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Збираємо дані з форми
                string zExpr = txtZ.Text;
                int varCount = (int)nudVarCount.Value;
                bool isMin = rbMin.Checked;

                if (string.IsNullOrWhiteSpace(txtZ.Text))
                    throw new Exception("Введіть цільову функцію Z.");
                if (dgvConstraints_B.Rows.Count == 0)
                    throw new Exception("Введіть хоча б одне обмеження.");

                List<string> constraintLines = new List<string>();
                foreach (DataGridViewRow row in dgvConstraints_B.Rows)
                {
                    if (row.Cells[0].Value != null)
                        constraintLines.Add(row.Cells[0].Value.ToString());
                }

                StringBuilder fullLog = new StringBuilder();
                fullLog.AppendLine("Згенерований протокол обчислення:\n");
                fullLog.AppendLine("Постановка задачі:\n");
                fullLog.AppendLine($"Z = {zExpr} -> {(isMin ? "min" : "max")}\n");
                fullLog.AppendLine("при обмеженнях:\n");
                foreach (var c in constraintLines) fullLog.AppendLine(c);
                fullLog.AppendLine($"\nx[j]>=0, j=1,{varCount}\n");

                // 2. Будуємо початкову таблицю
                int[] rowVars, colVars;
                double[,] table = MathHelper.BuildInitialTable(zExpr, constraintLines, varCount, isMin, out rowVars, out colVars);

                fullLog.AppendLine("Вхідна симплекс-таблиця:");
                fullLog.Append(MathHelper.PrintTableToLog(table, rowVars, colVars, varCount));

                // 3. Шукаємо опорний розв'язок
                string stepLog;
                table = MathHelper.FindFeasibleSolution(table, ref rowVars, ref colVars, varCount, out stepLog);
                fullLog.Append(stepLog); // Додаємо лог опорного рішення

                if (table != null)
                {
                    fullLog.AppendLine("Знайдено опорний розв'язок:\n");
                    fullLog.AppendLine(MathHelper.GetXVectorString(table, rowVars, colVars, varCount) + "\n");

                    // 4. Шукаємо оптимальний розв'язок
                    table = MathHelper.FindOptimalSolution(table, ref rowVars, ref colVars, varCount, out stepLog);
                    fullLog.Append(stepLog); // Додаємо лог оптимального рішення

                    if (table != null)
                    {
                        fullLog.AppendLine("Знайдено оптимальний розв'язок:\n");
                        fullLog.AppendLine(MathHelper.GetXVectorString(table, rowVars, colVars, varCount) + "\n");

                        double zRaw = table[table.GetLength(0) - 1, table.GetLength(1) - 1];
                        double zValue = isMin ? -zRaw : zRaw;
                        fullLog.AppendLine($"{(isMin ? "Min" : "Max")} (Z) = {zValue:F2}");

                        DisplayFinalResult(table, rowVars, colVars, varCount, isMin, txtX, txtY);
                    }
                }

                // Зберігаємо повний сформований лог
                SaveLogToFile(fullLog.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Common
        private void SaveLogToFile(string logContent)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = "Protocol.txt";

                File.WriteAllText(sfd.FileName, logContent, Encoding.UTF8);
            }
        }

        private void DisplayFinalResult(double[,] table, int[] rowVars, int[] colVars, int varCount, bool isMin, TextBox txtX, TextBox txtZ)
        {
            int rows = table.GetLength(0);
            int cols = table.GetLength(1);
            int zRowIdx = rows - 1;
            int rhsCol = cols - 1;

            // Збираємо значення змінних x1...xN
            double[] xValues = new double[varCount];

            for (int i = 0; i < rows - 1; i++)
            {
                int varIndex = rowVars[i]; // наприклад, 1 -> x1, 2 -> x2
                if (varIndex >= 1 && varIndex <= varCount)
                {
                    xValues[varIndex - 1] = table[i, rhsCol];
                }
            }

            // Змінні у стовпцях (небазисні) = 0, але перевіряємо на всяк випадок
            for (int j = 0; j < cols - 1; j++)
            {
                int varIndex = colVars[j];
                if (varIndex >= 1 && varIndex <= varCount)
                {
                    xValues[varIndex - 1] = 0;
                }
            }

            // Формуємо рядок x1=..., x2=...
            StringBuilder sbX = new StringBuilder();
            for (int k = 0; k < varCount; k++)
            {
                sbX.AppendLine($"{xValues[k]:F1}; ");
            }
            txtX.Text = $"({sbX.ToString().Trim()})";

            // Значення Z: у таблиці зберігається -Zmin (або Zmax), тому при мін множимо на -1
            double zRaw = table[zRowIdx, rhsCol];
            double zValue = isMin ? -zRaw : zRaw;
            txtZ.Text = $"Z = {zValue:F1}";
        }

        //Part_C

        private void btnSolveLP_C_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Збираємо дані з форми
                string zExpr_С = txtZ_C.Text;
                int varCount_С = (int)nudVarCount_C.Value;
                bool isMin_С = rbMin_C.Checked;

                if (string.IsNullOrWhiteSpace(txtZ_C.Text))
                    throw new Exception("Введіть цільову функцію Z.");
                if (dgvConstraints_C.Rows.Count == 0)
                    throw new Exception("Введіть хоча б одне обмеження.");

                List<string> constraintLines = new List<string>();
                foreach (DataGridViewRow row in dgvConstraints_C.Rows)
                {
                    if (row.Cells[0].Value != null)
                        constraintLines.Add(row.Cells[0].Value.ToString());
                }

                StringBuilder fullLog = new StringBuilder();
                fullLog.AppendLine("Згенерований протокол обчислення:\n");
                fullLog.AppendLine("Постановка задачі:\n");
                fullLog.AppendLine($"Z = {zExpr_С} -> {(isMin_С ? "min" : "max")}\n");
                fullLog.AppendLine("при обмеженнях:\n");
                foreach (var c in constraintLines) fullLog.AppendLine(c);
                fullLog.AppendLine($"\nx[j]>=0, j=1,{varCount_С}\n");

                // 2. Будуємо початкову таблицю
                int[] rowVars, colVars;
                double[,] table = MathHelper.BuildInitialTable(zExpr_С, constraintLines, varCount_С, isMin_С, out rowVars, out colVars);
                fullLog.AppendLine("Вхідна симплекс-таблиця:");
                fullLog.Append(MathHelper.PrintTableToLog(table, rowVars, colVars, varCount_С));

                string removeZeroLog;
                MathHelper.RemoveZeroRows(ref table, ref rowVars, ref colVars, varCount_С, out removeZeroLog);
                fullLog.Append(removeZeroLog);

                if (table == null)
                {
                    //txtFullLog= fullLog.ToString();
                    MessageBox.Show("Система суперечлива на етапі видалення 0-рядків.");
                    return;
                }

                // 3. Шукаємо опорний розв'язок
                string stepLog;
                table = MathHelper.FindFeasibleSolution(table, ref rowVars, ref colVars, varCount_С, out stepLog);
                fullLog.Append(stepLog); // Додаємо лог опорного рішення

                if (table != null)
                {
                    fullLog.AppendLine("Знайдено опорний розв'язок:\n");
                    fullLog.AppendLine(MathHelper.GetXVectorString(table, rowVars, colVars, varCount_С) + "\n");

                    // 4. Шукаємо оптимальний розв'язок
                    table = MathHelper.FindOptimalSolution(table, ref rowVars, ref colVars, varCount_С, out stepLog);
                    fullLog.Append(stepLog); // Додаємо лог оптимального рішення

                    if (table != null)
                    {
                        fullLog.AppendLine("Знайдено оптимальний розв'язок:\n");
                        fullLog.AppendLine(MathHelper.GetXVectorString(table, rowVars, colVars, varCount_С) + "\n");

                        double zRaw = table[table.GetLength(0) - 1, table.GetLength(1) - 1];
                        double zValue = isMin_С ? -zRaw : zRaw;
                        fullLog.AppendLine($"{(isMin_С ? "Min" : "Max")} (Z) = {zValue:F2}");
                        DisplayFinalResult(table, rowVars, colVars, varCount_С, isMin_С, txtX_C, txtY_C);
                    }
                }

                // Зберігаємо повний сформований лог
                SaveLogToFile(fullLog.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExample_C_Click(object sender, EventArgs e)
        {
            nudVarCount_C.Value = 4;
            txtZ_C.Text = "10x1-x2-42x3-52x4";
            rbMin_C.Checked = false;

            dgvConstraints_C.Rows.Clear();
            dgvConstraints_C.Rows.Add("-2x1+x2+x3+3x4=2");
            dgvConstraints_C.Rows.Add("-3x1+2x2-3x3=7");
            dgvConstraints_C.Rows.Add("-3x1+x2+4x3+x4<=1");
            dgvConstraints_C.Rows.Add("3x1-2x2+2x3-2x4<=-9");

            txtX_C.Text = "";
            txtY_C.Text = "";

            //nudVarCount_C.Value = 2;
            //txtZ_C.Text = "-3x1+6x2";
            //rbMin_C.Checked = false;

            //dgvConstraints_C.Rows.Clear();
            //dgvConstraints_C.Rows.Add("x1+2x2+1>=0");
            //dgvConstraints_C.Rows.Add("2x1+x2-4>=0");
            //dgvConstraints_C.Rows.Add("x1-x2+1>=0");
            //dgvConstraints_C.Rows.Add("x1-4x2+13>=0");
            //dgvConstraints_C.Rows.Add("-4x1+x2+23>=0");

            //txtX_C.Text = "";
            //txtY_C.Text = "";
        }

        //Part_D

        private void btnExample_D_Click(object sender, EventArgs e)
        {
            //nudVarCount_D.Value = 3;
            //txtZ_D.Text = "4x1+5x2+x3";
            //rbMin_D.Checked = false;

            //dgvConstraints_D.Rows.Clear();
            //dgvConstraints_D.Rows.Add("3x1+2x2<=10");
            //dgvConstraints_D.Rows.Add("x1+4x2<=11");
            //dgvConstraints_D.Rows.Add("3x1+3x2+x3<=13");

            //txtX_D.Text = "";
            //txtY_D.Text = "";

            //Варіант 15
            nudVarCount_D.Value = 4;
            txtZ_D.Text = "x1+x3+x6";
            rbMin_D.Checked = false;

            dgvConstraints_D.Rows.Clear();
            dgvConstraints_D.Rows.Add("x1+x2+x3+x4+x5+3x6<=4");
            dgvConstraints_D.Rows.Add("x1-4x2+x4+10x5-x6<=5");
            dgvConstraints_D.Rows.Add("x1-3x2+7x3+x4+15x5-x6<=2");

            txtX_D.Text = "";
            txtY_D.Text = "";
        }

        private void btnSolveLP_D_Click(object sender, EventArgs e)
        {
            try
            {
                string zExpr = txtZ_D.Text;
                int varCount = (int)nudVarCount_D.Value;
                bool isMin = rbMin_D.Checked;

                if (string.IsNullOrWhiteSpace(txtZ_D.Text)) throw new Exception("Введіть цільову функцію Z.");
                if (dgvConstraints_D.Rows.Count == 0) throw new Exception("Введіть хоча б одне обмеження.");

                List<string> constraintLines = new List<string>();
                foreach (DataGridViewRow row in dgvConstraints_D.Rows)
                {
                    if (row.Cells[0].Value != null)
                        constraintLines.Add(row.Cells[0].Value.ToString());
                }

                StringBuilder fullLog = new StringBuilder();
                fullLog.AppendLine("Постановка задачі (Цілочислове програмування):");
                fullLog.AppendLine($"Z = {zExpr} -> {(isMin ? "min" : "max")}");
                fullLog.AppendLine("при обмеженнях:");
                foreach (var c in constraintLines) fullLog.AppendLine(c);

                int[] rowVars, colVars;
                double[,] table = MathHelper.BuildInitialTable(zExpr, constraintLines, varCount, isMin, out rowVars, out colVars);

                fullLog.AppendLine("\nВхідна симплекс-таблиця:");
                fullLog.Append(MathHelper.PrintTableToLog(table, rowVars, colVars, varCount));

                table = MathHelper.SolveIntegerGomory(table, ref rowVars, ref colVars, varCount, out string gomoryLog);
                fullLog.Append(gomoryLog);

                if (table != null)
                {
                    fullLog.AppendLine("Знайдено фінальний ЦІЛОЧИСЛОВИЙ оптимальний розв'язок:\n");
                    fullLog.AppendLine(MathHelper.GetXVectorString(table, rowVars, colVars, varCount) + "\n");

                    double zRaw = table[table.GetLength(0) - 1, table.GetLength(1) - 1];
                    double zValue = isMin ? -zRaw : zRaw;
                    fullLog.AppendLine($"{(isMin ? "Min" : "Max")} (Z) = {zValue:F2}");

                    DisplayFinalResult(table, rowVars, colVars, varCount, isMin, txtX_D, txtY_D);
                }

                SaveLogToFile(fullLog.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Part_2
        private void btnSolveLP_2_Click(object sender, EventArgs e)
        {
            try
            {
                string zExpr = txtZ_2.Text; // Змініть на свій TextBox
                int varCount = (int)nudVarCount_2.Value;
                bool isMin = rbMin_2.Checked;

                if (string.IsNullOrWhiteSpace(zExpr)) throw new Exception("Введіть цільову функцію Z.");
                if (dgvConstraints_2.Rows.Count == 0) throw new Exception("Введіть хоча б одне обмеження.");

                List<string> constraintLines = new List<string>();
                foreach (DataGridViewRow row in dgvConstraints_2.Rows)
                {
                    if (row.Cells[0].Value != null)
                        constraintLines.Add(row.Cells[0].Value.ToString());
                }

                StringBuilder fullLog = new StringBuilder();
                fullLog.AppendLine("Згенерований протокол обчислення:\n");
                fullLog.AppendLine("Постановка прямої задачі:\n");
                fullLog.AppendLine($"Z = {zExpr} -> {(isMin ? "min" : "max")}\n");
                fullLog.AppendLine("при обмеженнях:\n");
                foreach (var c in constraintLines) fullLog.AppendLine(c);
                fullLog.AppendLine($"\nx[j]>=0, j=1,{varCount}\n");

                int[] rowVars, colVars;
                double[,] table = MathHelper.BuildInitialTable(zExpr, constraintLines, varCount, isMin, out rowVars, out colVars);

                fullLog.AppendLine("Перепишемо систему обмежень прямої задачі:\n");

                int m = table.GetLength(0) - 1;
                int n = table.GetLength(1) - 1;
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        fullLog.Append($"({table[i, j]:F2}) * X[{j + 1}]" + (j < n - 1 ? " + " : ""));
                    }
                    fullLog.AppendLine($" + ({table[i, n]:F2}) >= 0");
                }
                fullLog.AppendLine();

                table = MathHelper.SolveDualPair(table, ref rowVars, ref colVars, varCount, isMin, out string dualLog);
                fullLog.Append(dualLog);

                if (table != null)
                {
                    double zRaw = table[table.GetLength(0) - 1, table.GetLength(1) - 1];
                    double zValue = isMin ? -zRaw : zRaw;

                    fullLog.AppendLine($"Max (Z) = {zValue:F2}");
                    fullLog.AppendLine($"Min (W) = {zValue:F2}");

                    txtX_2.Text = $"Розв'язки прямої задачі:";
                    txtX_2.Text += Environment.NewLine + MathHelper.GetXVectorString(table, rowVars, colVars, varCount);
                    txtX_2.Text += Environment.NewLine + $"Розв'язки двоїстої задачі:";
                    txtX_2.Text += Environment.NewLine + MathHelper.GetUVectorString(table, rowVars, colVars, m);
                    txtX_2.Text += Environment.NewLine + $"Max (Z) = {zValue:F2}";
                    txtX_2.Text += Environment.NewLine + $"Min (W) = {zValue:F2}";
                }

                SaveLogToFile(fullLog.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExample_2_Click(object sender, EventArgs e)
        {
            //nudVarCount_2.Value = 4;
            //txtZ_2.Text = "x1+2x2-x3-x4";
            //rbMin_2.Checked = false;

            //dgvConstraints_2.Rows.Clear();
            //dgvConstraints_2.Rows.Add("x1+x2-x3-2x4<=6");
            //dgvConstraints_2.Rows.Add("x1+x2+x3-x4>=5");
            //dgvConstraints_2.Rows.Add("2x1-x2+3x3+4x4<=10");

            //txtX_2.Text = "";

            nudVarCount_2.Value = 2;
            txtZ_2.Text = "3x1+x2";
            rbMin_2.Checked = false;

            dgvConstraints_2.Rows.Clear();
            dgvConstraints_2.Rows.Add("x1-x2=-1");
            dgvConstraints_2.Rows.Add("x1+2x2<=5");
            dgvConstraints_2.Rows.Add("x1>=0");
            dgvConstraints_2.Rows.Add("x2>=0");

            txtX_2.Text = "";
        }

        //Part_3
        private void btnSolveGame_Click(object sender, EventArgs e)
        {
            try
            {
                // Зчитування матриці гри з dgvMatrixGame
                globalMatrix = GetMatrixFromGrid(dgvMatrixGame);

                StringBuilder log = new StringBuilder();
                log.AppendLine("Матриця А:\n");
                MathHelper.LogMatrix(log, globalMatrix);
                log.AppendLine();

                double v;

                // 1. Шукаємо сідлову точку
                if (MathHelper.FindSaddlePoint(globalMatrix, out v, out globalP, out globalQ, out string saddleLog))
                {
                    log.Append(saddleLog);
                }
                else
                {
                    log.Append(saddleLog);
                    // 2. Якщо сідлової точки немає, розв'язуємо через ЛП
                    MathHelper.SolveMatrixGameLP(globalMatrix, out v, out globalP, out globalQ, out string lpLog);
                    log.Append(lpLog);
                }

                // Виведення результатів на форму
                txtP.Text = string.Join("; ", globalP.Select(x => x.ToString("F2")));
                txtQ.Text = string.Join("; ", globalQ.Select(x => x.ToString("F2")));
                txtV.Text = v.ToString("F2");

                SaveLogToFile(log.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }

        private void btnSimulate_Click(object sender, EventArgs e)
        {
            try
            {
                if (globalP == null || globalQ == null)
                    throw new Exception("Спочатку знайдіть розв'язок гри!");

                int iterations = (int)nudIterations.Value; // Зчитати кількість партій (напр. 50)

                // Моделюємо гру
                System.Data.DataTable simTable = MathHelper.SimulateGame(globalMatrix, globalP, globalQ, iterations);

                // Виводимо в таблицю
                dgvSimulation.DataSource = simTable;

                dgvSimulation.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // 2. Дозволяємо системі автоматично підлаштувати висоту рядків під контент
                dgvSimulation.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                // 3. Робимо шрифти трохи більшими та вирівнюємо текст по центру для гарного вигляду
                dgvSimulation.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSimulation.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                double finalAverage = Convert.ToDouble(simTable.Rows[iterations - 1]["Середній виграш"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }

        private void btnExamplePart3_Click(object sender, EventArgs e)
        {
            dgvNatureGame.Rows.Clear();
            //dgvMatrixGame.ColumnCount = 3;

            //dgvMatrixGame.Rows.Add(5, 2, 7);
            //dgvMatrixGame.Rows.Add(1, 4, 3);
            //dgvMatrixGame.Rows.Add(6, 1, 5);


            //dgvMatrixGame.ColumnCount = 4;

            //dgvMatrixGame.Rows.Add(2, -1, 3, 3);
            //dgvMatrixGame.Rows.Add(-1, 2, 2, 7);
            //dgvMatrixGame.Rows.Add(1, 1, 1, 2);


            //dgvMatrixGame.ColumnCount = 4;

            //dgvMatrixGame.Rows.Add(3, 2, 6, 9);
            //dgvMatrixGame.Rows.Add(10, 8, 1, 3);


            //dgvMatrixGame.ColumnCount = 3;

            //dgvMatrixGame.Rows.Add(3, 5, 1);
            //dgvMatrixGame.Rows.Add(-2, 2, -3);
            //dgvMatrixGame.Rows.Add(1, 3, -2);


            //dgvMatrixGame.ColumnCount = 2;

            //dgvMatrixGame.Rows.Add(15, 19);
            //dgvMatrixGame.Rows.Add(17, 11);


            dgvMatrixGame.ColumnCount = 4;

            dgvMatrixGame.Rows.Add(8, 12, 6, 10);
            dgvMatrixGame.Rows.Add(14, 7, 12, 4);
        }

        private void btnSolveNatureGame_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Зчитування матриці гри
                double[,] matrix = GetMatrixFromGrid(dgvNatureGame);

                // 2. Зчитування коефіцієнта альфа (Гурвіц)
                double alpha = 0.5; // Значення за замовчуванням
                if (!string.IsNullOrWhiteSpace(txtAlpha.Text))
                {
                    alpha = Convert.ToDouble(txtAlpha.Text.Replace(".", ","));
                    if (alpha < 0 || alpha > 1) throw new Exception("Коефіцієнт альфа має бути в межах від 0 до 1.");
                }

                // 3. Зчитування ймовірностей (Байєс)
                double[] probabilities = null;
                if (!string.IsNullOrWhiteSpace(txtProbabilities.Text))
                {
                    string[] parts = txtProbabilities.Text.Split(new[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    probabilities = new double[parts.Length];
                    for (int i = 0; i < parts.Length; i++)
                    {
                        probabilities[i] = Convert.ToDouble(parts[i].Replace(".", ","));
                    }
                }

                // 4. Виклик математики (З ВЕЛИКОЮ КІЛЬКІСТЮ OUT ПАРАМЕТРІВ)
                MathHelper.SolveGameAgainstNature(
                    matrix, alpha, probabilities,
                    out string log,
                    out string resWald, out string resOpt, out string resHurwicz,
                    out string resSavage, out string resLaplace, out string resBayes,
                    out string bestOverall);

                // 5. Виведення результатів у відповідні TextBox-и
                txtWald.Text = resWald;
                txtOpt.Text = resOpt;
                txtHurwicz.Text = resHurwicz;
                txtSavage.Text = resSavage;
                txtLaplace.Text = resLaplace;
                txtBayes.Text = resBayes;
                txtBestOverall.Text = bestOverall;

                // 6. Вивід протоколу в багаторядкове поле та збереження у файл
                SaveLogToFile(log);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPart_4_Click(object sender, EventArgs e)
        {
            dgvNatureGame.Rows.Clear();
            //dgvNatureGame.ColumnCount = 4;

            //dgvNatureGame.Rows.Add(-1, 1, 1, 4);
            //dgvNatureGame.Rows.Add(-1, -2, 2, 3);
            //dgvNatureGame.Rows.Add(3, -1, 3, 2);
            //txtProbabilities.Text = "0.2 0.4 0.1 0.3";
            //txtAlpha.Text = "0.3";

            //dgvNatureGame.ColumnCount = 4;

            //dgvNatureGame.Rows.Add(2, -1, 3, 4);
            //dgvNatureGame.Rows.Add(-1, 2, 3, 7);
            //dgvNatureGame.Rows.Add(5, 4, 6, 2);
            //txtProbabilities.Text = "0.4 0.1 0.2 0.3";
            //txtAlpha.Text = "0.4";

            dgvNatureGame.ColumnCount = 4;

            dgvNatureGame.Rows.Add(4, -2, -3, 1);
            dgvNatureGame.Rows.Add(-1, 1, -2, 2);
            dgvNatureGame.Rows.Add(-1, -1, -4, 6);
            txtProbabilities.Text = "0.2 0.3 0.3 0.2";
            txtAlpha.Text = "0.3";
        }
    }
}
