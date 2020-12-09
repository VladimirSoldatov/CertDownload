using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace Web_client
{

    public partial class Main_menu : Form
    {
        string my_site = "http://cdp.tularegion.ru";
        string my_site2 = "http://crl.roskazna.ru";
        List<string> HREF = new List<string>();
        Regex regex = new Regex(@"/(\w*)");
        public Main_menu()
        {
            InitializeComponent();
 

  



        }
        public void copy_files()
        {
            WebClient wc = new WebClient();
            foreach (string href in HREF)
            {
                try
                {
                    wc.DownloadFile(href, href.Split('/')[href.Split('/').Length - 1]);
                    listBox1.Items.Add(href);
                }
                catch
                {
                    //MessageBox.Show(ex.Message);s;
                    listBox2.Items.Add(href);
                }
            }
        }
        public void get_crl(string site)
        {
            WebClient web_Client = new WebClient();
            string answer = String.Empty;
            string answer2 = String.Empty; 
            try
            {
    
                using (Stream data = web_Client.OpenRead(site))
                {
                    using (StreamReader reader = new StreamReader(data))
                    {
                        answer = reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                answer = String.Empty;
            }
     


            string buf = String.Empty;

            for (int i = 0; i < answer.Length - 4; i++)
            {
                if (answer.Substring(i, 4) == "HREF"|| answer.Substring(i, 4) == "href")
                {
                    buf = String.Empty;
                    i += 6;
                    while (answer.Substring(i, 1) != @"""")
                    {
                        buf += answer.Substring(i, 1);
                        i++;
                    }
                }
                else
                    continue;
                //regex.Match(buf);
                string temp=String.Empty;
                 if (site.Contains("tularegion"))
                    {
                        temp = my_site;
                    }
                    else
                    {
                        temp = site;
                    }
                if (buf.Contains(".crl") || buf.Contains(".cer") || buf.Contains(".crt"))
                {
                    if (!buf.Contains("/"))
                        buf = "/"+ buf;
               



                    if (buf.ToLower().Contains("http"))
                            HREF.Add(buf);
                    else
                            HREF.Add(temp + buf);

                }
                else
                {

                    if (buf != "/" && !buf.Contains("."))
                        get_crl(temp + buf);

                    //continue; ;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            get_crl(my_site);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            copy_files();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            get_crl(my_site2);
        }
    }
}
