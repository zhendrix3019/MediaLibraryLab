public class Movie
{
    public int MediaId { get; set; }
    public string Title { get; set; }
    public string Director { get; set; }
    public TimeSpan RunningTime { get; set; }
    public List<string> Genres { get; set; }

    public string Display()
    {
        return $"Id: {MediaId}\nTitle: {Title}\nDirector: {Director}\nRun time: {RunningTime}\nGenres: {string.Join(", ", Genres)}";
    }
}
