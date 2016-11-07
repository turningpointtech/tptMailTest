using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImapX;
using OpenPop;
using OpenPop.Pop3;
using System.Threading;

namespace tptMailTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void updatePortBox()
        {
            if (checkBox1.Checked)
            {
                if (radioButton2.Checked)
                {
                    portBox.Text = "995";
                }
                else
                {
                    portBox.Text = "993";
                }
            }
            else
            {
                if (radioButton2.Checked)
                {
                    portBox.Text = "110";
                }
                else
                {
                    portBox.Text = "143";
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            updatePortBox();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            updatePortBox();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            updatePortBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            CheckForIllegalCrossThreadCalls = false;
            Thread th = new Thread(new ThreadStart(testServer));
            th.Start();
        }

        public void testServer()
        {
            if (radioButton1.Checked)
            {
                imapTest();
            }
            else
            {
                popTest();
            }
        }

        public void addLog(string str)
        {
            listBox1.Items.Add(str);
        }

        public void popTest()
        {
            if (checkBox1.Checked) addLog("Beginning SECURE POP3 test...");
            else addLog("Beginning POP3 test...");
            using (Pop3Client client = new Pop3Client())
            {
                try
                {
                    client.Connect(serverBox.Text, Convert.ToInt32(portBox.Text), checkBox1.Checked);
                    if (client.Connected)
                    {
                        addLog("Connected to server!");
                        addLog("Attempting to authenticate...");
                        try
                        {
                            client.Authenticate(userBox.Text, passBox.Text);
                            addLog("Authentication successful!");
                            addLog("Test passed!");
                        } catch (Exception ez)
                        {
                            addLog("Authentication failed! " + ez.Message);
                            addLog("Test FAILED!");
                        }
                    }
                    else
                    {
                        addLog("Failed to connect to server!");
                    }
                }
                catch (Exception ex)
                {
                    addLog("Failed to connect to server! " + ex.Message);
                }
            }
        }

        public void imapTest()
        {
            if (checkBox1.Checked) addLog("Beginning SECURE IMAP test...");
            else addLog("Beginning IMAP test...");
            using (ImapClient client = new ImapClient(serverBox.Text, Convert.ToInt32(portBox.Text), checkBox1.Checked, false))
            {

                if (client.Connect())
                {
                    addLog("Connected to server!");
                    addLog("Attempting to authenticate...");
                    if (client.Login(userBox.Text, passBox.Text))
                    {
                        addLog("Authentication successful!");
                        addLog("Test passed!");
                    }
                    else
                    {
                        addLog("Authentication FAILED!");
                        addLog("Test FAILED!");
                    }
                }
                else
                {
                    addLog("Failed to connect to server!");
                }
            }
        }
    }
}
