using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Пример входных данных
        string[] startTimes = { "09:00", "11:30", "14:00" }; // Начало занятых промежутков
        int[] durations = { 30, 60, 30 }; // Длительность занятых промежутков в минутах
        int consultationTime = 30; // Минимальное необходимое время для работы менеджера в минутах
        string beginWorkingTime = "09:00"; 
        string endWorkingTime = "17:00"; 

        List<string> availableSlots = GetAvailableTimeSlots(startTimes, durations, consultationTime, beginWorkingTime, endWorkingTime);

        Console.WriteLine("Свободные временные интервалы:");
        foreach (var slot in availableSlots)
        {
            Console.WriteLine(slot);
        }
    }

    static List<string> GetAvailableTimeSlots(string[] startTimes, int[] durations, int consultationTime, string beginWorkingTime, string endWorkingTime)
    {
        // Преобразование начала и конца рабочего дня в минуты
        int workingStart = ConvertTimeToMinutes(beginWorkingTime);
        int workingEnd = ConvertTimeToMinutes(endWorkingTime);

        // Формируем список занятых временных интервалов в виде пар (начало, конец)
        List<(int Start, int End)> busyIntervals = new List<(int Start, int End)>();
        for (int i = 0; i < startTimes.Length; i++)
        {
            int start = ConvertTimeToMinutes(startTimes[i]);
            int end = start + durations[i];
            busyIntervals.Add((start, end));
        }

        // Сортируем занятые интервалы по времени начала
        busyIntervals.Sort((x, y) => x.Start.CompareTo(y.Start));

        List<string> availableSlots = new List<string>();
        int lastEndTime = workingStart;

        // Проверяем интервалы между занятыми временными промежутками
        foreach (var interval in busyIntervals)
        {
            if (interval.Start - lastEndTime >= consultationTime)
            {
                availableSlots.Add($"{ConvertMinutesToTime(lastEndTime)}-{ConvertMinutesToTime(interval.Start)}");
            }
            lastEndTime = Math.Max(lastEndTime, interval.End);
        }

        // Проверяем интервал после последнего занятого промежутка до конца рабочего дня
        if (workingEnd - lastEndTime >= consultationTime)
        {
            availableSlots.Add($"{ConvertMinutesToTime(lastEndTime)}-{ConvertMinutesToTime(workingEnd)}");
        }

        return availableSlots;
    }

    // Метод для преобразования строки времени в минуты
    static int ConvertTimeToMinutes(string time)
    {
        var parts = time.Split(':');
        return int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
    }

    // Метод для преобразования минут в строку времени формата HH:mm
    static string ConvertMinutesToTime(int minutes)
    {
        int hours = minutes / 60;
        int mins = minutes % 60;
        return $"{hours:D2}:{mins:D2}";
    }
}
