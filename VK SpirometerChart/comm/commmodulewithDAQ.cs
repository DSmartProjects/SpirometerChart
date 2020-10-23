using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VideokallSBCDataAcquisionApp.comm
{
   public class Commmodule 
    {
        UdpClient udpClient;
        public int Portnumber { get; set; } = 9889;
        //   public int SMCPortnumber { get; set; } = 9856;
        public IPEndPoint SMCEndpoint;
        public delegate void NotifyMessage(string msg);
        public delegate void StartTimer( bool b );
        public NotifyMessage ReceivedMessage;
      public  Thread backgroundThread = null;
        public bool StopBackgroundThread { get; set; } = false;

      public  void initialize()
        {
           
            udpClient = new UdpClient(Portnumber);
            //   IPAddress ipaddress = IPAddress.Parse("192.168.0.17");
            //  SMCEndpoint = new IPEndPoint(ipaddress, SMCPortnumber);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            ThreadStart TS = new ThreadStart(readData);
            backgroundThread = new Thread(TS);
            backgroundThread.Start();
         //   readDataasync();
        }

        void LogMessage(string msg)
        {
            try
            {
                using (System.IO.StreamWriter file =
           new System.IO.StreamWriter("exceptionlogs.txt", true))
                {
                    file.WriteLine(DateTime.Now.ToString() + ": " + msg);
                }
            }
            catch (Exception ex)
            {

            }

        }

        public async void readDataasync()
        {
            try
            {
                UdpReceiveResult res = await udpClient.ReceiveAsync();
                SMCEndpoint = res.RemoteEndPoint;
                byte[] buffer = res.Buffer;

                string str = Encoding.Unicode.GetString(buffer);

                ReceivedMessage?.Invoke(str);
                
                LogMessage(str);

            }
            catch (Exception ex) { LogMessage(ex.Message); }
        }
        public   void readData()
        {

            try
            {
                while (true)
                {
                    if (StopBackgroundThread)
                        break;

                    Console.WriteLine("Waiting for broadcast");
                    byte[] buffer = udpClient.Receive(ref SMCEndpoint);
                    string str = Encoding.Unicode.GetString(buffer);
                    //  string str = Encoding.ASCII.GetString(buffer);
                    ReceivedMessage?.Invoke(str);
                  //  Thread.Sleep(10);
                    
                    //Console.WriteLine($"Received broadcast from {groupEP} :");
                   // Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                udpClient.Close();
            }

            //try {
            //    UdpReceiveResult res = await udpClient.ReceiveAsync();
            //    SMCEndpoint = res.RemoteEndPoint;
            //    byte[] buffer = res.Buffer;

            //    string str = Encoding.ASCII.GetString(buffer);

            //    ReceivedMessage?.Invoke(str);

            //    LogMessage(str);

            //} catch (Exception ex) { LogMessage(ex.Message); } 
        }

        public void SendData(string msg)
        {
            try {

                if (SMCEndpoint != null)
                {
                    byte[] buffer = Encoding.Unicode.GetBytes(msg);
                    udpClient.Send(buffer, buffer.Length, SMCEndpoint);
                }


            } catch (Exception ex )
            {
                LogMessage(ex.Message);
            }
           
        }



        public void SendByteData(byte[] buffer)
        {
            if (SMCEndpoint != null)
            {
              //  byte[] buffer = Encoding.Unicode.GetBytes(msg);
                udpClient.Send(buffer, buffer.Length, SMCEndpoint);
            }
        }


        public void Reset()
        {
            udpClient.Close();
            udpClient.Dispose();

        }

    }
}
