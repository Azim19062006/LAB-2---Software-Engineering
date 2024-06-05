using System;
using System.Text;

namespace LAB2PO
{
    public class Matrix
    {
        private readonly double[,] matrix;
        public Matrix(double[,] vals)
        {
            this.matrix = vals;
        }

        public int a_rows
        {
            get
            {
                return matrix.GetLength(0);
            }
        }

        public int a_cols
        {
            get
            {
                return matrix.GetLength(1);
            }
        }

        public double this[int i, int j]
        {
            get
            {
                return matrix[i, j];
            }
        }

        public static Matrix Zero(int row, int col)
        {
            double[,] res = new double[row, col];
            return new Matrix(res);
        }

        public static Matrix Zero(int n)
        {
            return Zero(n, n);
        }

        public static Matrix Identity(int n)
        {
            double[,] res = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                res[i, i] = 1;
            }
            return new Matrix(res);
        }

        public Matrix Transpose()
        {
            double[,] res = new double[a_cols, a_rows];
            for (int i = 0; i < a_rows; i++)
            {
                for (int j = 0; j < a_cols; j++)
                {
                    res[j, i] = matrix[i, j];
                }
            }
            return new Matrix(res);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < a_rows; i++)
            {
                for (int j = 0; j < a_cols; j++)
                {
                    sb.Append(matrix[i, j] + " ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public override bool Equals(object ob)
        {
            if (ob == null || GetType() != ob.GetType())
            {
                return false;
            }

            Matrix o = (Matrix)ob;
            if (a_rows != o.a_rows || a_cols != o.a_cols)
            {
                return false;
            }

            for (int i = 0; i < a_rows; i++)
            {
                for (int j = 0; j < a_cols; j++)
                {
                    if (matrix[i, j] != o.matrix[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            const int p = 31;
            int h = 17;
            h = h * p + a_rows.GetHashCode();
            h = h * p + a_cols.GetHashCode();
            for (int i = 0; i < a_rows; i++)
            {
                for (int j = 0; j < a_cols; j++)
                {
                    h = h * p + matrix[i, j].GetHashCode();
                }
            }
            return h;
        }

        public static Matrix operator *(Matrix m, double scal)
        {
            return MatrixOperations.MultiplyScalar(m, scal);
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            return MatrixOperations.Multiply(m1, m2);
        }

        public static Matrix operator /(Matrix m1, Matrix m2)
        {
            return MatrixOperations.Divide(m1, m2);
        }

        public static Matrix operator /(Matrix m, double div)
        {
            return MatrixOperations.Divide(m, div);
        }

        public static Matrix operator ~(Matrix m)
        {
            return MatrixOperations.Transpose(m);
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            return MatrixOperations.Add(m1, m2);
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return MatrixOperations.Subtract(m1, m2);
        }

        public static Matrix operator !(Matrix m)
        {
            double[,] res = new double[m.a_cols, m.a_rows];
            for (int i = 0; i < m.a_rows; i++)
            {
                for (int j = 0; j < m.a_cols; j++)
                {
                    res[i, j] = -m.matrix[i, j];
                }
            }
            return new Matrix(res);
        }
        public static Matrix Inverse(Matrix m)
        {
            return MatrixOperations.Inverse(m);
        }
    }
}