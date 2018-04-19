using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using SeasideResearch.LibCurlNet;  

namespace CS_libcurl
{
    class Program
    {
        private static BinaryWriter bw = null;  
        public static Int32 OnWriteData(Byte[] buf, Int32 size, Int32 nmemb,
                                        Object extraData)
        {
            Program.bw.Write(buf);
            return size * nmemb;
        } 
        static void Main(string[] args)
        {
            try
            {
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                Easy easy = new Easy();
                Easy.WriteFunction wf = new Easy.WriteFunction(OnWriteData);

                if (args.Length > 1)
                {
                    easy.SetOpt(CURLoption.CURLOPT_URL, args[0]);
                }
                else
                {
                    string url = string.Empty;
                    Console.WriteLine("Download URL: ");
                    url = "http://jashliao.pixnet.net/blog";//Console.ReadLine();

                    easy.SetOpt(CURLoption.CURLOPT_URL, url);
                }
                easy.SetOpt(CURLoption.CURLOPT_VERBOSE, 1);
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);
                easy.SetOpt(CURLoption.CURLOPT_NOPROGRESS, 1);

                string path = string.Empty;
                Console.WriteLine("Save Path: ");
                path = "123.html";//Console.ReadLine();
                Program.bw = new BinaryWriter(new FileStream(path, FileMode.Create));

                easy.Perform();

                easy.Cleanup();

                Program.bw.Close();

                Curl.GlobalCleanup();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }  
        }
    }
}
