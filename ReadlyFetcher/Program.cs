using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace ReadlyFetcher
{
    class Program
    {
        public static string auth = string.Empty;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine();
                Console.WriteLine("====================== No arguments found! ======================");
                Console.WriteLine("Valid arguments are below, but it MUST start with your auth token!");
                Console.WriteLine("=============================================================================");
                Console.WriteLine();
                Console.WriteLine("=============================================================================");
                Console.WriteLine("-a=TOKEN - This is your auth token. you need a valid subscription.");
                Console.WriteLine("Whilst on a mag page press Ctrl+Shift+i and get your auth token like below");
                Console.WriteLine("https://i.imgur.com/UQZyqKT.png");
                Console.WriteLine("=============================================================================");
                Console.WriteLine();
                Console.WriteLine("=============================================================================");
                Console.WriteLine("-i=ISSUEID - This is for the ID of a specific issue.");
                Console.WriteLine("-p=PUBLICATIONID - This is for the ID of a mag and all issues.");
                Console.WriteLine("IDs can be found by going to the url of the mag, for example, ");
                Console.WriteLine("https://go.readly.com/magazines/PUBID/ISSUEID/1");
                Console.WriteLine("The first string of numbers and letters is the publication ID");
                Console.WriteLine("The second is the specific issue ID. ");
                Console.WriteLine("=============================================================================");
                Console.WriteLine();
                Console.WriteLine("=============================================================================");
                Console.WriteLine("all - will download ALL mags, this is dumb and probably should be avoided.");
                Console.WriteLine("=============================================================================");
                Console.WriteLine("Press return/enter to exit.");
                Console.ReadLine();

                //Console.WriteLine("Please enter a ISO 3166 country code\nor \"all\" for all countries then press enter");
                //string reply = Console.ReadLine();
                //if (reply == "all")
                //{
                //    string codes = "AF,AX,AL,DZ,AS,AD,AO,AI,AQ,AG,AR,AM,AW,AC,AU,AT,AZ,BS,BH,BB,BD,BY,BE,BZ,BJ,BM,BT,BW,BO,BA,BV,BR,IO,BN,BG,BF,BI,KH,CM,CA,CV,KY,CF,TD,CL,CN,CX,CC,CO,KM,CG,CD,CK,CR,CI,HR,CU,CY,CZ,DK,DJ,DM,DO,TP,EC,EG,SV,GQ,ER,EE,ET,EU,FK,FO,FJ,FI,FR,FX,GF,PF,TF,MK,GA,GM,GE,DE,GH,GI,GB,GR,GL,GD,GP,GU,GT,GG,GN,GW,GY,HT,HM,HN,HK,HU,IS,IN,ID,IR,IQ,IE,IL,IM,IT,JE,JM,JP,JO,KZ,KE,KI,KP,KR,XK,KW,KG,LA,LV,LB,LI,LR,LY,LS,LT,LU,MO,MG,MW,MY,MV,ML,MT,MH,MQ,MR,MU,YT,MX,FM,MC,MD,MN,ME,MS,MA,MZ,MM,NA,NR,NP,NL,AN,NT,NC,NZ,NI,NE,NG,NU,NF,MP,NO,OM,PK,PW,PS,PA,PG,PY,PE,PH,PN,PL,PT,PR,QA,RE,RO,RU,RW,GS,SH,KN,LC,MF,VC,WS,SM,ST,SA,SN,RS,YU,SC,SL,SG,SI,SK,SB,SO,ZA,SS,ES,LK,SD,SR,SJ,SZ,SE,CH,SY,TW,TJ,TZ,TH,TG,TK,TO,TT,TN,TR,TM,TC,TV,UG,UA,AE,UK,US,UM,UY,SU,UZ,VU,VA,VE,VN,VG,VI,WF,EH,YE,ZM,ZW";
                //    getPublications(codes);
                //}
                //else
                //{
                //    getPublications(reply);
                //}
            }
            else
            {
                if (args[0].StartsWith("-a="))
                {
                    auth = args[0].Split('=')[1];

                    foreach (var argument in args)
                    {
                        if (argument.StartsWith("-p="))
                        {
                            string pubid = argument.Split('=')[1];
                            getIssues(pubid, null);
                        }
                        else if (argument.StartsWith("-i="))
                        {
                            string issueid = argument.Split('=')[1];
                            getSingleIssue(null, issueid);
                        }
                        else if (argument == "all")
                        {
                            Console.WriteLine("Downloading all mags... Good luck, your gonna need it.");
                            string codes = "AF,AX,AL,DZ,AS,AD,AO,AI,AQ,AG,AR,AM,AW,AC,AU,AT,AZ,BS,BH,BB,BD,BY,BE,BZ,BJ,BM,BT,BW,BO,BA,BV,BR,IO,BN,BG,BF,BI,KH,CM,CA,CV,KY,CF,TD,CL,CN,CX,CC,CO,KM,CG,CD,CK,CR,CI,HR,CU,CY,CZ,DK,DJ,DM,DO,TP,EC,EG,SV,GQ,ER,EE,ET,EU,FK,FO,FJ,FI,FR,FX,GF,PF,TF,MK,GA,GM,GE,DE,GH,GI,GB,GR,GL,GD,GP,GU,GT,GG,GN,GW,GY,HT,HM,HN,HK,HU,IS,IN,ID,IR,IQ,IE,IL,IM,IT,JE,JM,JP,JO,KZ,KE,KI,KP,KR,XK,KW,KG,LA,LV,LB,LI,LR,LY,LS,LT,LU,MO,MG,MW,MY,MV,ML,MT,MH,MQ,MR,MU,YT,MX,FM,MC,MD,MN,ME,MS,MA,MZ,MM,NA,NR,NP,NL,AN,NT,NC,NZ,NI,NE,NG,NU,NF,MP,NO,OM,PK,PW,PS,PA,PG,PY,PE,PH,PN,PL,PT,PR,QA,RE,RO,RU,RW,GS,SH,KN,LC,MF,VC,WS,SM,ST,SA,SN,RS,YU,SC,SL,SG,SI,SK,SB,SO,ZA,SS,ES,LK,SD,SR,SJ,SZ,SE,CH,SY,TW,TJ,TZ,TH,TG,TK,TO,TT,TN,TR,TM,TC,TV,UG,UA,AE,UK,US,UM,UY,SU,UZ,VU,VA,VE,VN,VG,VI,WF,EH,YE,ZM,ZW";
                            getPublications(codes);
                        }
                    }
                }
            }

            Console.WriteLine("=============================================================================");
            Console.WriteLine("Downloads complete, Check to see if any error.txt was made");
            Console.WriteLine("If it was just run the command again and see if it works.");
            Console.WriteLine("=============================================================================");

        }

        static void getPublications(string country)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json");
                string mainjson = client.DownloadString("https://d3og6tlt23zks5.cloudfront.net/magazines?ppage=1&per_page=1000000&countries=" + country + "&sort=publish_date&safe=false");
                Publications.Root publications = JsonConvert.DeserializeObject<Publications.Root>(mainjson);

                Console.WriteLine("Found: " + publications.Content.Count + " publications.");

                foreach (Publications.Content publication in publications.Content)
                {
                    Console.WriteLine("Fetching: " + publication.Title);
                    getIssues(publication.Id, null);
                }
            }
        }

        static void getIssues(string pubid, string issueid)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json");
                string json = client.DownloadString("https://d3og6tlt23zks5.cloudfront.net/magazines/" + pubid + "?ppage=1&per_page=1000000");

                Issues.Root issues = JsonConvert.DeserializeObject<Issues.Root>(json);
                //client.Headers.Clear();

                Console.WriteLine("Found: " + issues.Content.Count + " issues.");
                foreach (Issues.Content issue in issues.Content)
                {
                    if (!File.Exists(MakeValidFileName(issue.Title) + "\\" + MakeValidFileName(issue.Title) + " - " + MakeValidFileName(issue.Issue) + ".pdf"))
                    {
                        Console.WriteLine("=============================================================================");
                        Console.WriteLine("Fetching: " + issue.Title + " - " + issue.Issue);
                        getPages(issue.Id, issue.Title, issue.Issue);
                    }
                    else
                    {
                        Console.WriteLine(issue.Title + " - " + issue.Issue + "Already Exists, Skipping.");
                    }
                }

            }
        }

        static void getSingleIssue(string pubid, string issueid)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json");
                string json = client.DownloadString("https://d3og6tlt23zks5.cloudfront.net/content/" + issueid);

                SingleIssue singleissue = JsonConvert.DeserializeObject<SingleIssue>(json);
                //client.Headers.Clear();
                if (!File.Exists(MakeValidFileName(singleissue.Title) + "\\" + MakeValidFileName(singleissue.Title) + " - " + MakeValidFileName(singleissue.Issue) + ".pdf"))
                {
                    Console.WriteLine("=============================================================================");
                    Console.WriteLine("Fetching: " + singleissue.Title + " - " + singleissue.Issue);
                    getPages(singleissue.Id, singleissue.Title, singleissue.Issue);
                }
                else
                {
                    Console.WriteLine(singleissue.Title + " - " + singleissue.Issue + " Already Exists, Skipping.");
                }
            }
        }

        static void getPages(string id, string title, string issue)
        {
            title = MakeValidFileName(title);
            issue = MakeValidFileName(issue);
            Directory.CreateDirectory(title);

            using (var client = new WebClient())
            {
                client.Headers.Add("Referer", "https://go.readly.com/magazines/" + id);
                //Your own auth token needs to be entered below, you can get this from firefoxes developer tools.
                //you need to be a subscriber.
                client.Headers.Add("X-AUTH-TOKEN", auth);
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; …) Gecko/20100101 Firefox/66.0 ");
                string token = string.Empty;
                try
                {
                    token = client.DownloadString("https://api-eu.readly.com/issue/" + id + "/content/token?format=jpg&r=2400");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error downloading info, probably invalid auth token");
                    Console.WriteLine("Get a new auth code and try again.");
                    Console.WriteLine("Press enter to exit.");
                    Console.ReadLine();
                    Environment.Exit(0);
                }

                client.Headers.Add("Content-Type", "application/json");
                string issuejson = client.DownloadString("https://api-eu.readly.com/content/" + token);
                Pages pages = JsonConvert.DeserializeObject<Pages>(issuejson);

                int pagenum = 0;
                if (pages.Success == false)
                {
                    using (StreamWriter w = File.AppendText(title + "\\error.txt"))
                    {
                        w.WriteLine("ERROR DOWNLOADING: " + title + " - " + issue);
                    }
                    Console.WriteLine("ERROR DOWNLOADING PAGES FOR: " + title + " - " + issue);
                    return;
                }

                PdfDocument pdfDoc = new PdfDocument(new PdfWriter(title + "\\" + title + " - " + issue + ".pdf"));
                foreach (var page in pages.Content)
                {
                    Console.Write("\rPage: " + (pagenum + 1));
                    byte[] data = client.DownloadData(page);

                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = (byte)(data[i] ^ id[i % id.Length]);
                    }

                    Image image = new Image(ImageDataFactory.Create(data));
                    Document doc = new Document(pdfDoc, new PageSize(image.GetImageWidth(), image.GetImageHeight()));

                    image = new Image(ImageDataFactory.Create(data));
                    pdfDoc.AddNewPage(new PageSize(image.GetImageWidth(), image.GetImageHeight()));
                    image.SetFixedPosition(pagenum + 1, 0, 0);
                    doc.Add(image);

                    pagenum++;

                    if (pagenum == pages.Content.Count)
                    {
                        doc.Close();
                        Console.WriteLine("\nFinished: " + title + " - " + issue);
                    }
                }
            }
        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "");
        }
    }
}
