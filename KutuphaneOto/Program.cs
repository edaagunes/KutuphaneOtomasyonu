using System;
using System.Collections.Generic;
using System.IO;

// Kitap sınıfı
class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int TotalCopies { get; set; }
    public int BorrowedCopies { get; set; }
    public DateTime DueDate { get; set; }


    public override string ToString()
    {
        return $"{Title},{Author},{ISBN},{TotalCopies},{BorrowedCopies},{DueDate.ToString("dd.MM.yyyy")}";
    }

    public static Book Parse(string line)
    {
        string[] parts = line.Split(',');

        return new Book
        {
            Title = parts[0],
            Author = parts[1],
            ISBN = parts[2],
            TotalCopies = int.Parse(parts[3]),
            BorrowedCopies = int.Parse(parts[4]),
            DueDate = DateTime.ParseExact(parts[5], "dd.MM.yyyy", null)

        };
    }

}

    // Kütüphane sınıfı
    class Library
    {
        private List<Book> books;
        private string filePath = "librarydata.txt";

        public Library()
        {
            books = new List<Book>();
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    books.Add(Book.Parse(line));
                }
            }
        }

        private void SaveData()
        {
            List<string> lines = new List<string>();

            foreach (Book book in books)
            {
                lines.Add(book.ToString());
            }
            File.WriteAllLines(filePath, lines);
        }

        public void AddBook(Book book)
        {

            books.Add(book);
            SaveData();
            Console.WriteLine($"{book.Title} başlıklı kitap eklenmiştir. ");


        }

        public void DisplayAllBooks()
        {
            Console.WriteLine("Tüm Kitaplar:");
            foreach (var book in books)
            {
                Console.WriteLine($"Başlık: {book.Title}, Yazar: {book.Author}, ISBN: {book.ISBN}, Toplam Kopya: {book.TotalCopies}, Ödünç Alınan Kopya: {book.BorrowedCopies}");
            }
            Console.WriteLine();
        }

        public void SearchBook(string keyword)
        {
            var result = books.FindAll(book => book.Title.Contains(keyword) || book.Author.Contains(keyword));
            if (result.Count > 0)
            {
                Console.WriteLine("Arama Sonuçları:");
                foreach (var book in result)
                {
                    Console.WriteLine($"Başlık: {book.Title}, Yazar: {book.Author}, ISBN: {book.ISBN}, Toplam Kopya: {book.TotalCopies}, Ödünç Alınan Kopya: {book.BorrowedCopies}");
                }
            }
            else
            {
                Console.WriteLine("Aranan kitap bulunamadı.");
            }
            Console.WriteLine();
        }

        public void BorrowBook(string title)
        {
            var book = books.Find(b => b.Title.Equals(title) && b.TotalCopies > b.BorrowedCopies);
            if (book != null)
            {
                Console.Write($"Kaç gün süreyle ödünç almak istiyorsunuz? ");

                if (int.TryParse(Console.ReadLine(), out int loanDurationDays))
                {
                    book.BorrowedCopies++;

                    book.DueDate = DateTime.Now.AddDays(loanDurationDays);
                    Console.WriteLine($"{title} başlıklı kitap ödünç alındı. İade tarihi: {book.DueDate.ToString("dd/MM/yyyy")}");
                }
                else
                {
                    Console.WriteLine("Geçerli bir iade süresi girmediniz.");
                }

            }
            else
            {
                Console.WriteLine("Kitap ödünç alınamadı. Stokta yeterli kopya yok veya kitap bulunamadı.");
            }
            Console.WriteLine();
        }

        public void ReturnBook(string title)
        {
            var book = books.Find(b => b.Title.Equals(title) && b.BorrowedCopies > 0);
            if (book != null)
            {
                if (DateTime.Now > book.DueDate)
                {
                    Console.WriteLine($"{title} başlıklı kitap süresi geçmiş. Ceza uygulanabilir. ");
                }
                book.BorrowedCopies--;
                Console.WriteLine($"{title} başlıklı kitap iade edildi.İade tarihi: {DateTime.Now.ToString("dd.MM.yyyy")}");
            }
            else
            {
                Console.WriteLine("Kitap iade edilemedi. Geçerli bir ödünç alınmış kitap bulunamadı.");
            }
            Console.WriteLine();
        }

        public void DisplayOverdueBooks()
        {
            var overdueBooks = books.FindAll(b => b.BorrowedCopies > 0 && b.TotalCopies == b.BorrowedCopies && b.DueDate < DateTime.Now);
            if (overdueBooks.Count > 0)
            {
                Console.WriteLine("Süresi Geçmiş Kitaplar:");
                foreach (var book in overdueBooks)
                {
                    Console.WriteLine($"Başlık: {book.Title}, Yazar: {book.Author}, ISBN: {book.ISBN}");
                }
            }
            else
            {
                Console.WriteLine("Süresi geçmiş kitap bulunmamaktadır.");
            }
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main()
        {
            Library library = new Library();

            while (true)
            {
                Console.WriteLine("1. Kitap Ekle");
                Console.WriteLine("2. Tüm Kitapları Görüntüle");
                Console.WriteLine("3. Kitap Ara");
                Console.WriteLine("4. Kitap Ödünç Al");
                Console.WriteLine("5. Kitap İade Et");
                Console.WriteLine("6. Süresi Geçmiş Kitapları Görüntüle");
                Console.WriteLine("7. Çıkış");

                Console.Write("Yapmak istediğiniz işlemi seçin (1-7): ");
                string choice = Console.ReadLine();

                int loanDurationDays = 0;

                switch (choice)
                {
                    case "1":
                        Console.Write("Başlık: ");
                        string title = Console.ReadLine();
                        Console.Write("Yazar: ");
                        string author = Console.ReadLine();
                        Console.Write("ISBN: ");
                        string isbn = Console.ReadLine();
                        Console.Write("Toplam Kopya Sayısı: ");
                        int totalCopies = Convert.ToInt32(Console.ReadLine());

                        Book newBook = new Book
                        {
                            Title = title,
                            Author = author,
                            ISBN = isbn,
                            TotalCopies = totalCopies
                        };

                        library.AddBook(newBook);
                        Console.WriteLine("Kitap başarıyla eklendi.\n");
                        break;

                    case "2":
                        library.DisplayAllBooks();
                        break;

                    case "3":
                        Console.Write("Arama Terimi: ");
                        string searchTerm = Console.ReadLine();
                        library.SearchBook(searchTerm);
                        break;

                    case "4":
                        Console.Write("Ödünç Alınacak Kitabın Başlığı: ");
                        string borrowTitle = Console.ReadLine();
                        library.BorrowBook(borrowTitle);
                        break;

                    case "5":
                        Console.Write("İade Edilecek Kitabın Başlığı: ");
                        string returnTitle = Console.ReadLine();
                        library.ReturnBook(returnTitle);
                        break;

                    case "6":
                        library.DisplayOverdueBooks();
                        break;

                    case "7":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Geçersiz bir seçenek girdiniz. Lütfen tekrar deneyin.\n");
                        break;
                }
            }
        }
    }


