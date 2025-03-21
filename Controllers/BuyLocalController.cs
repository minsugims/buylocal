using BuyLocalApp.Data;
using BuyLocalApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeBeanAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CoffeeBeanController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CoffeeBeanController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBeans()
    {
        var beans = await _context.CoffeeBeans.ToListAsync();
        return Ok(beans);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBeanById(int id)
    {
        var bean = await _context.CoffeeBeans.FindAsync(id);
        if (bean == null) return NotFound();
        return Ok(bean);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<IActionResult> AddBean([FromBody] CoffeeBean bean)
    {
        _context.CoffeeBeans.Add(bean);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBeanById), new { id = bean.Id }, bean);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBean(int id, [FromBody] CoffeeBean bean)
    {
        var existingBean = await _context.CoffeeBeans.FindAsync(id);
        if (existingBean == null) return NotFound();

        existingBean.Name = bean.Name;
        existingBean.Roaster = bean.Roaster;
        existingBean.Location = bean.Location;
        existingBean.Price = bean.Price;
        existingBean.FlavorNotes = bean.FlavorNotes;
        existingBean.ImageUrl = bean.ImageUrl;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBean(int id)
    {
        var bean = await _context.CoffeeBeans.FindAsync(id);
        if (bean == null) return NotFound();

        _context.CoffeeBeans.Remove(bean);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}