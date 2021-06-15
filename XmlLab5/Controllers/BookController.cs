using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;


namespace XmlLab5.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            IList<Models.Book> bookList = new List<Models.Book>();
            
            string path = Request.PathBase + "App_Data/books.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList books = doc.GetElementsByTagName("book");

                foreach (XmlElement b in books)
                {
                    Models.Book book = new Models.Book();
                    book.id = b.GetElementsByTagName("id")[0].InnerText;
                    book.title = b.GetElementsByTagName("title")[0].InnerText;
                    //book.authorTitle = b.Attributes["title"].InnerText;
                    book.firstname = b.GetElementsByTagName("firstname")[0].InnerText;
                    
                    book.middlename = "";
                    if(b.GetElementsByTagName("middlename")[0] != null)
                    {
                        book.middlename = b.GetElementsByTagName("middlename")[0].InnerText;
                    }

                    book.lastname = b.GetElementsByTagName("lastname")[0].InnerText;

                    bookList.Add(book);
                }
            }

            return View(bookList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var book = new Models.Book();
            return View(book);
        }


        [HttpPost]
        public IActionResult Create(Models.Book b)
        {
            string path = Request.PathBase + "App_Data/books.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {

                doc.Load(path);

                XmlElement book = _CreateBookElement(doc, b);

                doc.DocumentElement.AppendChild(book);
            }
            else
            {
                XmlNode dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(dec);
                XmlNode root = doc.CreateElement("books");


                //create new book
                XmlElement book = _CreateBookElement(doc, b);
                root.AppendChild(book);
                doc.AppendChild(root);

            }

            doc.Save(path);

            return View();
        }

        private XmlElement _CreateBookElement(XmlDocument doc, Models.Book newBook)
        {
            XmlElement book = doc.CreateElement("book");

            XmlNodeList lastId = doc.SelectNodes("//book/id");
            Debug.WriteLine("The last id is " + lastId);

            int newId = lastId.Count;
            newId++;
            Debug.WriteLine("The new id is " + newId);
            
            XmlNode id = doc.CreateElement("id");
            string idText = newId.ToString();
            id.InnerText = idText;


            XmlNode title = doc.CreateElement("title");
            title.InnerText = newBook.title;

            XmlNode author = doc.CreateElement("author");
            XmlAttribute authorTitle = doc.CreateAttribute("title");
            authorTitle.Value = newBook.authorTitle;
            author.Attributes.Append(authorTitle);

            XmlNode firstname = doc.CreateElement("firstname");
            firstname.InnerText = newBook.firstname;
            XmlNode middlename = doc.CreateElement("middlename");
            middlename.InnerText = newBook.middlename;
            XmlNode lastname = doc.CreateElement("lastname");
            lastname.InnerText = newBook.lastname;

            author.AppendChild(firstname);
            author.AppendChild(middlename);
            author.AppendChild(lastname);

            book.AppendChild(id);
            book.AppendChild(title);
            book.AppendChild(author);

            return book;
        }

    }
}
