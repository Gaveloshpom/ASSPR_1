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
                // Стовпці 0..n-1 → коефіцієнти A; стовпець n → –B
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

            // Множення матриці на вектор
            public static double[] MultiplyMatrixByVector(double[,] matrix, double[] vector)
            {
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1);
                double[] result = new double[rows];

                for (int i = 0; i < rows; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < cols; j++)
                    {
                        sum += matrix[i, j] * vector[j];
                    }
                    result[i] = sum;
                }
                return result;
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

            // ── Крок МЖВ ─────────────────────────────────────────────────────────────
            public static double[,] MjeStep(double[,] T, int r, int s, int rows, int cols)
            {
                double pivot = T[r, s];
                if (Math.Abs(pivot) < 1e-12)
                    throw new Exception($"Розв'язувальний елемент [{r},{s}] = 0.");

                double[,] N = new double[rows, cols];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                    {
                        if (i == r && j == s) N[i, j] = 1.0 / pivot;
                        else if (i == r) N[i, j] = T[r, j] / pivot;
                        else if (j == s) N[i, j] = -T[i, s] / pivot;
                        else N[i, j] = T[i, j] - T[i, s] * T[r, j] / pivot;
                    }
                return N;
            }

            // ── Парсери ───────────────────────────────────────────────────────────────
            public static double[] ParseObjective(string expr, int nVars)
            {
                double[] c = new double[nVars];
                string clean = expr.Replace(" ", "");
                if (!clean.StartsWith("-")) clean = "+" + clean;
                foreach (Match m in Regex.Matches(clean, @"([+-]\d*\.?\d*)[xX](\d+)"))
                {
                    int idx = int.Parse(m.Groups[2].Value) - 1;
                    if (idx < nVars) c[idx] = ParseCoef(m.Groups[1].Value);
                }
                return c;
            }

            public static void ParseConstraint(string raw, int nVars,
                out double[] row, out double rhs, out string type)
            {
                row = new double[nVars]; rhs = 0; type = "<=";
                string s = raw.Replace(" ", "");
                string lhs;

                if (s.Contains("<=")) { var p = s.Split(new[] { "<=" }, 2, StringSplitOptions.None); lhs = p[0]; rhs = ParseNum(p[1]); type = "<="; }
                else if (s.Contains(">=")) { var p = s.Split(new[] { ">=" }, 2, StringSplitOptions.None); lhs = p[0]; rhs = ParseNum(p[1]); type = ">="; }
                else if (s.Contains("=")) { var p = s.Split(new[] { "=" }, 2, StringSplitOptions.None); lhs = p[0]; rhs = ParseNum(p[1]); type = "="; }
                else throw new Exception($"Не вдалося розпізнати обмеження: «{raw}»");

                if (!lhs.StartsWith("-")) lhs = "+" + lhs;
                foreach (Match m in Regex.Matches(lhs, @"([+-]\d*\.?\d*)[xX](\d+)"))
                {
                    int idx = int.Parse(m.Groups[2].Value) - 1;
                    if (idx < nVars) row[idx] = ParseCoef(m.Groups[1].Value);
                }
            }

            public static double ParseCoef(string s)
            {
                if (s == "+" || s == "") return 1.0;
                if (s == "-") return -1.0;
                return double.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static double ParseNum(string s)
                => double.Parse(s, System.Globalization.CultureInfo.InvariantCulture);

            public static string FormatNum(double v)
            {
                double r = Math.Round(v, 6);
                return r == Math.Floor(r)
                    ? ((long)r).ToString()
                    : r.ToString("G6", System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string FormatVector(double[] X, int n)
            {
                var parts = new string[n];
                for (int i = 0; i < n; i++) parts[i] = FormatNum(X[i]);
                return "(" + string.Join("; ", parts) + ")";
            }

            // ═══════════════════════════════════════════════════════════════════════════
            // BIG-M SIMPLEX (МЖВ)
            // ═══════════════════════════════════════════════════════════════════════════
            //
            // Структура таблиці (стовпці):
            //   [0 .. nVars-1]              – початкові змінні  x_j
            //   [nVars .. nVars+nC-1]       – слабкі/надлишкові s_i  (+1 для <=, -1 для >=)
            //   [nVars+nC .. total-1]       – штучні змінні     a_i  (для >= та =)
            //   [total]                     – права частина     b
            //
            // Z-рядок (рядок nC):
            //   max: -c_j для x; +M для a_i  → шукаємо від'ємний мінімум
            //   min: +c_j для x; -M для a_i  → шукаємо від'ємний мінімум у нагованому рядку
            //   Після початкового базисного виключення штучних a_i рядок готовий.
            //
            // Значення ЦФ після оптимізації:
            //   max: T[zRow, total] =  Z*
            //   min: T[zRow, total] = -Z*  
            // ═══════════════════════════════════════════════════════════════════════════
            public static double[] SolveBigM(
                double[] cObj, List<double[]> A, List<double> b, List<string> types,
                int nVars, bool isMax, out double optZ, out string log)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Згенерований протокол обчислення:\n");
                sb.AppendLine("Постановка задачі:");

                // Логуємо цільову функцію
                sb.Append("Z = ");
                for (int j = 0; j < nVars; j++)
                {
                    if (j > 0 && cObj[j] >= 0) sb.Append("+");
                    sb.Append($"{cObj[j]:F2}*x{j + 1}");
                }
                sb.AppendLine(isMax ? " -> max" : " -> min");

                // Логуємо обмеження
                sb.AppendLine("\nпри обмеженнях:");
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < nVars; j++)
                    {
                        if (j > 0 && A[i][j] >= 0) sb.Append("+");
                        sb.Append($"{A[i][j]:F2}*x{j + 1}");
                    }
                    sb.AppendLine($"{types[i]}{b[i]:F2}");
                }
                sb.AppendLine();

                int nC = A.Count;

                // Гарантуємо b[i] >= 0
                for (int i = 0; i < nC; i++)
                {
                    if (b[i] < -1e-12)
                    {
                        for (int j = 0; j < nVars; j++) A[i][j] = -A[i][j];
                        b[i] = -b[i];
                        types[i] = types[i] == "<=" ? ">=" : types[i] == ">=" ? "<=" : "=";
                    }
                }

                int nArt = 0;
                foreach (var t in types) if (t == ">=" || t == "=") nArt++;

                int nSlack = nC;
                int total = nVars + nSlack + nArt;
                double[,] T = new double[nC + 1, total + 1];

                const double BIG_M = 1e8; // Трохи збільшимо для точності
                int[] basis = new int[nC];
                int artIdx = 0;

                // Заповнення обмежень
                for (int i = 0; i < nC; i++)
                {
                    for (int j = 0; j < nVars; j++) T[i, j] = A[i][j];
                    T[i, total] = b[i];

                    if (types[i] == "<=")
                    {
                        T[i, nVars + i] = 1.0;
                        basis[i] = nVars + i;
                    }
                    else if (types[i] == ">=")
                    {
                        T[i, nVars + i] = -1.0;
                        T[i, nVars + nSlack + artIdx] = 1.0;
                        basis[i] = nVars + nSlack + artIdx++;
                    }
                    else // "="
                    {
                        T[i, nVars + nSlack + artIdx] = 1.0;
                        basis[i] = nVars + nSlack + artIdx++;
                    }
                }

                // Z-рядок: коефіцієнти -c_j
                int zRow = nC;
                for (int j = 0; j < nVars; j++)
                    T[zRow, j] = -cObj[j];

                // Штрафи Big-M для штучних змінних
                for (int j = nVars + nSlack; j < total; j++)
                    T[zRow, j] = isMax ? BIG_M : -BIG_M;

                // Базисне виключення штучних змінних із Z-рядка
                int currentArtIdx = 0;
                for (int i = 0; i < nC; i++)
                {
                    if (types[i] == ">=" || types[i] == "=")
                    {
                        int aCol = nVars + nSlack + currentArtIdx++;
                        double coef = T[zRow, aCol];
                        for (int j = 0; j <= total; j++)
                            T[zRow, j] -= coef * T[i, j];
                    }
                }

                // Локальна функція для генерації назв змінних (x1, s1, a1, B)
                Func<int, string> getVarName = (col) =>
                {
                    if (col < nVars) return $"x{col + 1}";
                    if (col < nVars + nSlack) return $"s{col - nVars + 1}";
                    if (col < total) return $"a{col - nVars - nSlack + 1}";
                    return "B";
                };

                // Локальна функція для виводу таблиці T в лог
                Action logTableau = () =>
                {
                    sb.AppendFormat("{0,5} | ", "Базис");
                    for (int j = 0; j <= total; j++)
                    {
                        sb.AppendFormat("{0,9}", j == total ? "1" : getVarName(j));
                    }
                    sb.AppendLine();
                    sb.AppendLine(new string('-', 8 + 9 * (total + 1)));

                    for (int i = 0; i <= nC; i++)
                    {
                        string rowName = (i == zRow) ? "Z" : getVarName(basis[i]);
                        sb.AppendFormat("{0,5} | ", rowName);
                        for (int j = 0; j <= total; j++)
                        {
                            if (Math.Abs(T[i, j]) >= BIG_M / 10)
                                sb.AppendFormat("{0,9}", T[i, j] > 0 ? "M" : "-M");
                            else
                                sb.AppendFormat("{0,9:F2}", T[i, j]);
                        }
                        sb.AppendLine();
                    }
                    sb.AppendLine();
                };

                sb.AppendLine("Вхідна симплекс-таблиця:");
                logTableau();

                for (int iter = 0; iter < 1000; iter++)
                {
                    int s = -1;
                    if (isMax)
                    {
                        double minVal = -1e-9;
                        for (int j = 0; j < total; j++)
                            if (T[zRow, j] < minVal) { minVal = T[zRow, j]; s = j; }
                    }
                    else
                    {
                        double maxVal = 1e-9;
                        for (int j = 0; j < total; j++)
                            if (T[zRow, j] > maxVal) { maxVal = T[zRow, j]; s = j; }
                    }

                    if (s == -1) break; // Оптимум знайдено

                    int r = -1;
                    double minRatio = double.MaxValue;
                    for (int i = 0; i < nC; i++)
                    {
                        if (T[i, s] > 1e-9)
                        {
                            double ratio = T[i, total] / T[i, s];
                            if (ratio < minRatio) { minRatio = ratio; r = i; }
                        }
                    }
                    if (r == -1)
                    {
                        sb.AppendLine("Задача необмежена.");
                        throw new Exception("Задача необмежена.");
                    }

                    // Логуємо поточний крок
                    sb.AppendLine($"Пошук опорного розв'язку (Ітерація {iter + 1}):");
                    sb.AppendLine($"Розв'язувальний рядок:   {getVarName(basis[r])}");
                    sb.AppendLine($"Розв'язувальний стовпець: {getVarName(s)}\n");

                    T = MjeStep(T, r, s, nC + 1, total + 1);
                    basis[r] = s;

                    logTableau();
                }

                // Перевірка на допустимість (штучні змінні мають бути 0)
                for (int i = 0; i < nC; i++)
                    if (basis[i] >= nVars + nSlack && Math.Abs(T[i, total]) > 1e-6)
                    {
                        sb.AppendLine("Система обмежень несумісна.");
                        throw new Exception("Система обмежень несумісна.");
                    }

                double[] X = new double[nVars];
                for (int i = 0; i < nC; i++)
                    if (basis[i] < nVars) X[basis[i]] = T[i, total];

                optZ = T[zRow, total]; // Значення Z беремо прямо з таблиці

                // Логуємо фінальний результат
                sb.AppendLine("Знайдено оптимальний розв'язок:");
                sb.Append("X = (");
                for (int i = 0; i < nVars; i++)
                {
                    sb.Append($"{X[i]:F2}");
                    if (i < nVars - 1) sb.Append("; ");
                }
                sb.AppendLine(")\n");
                sb.AppendLine($"{(isMax ? "Max" : "Min")} (Z) = {optZ:F2}");

                log = sb.ToString();
                return X;
            }

        }
    }
}