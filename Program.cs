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
            // Пошук оберненої матриці
            public static double[,] Inverse(double[,] matrix, out string log)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Вхідна матриця:");
                LogMatrix(sb, matrix); // Використовуємо раніше створений допоміжний метод
                sb.AppendLine("\nПротокол обчислення:\n");

                int n = matrix.GetLength(0);
                if (n != matrix.GetLength(1)) throw new Exception("Матриця має бути квадратною!");

                double[,] result = (double[,])matrix.Clone();

                for (int i = 0; i < n; i++)
                {
                    sb.AppendLine($"Крок #{i + 1}");
                    sb.AppendFormat("Розв’язувальний елемент: A[{0}, {1}] = {2:F2}\n\n", i + 1, i + 1, result[i, i]);

                    // Виконання кроку ЗЖВ (логіка залишається оригінальною)
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

            // Обчислення рангу матриці
            public static int Rank(double[,] A, out string log)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Протокол обчислення рангу матриці:\n");
                sb.AppendLine("Вхідна матриця:");
                LogMatrix(sb, A);

                int n = A.GetLength(0);
                int m = A.GetLength(1);
                double[,] current = (double[,])A.Clone();
                int r = 0;

                for (int i = 0; i < Math.Min(n, m); i++)
                {
                    sb.AppendLine($"\nКрок #{i + 1}");
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

            // Базовий алгоритм ЗЖВ
            public static double[,] ZHV_Step(double[,] A, int r, int s)
            {
                int n = A.GetLength(0);
                int m = A.GetLength(1);
                double[,] B = new double[n, m];
                double pivot = A[r, s];

                if (Math.Abs(pivot) < 1e-9)
                    throw new Exception("Розв'язувальний елемент дорівнює нулю!");

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (i == r && j == s) B[i, j] = 1.0;
                        else if (i == r) B[i, j] = -A[r, j];
                        else if (j == s) B[i, j] = A[i, s];
                        else B[i, j] = A[i, j] * pivot - A[i, s] * A[r, j];

                        B[i, j] /= pivot;
                    }
                }
                return B;
            }

            // Спосіб 2: Приведення до вигляду AX - B = 0 з ЛОГУВАННЯМ
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
                double[,] augmented = new double[n, n + 1];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++) augmented[i, j] = A[i, j];
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

            // Спосіб 1: Розв'язання СЛАР (X = A^(-1) * B) з ЛОГУВАННЯМ
            public static double[] SolveMethod1(double[,] A, double[] B_vec, out string log)
            {
                StringBuilder sb = new StringBuilder();
                int n = A.GetLength(0);

                sb.AppendLine("Вхідна матриця:");
                LogMatrix(sb, A);
                sb.AppendLine("\nПротокол обчислення:\n");

                double[,] result = (double[,])A.Clone();

                for (int i = 0; i < n; i++)
                {
                    sb.AppendLine($"Крок #{i + 1}\n");
                    sb.AppendFormat("Розв'язувальний елемент: A[{0}, {1}] = {2:F2}\n\n", i + 1, i + 1, result[i, i]);

                    result = ZHV_Step(result, i, i);

                    sb.AppendLine("Матриця після виконання ЗЖВ:");
                    LogMatrix(sb, result);
                    sb.AppendLine();
                }

                sb.AppendLine("Обернена матриця:");
                LogMatrix(sb, result);

                sb.AppendLine("\nВхідна матриця B:");
                LogVector(sb, B_vec);

                double[] X = new double[n];
                sb.AppendLine("\nОбчислення розв'язків:\n");
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    sb.AppendFormat("X[{0}] = ", i + 1);
                    for (int j = 0; j < n; j++)
                    {
                        sum += result[i, j] * B_vec[j];
                        sb.AppendFormat("{0:F2} * {1:F2}", B_vec[j], result[i, j]);
                        if (j < n - 1) sb.Append(" + ");
                    }
                    X[i] = sum;
                    sb.AppendFormat(" = {0:F2}\n", X[i]);
                }

                log = sb.ToString();
                return X;
            }

            // Спосіб 3: Розв'язання СЛАР Методом Гаусса
            public static double[] SolveMethod3(double[,] A, double[] B_vec, out string log)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Протокол обчислення: Метод Гаусса\n");

                int n = A.GetLength(0);
                double[,] augmented = new double[n, n + 1];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++) augmented[i, j] = A[i, j];
                    augmented[i, n] = B_vec[i];
                }

                sb.AppendLine("Розширена матриця [A|B]:");
                LogMatrix(sb, augmented);

                // Прямий хід
                for (int i = 0; i < n; i++)
                {
                    sb.AppendLine($"\nПрямий хід: виключення для x{i + 1}");
                    if (Math.Abs(augmented[i, i]) < 1e-9)
                    {
                        // Пошук рядка для перестановки
                        bool swapped = false;
                        for (int k = i + 1; k < n; k++)
                        {
                            if (Math.Abs(augmented[k, i]) > 1e-9)
                            {
                                sb.AppendLine($"Перестановка рядків {i + 1} та {k + 1}");
                                for (int j = 0; j <= n; j++)
                                {
                                    double temp = augmented[i, j];
                                    augmented[i, j] = augmented[k, j];
                                    augmented[k, j] = temp;
                                }
                                swapped = true;
                                break;
                            }
                        }
                        if (!swapped) continue;
                    }

                    for (int k = i + 1; k < n; k++)
                    {
                        double factor = augmented[k, i] / augmented[i, i];
                        sb.AppendLine($"Рядок {k + 1} = Рядок {k + 1} - ({factor:F4} * Рядок {i + 1})");
                        for (int j = i; j <= n; j++)
                            augmented[k, j] -= factor * augmented[i, j];
                    }
                    LogMatrix(sb, augmented);
                }

                // Зворотний хід
                double[] x = new double[n];
                sb.AppendLine("\nЗворотний хід:");
                for (int i = n - 1; i >= 0; i--)
                {
                    double sum = 0;
                    for (int j = i + 1; j < n; j++) sum += augmented[i, j] * x[j];
                    x[i] = (augmented[i, n] - sum) / augmented[i, i];
                    sb.AppendLine($"x{i + 1} = ({augmented[i, n]:F2} - {sum:F2}) / {augmented[i, i]:F2} = {x[i]:F2}");
                }

                log = sb.ToString();
                return x;
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

            public static void ParseConstraint(string raw, int nVars, out double[] row, out double rhs, out string type)
            {
                row = new double[nVars];
                rhs = 0;
                type = "<=";

                // Визначаємо тип обмеження
                if (raw.Contains("<=")) type = "<=";
                else if (raw.Contains(">=")) type = ">=";
                else if (raw.Contains("=")) type = "=";
                else throw new Exception($"Невідомий оператор у: {raw}");

                string[] parts = raw.Split(new[] { "<=", ">=", "=" }, StringSplitOptions.None);
                string lhs = parts[0].Replace(" ", "");
                double originalRhs = ParseNum(parts[1]);

                // Регулярний вираз для пошуку x_i (коефіцієнт та індекс)
                // Шукає групи: (коефіцієнт)x(індекс)
                var xMatches = Regex.Matches(lhs, @"([+-]?\d*\.?\d*)x(\d+)");
                double constantOnLhs = 0;

                // Видаляємо знайдені змінні з рядка, щоб знайти чисті константи
                string remainingLhs = lhs;
                foreach (Match m in xMatches)
                {
                    int idx = int.Parse(m.Groups[2].Value) - 1;
                    if (idx < nVars)
                    {
                        row[idx] += ParseCoef(m.Groups[1].Value);
                    }
                    // Замінюємо знайдену частину на пусте місце, щоб вона не заважала
                    remainingLhs = remainingLhs.Replace(m.Value, "");
                }

                // Шукаємо константи, що залишилися на LHS (наприклад, "+1" у "x1+2x2+1>=0")
                var constMatches = Regex.Matches(remainingLhs, @"[+-]?\d+\.?\d*");
                foreach (Match m in constMatches)
                {
                    constantOnLhs += ParseNum(m.Value);
                }

                // Фінальний RHS = Те, що було праворуч - Те, що перенесли зліва
                rhs = originalRhs - constantOnLhs;
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
            //   min: T[zRow, total] = -Z*  (тому знак змінюємо)
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

                // ════ ВАША ОРИГІНАЛЬНА ЛОГІКА (БЕЗ ЗМІН) ════
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
                // ════ КІНЕЦЬ БАЗОВОЇ ІНІЦІАЛІЗАЦІЇ ════

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

                // ════ ВАША ОРИГІНАЛЬНА ЛОГІКА ІТЕРАЦІЙ ════
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
                                       // ════ КІНЕЦЬ ВАШОЇ ОРИГІНАЛЬНОЇ ЛОГІКИ ════

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


            //Part_C

            // Головний метод для розв'язання задачі зі змішаними обмеженнями через МЖВ
            public static double[] SolveMixedMJE(
                double[] cObj, 
                List<double[]> A, 
                List<double> b, 
                List<string> types, 
                int nVars, 
                bool isMax, 
                out double optZ, 
                out string log, 
                out string freeVars)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=== Розв'язання задачі ЗЛП методом МЖВ ===");

                int nC = A.Count;
                int cols = nVars + 1;
                int rows = nC + 1;
                int freeCol = cols - 1;
                int zRow = rows - 1;

                double[,] T = new double[rows, cols];
                List<string> rowHeaders = new List<string>();
                List<string> colHeaders = new List<string>();

                for (int j = 0; j < nVars; j++) colHeaders.Add($"x{j + 1}");
                colHeaders.Add("1");

                // Ініціалізація початкової симплекс-таблиці
                for (int i = 0; i < nC; i++)
                {
                    if (types[i] == "<=")
                    {
                        for (int j = 0; j < nVars; j++) T[i, j] = A[i][j];
                        T[i, freeCol] = b[i];
                        rowHeaders.Add($"y{i + 1}");
                    }
                    else if (types[i] == ">=")
                    {
                        for (int j = 0; j < nVars; j++) T[i, j] = -A[i][j];
                        T[i, freeCol] = -b[i];
                        rowHeaders.Add($"y{i + 1}");
                    }
                    else // "="
                    {
                        for (int j = 0; j < nVars; j++) T[i, j] = A[i][j];
                        T[i, freeCol] = b[i];
                        // У 0-рядках вільний член має бути >= 0
                        if (T[i, freeCol] < 0)
                        {
                            for (int j = 0; j <= nVars; j++) T[i, j] = -T[i, j];
                        }
                        rowHeaders.Add("0");
                    }
                }

                // Рядок цільової функції Z
                for (int j = 0; j < nVars; j++) T[zRow, j] = isMax ? -cObj[j] : cObj[j];
                T[zRow, freeCol] = 0;
                rowHeaders.Add("Z");

                sb.AppendLine("\nПочаткова симплекс-таблиця:");
                LogTableauMJE(sb, T, rowHeaders, colHeaders);

                // --- ЕТАП 1: Видалення 0-рядків (Рисунок 3.2) ---
                sb.AppendLine("\n--- Етап 1: Видалення 0-рядків ---");
                while (rowHeaders.Contains("0"))
                {
                    int r0 = rowHeaders.IndexOf("0");

                    int s = -1;
                    for (int j = 0; j < colHeaders.Count - 1; j++)
                    {
                        if (T[r0, j] > 1e-9) { s = j; break; }
                    }

                    if (s == -1) throw new Exception("Система обмежень є суперечливою (немає додатного елемента в 0-рядку).");

                    int pivot_r = -1;
                    double minRatio = double.MaxValue;
                    for (int i = 0; i < rowHeaders.Count - 1; i++) // Без рядка Z
                    {
                        if (T[i, s] > 1e-9)
                        {
                            double ratio = T[i, freeCol] / T[i, s];
                            if (ratio < minRatio) { minRatio = ratio; pivot_r = i; }
                        }
                    }

                    sb.AppendLine($"Розв'язувальний елемент: [{rowHeaders[pivot_r]}, {colHeaders[s]}] = {T[pivot_r, s]:F3}");
                    T = MjeStep(T, pivot_r, s, T.GetLength(0), T.GetLength(1));

                    string temp = rowHeaders[pivot_r];
                    rowHeaders[pivot_r] = colHeaders[s].Replace("-", ""); // Прибираємо мінус, якщо був
                    colHeaders[s] = temp;

                    if (colHeaders[s] == "0")
                    {
                        T = DeleteColumn(T, s);
                        colHeaders.RemoveAt(s);
                        freeCol--; // Кількість стовпців зменшилась
                    }
                    LogTableauMJE(sb, T, rowHeaders, colHeaders);
                }

                // --- ЕТАП 2: Пошук опорного розв'язку ---
                sb.AppendLine("\n--- Етап 2: Пошук опорного розв'язку ---");
                while (true)
                {
                    int r = -1;
                    double minFree = -1e-9;
                    for (int i = 0; i < rowHeaders.Count - 1; i++)
                    {
                        if (T[i, freeCol] < minFree) { minFree = T[i, freeCol]; r = i; }
                    }

                    if (r == -1) break; // Опорний розв'язок знайдено

                    int s = -1;
                    for (int j = 0; j < colHeaders.Count - 1; j++)
                    {
                        if (T[r, j] < -1e-9) { s = j; break; }
                    }

                    if (s == -1) throw new Exception("Система не має опорних розв'язків.");

                    // Для пошуку опорного розв'язку робимо звичайний крок МЖВ
                    sb.AppendLine($"Розв'язувальний елемент: [{rowHeaders[r]}, {colHeaders[s]}] = {T[r, s]:F3}");
                    T = MjeStep(T, r, s, T.GetLength(0), T.GetLength(1));

                    string temp = rowHeaders[r];
                    rowHeaders[r] = colHeaders[s];
                    colHeaders[s] = temp;

                    LogTableauMJE(sb, T, rowHeaders, colHeaders);
                }

                // --- ЕТАП 3: Пошук оптимального розв'язку ---
                sb.AppendLine("\n--- Етап 3: Пошук оптимального розв'язку ---");
                while (true)
                {
                    int s = -1;
                    double maxVal = -1e-9;
                    for (int j = 0; j < colHeaders.Count - 1; j++)
                    {
                        if (T[zRow, j] < maxVal) { maxVal = T[zRow, j]; s = j; }
                    }

                    if (s == -1) break; // Оптимум знайдено!

                    int pivot_r = -1;
                    double minRatio = double.MaxValue;
                    for (int i = 0; i < rowHeaders.Count - 1; i++)
                    {
                        if (T[i, s] > 1e-9)
                        {
                            double ratio = T[i, freeCol] / T[i, s];
                            if (ratio < minRatio) { minRatio = ratio; pivot_r = i; }
                        }
                    }

                    if (pivot_r == -1) throw new Exception("Функція не обмежена.");

                    sb.AppendLine($"Розв'язувальний елемент: [{rowHeaders[pivot_r]}, {colHeaders[s]}] = {T[pivot_r, s]:F3}");
                    T = MjeStep(T, pivot_r, s, T.GetLength(0), T.GetLength(1));

                    string temp = rowHeaders[pivot_r];
                    rowHeaders[pivot_r] = colHeaders[s];
                    colHeaders[s] = temp;

                    LogTableauMJE(sb, T, rowHeaders, colHeaders);
                }

                var freeList = new List<string>();
                for (int j = 0; j < colHeaders.Count - 1; j++)
                {
                    // Додаємо лише ті, що є іксами (x1, x2...)
                    if (colHeaders[j].StartsWith("x")) freeList.Add(colHeaders[j]);
                }
                freeVars = string.Join(" ", freeList);

                // Формування кінцевого результату
                double[] X = new double[nVars];
                for (int j = 0; j < nVars; j++)
                {
                    string vName = $"x{j + 1}";
                    int idx = rowHeaders.IndexOf(vName);
                    if (idx != -1) X[j] = T[idx, freeCol];
                    else X[j] = 0; // Якщо змінна залишилась не базисною
                }

                optZ = T[zRow, freeCol];
                if (!isMax) optZ = -optZ; // Для мінімізації повертаємо знак назад

                sb.AppendLine($"\nОптимальний розв'язок:");
                sb.AppendLine($"Z = {optZ:F3}");
                log = sb.ToString();
                return X;
            }

            // Допоміжний метод для красивого логування компактної таблиці МЖВ
            public static void LogTableauMJE(StringBuilder sb, double[,] matrix, List<string> rowHeaders, List<string> colHeaders)
            {
                sb.AppendFormat("{0,5} |", "");
                for (int j = 0; j < colHeaders.Count; j++) sb.AppendFormat("{0,8}", colHeaders[j]);
                sb.AppendLine();
                sb.AppendLine(new string('-', 7 + 8 * colHeaders.Count));

                for (int i = 0; i < rowHeaders.Count; i++)
                {
                    sb.AppendFormat("{0,5} |", rowHeaders[i]);
                    for (int j = 0; j < colHeaders.Count; j++)
                    {
                        sb.AppendFormat("{0,8:F2}", matrix[i, j]);
                    }
                    sb.AppendLine();
                }
            }

            // Метод для видалення 0-рядків (для змішаних обмежень)
            public static double[,] RemoveZeroRows(double[,] table, ref List<string> rowHeaders, ref List<string> colHeaders, StringBuilder log)
            {
                int rows = table.GetLength(0);
                int cols = table.GetLength(1);

                for (int i = 0; i < rows; i++)
                {
                    // Шукаємо рядок, який позначений як "0 =" (базисна зміна 0)
                    if (rowHeaders[i] == "0")
                    {
                        log.AppendLine($"\nОбробка нуль-рядка #{i + 1}");

                        // Шукаємо додатний елемент у цьому рядку (крім вільного члена)
                        int s = -1;
                        for (int j = 0; j < cols - 1; j++)
                        {
                            if (table[i, j] > 1e-9) { s = j; break; }
                        }

                        if (s != -1)
                        {
                            log.AppendLine($"Розв’язувальний рядок: {rowHeaders[i]}, стовпець: {colHeaders[s]}");

                            // Робимо крок МЖВ
                            table = MjeStep(table, i, s);

                            // Викреслюємо стовпець (за алгоритмом Рисунку 3.2)
                            table = DeleteColumn(table, s);
                            colHeaders.RemoveAt(s);

                            // Рядок стає звичайною базисною змінною
                            rowHeaders[i] = colHeaders[s].Replace("-", "");

                            log.AppendLine("Матриця після видалення нуль-рядка:");
                            log.AppendLine(MatrixToString(table));

                            // Перевіряємо матрицю заново після зміни розмірності
                            return RemoveZeroRows(table, ref rowHeaders, ref colHeaders, log);
                        }
                        else if (Math.Abs(table[i, cols - 1]) > 1e-9)
                        {
                            throw new Exception("Система обмежень є суперечливою!");
                        }
                    }
                }
                return table;
            }

            // Допоміжний метод для видалення стовпця
            private static double[,] DeleteColumn(double[,] matrix, int colToDelete)
            {
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1);
                double[,] result = new double[rows, cols - 1];

                for (int i = 0; i < rows; i++)
                {
                    int targetCol = 0;
                    for (int j = 0; j < cols; j++)
                    {
                        if (j == colToDelete) continue;
                        result[i, targetCol] = matrix[i, j];
                        targetCol++;
                    }
                }
                return result;
            }

            // Універсальний крок Модифікованих Жорданових Виключень (МЖВ)
            public static double[,] MjeStep(double[,] T, int r, int s)
            {
                int rows = T.GetLength(0);
                int cols = T.GetLength(1);
                double pivot = T[r, s];

                if (Math.Abs(pivot) < 1e-12)
                    throw new Exception($"Розв'язувальний елемент [{r},{s}] дорівнює 0. Крок МЖВ неможливий.");

                double[,] N = new double[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (i == r && j == s)
                            N[i, j] = 1.0 / pivot;
                        else if (i == r)
                            N[i, j] = T[r, j] / pivot;
                        else if (j == s)
                            N[i, j] = -T[i, s] / pivot;
                        else
                            N[i, j] = T[i, j] - (T[i, s] * T[r, j]) / pivot;
                    }
                }
                return N;
            }

            // Допоміжний метод для перетворення матриці у рядок (для логування)
            public static string MatrixToString(double[,] matrix)
            {
                StringBuilder sb = new StringBuilder();
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        // Форматуємо кожен елемент з вирівнюванням по правому краю (8 символів, 2 знаки після коми)
                        sb.AppendFormat("{0,8:F2}", matrix[i, j]);
                    }
                    sb.AppendLine();
                }

                return sb.ToString();
            }
        }
    }
}