using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace LAB2PO
{
    public static class MatrixIO
    {
        public static async Task WriteMaAsync(this StreamWriter sw, Matrix m, string sep = " ")
        {
            await sw.WriteLineAsync($"{m.a_rows} {m.a_cols}");

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m.a_rows; i++)
            {
                for (int j = 0; j < m.a_cols; j++)
                {
                    sb.Append(m[i, j]);
                    if (j < m.a_cols - 1)
                    {
                        sb.Append(sep);
                    }
                }
                await sw.WriteLineAsync(sb.ToString());
                sb.Clear();
            }
        }

        public static async Task<Matrix> ReadMAsync(this StreamReader sr, string sep = " ")
        {
            string line;
            int rowc = 0;
            int colc = 0;
            double[,] res = null;

            while ((line = await sr.ReadLineAsync()) != null)
            {
                string[] el = line.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
                if (rowc == 0)
                {
                    rowc = 1;
                    colc = el.Length;
                    res = new double[rowc, colc];
                }

                else
                {
                    if (el.Length != colc)
                    {
                        throw new FormatException("Impossible");
                    }

                    double[,] temp = new double[rowc + 1, colc];
                    Array.Copy(res, temp, rowc * colc);
                    res = temp;
                    rowc++;
                }

                for (int i = 0; i < colc; i++)
                {
                    if (!double.TryParse(el[i], out double val))
                    {
                        throw new FormatException("No way");
                    }
                    res[rowc - 1, i] = val;
                }
            }

            if (res == null)
            {
                throw new FormatException("Nope");
            }
            return new Matrix(res);
        }

        public static void WriteMBinary(this BinaryWriter binwriter, Matrix m)
        {
            binwriter.Write(m.a_rows);
            binwriter.Write(m.a_cols);

            for (int i = 0; i < m.a_cols; i++)
            {
                for (int j = 0; j < m.a_cols; j++)
                {
                    binwriter.Write(m[i, j]);
                }
            }
        }

        public static Matrix ReadMBinary(this BinaryReader binread)
        {
            int r = binread.ReadInt32();
            int c = binread.ReadInt32();

            double[,] res = new double[r, c];

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    res[i, j] = binread.ReadDouble();
                }
            }
            return new Matrix(res);
        }

        public static async Task WriteMJsonAsync(this StreamWriter sw, Matrix m)
        {
            var marray = new double[m.a_rows][];
            for (int i = 0; i < m.a_rows; i++)
            {
                marray[i] = new double[m.a_cols];
                for (int j = 0; j < m.a_cols; j++)
                {
                    marray[i][j] = m[i, j];
                }
            }
            await JsonSerializer.SerializeAsync(sw.BaseStream, marray);
        }

        public static async Task<Matrix> ReadMJsonAsync(this StreamReader read)
        {
            var marray = await JsonSerializer.DeserializeAsync<double[][]>(read.BaseStream);

            int r = marray.Length;
            int c = marray[0].Length;
            double[,] res = new double[r, c];

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    res[i, j] = marray[i][j];
                }
            }
            return new Matrix(res);
        }

        public static void WriteM(string dir, string fn, Matrix m, Action<Matrix, Stream> writeF)
        {
            string path = Path.Combine(dir, fn);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                writeF(m, fs);
            }
        }

        public static async Task WriteMAsync(string dir, string fn, Matrix m, Func<Matrix, Stream, Task> writeF)
        {
            string path = Path.Combine(dir, fn);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                await writeF(m, fs);
            }
        }

        public static Matrix ReadM(string file, Func<Stream, Matrix> readF)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                return readF(fs);
            }
        }

        public static async Task<Matrix> ReadMAsync(string file, Func<Stream, Task<Matrix>> readF)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                return await readF(fs);
            }
        }
    }
}