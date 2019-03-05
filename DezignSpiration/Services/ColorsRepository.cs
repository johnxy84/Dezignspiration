using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DezignSpiration.Models;
using SQLite;
using DezignSpiration.Interfaces;
using DezignSpiration.Helpers;
using Newtonsoft.Json;

namespace DezignSpiration.Services
{
    public class ColorsRepository : IColorsRepository
    {
        SQLiteAsyncConnection dbConnection;
        private readonly INetworkClient httpClient;

        public ColorsRepository(INetworkClient httpClient)
        {
            dbConnection = App.dbConnection;
            this.httpClient = httpClient;

            try
            {
                dbConnection.CreateTableAsync<Color>().Wait();
            }
            catch (System.Exception ex)
            {
                Utils.LogError(ex, "ErrorInitializingColorsTable");
            }
        }

        public async Task<bool> InsertColors(IEnumerable<Color> colors)
        {
            return await dbConnection.InsertAllAsync(colors) == 1;
        }

        public async Task<bool> InsertColor(Color color)
        {
            return await dbConnection.InsertAsync(color) == 1;
        }

        public async Task<Color> GetColor(int colorId)
        {
            return await dbConnection.GetAsync<Color>(colorId);
        }

        public async Task<List<Color>> GetAllColors()
        {
            return await dbConnection.Table<Color>().ToListAsync();
        }

        public async Task<int> CountColor()
        {
            return await dbConnection.ExecuteScalarAsync<int>($"select count(*) from {typeof(Color).Name}");
        }

        public async Task<ObservableRangeCollection<Color>> GetFreshColors()
        {
            try
            {
                int totalColors = await CountColor();
                var response = await httpClient.Get($"/api/v1/list/colors?offset={totalColors}&limit={Constants.MAX_FETCH_QUOTE}");
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var colorsResponse = JsonConvert.DeserializeObject<ColorsResponse>(content);
                    await InsertColors(colorsResponse.Data.Colors);
                    return colorsResponse.Data.Colors;
                }
                Utils.TrackEvent("RefreshColors", content);
                return null;
            }
            catch (System.Exception ex)
            {
                Utils.LogError(ex, "GettingFreshColors");
                return null;
            }
        }

        public async Task<bool> AddColor(Color color, string deviceId = null)
        {
            var response = await httpClient.Post("/api/v1/colors", new
            {
                primary_color = color.PrimaryColor.ToUpper(),
                secondary_color = color.SecondaryColor.ToUpper(),
                device_id = deviceId
            });
            return response.IsSuccessStatusCode;
        }

    }
}
