using Microsoft.AspNetCore.Http;
using ViewModels.Catalog.Products;
using ViewModels.Common;

namespace Application.Catalog.Products
{
    public interface IPrivateProductService
    {
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);
        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int addQuantity);
        Task AddViewCount(int productId);
        Task<PagedResult<ProductViewModel>> GetAllPagding(GetPrivateProductPagingRequest request);
        Task<int> AddImages(int productId, ProductImageCreateRequest request);
        Task<int> UpdateImages(int imageId, ProductImageUpdateRequest request);
        Task<int> DeleteImages(int imageId);
        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductImageViewModel>> GetListImages(int productId);
    }
}
