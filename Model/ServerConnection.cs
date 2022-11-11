using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using soufGame.Model.NetworkModel;

namespace soufGame.Model;

public class ServerConnection
{
    public string name;

    public Thread thread;

    public NetworkChatUser[] latestChatUsers;

    public TcpClient tcpClient;
    public NetworkStream stream;

    public ServerConnection(string name)
    {
        this.name = name;
        latestChatUsers = new NetworkChatUser[0];
    }

    public void Start()
    {
        thread = new(processServer);
        thread.Start();
    }

    private void processServer()
    {
        try
        {

            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer
            // connected to the same address as specified by the server, port
            // combination.
            int port = 19727;

            // Prefer using declaration to ensure the instance is Disposed later.
            tcpClient = new("127.0.0.1", port);


            // Get a client stream for reading and writing.
            stream = tcpClient.GetStream();


            byte[] msgBytes = Encoding.ASCII.GetBytes(name);

            stream.Write(msgBytes, 0, msgBytes.Length);
            Console.WriteLine($"Sent: {name}");

            byte[] data = new byte[512];
            int msgByteLength = 0;
            string responseData = string.Empty;

            while (responseData != ";STOP")
            {
                Console.WriteLine("Starting to read now..");

                try
                {
                    msgByteLength = stream.Read(data, 0, data.Length);
                }
                catch (IOException e)
                {
                    Console.WriteLine($"IOException: Failed reading from stream. {e}");
                    break;
                }

                responseData = Encoding.ASCII.GetString(data, 0, msgByteLength);
                Console.WriteLine($"Received: {responseData}");
                try
                {
                    lock (latestChatUsers)
                    {
                        NetworkTopRecord topRecord = JsonConvert.DeserializeObject<NetworkTopRecord>(responseData);
                        latestChatUsers = topRecord.topUsers;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }

            }

            // Explicit close is not necessary since TcpClient.Dispose() will be
            // called automatically.
            stream.Close();
            tcpClient.Close();

        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"ArgumentNullException: {e}");
        }
        catch (SocketException e)
        {
            Console.WriteLine($"SocketException: {e}");
        }
        catch (IOException e)
        {
            Console.WriteLine($"IOException: {e}");
        }
    }

    public void Close()
    {
        tcpClient.Dispose();
        stream.Dispose();
    }
}