using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2PO
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            string calcDir = "Calculations";
            string textdir = "Text";
            string jsonDir = "Json";
            string binaryDir = "Binary";

            CreateDir(calcDir);
            CreateDir(textdir);
            CreateDir(jsonDir);
            CreateDir(binaryDir);

            Matrix[] a = RandomMCreator2(50, 500, 100);
            Matrix[] b = RandomMCreator2(50, 100, 500);

            Task calcTask = Task.Run(async () =>
            {
                await RunCalcAsync(a, b, calcDir);
            });

            Task writeAsync = Task.Run(async () =>
            {
                await WriteMatricesAsync(a, b, textDir, jsonDir, binaryDir);
            });

            Task readasyncT = Task.Run(async () =>
            {
                await ReadCompareMAsync(a, textdir, jsonDir);
            });

            await Task.WhenAll(calcTask, writeAsync, readasyncT);

            Console.WriteLine("Finished");
        }

        static Matrix[] RandomMCreator2(int count, int r, int c)
        {
            var m = new Matrix[count];
            for (int i = 0; i < count; i++)
            {
                m[i] = RandomMCreator(r, c);
            }
            return m;
        }

        static void CreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
            Directory.CreateDirectory(dir);
        }

        static async Task RunCalcAsync(Matrix[] a, Matrix[] b, string calcDir)
        {
            var task = new Task[]
            {
                Task.Run(() => MultiplySaveAsync(a, b, calcDir, "AB")),
                Task.Run(() => MultiplySaveAsync(b, a, calcDir, "BA")),
                Task.Run(() => ScalarProdSaveAsync(a, b, calcDir, "ScalarAB")),
                Task.Run(() => ScalarProdSaveAsync(b, a, calcDir, "ScalarBA"))
            };
            await Task.WhenAll(task);
        }

        static async Task MulitplySaveAsync(Matrix[] fir, Matrix[] sec, string dir, string prefix)
        {
            for (int i = 0; i < fir.Length; i++)
            {
                var res = fir[i] * sec[i];
                string fn = $"{prefix}_{i}.txt";
                await MatrixIO.WriteMAsync(res, Path.Combine(dir, fn));
                Console.WriteLine("Calc is finished");
            }
        }

        static async Task ScalarProdSaveAsync(Matrix[] fir, Matrix[] sec, string dir, string prefix)
        {
            for (int i = 0; i < fir.Length; i++)
            {
                var result = fir[i].MatrixIO.MultiplyScalar(sec[i]);
                string fileName = $"{prefix}{i}.txt";
                await MatrixIO.WriteMAsync(result, Path.Combine(dir, fileName));
                Console.WriteLine($"Calc is finished");
            }
        }

        static async Task WriteMsAsync(Matrix[] a, Matrix[] b, string textDir, string jsonDir, string binaryDir)
        {
            WriteMToDirAsync(a, textDir, "A_", "txt", MatrixIO.WriteMaAsync),
              WriteMToDirAsync(b, textDir, "B_", "txt", MatrixIO.WriteTextAsync),
              WriteMToDirAsync(a, jsonDir, "A_", "json", MatrixIO.WriteMJsonAsync),
              WriteMToDirAsync(b, jsonDir, "B_", "json", MatrixIO.WriteMJsonAsync),
              Task.Run(() => WriteMToDir(a, binaryDir, "A_", "bin", MatrixIO.WriteBin)),
              Task.Run(() => WriteMToDir(b, binaryDir, "B_", "bin", MatrixIO.WriteBin));
        }
    }
}