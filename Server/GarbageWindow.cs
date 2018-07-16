using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Threading.Timer;


namespace Server
{
    public partial class GarbageWindow : Form
    {
        public static List<TcpClient> Clients = new List<TcpClient>();
        public static Stopwatch StopWatch = new Stopwatch();
        public GarbageWindow()
        {
            InitializeComponent();
        }
        private void GarbageWindow_Paint(object sender, PaintEventArgs e)
        {
            Text = $"Clients: {Clients.Count} | {StopWatch.Elapsed.ToString()}";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //   Hide();
            // ShowIcon = false;
            // ShowInTaskbar = false;
            // FormBorderStyle = FormBorderStyle.None;
            StopWatch.Start();
            Console.Title = "TheArcaneChat Server -=- v1.0.0";

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            ServerSocket.Start();
            Log(" >> Server Started");
            //Visible = false;
            new Thread(() =>
            {
                //serverSocket.Server.Listen(1000);
                while (true)
                {
                    try
                    {

                        while (!ServerSocket.Pending()) Thread.Sleep(5);

                        ClientSocket = ServerSocket.AcceptTcpClient();
                        //Log($" >> Client No: {_counter++} started!");
                        Clients.Add(ClientSocket);
                        new HandleClient().StartClient(ClientSocket, _counter);
                        _counter++;
                        new Task(() =>
                        {
                            new SoundPlayer(@"C:\Windows\Media\Speech On.wav").Play();
                        }).Start();
                        this.Invoke(new MethodInvoker(()=> { ConnectionCount.Text = $"Connected clients: {Clients.Count}"; }));

                    }
                    catch (Exception exception)
                    {
                        //Log(exception);
                    }
                }

            }).Start();
        }


        public TcpListener ServerSocket = new TcpListener(IPAddress.Any, 8888);
        public TcpClient ClientSocket;
        private int _counter;
        public static void BroadcastMessage(string message)
        {
            try
            {
                /* var msgSend = new Task(() =>
                 {*/
                var sendBytes = Encoding.Unicode.GetBytes("\0SRVMSG\0ping".Replace("\n", "\0MSGEND\0") + "\0MSGEND\0");
                var tmpCli = Clients;

                var oldNumcli = Clients.Count;
                Console.Title = tmpCli.Count + "";
                try
                {
                    foreach (var client in tmpCli)
                    {
                        try
                        {

                            var networkStream = client.GetStream();
                            networkStream.Write(sendBytes, 0, sendBytes.Length);
                        }
                        catch
                        {
                            Clients.Remove(client);
                        }
                    }
                }
                catch { }
                sendBytes = Encoding.Unicode.GetBytes(message.Replace("\n", "\0MSGEND\0") + "\0MSGEND\0");
                try
                {
                    foreach (var client in tmpCli)
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
                        }
                    }
                }
                catch (Exception e)
                {
                    //  Log(e);
                }
                if (Clients.Count < oldNumcli) Log($"{oldNumcli - Clients.Count} client(s) have disconnected! Remaining: {Clients.Count}");
                /* });
                 msgSend.Start();*/
                //Log(" >> " + message);

            }
            catch
            {
                Log("fuck this shit");
                // ignored
            }

            // InvokeQueue.Add(() => ConnectionCount.Text = $"Connected clients: {Clients.Count}");
        }
        public static void WhisperMessage(TcpClient Client, string Message)
        {
            try
            {
                var sendBytes = Encoding.Unicode.GetBytes("*SYS*: " + Message.Replace("\n", "\0MSGEND\0") + "\0MSGEND\0");
                try
                {

                    var networkStream = Client.GetStream();
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                }
                catch
                {
                    Clients.Remove(Client);
                    Log("WTF? Whisper failed!");
                    //BroadcastMessage("A client has logged off! (MessageTransmitFailedException!)");
                }

                Log(" >> " + Message);
            }
            catch
            {
                // ignored
            }
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }
        public static void Log(string text)
        {
            LogBox.Items.Add(text);
            Console.WriteLine(text);
        }
    }
    //Class to handle each client request separatly
    public class HandleClient
    {
        TcpClient _clientSocket;
        int _clNo;
        public void StartClient(TcpClient inClientSocket, int cliNo)
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
            var error = 0;
            var username = $"User_{new Random().Next(0, 1000)}";
            GarbageWindow.Log($"Client #{_clNo} connected, IP: {_clientSocket.Client.RemoteEndPoint}");
            while (error < 1)
            {
                Thread.Sleep(100);
                try
                {
                    requestCount = requestCount + 1;
                    var networkStream = _clientSocket.GetStream();
                    var bytesFrom = new byte[_clientSocket.ReceiveBufferSize];
                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                    var utf8string = Encoding.UTF8.GetString(bytesFrom).TrimEnd('\0');

                    // GarbageWindow.Log(utf8string);
                    if (utf8string.Contains("GET") && utf8string.Contains("HTTP/1.1"))
                    {
                        var sendBytes = Encoding.UTF8.GetBytes("ARCANECHAT_SERVER.HTTP_BOT_FOUND_EXCEPTION: Nice attempt to connect to this server using a web browser, real clever...\nDid you really thing I am **THAT** stupid?");
                        try
                        {
                            var sendBytes2 = Encoding.UTF8.GetBytes("\n\n\nException occurred:\n");
                            networkStream.Write(sendBytes2, 0, sendBytes2.Length);
                            sendBytes = Encoding.UTF8.GetBytes(System.Convert.ToBase64String(sendBytes));
                            networkStream.Write(sendBytes, 0, sendBytes.Length);
                        }
                        catch
                        {
                        }
                        networkStream.Close();
                        GarbageWindow.Clients.Remove(_clientSocket);
                        GarbageWindow.BroadcastMessage("Kicked user attempting HTTP tapping");
                        break;
                    }
                    string dataFromClient = Encoding.Unicode.GetString(bytesFrom).TrimEnd('\0');
                    GarbageWindow.Log($"#{_clNo}: \"{dataFromClient}\"");
                    if (dataFromClient.StartsWith("\0CLIMSG\0"))
                    {
                        switch (dataFromClient.Replace("\0CLIMSG\0", ""))
                        {
                            case "exit":
                                GarbageWindow.Clients.Remove(_clientSocket);
                                _clientSocket.Close();
                                GarbageWindow.BroadcastMessage($"Client { _clNo } logged off!");
                                break;
                        }

                    }
                    else if (dataFromClient.StartsWith("\0cmd\0"))
                    {
                        switch (dataFromClient.Replace("\0cmd\0", "").Split(' ')[0].ToLower().Trim())
                        {
                            case "nick":
                                username = dataFromClient.Replace("\0cmd\0nick", "");
                                break;
                            case "logon":
                                username = dataFromClient.Replace("\0cmd\0logon", "");
                                GarbageWindow.BroadcastMessage($"Welcome {username} (#{_clNo} since server launch, {GarbageWindow.Clients.Count} currently logged in)");
                                break;
                        }

                    }
                    else if (dataFromClient.StartsWith("/"))
                    {
                        switch ((dataFromClient.Remove(0, 1) + " ").Split(' ')[0].ToLower().Trim())
                        {
                            case "cli":
                                GarbageWindow.BroadcastMessage("*working*");
                                GarbageWindow.BroadcastMessage($"SERVER BROADCAST: --CLIENTS CONNECTED: {GarbageWindow.Clients.Count}--");
                                break;
                            case "nick":
                                username = dataFromClient.Split(" ".ToCharArray(), 2)[1];
                                GarbageWindow.WhisperMessage(_clientSocket, "Your nickname has been changed to " + username);
                                break;
                            case "serverinfo":
                                GarbageWindow.BroadcastMessage($"Server memory usage: {GC.GetTotalMemory(false)} bytes\nConnection count: {GarbageWindow.Clients.Count}\nUptime: {GarbageWindow.StopWatch.Elapsed.ToString()}\nHost machine name: {Environment.MachineName}");
                                break;
                            case "userinfo":
                                GarbageWindow.BroadcastMessage($"Client number: {_clNo}\nJoin time: {joinTime.ToLongTimeString()}\nConnection time: {DateTime.Now.Subtract(joinTime)}");
                                break;

                            case "clilist":
                                var clis = GarbageWindow.Clients;
                                foreach (var cli in clis)
                                {
                                    GarbageWindow.BroadcastMessage($"Client #{_clNo}: {username} ");
                                }
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
                    GarbageWindow.Log("Exception occurred with a client: " + ex.StackTrace);
                    _clientSocket.Close();

                    error++;
                }
            }

        }


    }

    public class Client
    {
        public Client() { }
        //public int ClientNumber { get; get; }


    }

}
