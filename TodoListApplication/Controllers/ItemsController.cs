using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models;
using BusinessLogic.Repository;

namespace TodoListApplication.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemRepository _itemRepository;

        public ItemsController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            return View(await _itemRepository.ToListAsync());
              //return _context.Items != null ? 
              //            View(await _context.Items.ToListAsync()) :
              //            Problem("Entity set 'ApplicationDbContext.Items'  is null.");
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _itemRepository.ToListAsync() == null)
            {
                return NotFound();
            }

            var item = await _itemRepository.GetAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Date")] Item item)
        {
            if (ModelState.IsValid)
            {
                await _itemRepository.AddAsync(item);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _itemRepository.ToListAsync() == null)
            {
                return NotFound();
            }

            var item = await _itemRepository.GetAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Date")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _itemRepository.UpdateAsync(item);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_itemRepository.ItemExists(item.Id))
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
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _itemRepository.ToListAsync() == null)
            {
                return NotFound();
            }

            var item = await _itemRepository.GetAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _itemRepository.ToListAsync() == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Items'  is null.");
            }
            var item = await _itemRepository.GetAsync(id);
            if (item != null)
            {
                await _itemRepository.DeleteAsync(item.Id);
            }
            return RedirectToAction(nameof(Index));
        }        
    }
}
