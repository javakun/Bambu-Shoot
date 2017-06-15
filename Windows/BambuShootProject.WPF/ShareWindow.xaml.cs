using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BambuShootProject.WPF
{
    /// <summary>
    /// Interaction logic for ShareWindow.xaml
    /// </summary>
    public partial class ShareWindow : Window
    {
        private ReportWindow reportWindow;

        

        public ShareWindow(ReportWindow reportWindow)
        {
            this.reportWindow = reportWindow;
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            email_send();
            this.Hide();
        }
        public void email_send()
        {

            if (!IsValidEmail(textBox.Text)){

                MessageBox.Show(" Please entr a valid email");
            }
                    
            else{
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("uprm.bambushoot@gmail.com");
                mail.To.Add(textBox.Text);
                mail.Subject = "Test Mail - 1";
                mail.Body = "mail with attachment";

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment("C:/Users/Public/Pictures/BambuShoot/" + imageTitle.Text + "/Report.pdf");
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("uprm.bambushoot@gmail.com", "Capstone2017");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail); }
        }



        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
