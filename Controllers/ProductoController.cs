using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaTiendita.Models;
using LaTiendita.Stock;
using Microsoft.AspNetCore.Authorization;

namespace LaTiendita.Controllers
{
    public class ProductoController : Controller
    {
        private readonly BaseDeDatos _context;

        public ProductoController(BaseDeDatos context)
        {
            _context = context;
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            var baseDeDatos = _context.Producto.Include(p => p.Categoria);
            return View(await baseDeDatos.ToListAsync());
        }

        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio,Detalle,Imagen,CategoriaId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            ViewData["Talles"] = new SelectList(_context.Talles, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio,Detalle,Imagen,CategoriaId")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            ViewData["TalleId"] = new SelectList(_context.ProductoTalle, "TalleId", "Nombre", producto.Talles);
            return View(producto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Producto == null)
            {
                return Problem("Entity set 'BaseDeDatos.Producto'  is null.");
            }
            var producto = await _context.Producto.FindAsync(id);
            if (producto != null)
            {
                _context.Producto.Remove(producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
          return _context.Producto.Any(e => e.Id == id);
        }

        private async Task<bool> TalleExists(int id)
        {
            return await _context.Talles.AnyAsync(e => e.Id == id);
        }

        [HttpPost, ActionName("AddTalle")]
        public async Task<IActionResult> AddTalle(int productoId, int talleId, int cantidad)
        {
            //if (! await ProductoExists(productoId))
            //    return NotFound();

            //if (! await TalleExists(talleId))
            //    return NotFound();

            var productoTalle = await _context.ProductoTalle
                .FirstOrDefaultAsync(x => x.ProductoId == productoId && x.TalleId == talleId);

            if (productoTalle is null)
                _context.ProductoTalle.Add(new ProductoTalle() { ProductoId = productoId, TalleId = talleId, Cantidad = cantidad });
            else
            {
                productoTalle.Cantidad += cantidad;
                _context.ProductoTalle.Update(productoTalle);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }



    }
}
