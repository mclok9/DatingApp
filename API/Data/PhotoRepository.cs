using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class PhotoRepository : IPhotoRepository
{
    private readonly DataContext _context;
    
    public PhotoRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos()
    {
        return await _context.Photos.IgnoreQueryFilters().Where(p => p.IsApproved == false)
            .Select(x => new PhotoForApprovalDto
            {
                Id = x.Id,
                Username = x.AppUser.UserName,
                Url = x.Url,
                IsApproved = x.IsApproved
            }).ToListAsync();
    }

    public async Task<Photo> GetPhotoById(int id)
    {
        return await _context.Photos.IgnoreQueryFilters().SingleOrDefaultAsync(x => x.Id == id);
    }

    public void RemovePhoto(Photo photo)
    {
        _context.Photos.Remove(photo);
    }
}
