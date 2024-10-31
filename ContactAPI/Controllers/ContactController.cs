using ContactCore.DTOs;
using ContactCore.Entity;
using ContactCore.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ContactAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactRepository _repository;
    private readonly IValidator<ContactDTO> _validator;

    public ContactController(IContactRepository repository, IValidator<ContactDTO> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contact>>> GetAll()
    {
        var contacts = await _repository.GetAllAsync();
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetById(int id)
    {
        var contact = await _repository.GetByIdAsync(id);
        if (contact == null)
            return NotFound();

        return Ok(contact);
    }

    [HttpGet("ddd/{ddd}")]
    public async Task<ActionResult<IEnumerable<Contact>>> GetByDDD(string ddd)
    {
        var contacts = await _repository.GetByDDDAsync(ddd);
        return Ok(contacts);
    }

    [HttpPost]
    public async Task<ActionResult<Contact>> Create(ContactDTO contactDto)
    {
        var validationResult = await _validator.ValidateAsync(contactDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var contact = new Contact
        {
            Name = contactDto.Name,
            Phone = contactDto.Phone,
            Email = contactDto.Email,
            DDD = contactDto.DDD
        };

        var result = await _repository.AddAsync(contact);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ContactDTO contactDto)
    {
        var validationResult = await _validator.ValidateAsync(contactDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var existingContact = await _repository.GetByIdAsync(id);
        if (existingContact == null)
            return NotFound();

        existingContact.Name = contactDto.Name;
        existingContact.Phone = contactDto.Phone;
        existingContact.Email = contactDto.Email;
        existingContact.DDD = contactDto.DDD;

        await _repository.UpdateAsync(existingContact);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await _repository.GetByIdAsync(id);
        if (contact == null)
            return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
