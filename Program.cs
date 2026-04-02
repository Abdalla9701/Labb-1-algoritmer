using System.Diagnostics;
using System.Text;

const int MinValue = 1;
const int MaxValue = 100;

var csv = new StringBuilder();
csv.AppendLine("Storlek,Run,ArraySortMs,RakningssorteringMs");

string filePath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
    "resultat.csv");

int[] sizes = { 100, 500, 1000, 2000, 5000, 10000, 20000, 50000, 100000 };
int numberOfRuns = 10;
var rand = new Random(42);

foreach (int size in sizes)
{
    for (int run = 1; run <= numberOfRuns; run++)
    {
        int[] original = GenerateRandomArray(size, rand, MinValue, MaxValue);
        int[] arrayForSort = (int[])original.Clone();
        int[] arrayForCountingSort = (int[])original.Clone();

        long arraySortMs = MeasureMilliseconds(() => Array.Sort(arrayForSort));
        long countingSortMs = MeasureMilliseconds(() => Rakningssortering(arrayForCountingSort, MinValue, MaxValue));

        csv.AppendLine($"{size},{run},{arraySortMs},{countingSortMs}");
    }
}

File.WriteAllText(filePath, csv.ToString());
Console.WriteLine($"Klar. Resultat sparat i: {filePath}");

static int[] GenerateRandomArray(int size, Random rand, int minValue, int maxValue)
{
    var values = new int[size];
    for (int i = 0; i < size; i++)
    {
        values[i] = rand.Next(minValue, maxValue + 1);
    }

    return values;
}

static long MeasureMilliseconds(Action action)
{
    var sw = Stopwatch.StartNew();
    action();
    sw.Stop();
    return sw.ElapsedMilliseconds;
}

static void Rakningssortering(int[] arr, int minValue, int maxValue)
{
    int[] count = new int[maxValue - minValue + 1];

    for (int i = 0; i < arr.Length; i++)
    {
        count[arr[i] - minValue]++;
    }

    int index = 0;
    for (int value = minValue; value <= maxValue; value++)
    {
        int occurrences = count[value - minValue];
        for (int i = 0; i < occurrences; i++)
        {
            arr[index] = value;
            index++;
        }
    }
}