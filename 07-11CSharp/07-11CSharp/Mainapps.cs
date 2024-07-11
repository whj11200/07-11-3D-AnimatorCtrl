
using System.Data;

namespace _07_11CSharp
{
    interface ILogger //인터페이스 선언만 가능 하다
    {                  // 함수의 정의는 불가능
        void writLog(string message);
        
    }
    class ConsoleLog : ILogger
    {  // 인터페이스 상속
        // 이렇게 인터페이스를 상속 하면 인터페이스 안에
        // 선언된 함수를 강제 구현 하게 만든다.
        
        public void writLog(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToLocalTime() }{message}");
        }
       
    }
    class FileLogger : ILogger
    {
        private StreamWriter _writer;
        public FileLogger(string path)
        {
            this._writer = File.CreateText(path);
            _writer.AutoFlush = true; // 버퍼 플러시
        }
        public void writLog(string message)
        {
            _writer.WriteLine($"{DateTime.Now.ToShortTimeString()},{message}");
        }
    }
    class ClimeateMonitor
    {
        // 생성자
        private ILogger _logger;
        // 
        public ClimeateMonitor(ILogger logger)
        {
            this._logger = logger;
        }
        public void start()
        {
            while (true)
            {
                Console.WriteLine("온도를 입력하시오");
                string temperature = Console.ReadLine();
                if (temperature == "") break;

                _logger.writLog("현재온도" + temperature);


            }
        }
    }
    // Tuple
    //인터페이스 와 추상 클래스

    // 부모 -> 추상클래스 == abstract // 부모클래스는 추성클래스를 만드는데 추상클래스는: 객체가 생성이안되는 클래스
    //  ||
    // 자식
    // 인터페이스로 부터 상속하는 방법을 이해
    // 추상클래스가 무엇인지 
    internal class Mainapps
    {
        // 튜플도 여러필드를 담을 수 있는 구조체 이다.
        // 앞서 했던 구조체와의 차이는 형식 이름이 없다.
        // 튜플은 응용 프로그램 전체에서 사용할 형식을 
        // 선언 할때가 아닌 즉석에서 사용 할 복합 데이터 형식을 선언 할때
        // 적합하다.
        static void Main(string[] args)
        {
            #region Tuple
            // 컨파일러가 튜플 모양을 보고 직접 형식을 결정하도록
            // var를 이용 해서 사용한다.
            //var Tuple = (123, 456);
            //Console.WriteLine($"{Tuple.Item1}:{Tuple.Item2}");
            //var Tuple2 = (Name: "박찬호", Age: 44);
            //Console.WriteLine($"Name :{Tuple2.Name} Age : {Tuple2.Age}");
            //// 튜플을 분해
            //var tuple3 = (Name: "양세찬", Age: "37");

            //var (name, age) = Tuple;
            //Console.WriteLine($"{name}{age}");
            //var tuple4 = (Name2: "박나래", Age: 43);
            //var (Name2, _) = tuple4;
            //Console.WriteLine($"{Name2}");
            //var (Name3, age2) = ("박문수", 44);
            //Console.WriteLine($"{Name3},{age2}");

            //var unnamed = ("슈퍼맨",9999);//string, int
            //var nameed = (Name: "유재석",Age: 56);
            //nameed = unnamed;
            //Console.WriteLine($"{nameed.Name}{nameed.Age}");
            #endregion
            ClimeateMonitor monitor = new ClimeateMonitor(new FileLogger("mylog.text"));
            monitor.start();

        }
    }
}
