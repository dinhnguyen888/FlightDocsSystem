using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlightDocsSystem.Controllers
{
    [ApiController]
    //[Authorize(Policy = "ActiveToken")]
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
            var accounts = await _accountService.GetAllAsync();
            return Ok(accounts);
        }

        /// <summary>
        /// Get Account By Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account == null) return NotFound(new { message = "Account not found" });
            return Ok(account);
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Account
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountUpdateDto accountDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isUpdated = await _accountService.UpdateAsync(id, accountDto);
            if (!isUpdated) return NotFound(new { message = "Account not found" });

            return NoContent();
        }

        /// <summary>
        /// Delete Account
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _accountService.DeleteAsync(id);
            if (!isDeleted) return NotFound(new { message = "Account not found" });

            return NoContent();
        }
    }
}
