using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace ServerWater2.APIs
{
    public class MyFirebase
    {
        private FirebaseApp myapp;
        public MyFirebase()
        {
            myapp = FirebaseApp.Create(new AppOptions()
            {
                ProjectId = "lux-japan-care-2b801",
                Credential = GoogleCredential.FromFile(@"lux-japan-care-2b801-firebase-adminsdk-1t3up-e92dd9e790.json"),
            }, "LuxJapanCare");
        }

        //public async void SendPushNotification()
        //{
        //          var app = FirebaseApp.Create(new AppOptions()
        //          {
        //		ProjectId = "lux-japan-care-2b801",
        //              Credential = GoogleCredential.FromFile(@"lux-japan-care-2b801-firebase-adminsdk-1t3up-e92dd9e790.json"),
        //          }, "LuxJapanCare");
        //	var message = new FirebaseAdmin.Messaging.Message {
        //              Data = new Dictionary<string, string> {
        //                  { "additional_data", "a string" },
        //                  { "another_data", "another string" },
        //              },
        //              Notification = new Notification
        //              {
        //                  Title = "a title",
        //                  Body = "a body",
        //                  ImageUrl = "https://a_image_url"
        //              },
        //              Token = "a valid token",
        //          };
        //          var firebaseMessagingInstance = FirebaseMessaging.GetMessaging(app);
        //          //FirebaseMessaging.GetMessaging(app).SendAsync(message).ConfigureAwait(false);
        //          var result = await firebaseMessagingInstance.SendAsync(message).ConfigureAwait(false);
        //      }

        public async Task<bool> SendPushNotificationAsync(string token, string title, string body, string image, Dictionary<string, string> data)
        {
            var message = new FirebaseAdmin.Messaging.Message
            {
                //Data = new Dictionary<string, string> {
                //    { "additional_data", "a string" },
                //    { "another_data", "another string" },
                //},
                //Notification = new Notification
                //{
                //    Title = "a title",
                //    Body = "a body",
                //    ImageUrl = "https://a_image_url"
                //},
                Data = data,
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                    ImageUrl = image,
                },
                Token = token,
            };
            var firebaseMessagingInstance = FirebaseMessaging.GetMessaging(myapp);
            //FirebaseMessaging.GetMessaging(app).SendAsync(message).ConfigureAwait(false);
            try
            {
                var result = await firebaseMessagingInstance.SendAsync(message).ConfigureAwait(true);
                return true;
            }
            catch (Exception e)
            {
                if (e.Message.CompareTo("The registration token is not a valid FCM registration token") == 0)
                {
                    Console.WriteLine(string.Format("Token ID : {0} - Registration token is not a valid FCM", message.Token));
                    return false;
                }
                else
                {
                    Console.WriteLine(e);
                    return true;
                }

            }
        }
    }

}
