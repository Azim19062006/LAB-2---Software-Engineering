using System;
using System.IO;
using System.Threading.Tasks;

namespace LAB2PO
{
    public class Program
    {
        public static Matrix RandomMCreator(int r, int c)
        {
            Random rand = new Random();
            double[,] res = new double[r, c];
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    res[i, j] = rand.NextDouble() * 20 - 10;
                }
            }
            return new Matrix(res);
        }

        public static Matrix[] SequentlyMultiply(Matrix[] fir, Matrix[] sec)
        {
            if (fir.Length != sec.Length)
            {
                throw new ArgumentException("Oh, no!");
            }

            Matrix[] res = new Matrix[fir.Length];
            for (int i = 0; i < fir.Length; i++)
            {
                res[i] = fir[i] * sec[i];
            }
            return res;
        }

        public static Matrix ScalarTwoM(Matrix[] fir, Matrix[] sec)
        {
            if (fir.Length != sec.Length)
            {
                throw new ArgumentException("Oh, no!");
            }

            Matrix res = Matrix.Zero(fir[0].a_rows, fir[0].a_cols);
            for (int i = 0; i < fir.Length; i++)
            {
                res += fir[i] * sec[i];
            }
            return res;
        }

        public static void WriteMToDir(Matrix[] m, string dir, string prefix, string extensions, Action<Matrix, Stream> writeF)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            for (int i = 0; i < m.Length; i++)
            {
                string fn = $"{prefix}{i}.{extensions}";
                string path = Path.Combine(dir, fn);
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    writeF(m[i], fs);
                }

                if ((i + 1) % 10 == 0)
                {
                    Console.WriteLine($"Matrix {i + 1} was written to {path}");
                }
            }
        }

        public static async Task WriteMToDirAsync(Matrix[] m, string dir, string prefix, string extensions, Func<Matrix, Stream, Task> writeF)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            for (int i = 0; i < m.Length; i++)
            {
                string fn = $"{prefix}{i}.{extensions}";
                string path = Path.Combine(dir, fn);
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    await writeF(m[i], fs);
                }

                if ((i + 1) % 10 == 0)
                {
                    Console.WriteLine($"Matrix {i + 1} was written to {path}");
                }
            }
        }

        public static Matrix[] ReadMFromDir(string dir, string prefix, string extensions, Func<Stream, Matrix> readF)
        {
            var files = Directory.GetFiles(dir, $"{prefix}*.{extensions}");
            Matrix[] res = new Matrix[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                using (var fs = new FileStream(files[i], FileMode.Open))
                {
                    res[i] = readF(fs);
                }
            }
            return res;
        }

        public static async Task<Matrix[]> ReadMFromDirAsync(string dir, string prefix, string extensions, Func<Stream, Task<Matrix>> readF)
        {
            var files = Directory.GetFiles(dir, $"{prefix} * {extensions}");
            Matrix[] res = new Matrix[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                using (var fs = new FileStream(files[i], FileMode.Open))
                {
                    res[i] = await readF(fs);
                }
            }
            return res;
        }

        public static bool EqualorNot(Matrix[] m1, Matrix[] m2)
        {
            if (m1.Length != m2.Length)
            {
                return false;
            }

            for (int i = 0; i < m1.Length; i++)
            {
                if (!m1[i].Equals(m2[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}