using Microsoft.EntityFrameworkCore;
using CorpEntities;
using Microsoft.AspNetCore.Authorization;

namespace CorpApi
{
    /// <summary>
    /// API /solditems
    /// </summary>
    public static class SoldItemApi
    {
        public static RouteGroupBuilder MapSoldItemApi(this RouteGroupBuilder api)
        {

            //Таблица детализации по ID продажи
            api.MapGet("/{saleId}", [Authorize] async (int saleId, PharmacyContext db) =>
            {
                api.RequireAuthorization();
                var soldItems = await db.SoldItems
                    .Include(si => si.Product) 
                    .Include(si => si.Sale)    
                    .Where(si => si.SaleId == saleId) 
                    .Select(si => new
                    {
                        si.SoldItemId,
                        ProductName = si.Product.Name, 
                        si.Quantity,
                        si.Price
                    })
                    .OrderBy(si => si.SoldItemId) 
                    .ToListAsync();

                return soldItems.Any()
                    ? Results.Ok(new
                    {
                        Message = $"Детализация продажи №{saleId}",
                        Data = soldItems
                    })
                    : Results.NotFound($"Проданные товары для продажи {saleId} не найдены");
            });
      
            return api;
        }
    }
}
