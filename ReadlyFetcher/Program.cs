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
    partial class Program
    {
        public static string auth = "";
        public static string outtype = "img";
        public static string issueformat = "issue";
        public static bool supress = false;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine();
                Console.WriteLine("====================== No arguments found! ======================");
                Console.WriteLine();
                Console.WriteLine("=============================================================================");
                Console.WriteLine("-a=TOKEN - For your auth token. you need a valid subscription. REQUIRED");
                Console.WriteLine("https://i.imgur.com/UQZyqKT.png");
                Console.WriteLine("=============================================================================");
                Console.WriteLine();
                Console.WriteLine("=============================================================================");
                Console.WriteLine("-i=ISSUEID - For the ID of a specific issue.");
                Console.WriteLine("-p=PUBLICATIONID - For the ID of a mag and all issues.");
                Console.WriteLine("IDs can be found by going to the url of the mag, for example, ");
                Console.WriteLine("https://go.readly.com/magazines/PUBID/ISSUEID/1");
                Console.WriteLine("=============================================================================");
                Console.WriteLine();
                Console.WriteLine("=============================================================================");
                //Console.WriteLine("all - will download ALL mags, this is dumb and probably should be avoided.");
                Console.WriteLine("-t=pdf|img - Used to select the output format, default img.");
                Console.WriteLine("-m or -n is to signify the publication type. m=mag n=newspaper");
                Console.WriteLine("-s is to supress error / already exists messages");
                Console.WriteLine("-d=issue|pub - Specifies the issue name format. issue is readlys issue number");
                Console.WriteLine("and pub is the date the publication was available.");
                Console.WriteLine("=============================================================================");
                Console.WriteLine("Press return/enter to exit.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            else
            {
                List<string> publications = new List<string>();
                List<string> issues = new List<string>();
                string pubtype = "mag";
                foreach (var argument in args)
                {
                    if (argument.StartsWith("-a="))
                    {
                        auth = argument.Split('=')[1];
                    }
                    else if(argument.StartsWith("-p="))
                    {
                        publications.Add(argument.Split('=')[1]);
                    }
                    else if (argument.StartsWith("-i="))
                    {
                        issues.Add(argument.Split('=')[1]);
                    }
                    else if (argument.StartsWith("-t="))
                    {
                        outtype = argument.Split('=')[1];
                    }
                    else if (argument == "-m")
                    {
                        pubtype = "mag";
                    }
                    else if (argument == "-n")
                    {
                        pubtype = "paper";
                    }
                    else if (argument == "-s")
                    {
                        supress = true;
                    }
                    else if (argument.StartsWith("-d="))
                    {
                        issueformat = argument.Split('=')[1];
                    }
                    else if (argument == "allnews")
                    {
                        Console.WriteLine("Downloading all mags... Good luck, your gonna need it.");
                        string codes = "AF,AX,AL,DZ,AS,AD,AO,AI,AQ,AG,AR,AM,AW,AC,AU,AT,AZ,BS,BH,BB,BD,BY,BE,BZ,BJ,BM,BT,BW,BO,BA,BV,BR,IO,BN,BG,BF,BI,KH,CM,CA,CV,KY,CF,TD,CL,CN,CX,CC,CO,KM,CG,CD,CK,CR,CI,HR,CU,CY,CZ,DK,DJ,DM,DO,TP,EC,EG,SV,GQ,ER,EE,ET,EU,FK,FO,FJ,FI,FR,FX,GF,PF,TF,MK,GA,GM,GE,DE,GH,GI,GB,GR,GL,GD,GP,GU,GT,GG,GN,GW,GY,HT,HM,HN,HK,HU,IS,IN,ID,IR,IQ,IE,IL,IM,IT,JE,JM,JP,JO,KZ,KE,KI,KP,KR,XK,KW,KG,LA,LV,LB,LI,LR,LY,LS,LT,LU,MO,MG,MW,MY,MV,ML,MT,MH,MQ,MR,MU,YT,MX,FM,MC,MD,MN,ME,MS,MA,MZ,MM,NA,NR,NP,NL,AN,NT,NC,NZ,NI,NE,NG,NU,NF,MP,NO,OM,PK,PW,PS,PA,PG,PY,PE,PH,PN,PL,PT,PR,QA,RE,RO,RU,RW,GS,SH,KN,LC,MF,VC,WS,SM,ST,SA,SN,RS,YU,SC,SL,SG,SI,SK,SB,SO,ZA,SS,ES,LK,SD,SR,SJ,SZ,SE,CH,SY,TW,TJ,TZ,TH,TG,TK,TO,TT,TN,TR,TM,TC,TV,UG,UA,AE,UK,US,UM,UY,SU,UZ,VU,VA,VE,VN,VG,VI,WF,EH,YE,ZM,ZW";
                        getAllNewspapers(codes);
                    }
                    else if (argument == "allmags")
                    {
                        Console.WriteLine("Downloading all mags... Good luck, your gonna need it.");
                        string codes = "AF,AX,AL,DZ,AS,AD,AO,AI,AQ,AG,AR,AM,AW,AC,AU,AT,AZ,BS,BH,BB,BD,BY,BE,BZ,BJ,BM,BT,BW,BO,BA,BV,BR,IO,BN,BG,BF,BI,KH,CM,CA,CV,KY,CF,TD,CL,CN,CX,CC,CO,KM,CG,CD,CK,CR,CI,HR,CU,CY,CZ,DK,DJ,DM,DO,TP,EC,EG,SV,GQ,ER,EE,ET,EU,FK,FO,FJ,FI,FR,FX,GF,PF,TF,MK,GA,GM,GE,DE,GH,GI,GB,GR,GL,GD,GP,GU,GT,GG,GN,GW,GY,HT,HM,HN,HK,HU,IS,IN,ID,IR,IQ,IE,IL,IM,IT,JE,JM,JP,JO,KZ,KE,KI,KP,KR,XK,KW,KG,LA,LV,LB,LI,LR,LY,LS,LT,LU,MO,MG,MW,MY,MV,ML,MT,MH,MQ,MR,MU,YT,MX,FM,MC,MD,MN,ME,MS,MA,MZ,MM,NA,NR,NP,NL,AN,NT,NC,NZ,NI,NE,NG,NU,NF,MP,NO,OM,PK,PW,PS,PA,PG,PY,PE,PH,PN,PL,PT,PR,QA,RE,RO,RU,RW,GS,SH,KN,LC,MF,VC,WS,SM,ST,SA,SN,RS,YU,SC,SL,SG,SI,SK,SB,SO,ZA,SS,ES,LK,SD,SR,SJ,SZ,SE,CH,SY,TW,TJ,TZ,TH,TG,TK,TO,TT,TN,TR,TM,TC,TV,UG,UA,AE,UK,US,UM,UY,SU,UZ,VU,VA,VE,VN,VG,VI,WF,EH,YE,ZM,ZW";
                        getAllMagazines(codes);
                    }
                }

                if (auth == "")
                {
                    Console.WriteLine("No auth code found!!");
                    Console.WriteLine("make sure you use an auth code with -a=");
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else
                {
                    foreach (var pub in publications)
                    {
                        if (pubtype == "mag")
                        {
                            getMagazineIssues(pub);
                        }
                        else if(pubtype == "paper")
                        {
                            getNewspaperIssues(pub);
                        }
                    }

                    foreach (var issue in issues)
                    {
                        getSingleIssue(issue);
                    }
                }
            }

            if (supress == false)
            {
                Console.WriteLine("=============================================================================");
                Console.WriteLine("Downloads complete, Check to see if any error.txt was made");
                Console.WriteLine("If it was just run the command again and see if it works.");
                Console.WriteLine("=============================================================================");
            }
        }

        static void getSingleIssue(string issueid)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/json");
                string json = client.DownloadString("https://d3og6tlt23zks5.cloudfront.net/content/" + issueid);

                SingleIssue singleissue = JsonConvert.DeserializeObject<SingleIssue>(json);

                string issue = "";
                if (issueformat == "issue")
                {
                    if (singleissue.Issue == null)
                    {
                        issue = singleissue.PublishDate.ToString(@"yyyy-MM-dd");
                    }
                    else
                    {
                        issue = singleissue.Issue;
                    }
                }
                else
                {
                    issue = singleissue.PublishDate.ToString(@"yyyy-MM-dd");
                }

                Console.WriteLine("=============================================================================");
                Console.WriteLine("Fetching: " + singleissue.Title + " - " + issue);
                if (outtype == "pdf")
                {
                    GetPDF(singleissue.Id, singleissue.Title, issue);
                }
                else
                {
                    getImg(singleissue.Id, singleissue.Title, issue);
                }
            }
        }

        static void getImg(string id, string title, string issue)
        {
            title = MakeValidFileName(title);
            issue = MakeValidFileName(issue);
            Directory.CreateDirectory(title);

            using (var client = new WebClient())
            {
                client.Headers.Add("X-AUTH-TOKEN", auth);
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; …) Gecko/20100101 Firefox/66.0 ");
                string token = string.Empty;
                try
                {
                    token = client.DownloadString("https://api-eu.readly.com/issue/" + id + "/content/token?format=jpg&r=2400");
                }
                catch (Exception)
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

                int pagenum = 1;
                if (pages.Success == false)
                {
                    using (StreamWriter w = File.AppendText(title + "\\error.txt"))
                    {
                        if (supress == false)
                        {
                            w.WriteLine("ERROR DOWNLOADING: " + title + " - " + issue);
                        }
                    }
                    if (supress == false)
                    {
                        Console.WriteLine("ERROR DOWNLOADING PAGES FOR: " + title + " - " + issue);
                    }
                    return;
                }
                Console.WriteLine("Total Pages: " + pages.Content.Count);
                foreach (var page in pages.Content)
                {
                    string filename = title + "\\" + issue + "\\" + title + " - " + issue + " - page-" + pagenum.ToString("000") + ".jpg";
                    if (!File.Exists(filename))
                    {
                        if (supress == false)
                        {
                            Console.Write("\r" + new String(' ', Console.BufferWidth - 1));
                        }
                        Console.Write("\rPage: " + (pagenum));
                        byte[] data = client.DownloadData(page);

                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (byte)(data[i] ^ id[i % id.Length]);
                        }

                        Directory.CreateDirectory(title + "\\" + issue);
                        File.WriteAllBytes(filename, data);

                    }
                    else
                    {
                        if (supress == false)
                        {
                            Console.Write("\rPage: " + filename + " - Already exists!");
                        }
                    }

                    if (pagenum == pages.Content.Count)
                    {
                        Console.WriteLine("\nFinished: " + title + " - " + issue);
                    }

                    pagenum++;
                }
            }
        }

        static void GetPDF(string id, string title, string issue)
        {
            title = MakeValidFileName(title);
            issue = MakeValidFileName(issue);
            Directory.CreateDirectory(title);

            string filename = title + "\\" + title + " - " + issue + ".pdf";

            if (!File.Exists(filename))
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("X-AUTH-TOKEN", auth);
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; …) Gecko/20100101 Firefox/66.0 ");
                    string token = string.Empty;
                    try
                    {
                        token = client.DownloadString("https://api-eu.readly.com/issue/" + id + "/content/token?format=jpg&r=2400");
                    }
                    catch (Exception)
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

                    int pagenum = 1;
                    if (pages.Success == false)
                    {
                        using (StreamWriter w = File.AppendText(title + "\\error.txt"))
                        {
                            if (supress == false)
                            {
                                w.WriteLine("ERROR DOWNLOADING: " + title + " - " + issue);
                            }
                        }
                        if (supress == false)
                        {
                            Console.WriteLine("ERROR DOWNLOADING PAGES FOR: " + title + " - " + issue);
                        }
                        return;
                    }

                    PdfDocument pdfDoc = new PdfDocument(new PdfWriter(title + "\\" + title + " - " + issue + ".pdf"));
                    Console.WriteLine("Total Pages: " + pages.Content.Count);
                    foreach (var page in pages.Content)
                    {
                        Console.Write("\rPage: " + (pagenum));
                        byte[] data = client.DownloadData(page);

                        for (int i = 0; i < data.Length; i++)
                        {
                            data[i] = (byte)(data[i] ^ id[i % id.Length]);
                        }

                        Image image = new Image(ImageDataFactory.Create(data));
                        Document doc = new Document(pdfDoc, new PageSize(image.GetImageWidth(), image.GetImageHeight()));

                        image = new Image(ImageDataFactory.Create(data));
                        pdfDoc.AddNewPage(new PageSize(image.GetImageWidth(), image.GetImageHeight()));
                        image.SetFixedPosition(pagenum, 0, 0);
                        doc.Add(image);

                        if (pagenum == pages.Content.Count)
                        {
                            doc.Close();
                            Console.WriteLine("\nFinished: " + title + " - " + issue);
                        }

                        pagenum++;
                    }
                }
            }
            else
            {
                if (supress == false)
                {
                    Console.WriteLine(filename + " - Already exists!");
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
