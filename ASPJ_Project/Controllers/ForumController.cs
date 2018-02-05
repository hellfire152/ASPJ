using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using ASPJ_Project.Context;
using System.IO;
using System.Net;
using ASPJ_Project.ViewModels;
using nClam;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using CaptchaMvc.Attributes;
using PagedList;

namespace ASPJ_Project.Controllers
{
    public class ForumController : Controller
    {
        private ForumContext db = new ForumContext();
        MySqlConnection conn;
        MySqlDataReader reader;
        string queryString;
        Database d = Database.CurrentInstance;

        // GET: Forum
        public ActionResult Home(string currentFilter, string searchString, int? page)
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var username = getUsername();
          
            List<Thread> threads = new List<Thread>();
            int pageSize = 10;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentView = searchString;
            try
            {
                if (d.OpenConnection())
                {
                    string query =
                        "SELECT thread.*," +
                        " (select COUNT(*) from comment where threadId = thread.id) AS c_count," +
                        " (SELECT COUNT(*) FROM vote v LEFT JOIN thread t ON (v.id = t.id) WHERE v.threadId = thread.id AND v.upvote = 1) - (SELECT COUNT(*) FROM vote v LEFT JOIN thread t on(v.id = t.id) where v.threadId = thread.id and v.downvote = 1) AS t_votes" +
                        " FROM thread" +
                        " GROUP BY thread.id" +
                        " ORDER BY t_votes DESC, thread.date DESC";
                    MySqlCommand c = new MySqlCommand(query, d.conn);
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            Thread thread = new Thread
                            {
                                id = ((int)r["id"]),
                                title = (r["title"].ToString()),
                                content = (r["content"].ToString()),
                                date = ((DateTime)r["date"]),
                                votes = ((long)r["t_votes"]),
                                username = (r["username"].ToString()),
                                comments = ((long)r["c_count"])
                            };
                            threads.Add(thread);

                        }

                    }
                }     
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("MySQL Error!");
                Debug.WriteLine(e.Message);
            }
            finally
            {
                d.CloseConnection();
            }
           if (!String.IsNullOrEmpty(searchString))
           {
              IEnumerable<Thread> threadss = threads.Where(t => t.title.Contains(searchString));
           }

            int pageNumber = (page ?? 1);
            return View(threads.ToPagedList(pageNumber, (int)pageSize));
        }
        public string getUsername()
        {
            
            string username = Session["uname"].ToString();
            //var username = "";
            //try
            //{
            //    if (d.OpenConnection())
            //    {
            //        string getUserQuery = "select username from users where userID = @userId";
            //        MySqlCommand cu = new MySqlCommand(getUserQuery, d.conn);
            //        cu.Parameters.AddWithValue("@userId", userID);
            //        using (MySqlDataReader r = cu.ExecuteReader())
            //        {
            //            while (r.Read())
            //            {
            //                username = r["username"].ToString();
            //            }
            //        }
            //    }
            //}
            //catch (MySqlException e)
            //{
            //    Debug.WriteLine(e.Message);
            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine(e.Message);
            //}
            //finally
            //{
            //    d.CloseConnection();
            //}
            return username;
        }
        //public ActionResult Home()
        //{
        //    Database d = Database.CurrentInstance;
        //    List<Thread> threads = new List<Thread>();

        //    try
        //    {
        //        if (d.OpenConnection())
        //        {
        //            string query = 
        //                "SELECT thread.*," +
        //                " (select COUNT(*) from comment where threadId = thread.id) AS c_count," +
        //                " (SELECT COUNT(*) FROM vote v LEFT JOIN thread t ON (v.id = t.id) WHERE v.threadId = thread.id AND v.upvote = 1) - (SELECT COUNT(*) FROM vote v LEFT JOIN thread t on(v.id = t.id) where v.threadId = thread.id and v.downvote = 1) AS t_votes" +
        //                " FROM thread" +
        //                " GROUP BY thread.id" +
        //                " ORDER BY t_votes DESC, thread.date DESC" +
        //                " LIMIT 0, 10";
        //            MySqlCommand c = new MySqlCommand(query, d.conn);
        //            using (MySqlDataReader r = c.ExecuteReader())
        //            {
        //                while (r.Read())
        //                {
        //                    Thread thread = new Thread
        //                    {
        //                        id = ((int)r["id"]),
        //                        title = (r["title"].ToString()),
        //                        content = (r["content"].ToString()),
        //                        date = ((DateTime)r["date"]),
        //                        votes = ((long)r["t_votes"]),
        //                        username = (r["username"].ToString()),
        //                        comments = ((long)r["c_count"])
        //                    };
        //                    threads.Add(thread);
        //                }
        //            }
        //        }
        //    }
        //    catch (MySqlException e)
        //    {
        //        Debug.WriteLine("MySQL Error!");
        //    }
        //    finally
        //    {
        //        d.CloseConnection();
        //    }
        //    return View(threads);
        //}
        [HttpGet]
        public ActionResult CreateThread()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetThread(int? id)
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var username = getUsername();
            Database d = Database.CurrentInstance;
            CommentViewModel viewModel = new CommentViewModel();
            List<Comment> comments = new List<Comment>();
            if (id == null)
            {
                return RedirectToAction("Home");
            }
            try
            {
                if (d.OpenConnection())
                {
                    string threadQuery = "SELECT t.id AS t_id, t.title AS t_title, t.content AS t_content, t.date AS t_date, t.username AS t_username, t.imageName, t.votes, c.id AS c_id, c.content AS c_content, c.date AS c_date, c.username AS c_username, (select count(*) FROM comment Where threadId = @id) AS c_count FROM thread t LEFT JOIN comment c ON (t.id = c.threadId) WHERE t.id = @id";
                    MySqlCommand c = new MySqlCommand(threadQuery, d.conn);
                    c.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            if ((int)r["t_id"] == id)
                            {
                                Thread thread = new Thread
                                {
                                    id = ((int)r["t_id"]),
                                    title = (r["t_title"].ToString()),
                                    content = (r["t_content"].ToString()),
                                    date = ((DateTime)r["t_date"]),
                                    imageName = (r["imageName"].ToString()),
                                    username = (r["t_username"].ToString()),
                                    upVoted = false,
                                    downVoted = false,
                                    isYou = false
                                };
                                if (username == thread.username)
                                {
                                    thread.isYou = true;
                                }
                                if ((long)r["c_count"] > 0)
                                {
                                    Comment comment = new Comment
                                    {
                                        id = ((int)r["c_id"]),
                                        content = (r["c_content"].ToString()),
                                        date = ((DateTime)r["c_date"]),
                                        username = (r["c_username"].ToString()),
                                        threadId = ((int)r["t_id"])
                                    };
                                    comments.Add(comment);
                                    Debug.WriteLine("running ");
                                };

                                viewModel.thread = thread;
                            }
                            else
                            {
                                return HttpNotFound();
                            }
                            
                        }
                        viewModel.comments = comments;
                    }
                }
                if (viewModel.thread == null)
                    return HttpNotFound();
                string voteQuery = "SELECT (SELECT COUNT(*) FROM vote v LEFT JOIN thread t on (v.id = t.id) where v.threadId = @id and v.upvote = 1) - (SELECT COUNT(*) FROM vote v LEFT JOIN thread t on (v.id = t.id) where v.threadId = @id and v.downvote = 1) AS t_votes;";
                MySqlCommand cmd = new MySqlCommand(voteQuery, d.conn);
                cmd.Parameters.AddWithValue("@id", viewModel.thread.id);
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        viewModel.thread.votes = ((long)r["t_votes"]);
                    }
                }
                string youVotedQuery = "SELECT v.upvote AS v_upvote, v.downvote AS v_downvote from vote v LEFT JOIN thread t on v.threadId = t.id where v.username = @username and v.threadId = @id";
                cmd = new MySqlCommand(youVotedQuery, d.conn);
                cmd.Parameters.AddWithValue("@id", viewModel.thread.id);
                cmd.Parameters.AddWithValue("@username", username);
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        viewModel.thread.upVoted = ((Boolean)r["v_upvote"]);
                        viewModel.thread.downVoted = ((Boolean)r["v_downvote"]);
                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                d.CloseConnection();
            }
                        
            return View(viewModel);
            //Thread thread = db.threads.Find(id);
            //if (thread == null)
            //    return HttpNotFound();
            //CommentViewModel viewModel = new CommentViewModel();
            //var comments = from c in db.comments
            //               join t in db.threads on c.threadId equals t.id
            //               where t.id == id
            //               select c;
            //viewModel.thread = thread;
            //viewModel.comments = comments.ToList();
            //return View(viewModel);
        }
        public async Task<FileScan> ScanImage(Stream File)
        {
            bool secure = false;
            string message = "empty";
            FileScan scan = new FileScan();
            scan.secure = secure;
            scan.message = message;
            try
            {
                var clam = new ClamClient("localhost", 3310);
                var scanResult = await clam.SendAndScanFileAsync(File).ConfigureAwait(false);

                switch (scanResult.Result)
                {
                    case ClamScanResults.Clean:
                        secure = true;
                        message = "File is clean";
                        break;
                    case ClamScanResults.VirusDetected:
                        secure = false;
                        message = "Virus detected, found virus: " + scanResult.InfectedFiles.First().VirusName;
                        break;
                    case ClamScanResults.Error:
                        secure = false;
                        message = "Error has occured, error: " + scanResult.RawResult;
                        break;
                }
                scan.secure = secure;
                scan.message = message;
            }
            catch
            {
                return scan;
            }
            return scan;

        }

        [HttpGet]
        public ActionResult Test()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Test(Thread thread)
        {

            HttpPostedFileBase UploadedImage = thread.image;
            thread.imageName = "";
            string FolderPath = "";
            try
            {
              
                if (ModelState.IsValid)
                {
                    if (thread.image != null)
                    {
                        var UploadedImageFileName = Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName());
                        string ext = Path.GetExtension(UploadedImage.FileName);
                        bool isValidFile = false;
                        
                            String format = "JPEG";
                            String[] formatTypes = { "tiff", "gif", "jpg", "bmp", "png", "ico", "jpeg" };
                            if (formatTypes.Contains(format.ToLower()))
                            {
                                isValidFile = true;
                            }
                            if (!isValidFile)
                            {
                                ViewBag.Message = "Invalid File. Please upload an image file ";
                                return View();
                            }
                            else
                            {
                                int fileSize = UploadedImage.ContentLength;
                                if (fileSize > 2097152)
                                {
                                    ViewBag.Message = "Maximum file size (2MB) exceeded";
                                    return View();
                                }
                                else
                                {
                                    string ImageFileName = Path.GetFileName(UploadedImageFileName) + Path.GetExtension(UploadedImage.FileName);
                                    FolderPath = Path.Combine(Server.MapPath("~/Content/UploadedImages"), ImageFileName);
                                    var fileScan = await ScanImage(thread.image.InputStream);
                                    if (fileScan.secure == false)
                                    {
                                        ViewBag.Message = fileScan.message;
                                        return View();
                                    }
                                    thread.imageName = ImageFileName;
                                    ViewBag.Message = "File uploaded successfully.";

                                }

                            }
                        }
                    }

                
                return View(thread);
            }
            catch (System.ArgumentException)
            {
                ViewBag.Message = "Invalid File. Please upload an image file ";
                return View(thread);
            }
            catch
            {
                return View();
            }

        }
        [HttpPost, CaptchaVerify("Captcha is not valid")]
        [ValidateInput(false)]
        public async Task<ActionResult> CreateThread(Thread thread)
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            ViewBag.Message = null;
            var username = getUsername();
         
            HttpPostedFileBase UploadedImage = thread.image;
            thread.imageName = "";
            string FolderPath = "";
            try
            {
                //if (ModelState["CaptchaInputText"].ToString() != null || ModelState["CaptchaInputText"].Errors.Count > 0)
                //{
                //    ViewBag.CaptchaError = "Invalid Captcha";
                //}
                if (ModelState.IsValid)
                {
                    if (thread.image != null)
                    {
                        var UploadedImageFileName = Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName());
                        string ext = Path.GetExtension(UploadedImage.FileName);
                        bool isValidFile = false;
                        if (UploadedImage.ContentLength > 0)
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(thread.image.InputStream);
                            string format = string.Empty;
                            if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Tiff.Guid)
                                format = "TIFF";
                            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid)
                                format = "GIF";
                            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                                format = "JPG";
                            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Bmp.Guid)
                                format = "BMP";
                            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid)
                                format = "PNG";
                            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Icon.Guid)
                                format = "ICO";
                            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                                format = "JPEG";
                            else
                                throw new ArgumentException();
                            //String format = "JPEG";
                            String[] formatTypes = { "tiff", "gif", "jpg", "bmp", "png", "ico", "jpeg" };
                            if (formatTypes.Contains(format.ToLower()))
                            {
                                isValidFile = true;
                            }
                            if (!isValidFile)
                            {
                                ViewBag.Message = "Invalid File. Please upload an image file ";
                                return View();
                            }
                            else
                            {
                                int fileSize = UploadedImage.ContentLength;
                                if (fileSize > 2097152)
                                {
                                    ViewBag.Message = "Maximum file size (2MB) exceeded";
                                    return View();
                                }
                                else
                                {
                                    string ImageFileName = Path.GetFileName(UploadedImageFileName) + Path.GetExtension(UploadedImage.FileName);
                                    FolderPath = Path.Combine(Server.MapPath("~/Content/UploadedImages"), ImageFileName);
                                    var fileScan = await ScanImage(thread.image.InputStream);
                                    if (fileScan.secure == false)
                                    {
                                        ViewBag.Message = fileScan.message;
                                        return View();
                                    }
                                    thread.imageName = ImageFileName;
                                    ViewBag.Message = "File uploaded successfully.";

                                }

                            }
                        }
                    }
                    try
                    {
                        if (d.OpenConnection())
                        {
                            thread.date = DateTime.Now;
                            thread.votes = 0;
                            thread.username = username;
                            MySqlCommand cmd = new MySqlCommand(queryString, d.conn);
                            queryString = "INSERT INTO thread(title, content, date, votes, imageName, username) VALUES(@title, @content, @date, @votes, @imageName, @username)";
                            cmd.CommandText = queryString;
                            cmd.Parameters.AddWithValue("@title", thread.title);
                            cmd.Parameters.AddWithValue("@content", thread.content);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@votes", thread.votes);
                            cmd.Parameters.AddWithValue("@username", thread.username);
                            cmd.Parameters.AddWithValue("@imageName", thread.imageName);
                            cmd.ExecuteNonQuery();
                            if(UploadedImage != null)
                            {
                                UploadedImage.SaveAs(FolderPath);
                            }

                            cmd = new MySqlCommand(queryString, d.conn);
                        }

                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = e.Message;
                        return View();
                    }
                    finally
                    {
                        d.CloseConnection();
                    }
                    //db.threads.Add(thread);
                    //db.SaveChanges();
                    return RedirectToAction("Home");


                }
                return View(thread);
            }
            catch (System.ArgumentException)
            {
                ViewBag.Message = "Invalid File. Please upload an image file ";
                return View(thread);
            }
            catch
            {
                return View();
            }

        }
        //Get: /Forum/DeleteThread/
        [HttpGet]
        public ActionResult DeleteThread(int? id)
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var username = getUsername();

            Database d = Database.CurrentInstance;
            Thread thread = new Thread();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            try
            {
                if (d.OpenConnection())
                {
                    string findThreadQuery = "SELECT * from thread where id = @id";
                    MySqlCommand c = new MySqlCommand(findThreadQuery, d.conn);
                    c.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                thread = new Thread
                                {
                                    id = ((int)r["id"]),
                                    title = (r["title"].ToString()),
                                    content = (r["content"].ToString()),
                                    date = ((DateTime)r["date"]),
                                    username = (r["username"].ToString()),
                                };
                            }
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                    string ownThreadQuery = "SELECT * from thread where id = @id and username = @username";
                    c = new MySqlCommand(ownThreadQuery, d.conn);
                    c.Parameters.AddWithValue("@id", id);
                    c.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                thread = new Thread
                                {
                                    id = ((int)r["id"]),
                                    title = (r["title"].ToString()),
                                    content = (r["content"].ToString()),
                                    date = ((DateTime)r["date"]),
                                    username = (r["username"].ToString())
                                };
                            }
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                        }

                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                d.CloseConnection();
            }
            return View(thread);
        }

        //Post: /Forum/DeleteThread/
        [HttpPost, ActionName("DeleteThread")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteThreadConfirmed(int? id)
        {
            CommentViewModel viewModel = new CommentViewModel();
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var username = getUsername();
           
            Database d = Database.CurrentInstance;
            Thread thread = new Thread();
            try
            {
                if (d.OpenConnection())
                {
                    string findThreadQuery = "SELECT * from thread where id = @id";
                    MySqlCommand c = new MySqlCommand(findThreadQuery, d.conn);
                    c.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                thread = new Thread
                                {
                                    id = ((int)r["id"]),
                                    content = (r["content"].ToString()),
                                    date = ((DateTime)r["date"]),
                                    username = (r["username"].ToString()),
                                    imageName = (r["imageName"].ToString())
                                };
                            }
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                    string query = "DELETE from thread where username = @username and id = @id";
                    c = new MySqlCommand(query, d.conn);
                    c.Parameters.AddWithValue("@username", username);
                    c.Parameters.AddWithValue("@id", id);
                    if (c.ExecuteNonQuery() == 1)
                    {
                        if (thread.imageName != "")
                        {
                            string folderpath = Path.Combine(Server.MapPath("~/content/uploadedimages"), thread.imageName);
                            if (System.IO.File.Exists(folderpath))
                            {
                                System.IO.File.Delete(folderpath);
                            }
                        }
                    } else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                    }

                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);

            }
            finally
            {
                d.CloseConnection();
            }
 

            return RedirectToAction("MyThreads");
        }

        [HttpGet]
        public ActionResult MyThreads()
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var username = getUsername();
           
            Database d = Database.CurrentInstance;
            List<Thread> threads = new List<Thread>();
            Thread thread = new Thread();
            try
            {
                if (d.OpenConnection())
                {
                    string threadQuery = "SELECT thread.*, (select COUNT(*) from comment where threadId = thread.id) AS c_count, (SELECT COUNT(*) FROM vote v LEFT JOIN thread t ON (v.id = t.id) WHERE v.threadId = thread.id AND v.upvote = 1 and t.username = @username) - (SELECT COUNT(*) FROM vote v LEFT JOIN thread t on(v.id = t.id) where v.threadId = thread.id and v.downvote = 1 and t.username = @username) AS t_votes FROM thread WHERE thread.username = @username GROUP BY thread.id ORDER BY t_votes DESC, thread.date DESC";
                    MySqlCommand c = new MySqlCommand(threadQuery, d.conn);
                    c.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {

                            thread = new Thread
                            {
                                id = ((int)r["id"]),
                                title = (r["title"].ToString()),
                                content = (r["content"].ToString()),
                                username = (r["username"].ToString()),
                                votes = ((long)r["t_votes"]),
                                comments = ((long)r["c_count"]),
                                date = ((DateTime)r["date"]),
                                imageName = (r["imageName"].ToString())
                            };

                                
                            threads.Add(thread);
                            
                  
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                d.CloseConnection();
            }
            return View(threads);
            //return View(db.threads.ToList());
        }
        [HttpGet]
        public ActionResult MyComments()
        {
            //set username cookie
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var username = getUsername();
           
            Database d = Database.CurrentInstance;
            List<Comment> comments = new List<Comment>();
            Comment comment = new Comment();
            try
            {
                if (d.OpenConnection())
                {
                    
                    string commentQuery = "SELECT t.id AS t_id, t.title AS t_title, t.imageName, t.content AS t_content, t.votes, t.username AS t_username, t.date AS t_date, c.id AS c_id, c.content AS c_content, c.date AS c_date, c.username AS c_username, (select count(*) FROM comment) AS c_count FROM comment c LEFT JOIN thread t ON (t.id = c.threadId) WHERE c.username = @username ORDER BY c.date DESC";
                    MySqlCommand c = new MySqlCommand(commentQuery, d.conn);
                    c.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                           
                            comment = new Comment
                            {
                                id = ((int)r["c_id"]),
                                content = (r["c_content"].ToString()),
                                date = ((DateTime)r["c_date"]),
                                username = (r["c_username"].ToString()),
                                threadId = ((int)r["t_id"]),
                                thread = new Thread
                                {
                                    id = ((int)r["t_id"]),
                                    title = (r["t_title"].ToString()),
                                    content = (r["t_content"].ToString()),
                                    username = (r["t_username"].ToString()),
                                    votes = ((int)r["votes"]),
                                    date = ((DateTime)r["t_date"]),
                                    imageName = (r["imageName"].ToString())
                                }

                            };
                            comments.Add(comment);

                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                d.CloseConnection();
            }
            //return View(db.comments.ToList());
            return View(comments);
        }
  
        [HttpPost, ActionName("GetThread")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Comment(int? id, CommentingViewModel comment)
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var username = getUsername();
           
            Thread thread = new Thread();
            Database d = Database.CurrentInstance;
            try
            {

                if (ModelState.IsValid)
                {

                    if (id == null)
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                    //Thread thread = db.threads.Find(id);
                    try
                    {
                        if (d.OpenConnection())
                        {
                            string threadQuery = "SELECT * from thread where id = @id";
                            MySqlCommand c = new MySqlCommand(threadQuery, d.conn);
                            c.Parameters.AddWithValue("@id", id);
                            using (MySqlDataReader r = c.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                  
                                    thread = new Thread
                                    {
                                        id = ((int)r["id"]),
                                        title = (r["title"].ToString()),
                                        content = (r["content"].ToString()),
                                        date = ((DateTime)r["date"]),
                                        votes = ((int)r["votes"]),
                                        imageName = (r["imageName"].ToString()),
                                        username = (r["username"].ToString())
                                    };


                                }

                            }
                            if (thread == null)
                                return HttpNotFound();
                            MySqlCommand cmd = new MySqlCommand(queryString, d.conn);
                            queryString = "INSERT INTO dububase.comment(content, date, username, threadId) VALUES(@content, @date, @username, @threadId)";
                            cmd.CommandText = queryString;
                            cmd.Parameters.AddWithValue("@content", comment.content);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@threadId", thread.id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (MySqlException e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    finally
                    {
                        d.CloseConnection();
                    }
                   
                    return RedirectToAction("GetThread", new { id = id});

                }
                

                return GetThread(id);

            }
            catch
            {
                return GetThread(id);
            }
        }
        [HttpGet]
        public ActionResult DeleteComment(int? id)
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var username = getUsername();
          
            CommentViewModel viewModel = new CommentViewModel();
            Database d = Database.CurrentInstance;
            Comment comment = new Comment();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //Comment comment = db.comments.Find(id);
            try
            {
                if (d.OpenConnection())
                {
                    string findCommentQuery = "SELECT * from dububase.comment where id = @id";
                    MySqlCommand c = new MySqlCommand(findCommentQuery, d.conn);
                    c.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                comment = new Comment
                                {
                                    id = ((int)r["id"]),
                                    content = (r["content"].ToString()),
                                    date = ((DateTime)r["date"]),
                                    username = (r["username"].ToString()),
                                    threadId = ((int)r["threadId"])
                                };
                                viewModel.comment = comment;
                            }
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                    string commentQuery = "SELECT * from dububase.comment where id = @id and username = @username";
                    c = new MySqlCommand(commentQuery, d.conn);
                    c.Parameters.AddWithValue("@id", id);
                    c.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                comment = new Comment
                                {
                                    id = ((int)r["id"]),
                                    content = (r["content"].ToString()),
                                    date = ((DateTime)r["date"]),
                                    username = (r["username"].ToString()),
                                    threadId = ((int)r["threadId"])
                                };     
                            }
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                        }

                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                d.CloseConnection();
            }
            if (comment.content == null)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            return View(comment);
        }
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCommentConfirmed(int? id)
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var username = getUsername();
            
            Database d = Database.CurrentInstance;
            Comment comment = new Comment();
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //comment = db.comments.Find(id);
                try
                {
                    if (d.OpenConnection())
                    {
                        string commentQuery = "SELECT * from dububase.comment where id = @id and username = @username";
                        MySqlCommand c = new MySqlCommand(commentQuery, d.conn);
                        c.Parameters.AddWithValue("@id", id);
                        c.Parameters.AddWithValue("@username", username);

                        using (MySqlDataReader r = c.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                if ((int)r["id"] == id)
                                {
                                    comment = new Comment
                                    {
                                        id = ((int)r["id"]),
                                        content = (r["content"].ToString()),
                                        date = ((DateTime)r["date"]),
                                        username = (r["username"].ToString()),
                                        threadId = ((int)r["threadId"])
                                    };
                                }
                                else
                                {
                                    return HttpNotFound();
                                }
                            }
                        }
                        if (comment.content == null)
                            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                        MySqlCommand cmd = new MySqlCommand(queryString, d.conn);
                        queryString = "DELETE FROM dububase.comment where id=@id and username = @username";
                        cmd.CommandText = queryString;
                        cmd.Parameters.AddWithValue("@id", comment.id);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException e)
                {
                    Debug.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                finally
                {
                    d.CloseConnection();
                }
            
                //db.comments.Remove(comment);
                //db.SaveChanges();
                return RedirectToAction("MyComments");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upvote(int? id)
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var username = getUsername();
          
            Thread thread = new Thread();
            Vote vote = new Vote();
            Database d = Database.CurrentInstance;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Thread thread = db.threads.Find(id);
            try
            {
                if (d.OpenConnection())
                {
                    string threadQuery = "SELECT * from thread where id = @id";
                    MySqlCommand c = new MySqlCommand(threadQuery, d.conn);
                    c.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            if ((int)r["id"] == id)
                            {
                                thread = new Thread
                                {
                                    id = ((int)r["id"]),
                                    title = (r["title"].ToString()),
                                    content = (r["content"].ToString()),
                                    date = ((DateTime)r["date"]),
                                    votes = ((int)r["votes"]),
                                    imageName = (r["imageName"].ToString()),
                                    username = (r["username"].ToString())
                                };

                            }
                            else
                            {
                                return HttpNotFound();
                            }

                        }

                    }
                    if (thread == null)
                        return HttpNotFound();
                    string voteQuery = "SELECT v.id AS V_id, v.upvote as v_upvote, v.downvote AS v_downvote, v.username AS v_username, v.threadId as v_threadId from vote v LEFT JOIN thread t ON v.threadId = t.id WHERE v.username = @username and v.threadId = @id";
                    c = new MySqlCommand(voteQuery, d.conn);
                    c.Parameters.AddWithValue("@username", username);
                    c.Parameters.AddWithValue("@id", thread.id);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                         
                            vote = new Vote
                            {
                                id = ((int)r["v_id"]),
                                upvote = ((Boolean)r["v_upvote"]),
                                downvote = ((Boolean)r["v_downvote"]),
                                username = (r["v_username"].ToString()),
                                threadId = ((int)r["v_threadId"])
                            };
                            
                        }
                    }

                    MySqlCommand cmd = new MySqlCommand(queryString, d.conn);
                    if (vote.upvote == false && vote.downvote == false)
                    {
                        queryString = "INSERT INTO dububase.vote(upvote, username, threadId ) VALUES(@upvote, @username, @threadId)";
                        cmd.CommandText = queryString;
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@upvote", 1);
                        cmd.Parameters.AddWithValue("@threadId", thread.id);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        queryString = "DELETE FROM vote where username = @username and threadId = @threadId";
                        cmd.CommandText = queryString;
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@threadId", thread.id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return RedirectToAction("GetThread", new { id = id });
            }
            finally
            {
                d.CloseConnection();
            }
            return RedirectToAction("GetThread", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Downvote(int? id)
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var username = getUsername();
            
            Thread thread = new Thread();
            Vote vote = new Vote();
            Database d = Database.CurrentInstance;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Thread thread = db.threads.Find(id);
            try
            {
                if (d.OpenConnection())
                {
                    string threadQuery = "SELECT * from thread where id = @id";
                    MySqlCommand c = new MySqlCommand(threadQuery, d.conn);
                    c.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            if ((int)r["id"] == id)
                            {
                                thread = new Thread
                                {
                                    id = ((int)r["id"]),
                                    title = (r["title"].ToString()),
                                    content = (r["content"].ToString()),
                                    date = ((DateTime)r["date"]),
                                    votes = ((int)r["votes"]),
                                    imageName = (r["imageName"].ToString()),
                                    username = (r["username"].ToString())
                                };

                            }
                            else
                            {
                                return HttpNotFound();
                            }

                        }

                    }
                    if (thread == null)
                        return HttpNotFound();
                    string voteQuery = "SELECT v.id AS V_id, v.upvote as v_upvote, v.downvote AS v_downvote, v.username AS v_username, v.threadId as v_threadId from vote v LEFT JOIN thread t ON v.threadId = t.id WHERE v.username = @username and v.threadId = @id";
                    c = new MySqlCommand(voteQuery, d.conn);
                    c.Parameters.AddWithValue("@username", username);
                    c.Parameters.AddWithValue("@id", thread.id);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {

                            vote = new Vote
                            {
                                id = ((int)r["v_id"]),
                                upvote = ((Boolean)r["v_upvote"]),
                                downvote = ((Boolean)r["v_downvote"]),
                                username = (r["v_username"].ToString()),
                                threadId = ((int)r["v_threadId"])
                            };

                        }
                    }

                    MySqlCommand cmd = new MySqlCommand(queryString, d.conn);
                    if (vote.upvote == false && vote.downvote == false)
                    {
                        queryString = "INSERT INTO dububase.vote(downvote, username, threadId ) VALUES(@downvote, @username, @threadId)";
                        cmd.CommandText = queryString;
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@downvote", 1);
                        cmd.Parameters.AddWithValue("@threadId", thread.id);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        queryString = "DELETE FROM vote where username = @username and threadId = @threadId";
                        cmd.CommandText = queryString;
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@threadId", thread.id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return RedirectToAction("GetThread", new { id = id });
            }
            finally
            {
                d.CloseConnection();
            }
            return RedirectToAction("GetThread", new { id = id });
        }

    }
}