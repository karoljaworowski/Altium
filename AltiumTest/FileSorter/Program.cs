using ShellProgressBar;
using System.Data;

Console.WriteLine("Sort data");

var listOfFiles = new List<string>();

var partsFilesPath = "C:/Karol/Parts";
if (Directory.Exists(partsFilesPath))
{
    Directory.Delete(partsFilesPath, true);
}
Directory.CreateDirectory(partsFilesPath);

var path = "C:/Karol/Parts/{0}_input.txt";
var outputPath = "C:/Karol/output.txt";
if (File.Exists(outputPath))
{
    File.Delete(outputPath);
}
var options = new ProgressBarOptions
{
    ProgressCharacter = '─',
    ProgressBarOnBottom = true
};

var totalLines = File.ReadLines(@"C:/Karol/input.txt").Count();

using (var pbar = new ProgressBar(totalLines, "Splitting data", options))
{
    foreach (string line in File.ReadLines(@"C:/Karol/input.txt"))
    {
        var data = line.Split(". ");
        var prefix = data[1].Substring(0, 5);
        var filePath = string.Format(path, prefix);
        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLine(line);
        }
        pbar.Tick();
        pbar.Message = $"Splitting data - {data[1]}";
    }
}

var fileOrderdList = new DirectoryInfo(partsFilesPath)
    .GetFileSystemInfos()
    .OrderBy(file => file.Name)
    .ToList();

using (StreamWriter sw = File.AppendText(outputPath))
{
    using (var pbar = new ProgressBar(fileOrderdList.Count, "Sorting data", options))
    {
        foreach (var file in fileOrderdList)
        {
            pbar.Message = $"File '{file.Name}' processing";
            var dataList = File.ReadAllLines(file.FullName)
                .Select(line => line.Split(". "))
                .Select(data => new Tuple<int, string>(int.Parse(data[0]), data[1]))
                .OrderBy(data => data.Item2).ThenBy(data => data.Item1)
                .ToList();

            foreach (var data in dataList)
            {
                var line = $"{data.Item1}. {data.Item2}";
                sw.WriteLine(line);
            }
            pbar.Tick();
        }
    }
}

