# Задание 9 - Работа с XML / JSON

Создать приложение для синхронизации файлов в разных директориях (например, директория на жестком диске и на флеш-накопителе).

Реализовать функции (с одной и другой стороны):
* Файл	изменен;
* Файл удален;
* Файл	создан.

Интерфейс приложения создать на основе архитектурного шаблона MVP с использованием Windows Forms.

1. Реализовать ведение логов в виде XML. Проводить анализ необходимости синхронизации, учитывая предыдущие изменения, записанные в XML-логе.
2. Реализовать ведение логов в виде JSON. Проводить анализ необходимости синхронизации, учитывая предыдущие изменения, записанные в JSON-логе.
   
Материалы по работе с JSON изучить самостоятельно.

Через инструмент NuGet Package Manager (Tools > NuGet Package Manager > Package Manager Console) установить необходимые для работы программы пакеты, используя следующие команды:
`NuGet\Install-Package Serilog.Sinks.File -Version 5.0.0`
`NuGet\Install-Package Serilog.Formatting.Compact -Version 2.0.0`
