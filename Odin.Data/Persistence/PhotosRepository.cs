using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;

namespace Odin.Data.Persistence
{
    public class PhotosRepository : IPhotosRepository
    {
        private readonly IApplicationDbContext _context;

        public PhotosRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Photo photo)
        {
            _context.Photos.Add(photo);
        }
        
    }
}
