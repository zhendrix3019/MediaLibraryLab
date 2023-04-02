using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog.Web;

namespace MediaLibrary
{
    class Program
    {
        private static readonly NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        private static readonly string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
        private static readonly MovieFile movieFile = new MovieFile(scrubbedFile);

        static void Main(string[] args)
        {
            logger.Info("Program started");

            int choice;
            do
            {
                Console.WriteLine("[1] Add Movie");
                Console.WriteLine("[2] Display All Movies");
                Console.WriteLine("[0] Quit");
                choice = Int32.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddMovie();
                        break;
                    case 2:
                        DisplayAllMovies();
                        break;
                    default:
                        break;
                }
            } while (choice != 0);

            logger.Info("Program ended");
        }

        private static void AddMovie()
        {
            Console.WriteLine("Enter movie title:");
            string title = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(title))
            {
                Console.WriteLine("Invalid title. Please try again.");
                return;
            }

            if (movieFile.MovieList.Any(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("That movie is already in the list!");
                return;
            }

            Console.WriteLine("Enter movie director:");
            string director = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(director))
            {
                Console.WriteLine("Invalid director. Please try again.");
                return;
            }

            List<string> genres = new List<string>();
            while (true)
            {
                Console.WriteLine("Enter genre (or 'done' to quit):");
                string genre = Console.ReadLine().Trim();
                if (genre.Equals("done", StringComparison.OrdinalIgnoreCase))
                {
                    if (genres.Count == 0)
                    {
                        Console.WriteLine("At least one genre is required. Please try again.");
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                genres.Add(genre);
            }

            Console.WriteLine("Enter running time in the format 'hh:mm:ss':");
            string runTime = Console.ReadLine().Trim();
            if (!TimeSpan.TryParse(runTime, out TimeSpan duration))
            {
                Console.WriteLine("Invalid duration. Please try again.");
                return;
            }

            Movie movie = new Movie
            {
                Id = movieFile.GetNewID(),
                Title = title,
                Director = director,
                Genres = genres,
                RunTime = duration
            };

            movieFile.AddMovie(movie);
            Console.WriteLine($"Movie with id {movie.Id} added.");

            logger.Info("Movie added:");
            Console.WriteLine(movie.Display());
        }

        private static void DisplayAllMovies()
        {
            foreach (Movie movie in movieFile.MovieList)
            {
                Console.WriteLine(movie.Display());
            }
        }
    }
}
