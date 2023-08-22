using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1;

public enum QualityType {
    Valid = 1,
    Defect = 2,
}
internal sealed class Program
{
    public static void Main(string[] args)
    {
        var client = new TcpClient();
        client.Connect("127.0.0.1", 8888);

        var stream = client.GetStream();
        byte[] data;

        while (true)
        {
            Console.WriteLine("1. Камера");
            Console.WriteLine("2. Толкатель");
            Console.WriteLine("3. Выход");
            Console.Write("Выберите действие: ");

            try
            {
                var choice = int.Parse(Console.ReadLine() ?? string.Empty);
            
                switch (choice)
                {
                    case 1:
                        QualityType quality;
                        do 
                        {
                            Console.Write("Годный (1) или брак (2)? ");
                            quality = (QualityType)int.Parse(Console.ReadLine()?? string.Empty);
                            if (quality != QualityType.Defect && quality != QualityType.Valid) 
                                Console.WriteLine("Пожалуйста введите корректное значение");
                        
                        } while (quality != QualityType.Defect && quality != QualityType.Valid);
                    
                        data = Encoding.ASCII.GetBytes($"CAMERA:{(int)quality}");
                        stream.Write(data, 0, data.Length);
                        break;
                    case 2:
                        data = Encoding.ASCII.GetBytes("PUSHER");
                        stream.Write(data, 0, data.Length);
                        break;
                    case 3:
                        stream.Close();
                        client.Close();
                        Console.WriteLine("Вы завершили работу программы");
                        return;
                    default: Console.WriteLine("Вы ввели число которое не было представлено в списке");
                        continue;

                }
            }
            catch (FormatException e)
            {
                Console.WriteLine("Введите корректное числовое значение");
            }
        }
    }
}
