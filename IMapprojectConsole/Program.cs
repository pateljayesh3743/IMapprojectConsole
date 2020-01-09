using System;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;
using System.Collections.Generic;
using System.IO;

namespace TestClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (var client = new ImapClient())
            {
                // For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

             //   client.Connect("imap.friends.com", 993, true);
                client.Connect("imap.gmail.com", 993, true);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

               // client.Authenticate("joey", "password");
                client.Authenticate("6576445@gmail.com", "9591173");

                int tmpcnt = 0;
                client.Inbox.Open(FolderAccess.ReadWrite);
                
                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
              //  inbox.Open(FolderAccess.ReadOnly);

                Console.WriteLine("Total messages: {0}", inbox.Count);
                Console.WriteLine("Recent messages: {0}", inbox.Recent);

                  foreach (var uid in client.Inbox.Search(SearchQuery.NotSeen))
                {
                    try
                    {
                        var message = client.Inbox.GetMessage(uid);
                        client.Inbox.SetFlags(uid, MessageFlags.Seen, true);

                        List<byte[]> listAttachment = new List<byte[]>();

                        if (message.Attachments!=null)
                        {
                            foreach (var objAttach in message.Attachments)
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    ((MimeKit.ContentObject)(((MimeKit.MimePart)(objAttach)).ContentObject)).Stream.CopyTo(ms);
                                    byte[] objByte = ms.ToArray();
                                    listAttachment.Add(objByte);
                                }
                            }
                        }

                        string subject = message.Subject;
                        string text = message.TextBody;
                       // var hubContext = GlobalHost.ConnectionManager.GetHubContext<myHub>();
                      //  hubContext.Clients.All.modify("fromMail", text);
                        tmpcnt++;
                    }
                    catch (Exception)
                    { }
                }
                client.Disconnect(true);
            }
        }

                //for (int i = 0; i < inbox.Count; i++)
                //{
                //    var message = inbox.GetMessage(i);
                //    Console.WriteLine("Subject: {0}", "-------start--------");
                //    Console.WriteLine("Subject: {0}", message.ResentFrom);
                //    Console.WriteLine("Subject: {0}", message.ResentCc);
                //    Console.WriteLine("Subject: {0}", message.ReplyTo);
                //    Console.WriteLine("Subject: {0}", message.Subject);
                //    Console.WriteLine("Subject: {0}", message.TextBody);
                //    Console.WriteLine("Subject: {0}", message.Date);
                //    Console.WriteLine("Subject: {0}", "------end---------");
                //}

              //  client.Disconnect(true);
            }
        }
    
