using System;
using System.Collections.Generic;
using System.IO;

namespace Dzshka2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "placedLog.txt");

            Shelf shelfA = new Shelf(5, "A");
            Shelf shelfB = new Shelf(5, "B");
            Shelf[] shelfArray = { shelfA, shelfB };
            var shelves = new Dictionary<string, Shelf> { { "A", shelfA }, { "B", shelfB } };

            var placedJournal = new Journal<PlacedEvent>();
            var takenJournal = new Journal<TakenEvent>();
            var movedJournal = new Journal<MovedEvent>();
            var failedJournal = new Journal<FailedAttemptEvent>();

            if (File.Exists(path1))
            {
                foreach (string line in File.ReadAllLines(path1))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    try
                    {
                        var ev = PlacedEvent.FromLogLine(line);
                        if (shelves.ContainsKey(ev.ShelfName))
                        {
                            shelves[ev.ShelfName].Place(ev.Slot, ev.ItemName);
                            placedJournal.Add(ev);
                        }
                    }
                    catch { }
                }
            }

            while (true)
            {
                Console.WriteLine("\n--- СКЛАД ---");
                foreach (var s in shelfArray)
                {
                    Console.Write($"Полка {s.Name}: ");
                    for (int i = 1; i <= s.Size; i++)
                        Console.Write($"[{i}:{(s.GetItem(i) ?? "пусто")}] ");
                    Console.WriteLine();
                }

                Console.WriteLine("\n1 - Положить товар | 2 - Забрать товар | 3 - Перенести товар | 4 - Показать журналы | 5 - Выход");
                Console.Write("Ваш выбор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Имя товара: ");
                        string itemName = Console.ReadLine();
                        string shelfName = EnterShelf(shelves);
                        int slotNumber = EnterSlot();

                        if (shelves[shelfName].IsEmpty(slotNumber))
                        {
                            shelves[shelfName].Place(slotNumber, itemName);
                            placedJournal.Add(new PlacedEvent(itemName, slotNumber, shelfName));
                            Console.WriteLine("Товар успешно размещен.");
                        }
                        else
                        {
                            string errorReason = "слот занят";
                            failedJournal.Add(new FailedAttemptEvent($"Положить | полка {shelfName} слот {slotNumber} | причина: {errorReason}"));
                            Console.WriteLine($"Неудача | {errorReason}");
                        }
                        break;

                    case "2":
                        string takeShelfName = EnterShelf(shelves);
                        int takeSlotNumber = EnterSlot();

                        if (!shelves[takeShelfName].IsEmpty(takeSlotNumber))
                        {
                            string item = shelves[takeShelfName].Take(takeSlotNumber);
                            takenJournal.Add(new TakenEvent(item, takeSlotNumber, takeShelfName));
                            Console.WriteLine($"Изъятие | полка {takeShelfName} | слот {takeSlotNumber} | товар «{item}»");
                        }
                        else
                        {
                            string errorReason = "слот пуст";
                            failedJournal.Add(new FailedAttemptEvent($"Забрать | полка {takeShelfName} слот {takeSlotNumber} | причина: {errorReason}"));
                            Console.WriteLine($"Неудача | {errorReason}");
                        }
                        break;

                    case "3":
                        Console.WriteLine("--- ОТКУДА ---");
                        string fromShelf = EnterShelf(shelves);
                        int fromSlot = EnterSlot();

                        Console.WriteLine("--- КУДА ---");
                        string toShelf = EnterShelf(shelves);
                        int toSlot = EnterSlot();

                        if (!shelves[fromShelf].IsEmpty(fromSlot) && shelves[toShelf].IsEmpty(toSlot))
                        {
                            string movedItem = shelves[fromShelf].Take(fromSlot);
                            shelves[toShelf].Place(toSlot, movedItem);
                            movedJournal.Add(new MovedEvent(movedItem, fromShelf, fromSlot, toShelf, toSlot));
                            Console.WriteLine("Перенос выполнен.");
                        }
                        else
                        {
                            string errorReason = shelves[fromShelf].IsEmpty(fromSlot) ? "исходный слот пуст" : "целевой слот занят";
                            failedJournal.Add(new FailedAttemptEvent($"Перенос | с {fromShelf} на {toShelf} | причина: {errorReason}"));
                            Console.WriteLine($"Неудача | {errorReason}");
                        }
                        break;

                    case "4":
                        Console.WriteLine("\n--- Размещения ---");
                        foreach (var e in placedJournal.GetAll()) Console.WriteLine(e.ToScreenLine());
                        Console.WriteLine("\n--- Изъятия ---");
                        foreach (var e in takenJournal.GetAll()) Console.WriteLine(e.ToScreenLine());
                        Console.WriteLine("\n--- Переносы ---");
                        foreach (var e in movedJournal.GetAll()) Console.WriteLine(e.ToScreenLine());
                        Console.WriteLine("\n--- Неуспешные попытки ---");
                        foreach (var e in failedJournal.GetAll()) Console.WriteLine(e.ToScreenLine());
                        break;

                    case "5":
                        placedJournal.SaveToFile("placedLog.txt");
                        takenJournal.SaveToFile("takenLog.txt");
                        movedJournal.SaveToFile("movedLog.txt");
                        return;
                }
            }
        }

        static int EnterSlot()
        {
            while (true)
            {
                Console.Write("Введите слот (1-5): ");
                if (int.TryParse(Console.ReadLine(), out int slot) && slot >= 1 && slot <= 5)
                    return slot;
                Console.WriteLine("Ошибка! Введите число от 1 до 5.");
            }
        }

        static string EnterShelf(Dictionary<string, Shelf> shelves)
        {
            while (true)
            {
                Console.Write("Введите полку (A/B): ");
                string input = Console.ReadLine().ToUpper();
                if (shelves.ContainsKey(input))
                    return input;
                Console.WriteLine("Ошибка! Такой полки нет.");
            }
        }
    }
}