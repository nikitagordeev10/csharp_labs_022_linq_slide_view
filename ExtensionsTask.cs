using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask {
    public static double Median(this IEnumerable<double> items) { // метод для нахождения медианы последовательности чисел

        var sorted = items.OrderBy(x => x).ToList(); // сортировать последовательность по возрастанию
        var elementСount = sorted.Count; // длина последовательности

        if (elementСount == 0) { // последовательность пуста
            throw new InvalidOperationException("Последовательность не содержит элементов"); // исключение
        }

        var mid = elementСount / 2; // среднее значение
        var median = elementСount % 2 != 0 ? sorted[mid] : (sorted[mid - 1] + sorted[mid]) / 2; // серединный элемент списка
        return median;
    }

    public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items) { // получения пар элементов из последовательности
        var prev = default(T); // значение по умолчанию для типа T
        var first = true;

        foreach (var curr in items) {
            if (!first) { // не первый элемент
                yield return (prev, curr); // сформировать пару из предыдущего и текущего элементов
            }
            prev = curr; // использовать в следующей паре
            first = false; // не возвращать пары с первым элементом
        }
    }
}