using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Custom Exception for invalid login attempts
public class InvalidLoginException : Exception
{
    public InvalidLoginException() : base("Invalid username or password") { }
    public InvalidLoginException(string message) : base(message) { }
}

// Interface for login functionality
public interface IAccount
{
    bool Login(string username, string password);
}

// Interface for animal management
public interface IManagement
{
    void AddAnimal(Animal animal);
    bool RemoveAnimal(int animalId);
    List<Animal> ListAnimals();
    void UpdateAnimal(int animalId, Animal updatedAnimal);
}

// Abstract Person class
public abstract class Person : IAccount
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string ID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public abstract bool Login(string username, string password);
}

// Animal class
public class Animal
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Species { get; set; }

    public override string ToString()
    {
        return $"ID: {ID}, Name: {Name}, Age: {Age}, Species: {Species}";
    }
}

// Admin class with management capabilities
public class Admin : Person, IManagement
{
    private List<Animal> animals;

    public Admin()
    {
        animals = new List<Animal>();
        LoadAnimals();
    }

    public override bool Login(string username, string password)
    {
        return username == Username && password == Password;
    }

    public void AddAnimal(Animal animal)
    {
        if (string.IsNullOrWhiteSpace(animal.Name))
            throw new ArgumentException("Animal name cannot be empty");
        if (animal.Age <= 0)
            throw new ArgumentException("Age must be positive");
        if (string.IsNullOrWhiteSpace(animal.Species))
            throw new ArgumentException("Species cannot be empty");

        animals.Add(animal);
        SaveAnimals();
    }

    public bool RemoveAnimal(int animalId)
    {
        var animal = animals.FirstOrDefault(a => a.ID == animalId);
        if (animal == null)
            return false;

        animals.Remove(animal);
        SaveAnimals();
        return true;
    }

    public List<Animal> ListAnimals()
    {
        return animals;
    }

    public void UpdateAnimal(int animalId, Animal updatedAnimal)
    {
        var existingAnimal = animals.FirstOrDefault(a => a.ID == animalId);
        if (existingAnimal == null)
            throw new ArgumentException("Animal not found");

        if (string.IsNullOrWhiteSpace(updatedAnimal.Name))
            throw new ArgumentException("Animal name cannot be empty");
        if (updatedAnimal.Age <= 0)
            throw new ArgumentException("Age must be positive");
        if (string.IsNullOrWhiteSpace(updatedAnimal.Species))
            throw new ArgumentException("Species cannot be empty");

        existingAnimal.Name = updatedAnimal.Name;
        existingAnimal.Age = updatedAnimal.Age;
        existingAnimal.Species = updatedAnimal.Species;
        SaveAnimals();
    }

    private void LoadAnimals()
    {
        if (File.Exists("animals.txt"))
        {
            var lines = File.ReadAllLines("animals.txt");
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 4 && int.TryParse(parts[0], out int id) && int.TryParse(parts[2], out int age))
                {
                    animals.Add(new Animal
                    {
                        ID = id,
                        Name = parts[1],
                        Age = age,
                        Species = parts[3]
                    });
                }
            }
        }
    }

    private void SaveAnimals()
    {
        var lines = animals.Select(a => $"{a.ID},{a.Name},{a.Age},{a.Species}");
        File.WriteAllLines("animals.txt", lines);
    }
}

// Farmer class with view-only capabilities
public class Farmer : Person
{
    private List<Animal> animals;

    public Farmer()
    {
        animals = new List<Animal>();
        LoadAnimals();
    }

    public override bool Login(string username, string password)
    {
        return username == Username && password == Password;
    }

    public List<Animal> ViewAnimals()
    {
        return animals;
    }

    private void LoadAnimals()
    {
        if (File.Exists("animals.txt"))
        {
            var lines = File.ReadAllLines("animals.txt");
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 4 && int.TryParse(parts[0], out int id) && int.TryParse(parts[2], out int age))
                {
                    animals.Add(new Animal
                    {
                        ID = id,
                        Name = parts[1],
                        Age = age,
                        Species = parts[3]
                    });
                }
            }
        }
    }
}

// Main program class
public class FarmManagementSystem
{
    private static Dictionary<string, Person> users = new Dictionary<string, Person>
    {
        { "admin", new Admin { Username = "admin", Password = "admin123", Name = "Admin", Age = 30, ID = "A001" } },
        { "farmer1", new Farmer { Username = "farmer1", Password = "farmer123", Name = "John Farmer", Age = 45, ID = "F001" } }
    };

    static void Main(string[] args)
    {
        Console.WriteLine("Farm Management System");
        Console.WriteLine("----------------------");

        try
        {
            Person currentUser = Login();
            if (currentUser is Admin admin)
            {
                AdminMenu(admin);
            }
            else if (currentUser is Farmer farmer)
            {
                FarmerMenu(farmer);
            }
        }
        catch (InvalidLoginException ex)
        {
            Console.WriteLine($"Login failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static Person Login()
    {
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();

        if (users.TryGetValue(username, out Person user) && user.Login(username, password))
        {
            Console.WriteLine($"Welcome, {user.Name}!");
            return user;
        }

        throw new InvalidLoginException();
    }

    private static void AdminMenu(Admin admin)
    {
        while (true)
        {
            Console.WriteLine("\nAdmin Menu");
            Console.WriteLine("1. Add Animal");
            Console.WriteLine("2. Remove Animal");
            Console.WriteLine("3. View All Animals");
            Console.WriteLine("4. Update Animal");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1:
                        AddAnimal(admin);
                        break;
                    case 2:
                        RemoveAnimal(admin);
                        break;
                    case 3:
                        ViewAnimals(admin.ListAnimals());
                        break;
                    case 4:
                        UpdateAnimal(admin);
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static void FarmerMenu(Farmer farmer)
    {
        while (true)
        {
            Console.WriteLine("\nFarmer Menu");
            Console.WriteLine("1. View All Animals");
            Console.WriteLine("2. Exit");
            Console.Write("Select an option: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1:
                        ViewAnimals(((Farmer)farmer).ViewAnimals());
                        break;
                    case 2:
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static void AddAnimal(Admin admin)
    {
        Console.WriteLine("\nAdd New Animal");
        Console.Write("ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            throw new ArgumentException("Invalid ID. Must be a number.");
        }

        Console.Write("Name: ");
        string name = Console.ReadLine();

        Console.Write("Age: ");
        if (!int.TryParse(Console.ReadLine(), out int age))
        {
            throw new ArgumentException("Invalid age. Must be a number.");
        }

        Console.Write("Species: ");
        string species = Console.ReadLine();

        Animal animal = new Animal { ID = id, Name = name, Age = age, Species = species };
        admin.AddAnimal(animal);
        Console.WriteLine("Animal added successfully!");
    }

    private static void RemoveAnimal(Admin admin)
    {
        Console.WriteLine("\nRemove Animal");
        Console.Write("Enter Animal ID to remove: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            throw new ArgumentException("Invalid ID. Must be a number.");
        }

        if (admin.RemoveAnimal(id))
        {
            Console.WriteLine("Animal removed successfully!");
        }
        else
        {
            Console.WriteLine("Animal not found.");
        }
    }

    private static void UpdateAnimal(Admin admin)
    {
        Console.WriteLine("\nUpdate Animal");
        Console.Write("Enter Animal ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            throw new ArgumentException("Invalid ID. Must be a number.");
        }

        var animal = admin.ListAnimals().FirstOrDefault(a => a.ID == id);
        if (animal == null)
        {
            Console.WriteLine("Animal not found.");
            return;
        }

        Console.WriteLine($"Current details: {animal}");
        Console.Write("New Name (leave blank to keep current): ");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            name = animal.Name;
        }

        Console.Write("New Age (leave blank to keep current): ");
        string ageInput = Console.ReadLine();
        int age = string.IsNullOrWhiteSpace(ageInput) ? animal.Age : int.Parse(ageInput);

        Console.Write("New Species (leave blank to keep current): ");
        string species = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(species))
        {
            species = animal.Species;
        }

        Animal updatedAnimal = new Animal { ID = id, Name = name, Age = age, Species = species };
        admin.UpdateAnimal(id, updatedAnimal);
        Console.WriteLine("Animal updated successfully!");
    }

    private static void ViewAnimals(List<Animal> animals)
    {
        Console.WriteLine("\nAnimal List");
        Console.WriteLine("-----------");
        if (animals.Count == 0)
        {
            Console.WriteLine("No animals found.");
            return;
        }

        foreach (var animal in animals)
        {
            Console.WriteLine(animal);
        }
    }
}