using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using Mapster;

namespace KASHOP.BLL.MapsterConfigrations
{
    public static class MapsterConfig
    {
        public static void MapsterConfRegister()
        {
            //TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
            //    .Map(dest => dest.Category_Id, source => source.Id);

            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.CreatedBy, source => source.User.UserName);

            TypeAdapterConfig<Category, CategoryUserResponse>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations.
                Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest => dest.MainImage, source => $"https://localhost:7237/Images/{source.MainImage}");

            TypeAdapterConfig<Product, ProductUserResponse>.NewConfig()
                  .Map(dest => dest.MainImage, source => $"https://localhost:7237/Images/{source.MainImage}")
                .Map(dest => dest.Name, source => source.Translations.
                Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Product, ProductUserDetails>.NewConfig()
               .Map(dest => dest.Name, source => source.Translations.
               Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
               .Select(t => t.Name).FirstOrDefault()).Map(dest => dest.Description, source => source.Translations.
               Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
               .Select(t => t.Description).FirstOrDefault());

            TypeAdapterConfig<Order, OrderResponse>.NewConfig()
             .Map(dest => dest.UserName, source => source.User.UserName);

        }
    }
}