using System;
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

            // Пошук оберненої матриці
            public static double[,] Inverse(double[,] matrix)
            {
                int n = matrix.GetLength(0);
                if (n != matrix.GetLength(1)) throw new Exception("Матриця має бути квадратною!");

                double[,] result = (double[,])matrix.Clone();

                for (int i = 0; i < n; i++)
                {
                    result = ZHV_Step(result, i, i);
                }

                return result;
            }

            // Обчислення рангу матриці
            public static int Rank(double[,] A)
            {
                int n = A.GetLength(0);
                int m = A.GetLength(1);
                double[,] current = (double[,])A.Clone();
                int r = 0;

                for (int i = 0; i < Math.Min(n, m); i++)
                {
                    if (Math.Abs(current[i, i]) > 1e-9)
                    {
                        current = ZHV_Step(current, i, i);
                        r++;
                    }
                }
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

            // Спосіб 1: Розв'язання СЛАР (X = A^(-1) * B)
            public static double[] SolveMethod1(double[,] A, double[] B_vec)
            {
                double[,] invA = Inverse(A);
                return MultiplyMatrixByVector(invA, B_vec);
            }

            // Спосіб 2: Приведення до вигляду AX - B = 0
            public static double[] SolveMethod2(double[,] A, double[] B_vec)
            {
                int n = A.GetLength(0);
                double[,] augmented = new double[n, n + 1];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++) augmented[i, j] = A[i, j];
                    augmented[i, n] = -B_vec[i];
                }

                double[,] current = augmented;
                for (int i = 0; i < n; i++)
                    current = ZHV_Step(current, i, i);

                double[] x = new double[n];
                for (int i = 0; i < n; i++) x[i] = current[i, n];
                return x;
            }

            // Спосіб 3: Розв'язання СЛАР Методом Гаусса
            public static double[] SolveMethod3(double[,] A, double[] B_vec)
            {
                int n = A.GetLength(0);
                double[,] augmented = new double[n, n + 1];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++) augmented[i, j] = A[i, j];
                    augmented[i, n] = B_vec[i];
                }

                for (int i = 0; i < n; i++)
                {
                    if (Math.Abs(augmented[i, i]) < 1e-9)
                    {
                        for (int k = i + 1; k < n; k++)
                        {
                            if (Math.Abs(augmented[k, i]) > 1e-9)
                            {
                                for (int j = 0; j <= n; j++)
                                {
                                    double temp = augmented[i, j];
                                    augmented[i, j] = augmented[k, j];
                                    augmented[k, j] = temp;
                                }
                                break;
                            }
                        }
                    }

                    for (int k = i + 1; k < n; k++)
                    {
                        double factor = augmented[k, i] / augmented[i, i];
                        for (int j = i; j <= n; j++)
                        {
                            augmented[k, j] -= factor * augmented[i, j];
                        }
                    }
                }

                double[] x = new double[n];
                for (int i = n - 1; i >= 0; i--)
                {
                    double sum = 0;
                    for (int j = i + 1; j < n; j++)
                    {
                        sum += augmented[i, j] * x[j];
                    }
                    x[i] = (augmented[i, n] - sum) / augmented[i, i];
                }

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
            //   min: T[zRow, total] = -Z*  (тому знак змінюємо)
            // ═══════════════════════════════════════════════════════════════════════════
            public static double[] SolveBigM(
                double[] cObj, List<double[]> A, List<double> b, List<string> types,
                int nVars, bool isMax, out double optZ)
            {
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
                // Для Max: Z = ... - M*a => Z + M*a = ... (коеф +M)
                // Для Min: Z = ... + M*a => Z - M*a = ... (коеф -M)
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

                // Симплекс-ітерації
                for (int iter = 0; iter < 1000; iter++)
                {
                    int s = -1;
                    if (isMax)
                    {
                        // Для максимізації шукаємо найбільш від'ємний (вхідний стовпець)
                        double minVal = -1e-9;
                        for (int j = 0; j < total; j++)
                            if (T[zRow, j] < minVal) { minVal = T[zRow, j]; s = j; }
                    }
                    else
                    {
                        // Для мінімізації шукаємо найбільш додатний (вхідний стовпець)
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
                    if (r == -1) throw new Exception("Задача необмежена.");

                    T = MjeStep(T, r, s, nC + 1, total + 1);
                    basis[r] = s;
                }

                // Перевірка на допустимість (штучні змінні мають бути 0)
                for (int i = 0; i < nC; i++)
                    if (basis[i] >= nVars + nSlack && Math.Abs(T[i, total]) > 1e-6)
                        throw new Exception("Система обмежень несумісна.");

                double[] X = new double[nVars];
                for (int i = 0; i < nC; i++)
                    if (basis[i] < nVars) X[basis[i]] = T[i, total];

                optZ = T[zRow, total]; // Значення Z беремо прямо з таблиці
                return X;
            }

        }
    }
}