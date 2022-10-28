using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    public class AlbumController : Controller
    {
        private readonly MusicContext _context;

        public AlbumController(MusicContext context)
        {
            _context = context;
        }

        // GET: Album
        public IActionResult Index(int ArtistSort=0, int GenreSort=0)
        {   //Allows users to sort by artist or genre in either ascending or decending order.
            if (ArtistSort == 1 & GenreSort == 0)
                return View(_context.Albums.Include(a => a.Artist).
                       OrderBy(a => a.Artist.Name).
                       Include(a => a.Genre).
                       ToList());
            else if (ArtistSort == 0 & GenreSort == 1)
                return View(_context.Albums.Include(a => a.Artist).
                       Include(a => a.Genre).
                       OrderBy(g => g.Genre.Name).
                       ToList());
            else if (ArtistSort == 2 & GenreSort == 0)
                return View(_context.Albums.Include(a => a.Artist).
                       OrderByDescending(a => a.Artist.Name).   
                       Include(a => a.Genre).
                       ToList());
            else if (ArtistSort == 0 & GenreSort == 2)
                return View(_context.Albums.Include(a => a.Artist).
                       Include(a => a.Genre).
                       OrderByDescending(g => g.Genre.Name).
                       ToList());
            else
                return View(_context.Albums.Include(a => a.Artist).
                       Include(a => a.Artist).
                       Include(a => a.Genre).
                       ToList());


            /*var musicContext = _context.Albums.Include(a => a.Artist).Include(a => a.Genre);
            return View(await musicContext.ToListAsync());*/
        }

        // GET: Album/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Album/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "ArtistId");
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreId");
            return View();
        }

        // POST: Album/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "ArtistId", album.ArtistId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreId", album.GenreId);
            return View(album);
        }

        // GET: Album/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "ArtistId", album.ArtistId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreId", album.GenreId);
            return View(album);
        }

        // POST: Album/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        {
            if (id != album.AlbumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.AlbumId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "ArtistId", album.ArtistId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreId", album.GenreId);
            return View(album);
        }

        // GET: Album/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Album/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.AlbumId == id);
        }
    }
}
