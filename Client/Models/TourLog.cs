using Client.Dao;
using Client.Models;

public class TourLog
{
    private ITourLogDao<TourLog> dao;
    public TourLog(ITourLogDao<TourLog> dao)
    {
        this.dao = dao;
        this.dao.SetModel(this);
    }
    public async Task<TourLog> Read()
    {
        return await this.dao.Read(this);
    }

    public async Task<IEnumerable<TourLog>> GetAll()
    {
        return await this.dao.ReadMultiple();
    }
    public int Id { get; set; } = 0;
    public DateTime Date { get; set; } = DateTime.Now;
    public string Comment { get; set; } = "";
    public float Difficulty { get; set; } = 0;
    public float Duration { get; set; } = 0;
    public float Rating { get; set; } = 0;
    public Tour Tour { get; set; } = new Tour();
}
