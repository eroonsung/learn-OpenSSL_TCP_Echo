﻿using System;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Client
{
    public partial class Form1 : Form
    {
        public string serverIP = "127.0.0.1";
        public int port = 9876;
        public string serverDomain = "localhost";
        public Form1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient(serverIP, port);
            SslStream sslStream = new SslStream(client.GetStream(),false, validateCertificate, null);
            sslStream.AuthenticateAsClient(serverDomain);

            byte[] buf = Encoding.ASCII.GetBytes("Hello SSL!");
            sslStream.Write(buf, 0, buf.Length);
            sslStream.Flush(); //에코 서버로부터 메시지를 전달 받음

            buf = new byte[4096];
            int length = sslStream.Read(buf, 0, 4096);
            messageText.Text = Encoding.ASCII.GetString(buf, 0, length);
        }
        //인증서 유효성 검사
        private bool validateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
           //항상 트루값 반환 => 테스트할 때 인증서 오류 없이 사설 인증서로 테스트 진행 가능
            return true;
        }
    }
}