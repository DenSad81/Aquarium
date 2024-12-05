using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Aquarium aquarium = new Aquarium();
        FishGenerator generatorOfFish = new FishGenerator();
        Game game = new Game();

        game.Live(aquarium, generatorOfFish);
    }
}

class Game
{
    public void Live(Aquarium aquarium, FishGenerator generatorOfFish)
    {
        const string CommandAddFish = "1";
        const string CommandTakeFish = "2";
        const string CommandExit = "9";
        bool isRun = true;

        while (isRun)
        {
            aquarium.ShowFishs();
            Console.SetCursorPosition(0, 10);
            Console.WriteLine($"Menu: {CommandAddFish}-Add fish;");
            Console.WriteLine($"      {CommandTakeFish}-Take fish;");
            Console.WriteLine($"      {CommandExit}-Exit;");

            switch (Utils.ReadString("Your shois: "))
            {
                case CommandAddFish:
                    aquarium.AddFish(generatorOfFish.CreateFish());
                    break;

                case CommandTakeFish:
                    aquarium.DeleteFish(Utils.ReadInt("Input id of fish: "));
                    break;

                case CommandExit:
                    isRun = false;
                    break;
            }

            aquarium.LiveDay();
            aquarium.DeleteDeadFishs();
            Console.ReadKey();
            Console.Clear();
        }
    }
}

class Aquarium
{
    private int _maxQuantityOfFishs = 8;
    private List<Fish> _fishs = new List<Fish>();

    public void AddFish(Fish fish)
    {
        if (_fishs.Count < _maxQuantityOfFishs)
            _fishs.Add(fish);
    }

    private bool TryGetFish(int id, out Fish fish)
    {
        for (int i = 0; i < _fishs.Count; i++)
        {
            if (_fishs[i].Id == id)
            {
                fish = _fishs[i];
                return true;
            }
        }

        Console.WriteLine("Wrong ID");
        fish = null;
        return false;
    }

    public void DeleteFish(int id)
    {
        if (TryGetFish(id, out Fish fish) == false)
            return;

        _fishs.Remove(fish);
    }

    public void ShowFishs()
    {
        Console.WriteLine("Id   Name   Rest of life");

        foreach (var fish in _fishs)
            fish.ShowStats();
    }

    public void LiveDay()
    {
        foreach (var fish in _fishs)
            fish.GrowOld();
    }

    public void DeleteDeadFishs()
    {
        int actualFishIndex = 0;

        while (actualFishIndex < _fishs.Count)
        {
            if (_fishs[actualFishIndex].CheckIsAlife)
                actualFishIndex++;
            else
                _fishs.RemoveAt(actualFishIndex);
        }
    }
}

class Fish
{
    private static int s_id = 0;
    private string _name;
    private int _maxAge;
    private int _actualAge;

    public Fish(string name, int maxAge)
    {
        _name = name;
        _maxAge = maxAge;
        Id = s_id;
        s_id++;
        _actualAge = 0;
    }

    public bool CheckIsAlife => _actualAge < _maxAge;
    public int Id { get; }

    public void ShowStats() =>
        Console.WriteLine(Id + "    " + _name + "     " + (_maxAge - _actualAge));

    public void GrowOld() =>
        _actualAge++;
}

class FishGenerator
{
    public Fish CreateFish()
    {
        int minLife = 3;
        int maxLife = 9;
        string[] names = new string[] { "Airo", "Amad", "Fair", "Hayk", "Alf", "Argo", "Nemo", "Maxx" };

        string name = names[Utils.GenerateRandomNumber(0, names.Length)];
        int age = Utils.GenerateRandomNumber(minLife, maxLife);
        return new Fish(name, age);
    }
}

static class Utils
{
    private static Random s_random = new Random();

    public static int GenerateRandomNumber(int min, int max)
    {
        return s_random.Next(min, max);
    }

    public static int ReadInt(string text = "", int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        int number;
        Console.Write(text + " ");

        while (int.TryParse(Console.ReadLine(), out number) == false || number > maxValue || number < minValue)
            Console.Write(text + " ");

        return number;
    }

    public static string ReadString(string text = "")
    {
        Console.Write(text + " ");
        string tempString = Console.ReadLine().ToLower();
        Console.WriteLine();
        return tempString;
    }
}