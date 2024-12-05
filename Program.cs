using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        const string CommandAddFish = "1";
        const string CommandTakeFish = "2";
        const string CommandExit = "9";
        bool isRun = true;
        Aquarium aquarium = new Aquarium();

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
                    aquarium.AddFish(GeneratorOfFish.CreeteNewFish());
                    break;
                case CommandTakeFish:
                    aquarium.DeleteFish(Utils.ReadInt("Input id of fish: "));
                    break;
                case CommandExit:
                    isRun = false;
                    break;
            }

            aquarium.LiveDay();
            aquarium.DeleteDeadBodys();
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

    public void DeleteFish(int id)
    {
        int index = GetIndexFromId(id);

        if (index < 0 || index >= _fishs.Count)
        {
            Console.WriteLine("Wrong index");
            return;
        }

        _fishs.RemoveAt(index);
    }

    public int GetIndexFromId(int id)
    {
        int index = -1;

        for (int i = 0; i < _fishs.Count; i++)
        {
            if (_fishs[i].Id == id)
                index = i;
        }

        return index;
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

    public void DeleteDeadBodys()
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
    private static int _idStatic = 0;
    private string _name;
    private int _maxAge;
    private int _actualAge = 0;

    public Fish(string name, int maxAge)
    {
        _name = name;
        _maxAge = maxAge;
        Id = _idStatic;
        _idStatic++;
    }

    public bool CheckIsAlife => _actualAge < _maxAge;
    public int Id { get; /*private set;*/ }

    public void ShowStats() => Console.WriteLine(Id + "    " + _name + "     " + (_maxAge - _actualAge));

    public void GrowOld() => _actualAge++;
}

static class GeneratorOfFish
{
    private static int _minLife = 3;
    private static int _maxLife = 9;
    private static string[] _names = new string[] { "Airo", "Amad", "Fair", "Hayk", "Alf", "Argo", "Nemo", "Maxx" };

    public static Fish CreeteNewFish(/*Random random*/)
    {
        string name = _names[Utils.GenerateRandomNumber(0, _names.Length)];
        int age = Utils.GenerateRandomNumber(_minLife, _maxLife);
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

    static public string ReadString(string text = "")
    {
        Console.Write(text + " ");
        string tempString = Console.ReadLine().ToLower();
        Console.WriteLine();
        return tempString;
    }
}