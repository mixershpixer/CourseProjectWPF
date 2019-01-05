namespace MixLib
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Books : DbContext
    {
        // Контекст настроен для использования строки подключения "Books" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "MixLib.Books" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "Books" 
        // в файле конфигурации приложения.
        public Books()
            : base("name=Books")
        {
        }

        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Book> MyBooks { get; set; }
    }

    public class Book
    {
        public int Id { get; set; }
        public string BookName{ get; set; }
        public string Author { get; set; }
        public int GodIzdaniya { get; set; }
        public string Country { get; set; }
        public string Genre { get; set; }
        public string Discription { get; set; }
        public string Picture { get; set; }
        public Book()
        {

        }
        public Book(int _Id, string _BookName, string _Author, int _GodIzdaniya, string _Country, string _Discription, string _Picture, string _Genre)
        {
            Id = _Id;
            BookName = _BookName;
            Author = _Author;
            GodIzdaniya = _GodIzdaniya;
            Country = _Country;
            Discription = _Discription;
            Picture = _Picture;
            Genre = _Genre;
        }
    }
}