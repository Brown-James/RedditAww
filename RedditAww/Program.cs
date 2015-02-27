using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using RedditSharp;

namespace RedditAww
{
    class Program
    {
        static void Main(string[] args)
        {
            Reddit reddit = new Reddit();
            var subreddit = reddit.GetSubreddit("/r/aww");

            foreach (var post in subreddit.New.Take(25))
            {
                Console.WriteLine(post.Title);
            }

            Console.ReadLine();
        }
    }
}
