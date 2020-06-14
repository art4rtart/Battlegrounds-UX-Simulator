using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SendEmail : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnButtonClick();
        }
    }
    public void OnButtonClick()
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("her0in941018@gmail.com"); // 보내는사람

        mail.To.Add("sq7r9760@naver.com"); // 받는 사람
        mail.To.Add("her0in941018@gmail.com"); // 받는 사람

        mail.Subject = "Test Mail";

        mail.Body = "This is for testing SMTP mail from GMAIL";


        // 첨부파일 - 대용량은 안됨.
        //System.Net.Mail.Attachment attachment;
        //attachment = new System.Net.Mail.Attachment("D:\\Test\\2018-06-11-09-03-17-E7104.mp4"); // 경로 및 파일 선택
        //mail.Attachments.Add(attachment);

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("her0in941018@gmail.com", "chldnwls9760") as ICredentialsByHost; // 보내는사람 주소 및 비밀번호 확인
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");
    }
}
