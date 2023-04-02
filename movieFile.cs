public class MovieFile
{
    private string _filePath;

    public MovieFile(string filePath)
    {
        _filePath = filePath;
    }

    public List<Movie> GetMovies()
    {
        List<Movie> movies = new List<Movie>();

        using (StreamReader reader = new StreamReader(_filePath))
        {
            reader.ReadLine(); // skip the header row
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');
                Movie movie = new Movie
                {
                    MediaId = int.Parse(values[0]),
                    Title = values[1],
                    Director = values[2],
                    RunningTime = TimeSpan.Parse(values[3]),
                    Genres = values[4].Split('|').ToList()
                };
                movies.Add(movie);
            }
        }

        return movies;
    }

    public void SaveMovie(Movie movie)
    {
        using (StreamWriter writer = new StreamWriter(_filePath, true))
        {
            writer.WriteLine($"{movie.MediaId},{movie.Title},{movie.Director},{movie.RunningTime},{string.Join("|", movie.Genres)}");
        }
    }

    public int GetNewId()
    {
        List<Movie> movies = GetMovies();
        return movies.Count > 0 ? movies.Max(m => m.MediaId) + 1 : 1;
    }
}
