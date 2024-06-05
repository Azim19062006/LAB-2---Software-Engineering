using System;
using System.Threading.Tasks;
using System.Threading;

namespace LAB2PO
{
    public static class MatrixOperations
    {
        public static Matrix Transpose(Matrix m)
        {
            double[,] res = new double[m.a_cols, m.a_rows];
            for (int i = 0; i < m.a_rows; i++)
            {
                for (int j = 0; j < m.a_cols; j++)
                {
                    res[j, i] = m[i, j];
                }
            }
            return new Matrix(res);
        }

        public static Matrix MultiplyScalar(Matrix m, double scal)
        {
            double[,] res = new double[m.a_rows, m.a_cols];
            for (int i = 0; i < m.a_rows; i++)
            {
                for (int j = 0; j < m.a_cols; j++)
                {
                    res[i, j] = m[i, j] * scal;
                }
            }
            return new Matrix(res);
        }

        public static Matrix Add(Matrix m1, Matrix m2)
        {
            if (m1.a_rows != m2.a_rows || m1.a_cols != m2.a_cols)
            {
                throw new Exception("Matrices have different sizes");
            }

            double[,] res = new double[m1.a_rows, m1.a_cols];
            for (int i = 0; i < m1.a_rows; i++)
            {
                for (int j = 0; j < m1.a_cols; j++)
                {
                    res[i, j] = m1[i, j] + m2[i, j];
                }
            }
            return new Matrix(res);
        }

        public static Matrix Subtract(Matrix m1, Matrix m2)
        {
            if (m1.a_rows != m2.a_rows || m1.a_cols != m2.a_cols)
            {
                throw new Exception("Matrices have different sizes");
            }

            double[,] res = new double[m1.a_rows, m1.a_cols];
            for (int i = 0; i < m1.a_rows; i++)
            {
                for (int j = 0; j < m1.a_cols; j++)
                {
                    res[i, j] = m1[i, j] - m2[i, j];
                }
            }
            return new Matrix(res);
        }

        public static Matrix Divide(Matrix m, double div)
        {
            double[,] res = new double[m.a_rows, m.a_cols];
            for (int i = 0; i < m.a_rows; i++)
            {
                for (int j = 0; j < m.a_cols; j++)
                {
                    res[i, j] = m[i, j] / div;
                }
            }
            return new Matrix(res);
        }

        public static Matrix Multiply(Matrix m1, Matrix m2)
        {
            if (m1.a_cols != m2.a_rows)
            {
                throw new ArgumentException("Impossible");
            }

            double[,] res = new double[m1.a_rows, m2.a_cols];
            Parallel.For(0, m1.a_rows, i =>
            {
                for (int j = 0; j < m2.a_cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < m1.a_cols; k++)
                    {
                        sum += m1[i, k] * m2[j, k];
                    }
                    Interlocked.Exchange(ref res[i, j], sum);
                }
            });
            return new Matrix(res);
        }

        public static Matrix Divide(Matrix m1, Matrix m2)
        {
            if (m1.a_cols != m2.a_rows)
            {
                throw new ArgumentException("Impossible");
            }

            try
            {
                Matrix in_m2 = Inverse(m2);
                return Multiply(m1, in_m2);
            }

            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("No way", ex);
            }
        }

        public static Matrix Inverse(Matrix m)
        {
            if (m.a_rows != m.a_cols)
            {
                throw new ArgumentException("Nope");
            }

            int n = m.a_rows;
            double[,] bigger = new double[n, 2 * n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    bigger[i, j] = m[i, j];
                    bigger[i, j + n] = (i == j) ? 1 : 0;
                }
            }

            for (int i = 0; i < n; i++)
            {
                int max_r = i;
                for (int k = i + 1; k < n; k++)
                {
                    if (Math.Abs(bigger[k, i]) > Math.Abs(bigger[max_r, i]))
                    {
                        max_r = k;
                    }
                }

                if (max_r != i)
                {
                    for (int k = 0; k < 2 * n; k++)
                    {
                        double temp = bigger[i, k];
                        bigger[i, k] = bigger[max_r, k];
                        bigger[max_r, k] = temp;
                    }
                }

                double p = bigger[i, i];
                if (p == 0)
                {
                    throw new ArgumentException("No way");
                }

                for (int k = 0; k < 2 * n; k++)
                {
                    bigger[i, k] /= p;
                }

                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        double f = bigger[j, i];
                        for (int k = 0; k < 2 * n; k++)
                        {
                            bigger[j, k] -= f * bigger[i, k];
                        }
                    }
                }
            }

            double[,] inver = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    inver[i, j] = bigger[i, j + n];
                }
            }
            return new Matrix(inver);
        }
    }
}