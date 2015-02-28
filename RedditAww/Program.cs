using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using RedditSharp;
using System.Net;

namespace RedditAww
{
    class Program
    {
        static void Main(string[] args)
        {
            Reddit reddit = new Reddit();
            var subreddit = reddit.GetSubreddit("/r/aww");
            
            using (WebClient client = new WebClient())
            {
                foreach (var post in subreddit.New.Take(25))
                {
                    if (!post.IsSelfPost)
                    {
                        Console.WriteLine("Downloading : " + post.Url);
                        Directory.CreateDirectory("E:/James/Test/" + post.Author.FullName);
                        client.DownloadFileAsync(post.Url, @"E:/James/Test/" + post.Author.FullName + "/test.png");
                    }
                }
            }
            
            Console.ReadLine();
        }
    }
}
