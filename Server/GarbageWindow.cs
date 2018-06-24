using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            ShowIcon = false;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            stopWatch.Start();
            Console.Title = "TheArcaneChat Server -=- v1.0.0";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            ServerSocket.Start();
            Console.WriteLine(" >> Server Started");
            Visible = false;
            var timer = new Timer(s =>
            {
                if (InvokeQueue.Count < 1) return;
                try
                {
                    // InvokeQueue[0].Invoke();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

                InvokeQueue.RemoveAt(0);
            }, null, 0, 100);
            new Thread(() =>
            {
                //serverSocket.Server.Listen(1000);
                while (true)
                {
                    try
                    {
                        while (!ServerSocket.Pending()) Thread.Sleep(10);
                        ClientSocket = ServerSocket.AcceptTcpClient();
                        Clients.Add(ClientSocket);
                        Console.WriteLine($" >> Client No: {_counter} started!");
                        _counter += 1;
                        new HandleClient().StartClient(ClientSocket, Convert.ToString(_counter));
                        new Task(()=>
                        {
                            new SoundPlayer(@"C:\Windows\Media\Windows Proximity Notification.wav").Play();
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

            }).Start();
        }

        public TcpListener ServerSocket = new TcpListener(IPAddress.Any, 8888);
        public TcpClient ClientSocket = default;
        private int _counter;
        public static void BroadcastMessage(string Message)
        {
            try
            {
                var sendBytes = Encoding.Unicode.GetBytes(Message.Replace("\n", "\0MSGEND\0") + "\0MSGEND\0");
                var tmpCli = Clients;
                foreach (var client in tmpCli)
                {
                    var msgSend = new Task(() =>
                    {
                        var cl = client;
                        try
                        {

                            var networkStream = cl.GetStream();
                            networkStream.Write(sendBytes, 0, sendBytes.Length);
                        }
                        catch
                        {
                            Clients.Remove(cl);

                            BroadcastMessage($"A client has logged off! (MessageTransmitFailedException!)");
                        }
                    });
                    msgSend.Start();
                    //msgSend.Wait(500);
                }
                Console.WriteLine(" >> " + Message);

            }
            catch
            {
                // ignored
            }

            InvokeQueue.Add(() => ConnectionCount.Text = $"Connected clients: {Clients.Count}");
        }
        public static void WhisperMessage(TcpClient Client, string Message)
        {
            try
            {
                var sendBytes = Encoding.Unicode.GetBytes("*SYS*: "+Message.Replace("\n", "\0MSGEND\0") + "\0MSGEND\0");
                try
                        {

                            var networkStream = Client.GetStream();
                            networkStream.Write(sendBytes, 0, sendBytes.Length);
                        }
                        catch
                        {
                            //Clients.Remove(Client);
                            Console.WriteLine("WTF? Whisper failed!");
                            // GarbageWindow.BroadcastMessage($"A client has logged off! (MessageTransmitFailedException!)");
                        }

                Console.WriteLine(" >> " + Message);
            }
            catch
            {
                // ignored
            }
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }
    }
    //Class to handle each client request separatly
    public class HandleClient
    {
        TcpClient _clientSocket;
        string _clNo;
        public void StartClient(TcpClient inClientSocket, string cliNo)
        {
            _clientSocket = inClientSocket;
            _clNo = cliNo;
            var ctThread = new Thread(DoChat);
            ctThread.Start();
        }
        private void DoChat()
        {
            var joinTime = DateTime.Now;
            var requestCount = 0;
            requestCount = 0;
            var error = 0;
            var username = $"User_{new Random().Next(0,1000)}";
            GarbageWindow.BroadcastMessage($"Welcome client #{_clNo}");
            Console.WriteLine($"Client #{_clNo} connected, IP: {_clientSocket.Client.RemoteEndPoint}");
            while (error < 1)
            {
                Thread.Sleep(100);
                try
                {
                    requestCount = requestCount + 1;
                    var networkStream = _clientSocket.GetStream();
                    var bytesFrom = new byte[_clientSocket.ReceiveBufferSize];
                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                    string dataFromClient = Encoding.Unicode.GetString(bytesFrom).TrimEnd('\0');
                    Console.WriteLine($" >> From client #{_clNo}: {dataFromClient}");
                    if (dataFromClient.StartsWith("\0CLIMSG\0"))
                    {
                        switch (dataFromClient.Replace("\0CLIMSG\0", ""))
                        {
                            case "exit":
                                networkStream.Close();
                                GarbageWindow.Clients.Remove(_clientSocket);

                                GarbageWindow.BroadcastMessage($"Client { _clNo } logged off!");
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
                                GarbageWindow.Clients.Remove(_clientSocket);
                                GarbageWindow.BroadcastMessage($"Client { _clNo } logged off!");
                                break;
                            case "nick":
                                username = dataFromClient.Split(" ".ToCharArray(), 2)[1];
                                GarbageWindow.WhisperMessage(_clientSocket, "Your nickname has been changed to " + username);
                                break;
                            case "serverinfo":
                                    GarbageWindow.BroadcastMessage($"Server memory usage: {GC.GetTotalMemory(false)} bytes\nConnection count: {GarbageWindow.Clients.Count}\nUptime: {GarbageWindow.stopWatch.Elapsed.ToString()}\nHost machine name: {Environment.MachineName}");
                                break;
                            case "userinfo":
                                GarbageWindow.BroadcastMessage($"Client number: {_clNo}\nJoin time: {joinTime.ToLongTimeString()}\nConnection time: {DateTime.Now.Subtract(joinTime)}");
                                break;

                            case "clilist":
                                
                                foreach (var cli in GarbageWindow.Clients)
                                {
                                    GarbageWindow.BroadcastMessage($"Client #{_clNo}: {username} ");
                                }
                                break;
                            case "default":

                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        GarbageWindow.BroadcastMessage($"{_clNo} - {username}: {dataFromClient}");
                    }


                }
                catch (Exception ex)
                {
                    GarbageWindow.BroadcastMessage("Exception occurred with a client: " + ex.StackTrace);
                    _clientSocket.Close();

                    error++;
                }
            }

        }


    }
}
