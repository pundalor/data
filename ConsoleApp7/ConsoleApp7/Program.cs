using System;

class Program
{
    static void Main()
    {
        // Пример входных данных
        string[] startTimes = { "10:00", "11:00", "15:00", "16:40" }; // Начало занятых промежутков
        int[] durations = { 60, 30, 10, 50 }; // Длительность занятых промежутков в минутах
        int consultationTime = 30; // Минимальное необходимое время для работы менеджера в минутах
        string beginWorkingTime = "08:00"; // Начало рабочего дня
        string endWorkingTime = "18:00"; // Конец рабочего дня

        string[] availableSlots = GetAvailableTimeSlots(startTimes, durations, consultationTime, beginWorkingTime, endWorkingTime);

        // Вывод результатов
        Console.WriteLine("Свободные временные интервалы:");
        foreach (var slot in availableSlots)
        {
            if (slot != null)
                Console.WriteLine(slot);
        }
    }

    static string[] GetAvailableTimeSlots(string[] startTimes, int[] durations, int consultationTime, string beginWorkingTime, string endWorkingTime)
    {
        int workingStart = ConvertTimeToMinutes(beginWorkingTime);
        int workingEnd = ConvertTimeToMinutes(endWorkingTime);

        // Создаем одномерный массив для начала и конца каждого интервала
        int n = startTimes.Length;
        int[][] busyIntervals = new int[n][];
        for (int i = 0; i < n; i++)
        {
            int start = ConvertTimeToMinutes(startTimes[i]);
            int end = start + durations[i];
            busyIntervals[i] = new int[] { start, end };
        }

        // Сортируем массив занятых интервалов по началу
        Array.Sort(busyIntervals, (a, b) => a[0].CompareTo(b[0]));

        // Создаем массив для хранения возможных свободных интервалов с шагом 30 минут
        string[] availableSlots = new string[100]; // массив большого размера для хранения всех возможных интервалов
        int slotIndex = 0;
        int lastEndTime = workingStart;

        // Проверяем интервалы между занятыми промежутками
        for (int i = 0; i < n; i++)
        {
            int intervalStart = busyIntervals[i][0];

            // Находим все свободные 30-минутные интервалы
            while (lastEndTime + 30 <= intervalStart)
            {
                availableSlots[slotIndex++] = $"{ConvertMinutesToTime(lastEndTime)}-{ConvertMinutesToTime(lastEndTime + 30)}";
                lastEndTime += 30;
            }

            lastEndTime = Math.Max(lastEndTime, busyIntervals[i][1]);
        }

        // Проверяем интервал между концом последнего занятого времени и концом рабочего дня
        while (lastEndTime + 30 <= workingEnd)
        {
            availableSlots[slotIndex++] = $"{ConvertMinutesToTime(lastEndTime)}-{ConvertMinutesToTime(lastEndTime + 30)}";
            lastEndTime += 30;
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
