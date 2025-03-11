
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
  public class ProductRepository : IProductRepository
  {
    private readonly StoreContext _context;

    public ProductRepository(StoreContext context)
    {
      _context = context;
    }

    public void AddProduct(Product product)
    {
      _context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
      _context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
      return await _context.Products
          .Select(p => p.Brand)
          .Distinct()
          .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
      return await _context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
      return await _context.Products.ToListAsync();
    }

    // ✅ Corrected Implementation for Filtering & Sorting
    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
      var query = _context.Products.AsQueryable();

      // 🔹 Apply brand filter if provided
      if (!string.IsNullOrEmpty(brand))
      {
        query = query.Where(p => p.Brand == brand);
      }

      // 🔹 Apply type filter if provided
      if (!string.IsNullOrEmpty(type))
      {
        query = query.Where(p => p.Type == type);
      }

      // 🔹 Apply sorting if provided
      switch (sort)
      {
        case "priceAsc":
          query = query.OrderBy(p => p.Price);
          break;
        case "priceDesc":
          query = query.OrderByDescending(p => p.Price);
          break;
        default:
          query = query.OrderBy(p => p.Name); // Default sorting by Name
          break;
      }

      return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
      return await _context.Products
          .Select(p => p.Type)
          .Distinct()
          .ToListAsync();
    }

    public bool ProductExists(int id)
    {
      return _context.Products.Any(x => x.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
      return await _context.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
      _context.Entry(product).State = EntityState.Modified;
    }
  }
}