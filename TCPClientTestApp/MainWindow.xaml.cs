﻿using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace TCPMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
		NetworkStream stream;
		BackgroundWorker bck;
		TcpClient client;

		public MainWindow()
		{
			InitializeComponent();            
        }
        
        private void btnListen_Click(object sender, RoutedEventArgs e)
        {
            tbDataReceived.Text = "Start Listening";
            bck = new BackgroundWorker();
            bck.WorkerReportsProgress = true;
            bck.ProgressChanged += Bck_ProgressChanged;
            bck.DoWork += Bck_DoWork;
            bck.RunWorkerAsync();
        }

        private void Bck_DoWork(object sender, DoWorkEventArgs e)
        {
            TcpListener server = null;
            try
            {
                Int32 port = 3000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                server = new TcpListener(localAddr, port);

                //Start listening for client requests.
                server.Start();

                //Buffer for reading data
                Byte[] bytes = new Byte[6];

                //Enter the listening loop.
                while (true)
                {
                    bck.ReportProgress(0, "Waiting for a connection... ");

                    //Accept TcpClient
                    TcpClient client = server.AcceptTcpClient();
                    bck.ReportProgress(0, "Connected!");


                    stream = client.GetStream();

                    int i;

                    //Get all data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        //display received data by reporting progress to the background worker
                        bck.ReportProgress(0, "I0: " + bytes[0].ToString() + 
                                            "\tI1: " + bytes[1].ToString() + 
                                            "\tI2: " + bytes[2].ToString() +
                                            "\tI3: " + bytes[3].ToString() +
                                            "\nO0: " + bytes[4].ToString() +
                                            "\tO1: " + bytes[5].ToString() + "\n");
                    }

                    //Shutdown and end connection
                    client.Close();
                }

            }
            catch (Exception ex)
            {
                bck.ReportProgress(0, string.Format("SocketException: {0}", ex.ToString()));
            }
        }

        private void Bck_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string data = (string)e.UserState;

            tbDataReceived.Text = string.Format("Received: {0}", data) + Environment.NewLine + tbDataReceived.Text;
        }

        private void SendMessage(byte[] command)
        {
            if(client==null)
            {
                client = new TcpClient("127.0.0.1", 2000);
            }
            
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = new byte[2];           
            bytesToSend = command;

            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        }

		private void btnOff_Click(object sender, RoutedEventArgs e)
		{
            try
			{
                SendMessage(new byte[] { 128, 0 });             
			}
			catch (Exception ex)
			{
				tbDataReceived.Text = tbDataReceived + ex.ToString();
			}
		}
        
        private void btnOn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SendMessage(new byte[] { 64, 128 });               
            }
            catch (Exception ex)
            {
                tbDataReceived.Text = tbDataReceived + ex.ToString();
            }
        }
        private void btnPump1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SendMessage(new byte[] { 16, 128 });
            }
            catch (Exception ex)
            {
                tbDataReceived.Text = tbDataReceived + ex.ToString();
            }
        }
        private void btnPump2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SendMessage(new byte[] { 8, 128 });
            }
            catch (Exception ex)
            {
                tbDataReceived.Text = tbDataReceived + ex.ToString();
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SendMessage(new byte[] { 4, 128 });
            }
            catch (Exception ex)
            {
                tbDataReceived.Text = tbDataReceived + ex.ToString();
            }
        }

        private void cb1_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                SendMessage(new byte[] { 2, 128 });
            }
            catch (Exception ex)
            {
                tbDataReceived.Text = tbDataReceived + ex.ToString();
            }
        }

        private void cb2_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                SendMessage(new byte[] { 1, 128 });
            }
            catch (Exception ex)
            {
                tbDataReceived.Text = tbDataReceived + ex.ToString();
            }
        }
    }
}
