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

        private void dgvMatrixA_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

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

        // Обробник натискання кнопки "Знайти обернену матрицю"
        private void btnInverse_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Отримуємо матрицю A з інтерфейсу
                double[,] A = GetMatrixFromGrid(dgvMatrixA);

                // 2. Обчислюємо за алгоритмом ЗЖВ
                double[,] invA = MathHelper.Inverse(A, out string log);

                // 3. Виводимо результат у таблицю для результатів
                ShowMatrixInGrid(invA, dgvVectorB);
                SaveLogToFile(log);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
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

        private void btnInverse_Click_1(object sender, EventArgs e)
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

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

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }
        // Допоміжний метод для налаштування таблиці (викликається кнопкою "Приклад")
        private void InitLPGrid(int varsCount, int constrCount)
        {
            dgvConstraints.AllowUserToAddRows = false;
            dgvConstraints.RowCount = constrCount + 1; // +1 для рядка Z
            dgvConstraints.ColumnCount = varsCount + 1; // +1 для стовпця вільних членів (B)

            // Називаємо колонки x1, x2... і B
            for (int j = 0; j < varsCount; j++) dgvConstraints.Columns[j].HeaderText = $"x{j + 1}";
            dgvConstraints.Columns[varsCount].HeaderText = "B";

            // Називаємо рядки
            for (int i = 0; i < constrCount; i++) dgvConstraints.Rows[i].HeaderCell.Value = $"О{i + 1}";
            dgvConstraints.Rows[constrCount].HeaderCell.Value = "Z";

            dgvConstraints.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        // Обробник для кнопки "Приклад"
        private void btnExample_Click(object sender, EventArgs e)
        {
            nudVarCount.Value = 4;
            txtZ.Text = "x1+2x2-x3-x4";
            rbMax.Checked = true;

            lstConstraints.Items.Clear();
            lstConstraints.Items.Add("x1+x2-x3-2x4<=6");
            lstConstraints.Items.Add("x1+x2+x3-x4>=5");
            lstConstraints.Items.Add("2x1-x2+3x3+4x4<=10");

            txtX.Text = "";
            txtY.Text = "";

            //nudVarCount.Value = 4;
            //txtZ.Text = "-2x1+3x2-3x4";
            //rbMax.Checked = false;

            //lstConstraints.Items.Clear();
            //lstConstraints.Items.Add("x1+x2-x3-2x4<=6");
            //lstConstraints.Items.Add("x1+x2+x3-x4>=5");
            //lstConstraints.Items.Add("2x1-x2+3x3+4x4<=10");

            //txtX.Text = "";
            //txtY.Text = "";
        }

        // Обробник для кнопки "Знайти оптимальний розв'язок"
        private void btnSolveLP_Click(object sender, EventArgs e)
        {
            try
            {
                int nVars = (int)nudVarCount.Value;
                bool isMax = rbMax.Checked;

                if (string.IsNullOrWhiteSpace(txtZ.Text))
                    throw new Exception("Введіть цільову функцію Z.");
                if (lstConstraints.Items.Count == 0)
                    throw new Exception("Введіть хоча б одне обмеження.");

                double[] cObj = MathHelper.ParseObjective(txtZ.Text, nVars);

                var A = new List<double[]>();
                var b = new List<double>();
                var types = new List<string>();

                foreach (var item in lstConstraints.Items)
                {
                    MathHelper.ParseConstraint(item.ToString(), nVars,
                        out double[] row, out double rhs, out string type);
                    A.Add(row);
                    b.Add(rhs);
                    types.Add(type);
                }

                double[] X = MathHelper.SolveBigM(cObj, A, b, types, nVars, isMax, out double optZ, out string log);

                txtX.Text = MathHelper.FormatVector(X, nVars);
                txtY.Text = MathHelper.FormatNum(optZ);
                SaveLogToFile(log);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveLogToFile(string logContent)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = "Protocol.txt";

                File.WriteAllText(sfd.FileName, logContent, Encoding.UTF8);
            }
        }
    }
}
