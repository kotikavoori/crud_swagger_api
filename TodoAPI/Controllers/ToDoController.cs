using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Context;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private ToDoContext _context;

        public ToDoController(ToDoContext context)
        {
            _context = context;

            if (_context.ToDoItems.Count() == 0)
            {
                _context.ToDoItems.Add(new Models.ToDoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<ToDoItem>> GetAll()
        {
            return _context.ToDoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetItem")]
        public ActionResult<ToDoItem> GetItemByID(long id)
        {
            var item = _context.ToDoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public IActionResult Create(ToDoItem item)
        {
            _context.ToDoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetItem", new { Id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, ToDoItem item)
        {
            var todoitem = _context.ToDoItems.Find(id);
            if (todoitem == null)
            {
                return NotFound();
            }

            todoitem.IsComplete = item.IsComplete;
            todoitem.Name = item.Name;

            _context.ToDoItems.Update(todoitem);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.ToDoItems.Find(id);

            if (todo == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}