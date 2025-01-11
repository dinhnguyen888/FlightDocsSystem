using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FlightDocsSystem.Controllers
{
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Get All Account
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var accounts = await _accountService.GetAllAsync();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while retrieving accounts: {ex.Message}" });
            }
        }

        /// <summary>
        /// Get Account By Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var account = await _accountService.GetByIdAsync(id);
                if (account == null)
                    return NotFound(new { message = "Account not found" });

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while retrieving the account: {ex.Message}" });
            }
        }

        /// <summary>
        /// Create Account
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountCreateDto accountDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdAccount = await _accountService.CreateAsync(accountDto);
                return CreatedAtAction(nameof(GetById), new { id = createdAccount.Id }, createdAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while creating the account: {ex.Message}" });
            }
        }

        /// <summary>
        /// Update Account
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountUpdateDto accountDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var isUpdated = await _accountService.UpdateAsync(id, accountDto);
                if (!isUpdated)
                    return NotFound(new { message = "Account not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while updating the account: {ex.Message}" });
            }
        }

        /// <summary>
        /// Delete Account
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var isDeleted = await _accountService.DeleteAsync(id);
                if (!isDeleted)
                    return NotFound(new { message = "Account not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while deleting the account: {ex.Message}" });
            }
        }
    }
}
