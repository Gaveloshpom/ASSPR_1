using Microsoft.VisualBasic.Logging;
using System.Text;
using static ASSPR_1.Program;

namespace ASSPR_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Console.OutputEncoding = Encoding.UTF8;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

            //dgvConstraints_2.Rows.Clear();
            //dgvConstraints_2.Rows.Add("x1+x2-x3-2x4<=6");
            //dgvConstraints_2.Rows.Add("x1+x2+x3-x4>=5");
            //dgvConstraints_2.Rows.Add("2x1-x2+3x3+4x4<=10");

            //txtX.Text = "";
            //txtY.Text = "";

            //nudVarCount.Value = 4;
            //txtZ.Text = "-2x1+3x2-3x4";
            //rbMin.Checked = true;

            //dgvConstraints_2.Rows.Clear();
            //dgvConstraints_2.Rows.Add("x1+x2-x3-2x4<=6");
            //dgvConstraints_2.Rows.Add("x1+x2+x3-x4>=5");
            //dgvConstraints_2.Rows.Add("2x1-x2+3x3+4x4<=10");

            //txtX.Text = "";
            //txtY.Text = "";

            nudVarCount.Value = 4;
            txtZ.Text = "x1+x3+x6";
            rbMin.Checked = false;

            dgvConstraints_2.Rows.Clear();
            dgvConstraints_2.Rows.Add("x1+x2+x3+x4+x5+3x6<=4");
            dgvConstraints_2.Rows.Add("x1-4x2+x4+10x5-x6<=5");
            dgvConstraints_2.Rows.Add("x1-3x2+7x3+x4+15x5-x6<=2");

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
                if (dgvConstraints_2.Rows.Count == 0)
                    throw new Exception("Введіть хоча б одне обмеження.");

                List<string> constraintLines = new List<string>();
                foreach (DataGridViewRow row in dgvConstraints_2.Rows)
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
    }
}
