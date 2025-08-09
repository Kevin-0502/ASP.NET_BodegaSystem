using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BodegaSystem.Models;

namespace BodegaSystem.Controllers
{
    public class MovimientoInventariosController : Controller
    {
        private readonly BodegaDBContext _context;

        public MovimientoInventariosController(BodegaDBContext context)
        {
            _context = context;
        }

        // GET: MovimientoInventarios
        public async Task<IActionResult> Index()
        {
            var bodegaDBContext = _context.Movimientos.Include(m => m.Producto);
            return View(await bodegaDBContext.ToListAsync());
        }

        // GET: MovimientoInventarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var movimientoInventario = await _context.Movimientos
                .Include(m => m.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movimientoInventario == null) return NotFound();

            return View(movimientoInventario);
        }

        // GET: MovimientoInventarios/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre");
            return View();
        }

        // POST: MovimientoInventarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductoId,Fecha,Cantidad,TipoMovimiento")] MovimientoInventario movimientoInventario)
        {
            if (ModelState.IsValid)
            {
                var producto = await _context.Productos.FindAsync(movimientoInventario.ProductoId);
                if (producto == null)
                {
                    ModelState.AddModelError("", "El producto no existe.");
                    ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", movimientoInventario.ProductoId);
                    return View(movimientoInventario);
                }

                if (movimientoInventario.Fecha == default)
                {
                    ModelState.AddModelError("Fecha", "Debe especificar una fecha válida.");
                }

                if (movimientoInventario.Cantidad <= 0)
                {
                    ModelState.AddModelError("Cantidad", "La cantidad debe ser mayor que cero.");
                }

                if (movimientoInventario.TipoMovimiento == "Salida" && producto.Existencia < movimientoInventario.Cantidad)
                {
                    ModelState.AddModelError("Cantidad", "No hay suficiente stock para realizar esta salida.");
                }

                if (!ModelState.IsValid)
                {
                    ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", movimientoInventario.ProductoId);
                    return View(movimientoInventario);
                }

                if (movimientoInventario.TipoMovimiento == "Entrada")
                {
                    producto.Existencia += movimientoInventario.Cantidad;
                }
                else if (movimientoInventario.TipoMovimiento == "Salida")
                {
                    producto.Existencia -= movimientoInventario.Cantidad;
                }

                _context.Add(movimientoInventario);
                _context.Update(producto);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", movimientoInventario.ProductoId);
            return View(movimientoInventario);
        }

        // GET: MovimientoInventarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movimientoInventario = await _context.Movimientos.FindAsync(id);
            if (movimientoInventario == null) return NotFound();

            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", movimientoInventario.ProductoId);
            return View(movimientoInventario);
        }

        // POST: MovimientoInventarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductoId,Fecha,Cantidad,TipoMovimiento")] MovimientoInventario movimientoInventario)
        {
            if (id != movimientoInventario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener movimiento original sin seguimiento para revertir stock
                    var movimientoOriginal = await _context.Movimientos.AsNoTracking()
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (movimientoOriginal == null)
                        return NotFound();

                    var producto = await _context.Productos.FindAsync(movimientoInventario.ProductoId);
                    if (producto == null)
                    {
                        ModelState.AddModelError("", "El producto no existe.");
                        ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", movimientoInventario.ProductoId);
                        return View(movimientoInventario);
                    }

                    // Validar fecha y cantidad
                    if (movimientoInventario.Fecha == default)
                    {
                        ModelState.AddModelError("Fecha", "Debe especificar una fecha válida.");
                    }
                    if (movimientoInventario.Cantidad <= 0)
                    {
                        ModelState.AddModelError("Cantidad", "La cantidad debe ser mayor que cero.");
                    }

                    // Revertir stock del movimiento original
                    if (movimientoOriginal.TipoMovimiento == "Entrada")
                    {
                        producto.Existencia -= movimientoOriginal.Cantidad;
                    }
                    else if (movimientoOriginal.TipoMovimiento == "Salida")
                    {
                        producto.Existencia += movimientoOriginal.Cantidad;
                    }

                    // Validar stock para nuevo movimiento si es salida
                    if (movimientoInventario.TipoMovimiento == "Salida" && producto.Existencia < movimientoInventario.Cantidad)
                    {
                        ModelState.AddModelError("Cantidad", "No hay suficiente stock para realizar esta salida.");
                    }

                    if (!ModelState.IsValid)
                    {
                        ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", movimientoInventario.ProductoId);
                        return View(movimientoInventario);
                    }

                    // Aplicar nuevo movimiento al stock
                    if (movimientoInventario.TipoMovimiento == "Entrada")
                    {
                        producto.Existencia += movimientoInventario.Cantidad;
                    }
                    else if (movimientoInventario.TipoMovimiento == "Salida")
                    {
                        producto.Existencia -= movimientoInventario.Cantidad;
                    }

                    _context.Update(movimientoInventario);
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovimientoInventarioExists(movimientoInventario.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", movimientoInventario.ProductoId);
            return View(movimientoInventario);
        }

        // GET: MovimientoInventarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movimientoInventario = await _context.Movimientos
                .Include(m => m.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movimientoInventario == null) return NotFound();

            return View(movimientoInventario);
        }

        // POST: MovimientoInventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movimientoInventario = await _context.Movimientos.FindAsync(id);
            if (movimientoInventario == null) return NotFound();

            var producto = await _context.Productos.FindAsync(movimientoInventario.ProductoId);
            if (producto == null) return NotFound();

            // Revertir stock según tipo de movimiento
            if (movimientoInventario.TipoMovimiento == "Entrada")
            {
                producto.Existencia -= movimientoInventario.Cantidad;
            }
            else if (movimientoInventario.TipoMovimiento == "Salida")
            {
                producto.Existencia += movimientoInventario.Cantidad;
            }

            _context.Movimientos.Remove(movimientoInventario);
            _context.Update(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MovimientoInventarioExists(int id)
        {
            return _context.Movimientos.Any(e => e.Id == id);
        }
    }
}
