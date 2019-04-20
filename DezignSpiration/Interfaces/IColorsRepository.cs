using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DezignSpiration.Models;

namespace DezignSpiration.Interfaces
{
    public interface IColorsRepository
    {
        Task<bool> InsertColors(IEnumerable<Color> colors);

        Task<bool> InsertColor(Color color);

        Task<Color> GetColor(int colorId);

        Task<List<Color>> GetAllColors();

        Task<int> CountColor();

        Task<ObservableRangeCollection<Color>> GetFreshColors();

        Task AddColor(Color color, string deviceId);

    }
}
