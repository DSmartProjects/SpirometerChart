using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MirDataTypes;
using MirDeviceManager;
using MirCharting;
using VideokallSBCDataAcquisionApp.comm;
using System.Windows.Threading;

namespace VK_SpirometerChart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Commmodule commtoDAQ = new Commmodule();
       // double appheight = 700;
       // double apptop = 50;
        public MainWindow()
        {
            InitializeComponent();
            commtoDAQ.ReceivedMessage += ReceivedMessage;
            commtoDAQ.initialize();          
            this.WindowState = WindowState.Maximized;
            double W = System.Windows.SystemParameters.PrimaryScreenWidth;
            double H = System.Windows.SystemParameters.PrimaryScreenHeight;

             
            //FVchart.Height = 300; //300; //300; //700;
            //FVchart.Width = 390;// 390; //390; //200;// 400;
             
            //FVchart.Height = 300; //300; //300; //700;
            //FVchart.Width = 390;// 390; //390; //200;// 400;
            //VTchart.Width = 400;
            //VTchart.Height = 200;

            //this.Topmost = true;
            //   this.Hide();
            // FVchart.ChartColor = Color.re
            this.WindowState = WindowState.Normal;
            this.WindowState = WindowState.Minimized;
        }

        void ReceivedMessage(string msg)
        {
            string[] cmd = msg.ToLower().Split('>');
            
            switch (cmd[0])
            {
                case "<startspirovc":
                    string[] ht1 = cmd[2].Split(':');
                    this.Dispatcher.Invoke(DispatcherPriority.Send,
                   new Action(delegate
                   {
                      // this.Top = 50;//this.Top = 200;// Convert.ToDouble(ht[1] ) ;
                       //this.Left = 0;// Convert.ToDouble(ht[0]);
                       //this.Width = Convert.ToDouble(ht1[3]) + 215;
                       //this.Height = Convert.ToDouble(ht1[2]) - 105;
                     //  appheight = Convert.ToDouble(ht1[2]) - 105; ;
                      // apptop = 50;// apptop = 200;
                       count = 0;
                      // this.Topmost = Topmost;
                       if (Height < 750)
                       {
                           BtnFullView.IsEnabled = true;
                       }
                       else
                       {
                           BtnFullView.IsEnabled = false;
                       }
                       VTchart.ClearCurveDataPointsFromGraph();
                       FVchart.ClearCurveDataPointsFromGraph();
                       this.WindowState = WindowState.Normal;
                   }));

                 
                    break;
                case "<startspirofvc":
                    string[] ht = cmd[2].Split(':');
                    this.Dispatcher.Invoke(DispatcherPriority.Send,
                   new Action(delegate
                   {
                       //this.Top = 50;//this.Top = 200;// Convert.ToDouble(ht[1] ) ;

                      // this.Left = 0;// Convert.ToDouble(ht[0]);
                      // this.Width = Convert.ToDouble(ht[3])+215;
                      //this.Height = Convert.ToDouble(ht[2])-105;
                     //  appheight = Convert.ToDouble(ht[2]) - 105; ;
                      // apptop = 50;//apptop = 200;
                       if (Height<750)
                       {
                           BtnFullView.IsEnabled = true; 
                       }
                       else
                       {
                           BtnFullView.IsEnabled = false;
                       }
                        

                       this.WindowState = WindowState.Normal;
                       count = 0;
                       VTchart.ClearCurveDataPointsFromGraph();
                       FVchart.ClearCurveDataPointsFromGraph();
                   }));

                   
                    break;
                case "spirofvcvt":
      //  Application.Current.Dispatcher.BeginInvoke(
      //  DispatcherPriority.Background,
      //  new Action(() =>
      //  {
      //      string[] vdata = cmd[1].Split(',');
      //      double vol = Convert.ToDouble(vdata[0]);
      //      double time = Convert.ToDouble(vdata[1]);
      //      VTchart.AddPointToLine(vol, time);
      //  }
      //  ));
        DisplayVolumeTimeData(cmd[1]);
                    break;
                case "spirofvc":
                    //  DisplayFlowVolumeData(cmd[1]);

            Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() => {
            string[] vdata = cmd[1].Split(',');
            double flow = Convert.ToDouble(vdata[0]);
            double vol = Convert.ToDouble(vdata[1]);
            count++;
            ///  this.WindowState = WindowState.Minimized;
          //  TxtLbl.Text = "";
         //   TxtLbl.Text = count.ToString();
            FVchart.AddPointToLine(flow, vol);
        }));

                
                    break;
                case "spirovc":

                    DisplayVolumeTimeData(cmd[1]);
                    break;

                case "<stopspiro":
                    this.Dispatcher.Invoke(DispatcherPriority.Send,
                     new Action(delegate
                         {
                             this.WindowState = WindowState.Minimized;

                         }));
                   
                    break;
            }
          //  commtoDAQ.readDataasync();
        }

        long count = 0;
        void DisplayVolumeTimeData(string data)
        {
            string[] vdata = data.Split(',');
            double vol = Convert.ToDouble(vdata[0]);
            double time = Convert.ToDouble(vdata[1]);


            //Dispatcher.BeginInvoke(new Action(delegate
            //{
            //    VTchart.AddPointToLine(vol, time);
            //}));
            this.Dispatcher.Invoke(DispatcherPriority.Send,
          new Action(delegate
          {
                //  TxtLbl.Text = "";
                // TxtLbl.Text = data;
                VTchart.AddPointToLine(vol, time);
          }));

        }
        void DisplayFlowVolumeData(string data)
        {
            string[] vdata = data.Split(',');
            double flow =   Convert.ToDouble(vdata[0]);
            double vol =   Convert.ToDouble(vdata[1]);
            count++;
            //Dispatcher.BeginInvoke(new Action(delegate
            //{
            //    FVchart.AddPointToLine(flow,vol );
            //}));
           
            this.Dispatcher.Invoke(DispatcherPriority.Send,
        new Action(delegate
        {
            if (count > 2000)
            {
                // FVchart.ClearCurveDataPointsFromGraph();
               //  count = 0;
               
            }
            //this.Show();
            //TxtLbl.Text = "";
            //TxtLbl.Text = count.ToString();
            FVchart.AddPointToLine(flow, vol);
           
        }));

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            commtoDAQ.StopBackgroundThread = true;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            commtoDAQ.StopBackgroundThread = true;
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            commtoDAQ.StopBackgroundThread = true;
         //   e.Cancel =true;
          
          //  commtoDAQ.backgroundThread.Join();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            commtoDAQ.StopBackgroundThread = true;
        }

        private void Window_Closing_2(object sender, System.ComponentModel.CancelEventArgs e)
        {
            commtoDAQ.StopBackgroundThread = true;
            e.Cancel = false;
        }

        private void Window_Unloaded_1(object sender, RoutedEventArgs e)
        {
            commtoDAQ.StopBackgroundThread = true;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            FVchart.ClearCurveDataPointsFromGraph();
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            VTchart.ClearCurveDataPointsFromGraph();
            FVchart.ClearCurveDataPointsFromGraph();
        }

        bool fullview = false;
        private void BtnFullView_Click(object sender, RoutedEventArgs e)
        {
            fullview  = !fullview;
            BtnFullView.Content = fullview ? "Normal View" : "Full View";
            if (fullview)
            {
               
              //  this.Top = 30;// Convert.ToDouble(ht[1] ) ;

               // this.Left = 0;// Convert.ToDouble(ht[0]);
                              //  this.Width = Convert.ToDouble(ht[3]) + 215;
                this.Height = 800;// Convert.ToDouble(ht[2]) - 105;
            }
            else
            {
                //this.Top = apptop;// Convert.ToDouble(ht[1] ) ;

                // this.Left = 0;// Convert.ToDouble(ht[0]);
                //  this.Width = Convert.ToDouble(ht[3]) + 215;
               // this.Height = appheight;// Convert.ToDouble(ht[2]) - 105;
            }
        }

        private void BtnClearvc_Click(object sender, RoutedEventArgs e)
        {
            VTchart.ClearCurveDataPointsFromGraph();
        }
    }
}
