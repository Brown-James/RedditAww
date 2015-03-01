using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

using RedditSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditAww
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a subbreddit: ");
            var subredditStr = Console.ReadLine();
            Reddit reddit = new Reddit();
            var subreddit = reddit.GetSubreddit(subredditStr);
            
            using (WebClient client = new WebClient())
            {
                foreach (var post in subreddit.New.Take(25))
                {
                    if (!post.IsSelfPost)
                    {
                        string imageUrl = post.Url.ToString();
                        
                        if(imageUrl.Contains("imgur.com"))
                        {
                            string imageId = "";

                            // Extract the image ID from the URL
                            if (imageUrl.Contains("i.imgur.com"))
                            {
                                imageId = imageUrl.Substring(imageUrl.Length - 11, 7);
                            }
                            else
                            {
                                imageId = imageUrl.Substring(imageUrl.Length - 7, 7);
                                Console.WriteLine("Hello! {0}", imageId);
                            }

                            // Send a request to the imgur API for more information about the image
                            // using the image ID
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.imgur.com/3/image/id/" + imageId);
                            request.Headers.Add("Authorization", "Client-ID 8c44ab1a312f478");
                            request.Method = WebRequestMethods.Http.Get;
                            request.Accept = "application/json";

                            // Read the response from the server
                            Stream response = request.GetResponse().GetResponseStream();
                            StreamReader reader = new StreamReader(response);
                            string responseFromServer = reader.ReadToEnd();
                            reader.Close();
                            response.Close();

                            // Parse the response into JSON, and take only the parts we are interested in
                            JObject imageSearch = JObject.Parse(responseFromServer);

                            string imageType = imageSearch["data"]["type"].ToString();
                            imageType = imageType.Substring(imageType.Length - 4, 4);

                            // If the URL is NOT of form i.imgur.com    i.e. imgur.com/XXXXXXX
                            if(!imageUrl.Contains("i.imgur.com"))
                            {
                                imageUrl = "http://i.imgur.com/" + imageId + "." + imageType;
                            }

                            Uri properImageUrl = new Uri(imageUrl);

                            Console.WriteLine("Downloading : " + post.Url + "     Author : " + post.Author.FullName);
                            Console.WriteLine("Post: " + post.Title);

                            DownloadImage(client, properImageUrl, "E:/James/" + subredditStr + "/" + post.Author.FullName, imageId, imageType);
                        }
                    }
                }
            }

            Console.WriteLine("Complete!");
            Console.ReadLine();
        }

        static void DownloadImage(WebClient client, Uri imageUrl, string savePath, string imageId, string imageType)
        {
            if(!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            client.DownloadFileAsync(imageUrl, @savePath + "/" + imageId + "." + imageType);
        }

        static void DownloadAlbum()
        {

        }
    }
}
