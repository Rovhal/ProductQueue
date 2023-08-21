using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1;

public enum QualityType {
    UnexpectedValue = 0,
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

            var choice = int.Parse(Console.ReadLine() ?? string.Empty);
            
            switch (choice)
            {
                case 1:
                    QualityType quality;
                    do 
                    {
                        Console.Write("Годный (1) или брак (2)? ");
                        quality = (QualityType)int.Parse(Console.ReadLine()?? string.Empty);
                        if (quality == QualityType.UnexpectedValue) Console.WriteLine("Пожалуйста введите корректное значение");
                        
                    } while (quality == QualityType.UnexpectedValue);
                    
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
                    break;
                defalt: throw new Exception("Не было выбрано ни одно действие");

            }
        }
    }
}
