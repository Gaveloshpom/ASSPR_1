using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ASSPR_1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        public class MathHelper
        {
            /// <summary>
            /// Виконує один крок перетворення матриці за алгоритмом Загального Жорданового Виключення
            /// </summary>
            /// <param name="A">Поточна матриця</param>
            /// <param name="r">Індекс розв'язувального рядка (pivot row)</param>
            /// <param name="s">Індекс розв'язувального стовпця (pivot column)</param>
            public static double[,] ZHV_Step(double[,] A, int r, int s)
            {
                int n = A.GetLength(0);
                int m = A.GetLength(1);
                double[,] B = new double[n, m];
                double pivot = A[r, s];

                // Перевірка на нульовий розв'язувальний елемент для уникнення ділення на 0
                if (Math.Abs(pivot) < 1e-9)
                    throw new Exception("Розв'язувальний елемент дорівнює нулю!");

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        // Розв'язувальний елемент замінюється на 1 (пізніше ділиться на pivot і стає 1/pivot)
                        if (i == r && j == s) B[i, j] = 1.0;
                        // Інші елементи розв'язувального рядка змінюють знак
                        else if (i == r) B[i, j] = -A[r, j];
                        // Елементи розв'язувального стовпця залишаються без змін
                        else if (j == s) B[i, j] = A[i, s];
                        // Всі інші елементи обчислюються за "правилом прямокутника"
                        else B[i, j] = A[i, j] * pivot - A[i, s] * A[r, j];
                        // Ділення всіх елементів на розв'язувальний елемент 
                        B[i, j] /= pivot;
                    }
                }
                return B;
            }

            /// <summary>
            /// Обчислює обернену матрицю шляхом послідовного застосування ЗЖВ до діагональних елементів
            /// </summary>
            public static double[,] Inverse(double[,] matrix, out string log)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Вхідна матриця:");
                LogMatrix(sb, matrix); 
                sb.AppendLine("\nПротокол обчислення:\n");

                int n = matrix.GetLength(0);
                if (n != matrix.GetLength(1)) throw new Exception("Матриця має бути квадратною!");

                double[,] result = (double[,])matrix.Clone();
                // Для отримання оберненої матриці потрібно пройти ЗЖВ по кожному діагональному елементу
                for (int i = 0; i < n; i++)
                {
                    sb.AppendLine($"Крок #{i + 1}");
                    sb.AppendFormat("Розв’язувальний елемент: A[{0}, {1}] = {2:F2}\n\n", i + 1, i + 1, result[i, i]);

                    // Виконання кроку ЗЖВ
                    result = ZHV_Step(result, i, i);

                    sb.AppendLine("Матриця після виконання ЗЖВ:");
                    LogMatrix(sb, result);
                    sb.AppendLine();
                }

                sb.AppendLine("Обернена матриця:");
                LogMatrix(sb, result);

                log = sb.ToString();
                return result;
            }

            /// <summary>
            /// Визначає ранг матриці, рахуючи кількість успішних кроків ЗЖВ (не нульових півотів)
            /// </summary>
            public static int Rank(double[,] A, out string log)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Протокол обчислення рангу матриці:\n");
                sb.AppendLine("Вхідна матриця:");
                LogMatrix(sb, A);

                int n = A.GetLength(0);
                int m = A.GetLength(1);
                double[,] current = (double[,])A.Clone();
                int r = 0; // Лічильник рангу

                for (int i = 0; i < Math.Min(n, m); i++)
                {
                    sb.AppendLine($"\nКрок #{i + 1}");
                    // Якщо діагональний елемент не нуль — виконуємо крок і збільшуємо ранг
                    if (Math.Abs(current[i, i]) > 1e-9)
                    {
                        sb.AppendLine($"Розв'язувальний елемент: A[{i + 1}, {i + 1}] = {current[i, i]:F2}");
                        current = ZHV_Step(current, i, i);
                        r++;
                        sb.AppendLine("Матриця після кроку ЗЖВ:");
                        LogMatrix(sb, current);
                    }
                    else
                    {
                        sb.AppendLine($"Елемент A[{i + 1}, {i + 1}] близький до 0, пропускаємо рядок.");
                    }
                }

                sb.AppendLine($"\nРезультат: Ранг = {r}");
                log = sb.ToString();
                return r;
            }

            /// <summary>
            /// Спосіб 1: Рішення через знаходження оберненої матриці X = A^(-1) * B
            /// </summary>
            public static double[] SolveMethod1(double[,] A, double[] B_vec, out string log)
            {
                StringBuilder sb = new StringBuilder();
                int n = A.GetLength(0);

                // 1. Знаходимо обернену матрицю за допомогою ЗЖВ
                double[,] invA = Inverse(A, out string invLog);
                sb.AppendLine(invLog);

                sb.AppendLine("\nВхідна матриця B:");
                LogVector(sb, B_vec);

                // 2. Множимо обернену матрицю на вектор вільних членів
                double[] X = new double[n];
                sb.AppendLine("\nОбчислення розв'язків (X = A^-1 * B):\n");
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    sb.AppendFormat("X[{0}] = ", i + 1);
                    for (int j = 0; j < n; j++)
                    {
                        sum += invA[i, j] * B_vec[j];
                        sb.AppendFormat("{0:F2} * {1:F2}", B_vec[j], invA[i, j]);
                        if (j < n - 1) sb.Append(" + ");
                    }
                    X[i] = sum;
                    sb.AppendFormat(" = {0:F2}\n", X[i]);
                }

                log = sb.ToString();
                return X;
            }

            /// <summary>
            /// Спосіб 2: Рішення шляхом перетворення розширеної матриці (AX - B = 0)
            /// </summary>
            public static double[] SolveMethod2(double[,] A, double[] B_vec, out string log)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Згенерований протокол обчислення:\n");
                sb.AppendLine("Знаходження розв'язків СЛАУ 2-м методом:\n");

                sb.AppendLine("Вхідна матриця А:");
                LogMatrix(sb, A);
                sb.AppendLine("\nВхідна матриця B:");
                LogVector(sb, B_vec);

                int n = A.GetLength(0);
                // Формуємо розширену матрицю [A | -B]
                double[,] augmented = new double[n, n + 1];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++) augmented[i, j] = A[i, j];
                    // Переносимо B в ліву частину, тому знак змінюється на мінус
                    augmented[i, n] = -B_vec[i];
                }

                sb.AppendLine("\nПротокол обчислення:\n");
                sb.AppendLine("Переписана система:");
                LogMatrix(sb, augmented);
                sb.AppendLine();

                double[,] current = augmented;
                for (int i = 0; i < n; i++)
                {
                    sb.AppendLine($"Крок #{i + 1}\n");
                    sb.AppendFormat("Розв'язувальний елемент: A[{0}, {1}] = {2:F2}\n\n", i + 1, i + 1, current[i, i]);

                    current = ZHV_Step(current, i, i);

                    sb.AppendLine("Матриця після виконання ЗЖВ:");
                    LogMatrix(sb, current);
                    sb.AppendLine();
                }

                // Після n кроків ЗЖВ, останній стовпець містить шукані значення Xi
                double[] x = new double[n];
                sb.AppendLine("Розв'язки:");
                for (int i = 0; i < n; i++)
                {
                    x[i] = current[i, n];
                    sb.AppendFormat("X[{0}] = {1,5:F2}\n", i + 1, x[i]);
                }

                log = sb.ToString();
                return x;
            }

            /// <summary>
            /// Спосіб 3: Класичний метод Гаусса (Прямий та зворотний хід)
            /// </summary>
            public static double[] SolveMethod3(double[,] A, double[] b, out string log)
            {
                StringBuilder sb = new StringBuilder();
                int n = A.GetLength(0);

                sb.AppendLine("Згенерований протокол обчислення:");
                sb.AppendLine("\nЗнаходження розв’язків СЛАУ методом Гаусса:");

                // Валідація вхідних даних
                if (A.GetLength(1) != n)
                    throw new ArgumentException("Матриця A повинна бути квадратною.");
                if (b.Length != n)
                    throw new ArgumentException("Розмір вектора b не відповідає розміру матриці A.");

                // Вивід вхідних даних
                sb.AppendLine("\nВхідна матриця А:");
                LogMatrix(sb, A);
                sb.AppendLine("\nВхідна матриця B:");
                LogVector(sb, b);

                sb.AppendLine("\nПротокол обчислення:");

                // Будуємо розширену матрицю системи AX – B = 0
                // Стовпці 0..n-1 -> коефіцієнти A; стовпець n -> –B
                double[,] aug = new double[n, n + 1];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                        aug[i, j] = A[i, j];

                    aug[i, n] = -b[i]; // знак змінено відповідно до форми AX – B = 0
                }

                sb.AppendLine("\nПереписана система:");
                LogMatrix(sb, aug);

                // Масив для збереження рядків-формул (для зворотного ходу)
                // Зберігаємо коефіцієнти для X[i] = k1*X[i+1] + ... + kn*X[n] + free_term
                double[][] formulas = new double[n][];

                // ── Процедура прямого ходу ──────────────────────────────────────
                for (int i = 0; i < n; i++)
                {
                    double pivot = aug[i, i];
                    sb.AppendLine($"\nКрок #{i + 1}");
                    // Розв'язувальний елемент = a_ii (діагональний)
                    if (Math.Abs(pivot) < 1e-9)
                        throw new InvalidOperationException(
                            $"Розв'язувальний елемент a[{i},{i}] ≈ 0. " +
                            "Система може бути виродженою або потребує перестановки рядків.");

                    sb.AppendFormat("Розв’язувальний елемент: A[{0}, {0}] = {1:F2}\n", i + 1, pivot);

                    // Формування тексту формули для логу: Xi = ...
                    // Формула: Xi = sum(-a_ij/a_ii * Xj) - a_in/a_ii
                    formulas[i] = new double[n + 1];
                    sb.AppendFormat("Розв’язувальний рядок: X[{0}] = ", i + 1);

                    bool first = true;
                    for (int j = i + 1; j < n; j++)
                    {
                        double coeff = -aug[i, j] / pivot;
                        formulas[i][j] = coeff;
                        if (!first && coeff >= 0) sb.Append(" + ");
                        else if (coeff < 0) sb.Append(" - ");

                        sb.AppendFormat("({0:F2}) * X[{1}]", Math.Abs(coeff), j + 1);
                        first = false;
                    }

                    double freeTerm = -aug[i, n] / pivot;
                    formulas[i][n] = freeTerm;
                    if (!first && freeTerm >= 0) sb.Append(" + ");
                    else if (freeTerm < 0) sb.Append(" - ");
                    sb.AppendFormat("{0:F2}\n", Math.Abs(freeTerm));

                    // Крок ЗЖВ: перетворює всю матрицю відносно опорного елемента (i, i)
                    aug = ZHV_Step(aug, i, i);

                    // Вивід підматриці (тільки ті елементи, що лишилися нижче і правіше i)
                    if (i < n - 1)
                    {
                        sb.AppendLine("Матриця після виконання ЗЖВ:");
                        for (int r = i + 1; r < n; r++)
                        {
                            for (int c = i + 1; c <= n; c++)
                            {
                                sb.AppendFormat("{0,8:F2}", aug[r, c]);
                            }
                            sb.AppendLine();
                        }
                    }
                }

                // ── Процедура зворотного ходу ───────────────────────────────────
                // Після n повних кроків ЗЖВ розв'язок Xi міститься в останньому стовпці
                sb.AppendLine("\nОбчислення розв’язків:");
                double[] X = new double[n];
                for (int i = n - 1; i >= 0; i--)
                {
                    double val = formulas[i][n]; // Вільний член

                    sb.AppendFormat("X[{0}] = ", i + 1);

                    // Візуалізація підстановки у лог
                    bool hasCoeffs = false;
                    for (int j = i + 1; j < n; j++)
                    {
                        double c = formulas[i][j];
                        val += c * X[j];

                        if (hasCoeffs && c >= 0) sb.Append(" + ");
                        else if (c < 0) sb.Append(" - ");
                        sb.AppendFormat("({0:F2}) * {1:F2}", Math.Abs(c), X[j]);
                        hasCoeffs = true;
                    }

                    if (hasCoeffs && formulas[i][n] >= 0) sb.Append(" + ");
                    else if (hasCoeffs && formulas[i][n] < 0) sb.Append(" - ");

                    if (hasCoeffs) sb.AppendFormat("{0:F2}", Math.Abs(formulas[i][n]));
                    else sb.AppendFormat("{0:F2}", val);

                    X[i] = val;
                    sb.AppendFormat(" = {0:F2}\n", X[i]);
                }

                log = sb.ToString();
                return X;
            }

            // Допоміжний метод для логування матриць
            public static void LogMatrix(StringBuilder sb, double[,] matrix)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        sb.AppendFormat("{0,8:F2}", matrix[i, j]);
                    }
                    sb.AppendLine();
                }
            }

            public static void LogVector(StringBuilder sb, double[] vector)
            {
                for (int i = 0; i < vector.Length; i++)
                {
                    sb.AppendFormat("{0,8:F2}\n", vector[i]);
                }
            }

            // Метод для витягування коефіцієнтів з рядка типу "2x1 - 3x2 + x4 <= 10"
            public static double[] ParseLine(string input, int varCount, out string sign, out double rhs)
            {
                double[] coeffs = new double[varCount];
                rhs = 0;
                sign = "<=";

                // Визначаємо знак і розділяємо на ліву та праву частини
                if (input.Contains("<=")) sign = "<=";
                else if (input.Contains(">=")) sign = ">=";
                else if (input.Contains("=")) sign = "=";

                string[] parts = input.Split(new[] { "<=", ">=", "=" }, StringSplitOptions.RemoveEmptyEntries);
                string lhs = parts[0];
                if (parts.Length > 1) double.TryParse(parts[1].Trim().Replace('.', ','), out rhs);

                // Регулярний вираз для пошуку [число]x[номер]
                // Група 1: число (коефіцієнт), Група 2: номер змінної
                Regex regex = new Regex(@"([+-]?\s*\d*(?:[.,]\d+)?)\s*x(\d+)", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(lhs);

                foreach (Match m in matches)
                {
                    string valStr = m.Groups[1].Value.Replace(" ", "").Replace('.', ',');
                    int varIdx = int.Parse(m.Groups[2].Value) - 1; // x1 -> індекс 0

                    double coeff = 0;
                    if (string.IsNullOrEmpty(valStr) || valStr == "+") coeff = 1;
                    else if (valStr == "-") coeff = -1;
                    else double.TryParse(valStr, out coeff);

                    if (varIdx < varCount) coeffs[varIdx] = coeff;
                }

                return coeffs;
            }

            public static double[,] BuildInitialTable(string zFunc, List<string> constraints, int varCount, bool isMin, out int[] rowVars, out int[] colVars)
            {
                int m = constraints.Count; // Кількість обмежень
                int n = varCount;          // Кількість змінних
                double[,] table = new double[m + 1, n + 1];
                rowVars = new int[m + 1]; // Індекси змінних зліва (базис: y1, y2...)
                colVars = new int[n + 1]; // Індекси змінних зверху (x1, x2...)
                // 1. Заповнюємо обмеження (рядки y1, y2...)
                for (int i = 0; i < m; i++)
                {
                    double[] coeffs = ParseLine(constraints[i], n, out string sign, out double rhs);

                    double mult;
                    bool isEquality = sign == "=";

                    if (sign == "<=")
                        mult = 1;
                    else if (sign == ">=")
                        mult = -1;
                    else // "="
                        mult = 1;

                    // Спочатку записуємо з урахуванням знаку нерівності
                    for (int j = 0; j < n; j++)
                        table[i, j] = coeffs[j] * mult;
                    table[i, n] = rhs * mult;

                    // Якщо вільний член від'ємний — множимо весь рядок на -1
                    //if (table[i, n] < 0)
                    //{
                    //    for (int j = 0; j <= n; j++)
                    //        table[i, j] *= -1;
                    //}

                    // Мітка рядка: 0-рядок для рівності, -i для нерівності
                    rowVars[i] = isEquality ? 0 : -(i + 1);
                }
                // 2. Заповнюємо цільову функцію Z (останній рядок)
                double[] zCoeffs = ParseLine(zFunc, n, out string dummySign, out double dummyRhs);
                rowVars[m] = 0; // Мітка для Z-рядка
                for (int j = 0; j < n; j++)
                {
                    // Синхронізуємо індекси colVars зі стовпцями матриці
                    colVars[j] = (j + 1); // Змінні x1, x2...
                    double c = zCoeffs[j];
                    // Записуємо в j-тий стовпець
                    table[m, j] = isMin ? c : -c;
                }
                // Задаємо початкове значення цільової функції (зазвичай 0) в останній стовпець
                table[m, n] = 0;
                // Мітка для стовпця вільних членів (опціонально, залежить від вашої логіки)
                colVars[n] = 0;
                return table;
            }

            //Part_B

            /// <summary>
            /// Процедура Модифікованих Жорданових Виключень (МЖВ)
            /// </summary>
            public static double[,] MJV_Procedure(double[,] A, int r, int s)
            {
                int rows = A.GetLength(0);
                int cols = A.GetLength(1);
                double pivot = A[r, s];

                if (Math.Abs(pivot) < 1e-9)
                    throw new Exception("Розв'язувальний елемент дорівнює нулю!");

                double[,] B = new double[rows, cols];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        // 1. Розв'язувальний елемент замінюють на «1»
                        if (i == r && j == s) B[i, j] = 1.0;

                        // 2. Інші елементи розв'язувального стовпця змінюють лише свої знаки
                        else if (j == s) B[i, j] = -A[i, s];

                        // 3. Інші елементи розв'язувального рядка залишаються без змін
                        else if (i == r) B[i, j] = A[r, j];

                        // 4. Усі інші елементи розраховують за формулою b_ij = a_ij*a_rs - a_is*a_rj
                        else B[i, j] = A[i, j] * pivot - A[i, s] * A[r, j];
                    }
                }

                // 5. Усі елементи в новій таблиці ділять на розв'язувальний елемент ars
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        B[i, j] /= pivot;
                    }
                }

                return B;
            }

            /// <summary>
            /// Алгоритм пошуку опорного розв'язку
            /// </summary>
            public static double[,] FindFeasibleSolution(double[,] table, ref int[] rowVars, ref int[] colVars, int varCount, out string log)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("--- Пошук опорного розв'язку ---");
                int iteration = 0;

                while (true)
                {
                    iteration++;
                    int rows = table.GetLength(0);
                    int cols = table.GetLength(1);
                    int rhsCol = cols - 1; // Індекс стовпця вільних членів (останній)
                    int sCol = -1;
                    int rRow = -1;

                    // 1. Пошук від'ємного елемента у стовпці вільних членів (крім Z-рядка)
                    int negativeRowIdx = -1;
                    for (int i = 0; i < rows - 1; i++)
                    {
                        if (table[i, rhsCol] < -1e-9) { negativeRowIdx = i; break; }
                    }

                    // Якщо від'ємних немає -> Опорний розв'язок знайдено
                    if (negativeRowIdx == -1)
                    {
                        //sb.AppendLine("Опорний розв'язок знайдено.");
                        log = sb.ToString();
                        return table;
                    }

                    // 2. Пошук від'ємного елемента в знайденому рядку -> розв'язувальний стовпець
                    for (int j = 0; j < cols - 1; j++)
                    {
                        if (table[negativeRowIdx, j] < -1e-9) { sCol = j; break; }
                    }

                    // Якщо в рядку немає від'ємних елементів, а вільний член від'ємний -> Система суперечлива
                    if (sCol == -1)
                    {
                        sb.AppendLine("Система обмежень є суперечливою.");
                        log = sb.ToString();
                        throw new Exception("Система обмежень є суперечливою.");
                        //return null;
                    }

                    // 3. РОЗВ'ЯЗУВАЛЬНИЙ РЯДОК — це рядок з від'ємним вільним членом
                    rRow = negativeRowIdx;

                    sb.AppendLine($"Розв'язувальний рядок:    {FormatVarName(rowVars[rRow], varCount)}");
                    sb.AppendLine($"Розв'язувальний стовпець: -{FormatVarName(colVars[sCol], varCount)}");
                    sb.AppendLine();

                    // 4. Процедура МЖВ
                    table = MJV_Procedure(table, rRow, sCol);

                    // 6. Зміна змінних у «шапці»
                    int temp = rowVars[rRow];
                    rowVars[rRow] = colVars[sCol];
                    colVars[sCol] = temp;

                    sb.Append(PrintTableToLog(table, rowVars, colVars, varCount));
                }
            }

            /// <summary>
            /// Алгоритм пошуку оптимального розв'язку 
            /// </summary>
            public static double[,] FindOptimalSolution(double[,] table, ref int[] rowVars, ref int[] colVars, int varCount, out string log)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("--- Пошук оптимального розв'язку ---");
                int iteration = 0;

                while (true)
                {
                    iteration++;
                    int rows = table.GetLength(0);
                    int cols = table.GetLength(1);
                    int zRowIdx = rows - 1;
                    int rhsCol = cols - 1;
                    int sCol = -1;

                    // Шукаємо від'ємний елемент у Z-рядку
                    for (int j = 0; j < cols - 1; j++)
                    {
                        if (table[zRowIdx, j] < -1e-9) { sCol = j; break; }
                    }

                    // Немає від'ємних -> оптимум знайдено
                    if (sCol == -1)
                    {
                        //sb.AppendLine("Оптимальний розв'язок знайдено.");
                        log = sb.ToString();
                        return table;
                    }

                    // Мінімальне невід'ємне відношення -> розв'язувальний рядок
                    double minRatio = double.MaxValue;
                    int rRow = -1;
                    for (int i = 0; i < rows - 1; i++)
                    {
                        if (table[i, sCol] > 1e-9)
                        {
                            double ratio = table[i, rhsCol] / table[i, sCol];
                            if (ratio >= 0 && ratio < minRatio)
                            {
                                minRatio = ratio;
                                rRow = i;
                            }
                        }
                    }

                    if (rRow == -1)
                    {
                        sb.AppendLine("Функція мети не обмежена.");
                        log = sb.ToString();
                        throw new Exception("Функція мети не обмежена.");
                        //return null;
                    }

                    sb.AppendLine($"Розв'язувальний рядок:    {FormatVarName(rowVars[rRow], varCount)}");
                    sb.AppendLine($"Розв'язувальний стовпець: -{FormatVarName(colVars[sCol], varCount)}");
                    sb.AppendLine();

                    table = MJV_Procedure(table, rRow, sCol);

                    int temp = rowVars[rRow];
                    rowVars[rRow] = colVars[sCol];
                    colVars[sCol] = temp;

                    sb.Append(PrintTableToLog(table, rowVars, colVars, varCount));
                }
            }

            public static string FormatVarName(int index, int varCount)
            {
                // Якщо індекс від 1 до varCount - це змінні x
                if (index >= 1 && index <= varCount) return "x" + index;
                // Якщо індекс більший за varCount - це додаткові змінні y (slack variables)
                if (index > varCount) return "y" + (index - varCount);
                // На випадок, якщо ви використовуєте від'ємні індекси для y
                if (index < 0) return "y" + Math.Abs(index);
                return "?";
            }

            public static string PrintTableToLog(double[,] table, int[] rowVars, int[] colVars, int varCount)
            {
                StringBuilder sb = new StringBuilder();
                int rows = table.GetLength(0);
                int cols = table.GetLength(1);

                // Шапка таблиці (-x1, -x2... 1)
                sb.Append("      ");
                for (int j = 0; j < cols - 1; j++)
                {
                    string colName = FormatVarName(colVars[j], varCount);
                    sb.Append($"{("-" + colName),10}");
                }
                sb.AppendLine($"{"1",10}");
                sb.AppendLine(new string('-', 8 + 10 * cols));

                // Рядки таблиці
                for (int i = 0; i < rows - 1; i++)
                {
                    string rowName = FormatVarName(rowVars[i], varCount);
                    sb.Append($"{rowName,-3} =");
                    for (int j = 0; j < cols; j++)
                    {
                        sb.Append($"{table[i, j],10:F2}");
                    }
                    sb.AppendLine();
                }

                // Z-рядок
                sb.Append("Z   =");
                for (int j = 0; j < cols; j++)
                {
                    sb.Append($"{table[rows - 1, j],10:F2}");
                }
                sb.AppendLine();
                sb.AppendLine();

                return sb.ToString();
            }

            public static string GetXVectorString(double[,] table, int[] rowVars, int[] colVars, int varCount)
            {
                int rows = table.GetLength(0);
                int cols = table.GetLength(1);
                int rhsCol = cols - 1;
                double[] xValues = new double[varCount];

                for (int i = 0; i < rows - 1; i++)
                {
                    int varIndex = rowVars[i];
                    if (varIndex >= 1 && varIndex <= varCount) xValues[varIndex - 1] = table[i, rhsCol];
                }

                var formatted = xValues.Select(v => v.ToString("F2"));
                return $"X = ({string.Join("; ", formatted)})";
            }


            //Part_C

            /// <summary>
            /// Алгоритм видалення 0-рядків симплекс-таблиці (Рисунок 3.2)
            /// Повертає false, якщо система обмежень є суперечливою
            /// </summary>
            /// <summary>
            /// Алгоритм видалення 0-рядків симплекс-таблиці (Рис. 3.2)
            /// </summary>
            public static void RemoveZeroRows(ref double[,] table, ref int[] rowVars, ref int[] colVars)
            {
                while (true)
                {
                    // Динамічно визначаємо кількість рядків і стовпців, 
                    // оскільки таблиця може зменшуватись після викреслювання 0-стовпця
                    int m = table.GetLength(0) - 1; // Індекс рядка Z
                    int n = table.GetLength(1) - 1; // Індекс стовпця вільних членів

                    // 1. Пошук 0-рядка в симплекс-таблиці
                    int zeroRowIndex = -1;
                    for (int i = 0; i < m; i++)
                    {
                        if (rowVars[i] == 0) // Значення 0 означає рівність (штучний базис)
                        {
                            zeroRowIndex = i;
                            break; // Беремо перший-ліпший 0-рядок
                        }
                    }

                    // Є 0-рядок? Ні -> Завершення
                    if (zeroRowIndex == -1)
                        break;

                    // 2. Пошук додатного елемента в 0-рядку -> розв'язувальний стовпець
                    int pivotCol = -1;
                    for (int j = 0; j < n; j++)
                    {
                        if (table[zeroRowIndex, j] > 0)
                        {
                            pivotCol = j;
                            break;
                        }
                    }

                    // Є додатний елемент у 0-рядку? Ні -> Система суперечлива
                    if (pivotCol == -1)
                    {
                        throw new Exception("Система обмежень є суперечливою (немає додатних елементів у 0-рядку).");
                    }

                    // 3. Розрахунок мінімального невід'ємного -> розв'язувальний рядок
                    int pivotRow = -1;
                    double minRatio = double.MaxValue;

                    for (int i = 0; i < m; i++)
                    {
                        if (table[i, pivotCol] > 0)
                        {
                            // Відношення вільного члена до елемента розв'язувального стовпця
                            double ratio = table[i, n] / table[i, pivotCol];
                            if (ratio >= 0 && ratio < minRatio)
                            {
                                minRatio = ratio;
                                pivotRow = i;
                            }
                        }
                    }

                    // Якщо з якихось причин (через похибки) мінімальний не знайдено, 
                    // примусово робимо розв'язувальним сам 0-рядок (бо там є додатний елемент)
                    if (pivotRow == -1)
                    {
                        pivotRow = zeroRowIndex;
                    }

                    // 4. Процедура МЖВ (Метод Жорданових Виключень)
                    //PerformMZhV(ref table, ref rowVars, ref colVars, pivotRow, pivotCol);
                    table = MJV_Procedure(table, pivotRow, pivotCol);

                    int temp = rowVars[pivotRow];
                    rowVars[pivotRow] = colVars[pivotCol];
                    colVars[pivotCol] = temp;

                    // 5. У симплекс-таблиці викреслюють 0-стовпець
                    // Якщо 0-рядок залишив базис (став стовпцем), мітка стовпця стане 0
                    if (colVars[pivotCol] == 0)
                    {
                        RemoveColumn(ref table, ref colVars, pivotCol);
                    }
                }
            }

            // Процедура викреслювання стовпця (створює новий зменшений масив)
            private static void RemoveColumn(ref double[,] table, ref int[] colVars, int colToRemove)
            {
                int rows = table.GetLength(0);
                int cols = table.GetLength(1);

                double[,] newTable = new double[rows, cols - 1];
                int[] newColVars = new int[cols - 1];

                for (int i = 0; i < rows; i++)
                {
                    int destCol = 0;
                    for (int j = 0; j < cols; j++)
                    {
                        if (j == colToRemove) continue; // Пропускаємо стовпець, який треба видалити

                        newTable[i, destCol] = table[i, j];

                        // Копіюємо мітки стовпців лише один раз
                        if (i == 0)
                        {
                            newColVars[destCol] = colVars[j];
                        }
                        destCol++;
                    }
                }

                table = newTable;
                colVars = newColVars;
            }
        }
    }
}