using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Timer = System.Threading.Timer;

namespace Server
{
    public partial class GarbageWindow : Form
    {
        public static List<MethodInvoker> InvokeQueue = new List<MethodInvoker>();
        public static List<TcpClient> Clients = new List<TcpClient>();
        public static Stopwatch stopWatch = new Stopwatch();
        public GarbageWindow()
        {
            InitializeComponent();
        }

        public void Invoke(MethodInvoker inv) => inv.Invoke();
        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            stopWatch.Start();
            Console.Title = "TheArcaneChat Server -=- v1.0.0";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            serverSocket.Start();
            Console.WriteLine(" >> Server Started");
            Visible = false;
            new Timer(s =>
            {
                if (InvokeQueue.Count >= 1)
                {
                    try
                    {
                       // InvokeQueue[0].Invoke();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }

                    InvokeQueue.RemoveAt(0);
                }
            }, null, 0, 100);
            Thread inHandlerThread = new Thread(() =>
            {
                //serverSocket.Server.Listen(1000);
                while (true)
                {
                    try
                    {
                        while (!serverSocket.Pending()) Thread.Sleep(10);
                        clientSocket = serverSocket.AcceptTcpClient();
                        Clients.Add(clientSocket);
                        Console.WriteLine($" >> Client No: {counter} started!");
                        counter += 1;
                        new handleClient().startClient(clientSocket, Convert.ToString(counter));
                        new Task(()=>
                        {
                            new System.Media.SoundPlayer(@"C:\Windows\Media\Windows Proximity Notification.wav").Play();
                            //Console.Beep(1000, 50);
                            //Console.Beep();
                            //Console.Beep(1500, 50);
                        }).Start();

                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }

            });
            inHandlerThread.Start();
        }
        TcpListener serverSocket = new TcpListener(IPAddress.Any, 8888);
        TcpClient clientSocket = default(TcpClient);
        int counter = 0;
        public static void BroadcastMessage(string Message)
        {
            try
            {
                Byte[] sendBytes = Encoding.Unicode.GetBytes(Message.Replace("\n", "\0MSGEND\0") + "\0MSGEND\0");
                var tmpCli = Clients;
                foreach (var Client in tmpCli)
                {
                    Task MsgSend = new Task(() =>
                    {
                        var Cl = Client;
                        try
                        {

                            NetworkStream networkStream = Cl.GetStream();
                            networkStream.Write(sendBytes, 0, sendBytes.Length);
                        }
                        catch (Exception e)
                        {
                            Clients.Remove(Cl);

                            GarbageWindow.BroadcastMessage($"A client has logged off! (MessageTransmitFailedException!)");
                        }
                    });
                    MsgSend.Start();
                    MsgSend.Wait(500);
                }
                Console.WriteLine(" >> " + Message);

            }
            catch (Exception e)
            {

            }

            InvokeQueue.Add(() => ConnectionCount.Text = $"Connected clients: {Clients.Count}");
        }
        public static void WhisperMessage(TcpClient Client, string Message)
        {
            try
            {
                Byte[] sendBytes = Encoding.Unicode.GetBytes("*SYS*: "+Message.Replace("\n", "\0MSGEND\0") + "\0MSGEND\0");
                try
                        {

                            NetworkStream networkStream = Client.GetStream();
                            networkStream.Write(sendBytes, 0, sendBytes.Length);
                        }
                        catch (Exception e)
                        {
                            //Clients.Remove(Client);
                            Console.WriteLine("WTF? Whisper failed!");
                           // GarbageWindow.BroadcastMessage($"A client has logged off! (MessageTransmitFailedException!)");
                        }

                Console.WriteLine(" >> " + Message);
            }
            catch (Exception e)
            {

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    //Class to handle each client request separatly
    public class handleClient
    {
        TcpClient clientSocket;
        string clNo;
        public void startClient(TcpClient inClientSocket, string cliNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = cliNo;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }
        private void doChat()
        {
            DateTime joinTime = DateTime.Now;
            int requestCount = 0;
            byte[] bytesFrom;
            string dataFromClient = null;
            string serverResponse = null;
            requestCount = 0;
            int error = 0;
            string username = $"User_{new Random().Next(0,1000)}";
            GarbageWindow.BroadcastMessage($"Welcome client #{clNo}");
            Console.WriteLine($"Client #{clNo} connected, IP: {clientSocket.Client.RemoteEndPoint}");
            while (error < 1)
            {
                Thread.Sleep(100);
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    bytesFrom = new byte[clientSocket.ReceiveBufferSize];
                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                    dataFromClient = System.Text.Encoding.Unicode.GetString(bytesFrom).TrimEnd('\0');
                    Console.WriteLine($" >> From client #{clNo}: {dataFromClient}");
                    if (dataFromClient.StartsWith("\0CLIMSG\0"))
                    {
                        switch (dataFromClient.Replace("\0CLIMSG\0", ""))
                        {
                            case "exit":
                                networkStream.Close();
                                GarbageWindow.Clients.Remove(clientSocket);

                                GarbageWindow.BroadcastMessage($"Client { clNo } logged off!");
                                break;
                        }

                    }
                    else if (dataFromClient.StartsWith("/"))
                    {
                        switch ((dataFromClient.Remove(0,1) + " ").Split(' ')[0].ToLower().Trim())
                        {
                            case "cli":
                                GarbageWindow.BroadcastMessage("*working*\n");
                                GarbageWindow.BroadcastMessage($"SERVER BROADCAST: --CLIENTS CONNECTED: {GarbageWindow.Clients.Count}--");
                                break;
                            case "exit":
                                networkStream.Close();
                                GarbageWindow.Clients.Remove(clientSocket);
                                GarbageWindow.BroadcastMessage($"Client { clNo } logged off!");
                                break;
                            case "nick":
                                username = dataFromClient.Split(" ".ToCharArray(), 2)[1];
                                GarbageWindow.WhisperMessage(clientSocket, "Your nickname has been changed to " + username);
                                break;
                            case "serverinfo":
                                    GarbageWindow.BroadcastMessage($"Server memory usage: {GC.GetTotalMemory(false)} bytes\nConnection count: {GarbageWindow.Clients.Count}\nUptime: {GarbageWindow.stopWatch.Elapsed.ToString()}\nHost machine name: {Environment.MachineName}");
                                break;
                            case "userinfo":
                                GarbageWindow.BroadcastMessage($"Client number: {clNo}\nJoin time: {joinTime.ToLongTimeString()}\nConnection time: {DateTime.Now.Subtract(joinTime)}");
                                break;

                            case "default":

                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        GarbageWindow.BroadcastMessage($"{clNo} - {username}: {dataFromClient}");
                    }


                }
                catch (Exception ex)
                {
                    GarbageWindow.BroadcastMessage("Exception occurred with a client: " + ex.StackTrace);
                    this.clientSocket.Close();

                    error++;
                }
            }

        }


    }
}
