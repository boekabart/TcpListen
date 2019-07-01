using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
namespace Listen
{
    class Program
    {
        static void Main(string[] args)
        {
			if (args.Length<1) return;
			List<Socket> sockets = new List<Socket>();
			foreach( var p in args)
			{
				var port = (short)int.Parse(p);
				sockets.Add(Listen(port));
				Console.WriteLine($"Listening on port {port}");
			}
				Console.WriteLine($"Press the <any> key to exit");
				Console.ReadKey();
				foreach (var s in sockets)
				s.Close();
        }
		
		static Socket Listen(short port)
		{
   // create the socket
       Socket listenSocket = new Socket(AddressFamily.InterNetwork, 
                                        SocketType.Stream,
                                        ProtocolType.Tcp);

       // bind the listening socket to the port
IPEndPoint ep = new IPEndPoint(0, port);

listenSocket.Bind(ep);

// start listening
listenSocket.Listen(1);
Receive(listenSocket);
return listenSocket;
		}
		
		static async void Receive(Socket listenSocket)
		{
			try{
Console.WriteLine("Accepting");
var sea = new SocketAsyncEventArgs();
var tcs = new TaskCompletionSource<bool>();
sea.Completed += (_,__) => tcs.SetResult(true);
listenSocket.AcceptAsync(sea);
await tcs.Task;
Console.WriteLine("Accepted!");
Receive(listenSocket);
var s2 = sea.AcceptSocket;
Console.WriteLine("Sending");
s2.Send(Encoding.UTF8.GetBytes("Hi!"));
Console.WriteLine("Sent");
s2.Close();
			} catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		
	}
}

