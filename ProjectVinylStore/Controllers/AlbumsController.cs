using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectVinylStore.DataAccess;
using ProjectVinylStore.DataAccess.Entities;
using ProjectVinylStore.Models;

namespace ProjectVinylStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AlbumsController(AppDbContext context)
        {
            _context = context;
        }

        // Fix for the CS0029 error in the GetAlbums method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataAccess.Entities.Album>>> GetAlbums()
        {
            return await _context.Albums.ToListAsync();
        }

        // GET: api/Albums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DataAccess.Entities.Album>> GetAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);

            if (album == null)
            {
                return NotFound();
            }

            return album; // Updated return type to match 'Album' instead of 'VinylRecord'
        }

        // POST: api/Albums
        [HttpPost]
        public async Task<ActionResult<DataAccess.Entities.Album>> PostAlbum(DataAccess.Entities.Album album)
        {
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlbum), new { id = album.Id }, album); // Updated type to 'Album'
        }

        // PUT: api/Albums/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlbum(int id, DataAccess.Entities.Album album)
        {
            if (id != album.Id)
            {
                return BadRequest();
            }

            _context.Entry(album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Albums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.Id == id); // Corrected 'Album' to 'Albums'
        }
    }
}