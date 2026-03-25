using System;

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

        public class MatrixMath
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
        }
    }
}