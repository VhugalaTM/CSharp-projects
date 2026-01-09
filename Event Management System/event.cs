using System;
using System.Collections.Generic;

namespace ConferenceEventManagement
{
    // Base class for all events
    public abstract class Event
    {
        public string EventName { get; set; }
        public string EventID { get; set; }
        private int _capacity;

        public int Capacity
        {
            get { return _capacity; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Capacity cannot be negative.");
                }
                _capacity = value;
            }
        }

        // Constructor
        protected Event(string eventName, string eventID, int capacity)
        {
            EventName = eventName;
            EventID = eventID;
            Capacity = capacity;
        }

        // Method overloading - Display with different detail levels
        public virtual void Display()
        {
            Console.WriteLine("Event: "+EventName+" ID: "+EventID+ " Capacity: "+ Capacity);
        }

        public virtual void Display(bool showDetails)
        {
            if (showDetails)
            Display();
        }
    }

    // Derived class for Workshop
    public class Workshop : Event
    {
        public string Topic { get; set; }
        public string Company { get; set; }

        public Workshop(string eventName, string eventID, int capacity, string topic, string company)
            : base(eventName, eventID, capacity)
        {
            Topic = topic;
            Company = company;
        }

        public override void Display(bool showDetails)
        {
            base.Display();
            if (showDetails)
            {
                Console.WriteLine("Type: Workshop, Topic: "+Topic+", Host Company: "+ Company);
            }
        }
    }

    // Derived class for Seminar
    public class Seminar : Event
    {
        public string Speaker { get; set; }

        public Seminar(string eventName, string eventID, int capacity, string speaker)
            : base(eventName, eventID, capacity)
        {
            Speaker = speaker;
        }

        public override void Display(bool showDetails)
        {
            base.Display();
            if (showDetails)
            {
                Console.WriteLine("Type: Seminar, Speaker: "+ Speaker);
            }
        }
    }

    class Program
    {
        static List<Event> events = new List<Event>();

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nConference Event Management System");
                Console.WriteLine("1. Add a Workshop");
                Console.WriteLine("2. Add a Seminar");
                Console.WriteLine("3. View all events");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");

                try
                {
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            AddWorkshop();
                            break;
                        case 2:
                            AddSeminar();
                            break;
                        case 3:
                            ViewAllEvents();
                            break;
                        case 4:
                            running = false;
                            Console.WriteLine("Exiting the program. Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: "+ ex.Message);
                }
            }
        }

        static void AddWorkshop()
        {
            try
            {
                Console.Write("Enter Workshop Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Event ID: ");
                string id = Console.ReadLine();

                Console.Write("Enter Capacity: ");
                int capacity = int.Parse(Console.ReadLine());

                Console.Write("Enter Topic: ");
                string topic = Console.ReadLine();

                Console.Write("Enter Host Company: ");
                string company = Console.ReadLine();

                Workshop workshop = new Workshop(name, id, capacity, topic, company);
                events.Add(workshop);

                Console.WriteLine("Workshop added successfully!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: "+ ex.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input for capacity. Please enter a number.");
            }
        }

        static void AddSeminar()
        {
            try
            {
                Console.Write("Enter Seminar Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Event ID: ");
                string id = Console.ReadLine();

                Console.Write("Enter Capacity: ");
                int capacity = int.Parse(Console.ReadLine());

                Console.Write("Enter Speaker Name: ");
                string speaker = Console.ReadLine();

                Seminar seminar = new Seminar(name, id, capacity, speaker);
                events.Add(seminar);

                Console.WriteLine("Seminar added successfully!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: "+ ex.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input for capacity. Please enter a number.");
            }
        }

        static void ViewAllEvents()
        {
            if (events.Count == 0)
            {
                Console.WriteLine("No events available.");
                return;
            }

            Console.WriteLine("\nAll Events:");
            Console.WriteLine("-----------");
            for (int i = 0; i < events.Count; i++)
            {
                Console.Write(i + 1+"." );
                events[i].Display(true);
                Console.WriteLine();
            }
        }
    }
}