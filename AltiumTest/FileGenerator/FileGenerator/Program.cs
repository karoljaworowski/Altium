// See https://aka.ms/new-console-template for more information
using Bogus;
using ShellProgressBar;

Console.WriteLine("Hello, World!");

var faker = new Faker();

Console.WriteLine("How many words do you want to draw");
int wordsNumer;
while (!int.TryParse(Console.ReadLine(), out wordsNumer))
{
    Console.WriteLine("How many words do you want to draw");
}

Console.WriteLine("Number of lines in file");
int linesNumber;
while (!int.TryParse(Console.ReadLine(), out linesNumber))
{
    Console.WriteLine("Number of lines");
}

Console.WriteLine("Range of numbers to draw");
int range;
while (!int.TryParse(Console.ReadLine(), out range))
{
    Console.WriteLine("Range of numbers");
}

var listOfWords = new List<string>();
for (int i = 0; i < wordsNumer; i++)
{
    var word = $"{faker.Vehicle.Model()} - {faker.Vehicle.Fuel()}";
    Console.WriteLine(word);
    listOfWords.Add(word);
}

var randomNumber = new Random();
var randomWord = new Random();
Directory.CreateDirectory("C:/Karol");
using (StreamWriter file = new("C:/Karol/input.txt"))
{
    var options = new ProgressBarOptions
    {
        ProgressCharacter = '─',
        ProgressBarOnBottom = true
    };
    using (var pbar = new ProgressBar(linesNumber, "File generating", options))
    {
        for (int i = 0; i < linesNumber; i++)
        {
            var line = $"{randomNumber.Next(range)}. {listOfWords[randomWord.Next(wordsNumer)]} ";
            await file.WriteLineAsync(line);
            pbar.Tick();
        }
    }
}