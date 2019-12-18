using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskLesson
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine($"Раб под номером: {Thread.CurrentThread.ManagedThreadId}");
            //MakeLonkWork().Start();
            //Console.WriteLine("Главный раб закончил работу");
            //Console.ReadKey();

            // 1-способ создания задачи(Task)  (!НЕ ЗАПУСКА!)
            //var task = new Task<int>(() =>
            //{
            //    Thread.Sleep(3000); 
            //    return 123;
            //});
            //var actionTask = new Task(() => Console.WriteLine());

            // Запуск задачи
            //task.Start();

            //// Получение результата
            //while (!task.IsCompleted)
            //{
            //    Console.WriteLine("Раб работает...");
            //    Thread.Sleep(300);
            //}

            //var res = task.Result;

            // 2-способ (тонкая настройка задачи)
            //Task.Factory.StartNew(() => Console.WriteLine("123123"), TaskCreationOptions.LongRunning);

            // 3-способ (запуск CPU-bound операции - всегда)
            //Task.Run(() => 1);

            // Отмена операции в процессе выполнения.
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var task = Task.Run<string>(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("Первая порция");
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Отмена");
                    return string.Empty;
                }

                Thread.Sleep(5000);
                Console.WriteLine("Вторая порция");
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Отмена");
                    return string.Empty;
                }

                Thread.Sleep(5000);
                Console.WriteLine("Жри.");
                return "Ответ";
            }, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            cancellationTokenSource.CancelAfter(5000);
            Console.WriteLine(task.Result);
            Console.ReadKey();
        }

        static private Task LongOperation()
        {
            if (false)
            {
                return Task.Run(() => Thread.Sleep(20000));
            }
            int x = 15 + 10;
            Console.WriteLine(x);
            return Task.CompletedTask;
            // если нужен результат, то return Task.FromResult(результат);
        }

        //static private Task MakeLonkWork()
        //{
        //    return new Task(() => Console.WriteLine($"Раб под номером: {Thread.CurrentThread.ManagedThreadId}"));
        //}
    }
}
